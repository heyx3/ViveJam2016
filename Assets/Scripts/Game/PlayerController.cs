using System;
using System.Collections.Generic;
using UnityEngine;
using Contr = SteamVR_Controller;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SteamVR_ControllerManager))]
public class PlayerController : Singleton<PlayerController>
{
	public float Speed = 1.0f,
				 Gravity = 5.0f,
				 MouseTurnSpeed = 1.0f;

	public KinematicsTracker LeftContr, RightContr;
	public Transform HeadTr;

	public GameObject GrenadePrefab;

	private float fallSpeed = 0.0f;


	public CharacterController Char { get; private set; }
	public Transform MyTr { get; private set; }
	public SteamVR_ControllerManager Contrs { get; private set; }


	protected override void Awake()
	{
		Char = GetComponent<CharacterController>();
		MyTr = transform;
		Contrs = GetComponent<SteamVR_ControllerManager>();
	}
	void Update()
	{
		//Keyboard movement.
		Vector2 move = Vector2.zero;
		if (Input.GetKey(KeyCode.W))
			move.y += 1.0f;
		if (Input.GetKey(KeyCode.S))
			move.y -= 1.0f;
		if (Input.GetKey(KeyCode.D))
			move.x += 1.0f;
		if (Input.GetKey(KeyCode.A))
			move.x -= 1.0f;

		//Mouse turning.
		Vector2 mp = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
		MyTr.Rotate(Vector3.up, mp.x * MouseTurnSpeed);
		if (!SteamVR.connected[0])
			HeadTr.Rotate(MyTr.right, mp.y * MouseTurnSpeed, Space.World);


		//Controller inputs.
		SteamVR_TrackedObject leftContr = Contrs.left.GetComponent<SteamVR_TrackedObject>(),
							  rightContr = Contrs.right.GetComponent<SteamVR_TrackedObject>();
		if (leftContr.gameObject.activeSelf)
		{
			int ij = Contr.GetDeviceIndex(Contr.DeviceRelation.Leftmost);
			Contr.Device leftC = Contr.Input(ij);
			move += leftC.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

			if (leftC.GetHairTriggerDown())
			{
				Transform tr = Instantiate(GrenadePrefab).transform;
				Rigidbody rgd = tr.GetComponent<Rigidbody>();
				tr.position = LeftContr.PositionLogs[LeftContr.GetLogIndex(0)];
				rgd.velocity = LeftContr.VelocityLogs[LeftContr.GetLogIndex(0)] * 2.5f;
			}
		}
		if (rightContr.gameObject.activeSelf)
		{
			int ij = Contr.GetDeviceIndex(Contr.DeviceRelation.Rightmost);
			Contr.Device rightC = Contr.Input(ij);
			
			if (rightC.GetHairTriggerDown())
			{
				Explosion(RightContr.PositionLogs[RightContr.GetLogIndex(0)], 2.5f);
			}

			if (rightC.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
			{
				RaycastHit info = new RaycastHit();
				if (Physics.Raycast(new Ray(RightContr.MyTransform.position, RightContr.MyTransform.up),
									out info))
				{
					MyTr.position = info.point + (info.normal * (Char.radius + 0.05f));
				}
			}
		}

		Char.Move(((MyTr.forward * move.y) + (MyTr.right * move.x)) *
				  Speed * Time.deltaTime);

		if (Input.GetMouseButtonDown(0))
		{
			Explosion(MyTr.position, 5.0f);
		}
		if (Input.GetMouseButtonDown(1))
		{
			Transform tr = Instantiate(GrenadePrefab).transform;
			Rigidbody rgd = tr.GetComponent<Rigidbody>();
			tr.position = MyTr.position + (MyTr.forward * 0.5f);
			rgd.velocity = MyTr.forward * 20.0f;
		}
		if (Input.GetMouseButtonDown(2))
		{
			RaycastHit info = new RaycastHit();
			if (Physics.Raycast(new Ray(MyTr.position, MyTr.forward),
								out info))
			{
				Char.Move((info.point + (info.normal * (Char.radius + 0.05f))) - MyTr.position);
			}
		}

		if (Char.isGrounded)
		{
			fallSpeed = 0.0f;
		}
		else
		{
			fallSpeed += Gravity * Time.deltaTime;
			Char.Move(Vector3.down * fallSpeed * Time.deltaTime);
		}
	}


	public static void Explosion(Vector3 pos, float radius)
	{
		MeshGenerator[,,] cells = WorldGenerator.Instance.Cells;

		Vector3 minF = pos - new Vector3(radius, radius, radius),
				maxF = pos + new Vector3(radius, radius, radius);
		minF /= (float)WorldGenerator.Instance.CellSize;
		maxF /= (float)WorldGenerator.Instance.CellSize;

		Vector3i min = new Vector3i((int)minF.x, (int)minF.y, (int)minF.z),
				 max = new Vector3i((int)maxF.x + 1, (int)maxF.y + 1, (int)maxF.z + 1);
		min = new Vector3i(Mathf.Max(min.x, 0), Mathf.Max(min.y, 0), Mathf.Max(min.z, 0));
		max = new Vector3i(Mathf.Min(cells.GetLength(0) - 1, max.x),
						   Mathf.Min(cells.GetLength(1) - 1, max.y),
						   Mathf.Min(cells.GetLength(2) - 1, max.z));

		for (int x = min.x; x <= max.x; ++x)
		{
			for (int y = min.y; y <= max.y; ++y)
			{
				for (int z = min.z; z <= max.z; ++z)
				{
					cells[x, y, z].Explode(pos, radius);
				}
			}
		}
	}
}
