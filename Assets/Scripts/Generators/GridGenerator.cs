using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

using Noise = NoiseAlgos3D;


/// <summary>
/// Generates a 3D grid on a different thread.
/// </summary>
public class GridGenerator
{
	public float[, ,] Voxels;


	public Thread Thr { get; private set; }
	public bool IsDone { get { return !Thr.IsAlive; } }


	/// <summary>
	/// Calling this constructor immediately kicks off the generation thread.
	/// </summary>
	public GridGenerator(int xSize, int ySize, int zSize,
						 int xOffset, int yOffset, int zOffset)
	{
		Thr = new Thread(() => Generate(xSize, ySize, zSize, xOffset, yOffset, zOffset));
		Thr.Start();
	}

	private void Generate(int xSize, int ySize, int zSize,
						  int xOffset, int yOffset, int zOffset)
	{
		Voxels = new float[xSize, ySize, zSize];

		for (int x = 0; x < xSize; ++x)
		{
			float posX = (float)(x + xOffset);
			for (int y = 0; y < ySize; ++y)
			{
				float posY = (float)(y + yOffset);
				for (int z = 0; z < zSize; ++z)
				{
					float posZ = (float)(z + zOffset);

					Vector3 pos = new Vector3(posX, posY, posZ);
					pos.x *= 0.125f;
					pos.y *= 0.25f;
					pos.z *= 0.125f;
					Voxels[x, y, z] = Noise.SmoothNoise(pos);
					
					const int startTop = 55,
							  endTop = 60;
					int yPos = y + yOffset;
					if (yPos >= startTop && yPos <= endTop)
					{
						float heightLerp = Mathf.InverseLerp(startTop, endTop, yPos);
						Voxels[x, y, z] = Mathf.Lerp(Voxels[x, y, z], 1.0f, heightLerp);
					}
					else if (yPos > endTop)
					{
						Voxels[x, y, z] = Mathf.Lerp(0.0f, 0.3999f,
													 NoiseAlgos2D.LinearNoise(new Vector2(pos.x, pos.z) * 6.0f));
					}

					Vector3 worldPos = new Vector3(x, y, z) + new Vector3(xOffset, yOffset, zOffset),
							holePos = new Vector3(30.0f, 60.0f, 30.0f);
					float distSqr = worldPos.DistanceSqr(holePos);
					if (distSqr < 32.0f)
					{
						float distLerp = Mathf.Sqrt(distSqr) / 32.0f;
						Voxels[x, y, z] = Mathf.Lerp(0.0f, Voxels[x, y, z], distLerp);
					}
				}
			}
		}
	}
}