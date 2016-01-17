using System;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : MonoBehaviour
{
	[NonSerialized]
	public bool Triggering = false;

	public float TriggerTime = 4.0f;
	public float ExplosionRadius = 4.5f;

	public GameObject ExplosionPrefab;


	void Update()
	{
		if (Triggering)
		{
			TriggerTime -= Time.deltaTime;
			if (TriggerTime <= 0.0f)
			{
				PlayerController.Explosion(transform.position, ExplosionRadius);
				Instantiate(ExplosionPrefab).transform.position = transform.position;
				Destroy(gameObject);
			}
		}
	}
	void OnCollisionEnter(Collision coll)
	{
		if (!Triggering)
		{
			Triggering = true;
		}
	}
}