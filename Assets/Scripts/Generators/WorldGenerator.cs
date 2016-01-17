using System;
using System.Collections.Generic;
using UnityEngine;


public class WorldGenerator : Singleton<WorldGenerator>
{
	public int NCellsAlongAxis = 10;
	public int CellSize = 16;
	public PhysicMaterial RockPhysMat;


	public Transform CellsContainer { get; private set; }
	public MeshGenerator[, ,] Cells { get; private set; }
	
	
	void Start()
	{
		CellsContainer = new GameObject("Cells").transform;
		Cells = new MeshGenerator[NCellsAlongAxis, NCellsAlongAxis, NCellsAlongAxis];

		for (int x = 0; x < NCellsAlongAxis; ++x)
		{
			for (int y = 0; y < NCellsAlongAxis; ++y)
			{
				for (int z = 0; z < NCellsAlongAxis; ++z)
				{
					GameObject go = new GameObject("Cell " + x + "," + y + "," + z);
					Transform tr = go.transform;
					MeshFilter mf = go.AddComponent<MeshFilter>();
					MeshRenderer mr = go.AddComponent<MeshRenderer>();
					TerrainMesh tm = go.AddComponent<TerrainMesh>();

					MeshGenerator mg = go.AddComponent<MeshGenerator>();
					mg.XSize = CellSize;
					mg.YSize = CellSize;
					mg.ZSize = CellSize;
					mg.XOffset = x * CellSize;
					mg.YOffset = y * CellSize;
					mg.ZOffset = z * CellSize;
					Cells[x, y, z] = mg;

					MeshCollider mc = go.AddComponent<MeshCollider>();
					mc.sharedMaterial = RockPhysMat;

					tr.parent = CellsContainer;
				}
			}
		}
	}
}