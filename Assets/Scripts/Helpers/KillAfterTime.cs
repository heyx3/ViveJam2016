using UnityEngine;


public class KillAfterTime : MonoBehaviour
{
	public float TimeLeft = 5.0f;

	void Update()
	{
		TimeLeft -= Time.deltaTime;
		if (TimeLeft <= 0.0f)
			Destroy(gameObject);
	}
}