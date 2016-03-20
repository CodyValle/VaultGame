using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPoolerSpt : Singleton<BulletPoolerSpt>
{
	[Header("Bullet Pool")]
	[Tooltip("The initial number of objects to pool. Will increase as needed.")]
	public int pooledBullets = 30;      // The number of bullets we want to pool
	
	private GameObject bulletPool;    // The object used as the bullet's parent
	private GameObject bullet;        // The object used as the bullet
	private List<GameObject> bullets; // The list of pooled boxes
	//private Bounds bulletBounds;        // The bounds of one box
	
	
	// This object is a singleton, ergo protected
	protected BulletPoolerSpt() {}
	
	public void Start()
	{
		bulletPool = new GameObject();
		bulletPool.name = "BulletPool";
		
		// Create our list
		bullets = new List<GameObject>();
		
		// Load the bullet GameObject
		bullet = Resources.Load("LevelObjects/Bullet") as GameObject;
		
		// Instantiate and store the pooled objects
		for (int i = 0; i < pooledBullets; i++)
		{
			GameObject b = (GameObject)Instantiate(bullet, Vector2.zero, Quaternion.identity);
			b.SetActive(false); // They are not active at the beginning
			b.transform.SetParent(bulletPool.transform);
			bullets.Add(b);
		}
	}
	
	// Returns a bullet
	public GameObject getBullet()
	{
		for (int i = 0; i < bullets.Count; i++)
			if (!bullets[i].activeInHierarchy)
		{
			bullets[i].SetActive(true);
			return bullets[i];
		}
		
		// We need more than we have, make another
		GameObject b = (GameObject)Instantiate(bullet, Vector2.zero, Quaternion.identity);
		bullets.Add(b);
		b.transform.SetParent(bulletPool.transform);
		return b;
	}
}
