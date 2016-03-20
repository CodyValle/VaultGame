using UnityEngine;
using System.Collections;

public class BulletSpt : MonoBehaviour
{
	[Header("Bullet Stats")]
	[Tooltip("The speed that the bullet travels.")]
	public float speed = 5;

	void Update()
	{
		// Move the bullet in its direction
		transform.Translate(Vector2.up * Time.deltaTime * speed);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (LayerMask.LayerToName(col.gameObject.layer) == "Boundary")
		{
			gameObject.SetActive(false);
		}
		if (LayerMask.LayerToName(col.gameObject.layer) == "Enemy")
		{
			print ("bullet hit an enemy");
		}
	}
}
