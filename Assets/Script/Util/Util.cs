using UnityEngine;
using System.Collections;

public class Util
{
	public static bool LeftSideOfScreen(Vector2 pos)
	{
		return pos.x < Screen.width / 2;
	}

	public static bool RightSideOfScreen(Vector2 pos)
	{
		return pos.x >= Screen.width / 2;
	}
	
	public static Bounds calculateBounds(GameObject go)
	{
		// First find a center for your bounds.
		Vector3 center = Vector3.zero;
		
		Renderer[] renders = go.transform.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer render in renders)
			center += render.bounds.center; 
		
		center /= renders.Length; // Center is average center of children
		
		// Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
		Bounds bounds = new Bounds(center, Vector3.zero); 
		
		foreach (Renderer render in renders)
			bounds.Encapsulate(render.bounds);
			
		return bounds;
	}
}
