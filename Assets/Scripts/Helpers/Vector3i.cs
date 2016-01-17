using System;
using Vector3 = UnityEngine.Vector3;

public struct Vector3i
{
	public int x, y, z;


	public Vector3i(int _x, int _y, int _z) { x = _x; y = _y; z = _z; }


	public static bool operator==(Vector3i lhs, Vector3i rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}
	public static bool operator!=(Vector3i lhs, Vector3i rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}
	
	public static Vector3i operator+(Vector3i lhs, Vector3i rhs)
	{
		return new Vector3i(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}
	public static Vector3i operator-(Vector3i lhs, Vector3i rhs)
	{
		return new Vector3i(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}
	public static Vector3i operator*(Vector3i lhs, int rhs)
	{
		return new Vector3i(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}
	public static Vector3i operator/(Vector3i lhs, int rhs)
	{
		return new Vector3i(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	
	public Vector3i LessX { get { return new Vector3i(x - 1, y, z); } }
	public Vector3i LessY { get { return new Vector3i(x, y - 1, z); } }
	public Vector3i LessZ { get { return new Vector3i(x, y, z - 1); } }
	public Vector3i MoreX { get { return new Vector3i(x + 1, y, z); } }
	public Vector3i MoreY { get { return new Vector3i(x, y + 1, z); } }
	public Vector3i MoreZ { get { return new Vector3i(x, y, z + 1); } }


	public override string ToString()
	{
		return "{" + x + ", " + y + ", " + z + "}";
	}
	public override int GetHashCode()
	{
		return (x * 73856093) ^ (y * 19349663) ^ (z * 786431);
	}
	public int GetHashCode(int w)
	{
		return (this * w).GetHashCode();
	}
	public override bool Equals(object obj)
	{
		return (obj is Vector3i) && ((Vector3i)obj) == this;
	}
}

/// <summary>
/// Add some stuff to Unity's Vector classes.
/// </summary>
public static class V3Extensions
{
	public static float DistanceSqr(this Vector3 v, Vector3 otherV) { return (otherV - v).sqrMagnitude; }
}