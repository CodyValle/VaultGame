using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxPoolerSpt : Singleton<BoxPoolerSpt>
{

	[Header("Box Pool")]
	[Tooltip("The initial number of objects to pool. Will increase as needed.")]
	public int pooledBoxes = 30;    // The number of boxes we want to pool

	private GameObject boxPool;      // The object used as the box's parent
	private GameObject box;          // The object used as the box
	private List<GameObject> boxes;  // The list of pooled boxes
	private Bounds boxBounds;        // The bounds of one box
	

	// This object is a singleton, ergo private
	protected BoxPoolerSpt() {}

	public void Start()
	{
		boxPool = new GameObject();

		// Create our list
		boxes = new List<GameObject>();
		
		// Load the box GameObject
		box = Resources.Load("LevelObjects/Box") as GameObject;
		
		// Instantiate and store the pooled objects
		for (int i = 0; i < 30; i++)
		{
			GameObject b = (GameObject)Instantiate(box, Vector2.zero, Quaternion.identity);
			b.SetActive(false); // They are not active at the beginning
			b.transform.SetParent(boxPool.transform);
			boxes.Add(b);
		}
		
		// Calculate the bounds of the first box
		// The box must be active
		boxes[0].SetActive(true);

		// First find a center for your bounds.
		Vector3 center = Vector3.zero;
		
		Renderer[] renders = boxes[0].transform.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer render in renders)
			center += render.bounds.center; 
		
		center /= renders.Length; //center is average center of children
		
		//Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
		boxBounds = new Bounds(center, Vector3.zero); 
		
		foreach (Renderer render in renders)
			boxBounds.Encapsulate(render.bounds);

		// Reset the box back to inactive
		boxes[0].SetActive(false);
	}

	// Returns the width of a box
	public float getWidth()
	{
		return boxBounds.size.x;
	}

	// Returns the height of a box
	public float getHeight()
	{
		return boxBounds.size.y;
	}

	// Returns a box
	public GameObject getBox()
	{
		for (int i = 0; i < boxes.Count; i++)
			if (!boxes[i].activeInHierarchy)
			{
				boxes[i].SetActive(true);
				return boxes[i];
			}

		// We need more than we have, make another
		GameObject b = (GameObject)Instantiate(box, Vector2.zero, Quaternion.identity);
		boxes.Add(b);
		b.transform.SetParent(boxPool.transform);
		return b;
	}
}
