using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxPoolerSpt : Singleton<BoxPoolerSpt>
{
	[Header("Box Pool")]
	[Tooltip("The initial number of objects to pool. Will increase as needed.")]
	public int pooledBoxes = 30;    // The number of boxes we want to pool
	
	[Tooltip("The width of everybox")]
	public float width = 5;
	[Tooltip("The height of everybox")]
	public float height = 5;
	

	private GameObject boxPool;      // The object used as the box's parent
	private GameObject box;          // The object used as the box
	private List<GameObject> boxes;  // The list of pooled boxes
	private Bounds boxBounds;        // The bounds of one box
	private Vector3 boxScale;	     // Scale so every box is desired size
	

	// This object is a singleton, ergo protected
	protected BoxPoolerSpt() {}

	public void Start()
	{
		boxPool = new GameObject();
		boxPool.name = "BoxPool";

		// Create our list
		boxes = new List<GameObject>();
		
		// Load the box GameObject
		box = Resources.Load("LevelObjects/Box") as GameObject;
		
		// Instantiate one box
		GameObject b = (GameObject)Instantiate(box, Vector2.zero, Quaternion.identity);
		b.transform.SetParent(boxPool.transform);
		boxes.Add(b);
		
		// Calculate the box's bounds
		boxBounds = Util.calculateBounds(b);
		
		// Set the scale to the desired dimensions
		boxScale = b.transform.localScale;
		b.transform.localScale = boxScale = new Vector3(width * boxScale.x / boxBounds.size.x, height * boxScale.y / boxBounds.size.y, 1);
		
		// Update the new bounds
		boxBounds = Util.calculateBounds(b);

		// Set the box to inactive
		b.SetActive(false);
		
		// Instantiate and store the pooled objects
		for (int i = 0; i < pooledBoxes - 1; i++)
			loadBox();
	}
	
	// Instantiates a box, then adds it to the pool
	private GameObject loadBox()
	{
		GameObject b = (GameObject)Instantiate(box, Vector2.zero, Quaternion.identity);
		b.transform.localScale = boxScale;
		b.SetActive(false); // They are not active at the beginning
		b.transform.SetParent(boxPool.transform);
		boxes.Add(b);
		return b;
	}

	// Returns the width of a box
	public float getWidth()
	{
		return width;
	}

	// Returns the height of a box
	public float getHeight()
	{
		return height;
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
		GameObject b = loadBox();
		b.SetActive(true);
		return b;
	}
}
