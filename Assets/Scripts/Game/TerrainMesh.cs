using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class TerrainMesh : MonoBehaviour
{
	void Start()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.sharedMaterial = TextureGenerator.Instance.RockMat;
		Destroy(this);
	}
}