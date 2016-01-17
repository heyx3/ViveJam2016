using System;
using System.Collections.Generic;
using UnityEngine;


public class TextureGenerator : Singleton<TextureGenerator>
{
	public int TexSize = 512;
	public Material RockMat;
	public Texture3D RockTex, GrassTex;


	protected override void Awake()
	{
		base.Awake();


		//Generate the rock texture data.
		//TODO: Make noise wrap.
		Color[] cols = new Color[TexSize * TexSize * TexSize];
		for (int x = 0; x < TexSize; ++x)
		{
			for (int y = 0; y < TexSize; ++y)
			{
				for (int z = 0; z < TexSize; ++z)
				{
					float val = NoiseAlgos3D.LinearNoise(new Vector3(x * 0.125f, y * 0.125f, z * 0.125f));
					cols[x + (TexSize * y) + (TexSize * TexSize * z)] =
						Color.Lerp(new Color(0.2f, 0.15f, 0.35f, 1.0f),
								   new Color(0.1f, 0.1f, 0.1f, 1.0f),
								   val);
				}
			}
		}

		//Create the rock texture.
		RockTex = new Texture3D(TexSize, TexSize, TexSize, TextureFormat.RGB24, true);
		RockTex.filterMode = FilterMode.Point;
		RockTex.SetPixels(cols);
		RockTex.Apply();


		//Generate the grass texture data.
		for (int x = 0; x < TexSize; ++x)
		{
			for (int y = 0; y < TexSize; ++y)
			{
				for (int z = 0; z < TexSize; ++z)
				{
					cols[x + (TexSize * y) + (TexSize * TexSize * z)] = new Color(0.0f, 1.0f, 0.0f, 1.0f);
				}
			}
		}

		//Create the grass texture.
		GrassTex = new Texture3D(TexSize, TexSize, TexSize, TextureFormat.RGB24, true);
		GrassTex.filterMode = FilterMode.Trilinear;
		GrassTex.SetPixels(cols);
		GrassTex.Apply();


		//Set the textures in the material.
		RockMat.SetTexture("_RockTex", RockTex);
		RockMat.SetTexture("_GrassTex", GrassTex);
	}
}