using UnityEngine;
using System.Collections;

// Require a RigidBody2D is attached to this object
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterSpt : MonoBehaviour
{
	[Header("Player Stats")]
	
	[Tooltip("The power of each jump.")]
	public float jumpForce = 5;    // Upward force of the jump
	[Tooltip("The speed the player moves.")]
	public float speed = 2;		   // Horizontal motion of the character
	[Tooltip("The speed the player slides down a wall.")]
	public float slideSpeed = 2;   // Vertical motion of the character
	
	private bool doJump = false;   // Has the player triggered a jump?
	private bool onFloor = false;  // Is the player on the floor?
	private bool onWall = false;   // Is the player clinging to a wall?
	private int  direction = 1;    // Direction the character is moving
	private bool doShoot = false;  // Should the character start shooting?
	private bool shooting = false; // Is the character currently shooting?
	private Vector3 aimDirection;  // The direction the character is shooting
	private Vector2 touchLocation;  // Helper for the direction the character is shooting
	private Rigidbody2D rBody;     // Reference to the rigid body

	void Start()
	{
		// Set up the references
		rBody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Look for input to trigger a jump
		if (Input.GetButtonDown("Jump"))
		{
			if (onFloor || onWall)
				doJump = true;
		}
		if (Input.touchCount > 0)
		{
			// Needs optimisation: for (Touch t: Input.touches)
			for (int i = 0; i < Input.touches.Length; i++)
			{
				if (Util.LeftSideOfScreen(Input.GetTouch(i).position) && Input.GetTouch(i).phase == TouchPhase.Began)
				{
					if (onFloor || onWall)
						doJump = true;
				}

				// If there is a touch on the right side of the screen...
				if (Util.RightSideOfScreen(Input.GetTouch(i).position))
				{
					// ...shoot stuff
					
					// Optimisation: use switch
					if (Input.GetTouch(i).phase == TouchPhase.Began)
					{
						touchLocation = Input.GetTouch(i).position;
					}
					
					if (Input.GetTouch(i).phase == TouchPhase.Moved)
					{
						doShoot = true;
						aimDirection = -(touchLocation - Input.GetTouch(i).position).normalized;
					}

					if (Input.GetTouch(i).phase == TouchPhase.Ended)
					{
						doShoot = false;
					}
				}
			}
		}

		// Start shooting if the character isn't, but should
		if (doShoot && !shooting)
		{
			shooting = true;
			InvokeRepeating("Shoot", 0, 0.2f);
		}
		// Stop shooting if the character is and shouldn't
		else if (!doShoot && shooting)
		{
			shooting = false;
			CancelInvoke("Shoot");
		}
		
		// The character is constantly moving
		rBody.velocity = new Vector2(speed * direction, rBody.velocity.y);
	}

	void FixedUpdate()
	{
		// If a jump is triggered...
		if (doJump) 
		{
			// Only one jump per update
			doJump = false;
			// Change direction if jumped off a wall
			direction *= onWall && !onFloor ?  -1 : 1;
			// You're in the air now
			onWall = false;
			onFloor = false;

			Vector2 vel = rBody.velocity;
			vel.y = 0;
			rBody.velocity = vel;
			// Add the jump force to the rigidbody
			rBody.AddForce(transform.up * jumpForce * 1000);
		}
	}

	// Shoots the bullets
	private void Shoot()
	{
		GameObject b = BulletPoolerSpt.Instance.getBullet();
		b.transform.position = transform.position;
		b.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimDirection);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// If you're touching a floor, you can jump
		if (col.tag == "Floor")
			onFloor = true;

		// If you're touching a wall, you can jump and you're on a wall
		if (col.tag == "Wall")
			onWall = true;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		// If the character is attached to a wall...
		if (col.tag == "Wall")
		{
			// ... we have to limit its descent so that it slides at a constant rate
			Vector2 vel = rBody.velocity;
			vel.y = Mathf.Clamp(rBody.velocity.y, slideSpeed * (10^(-12)), float.MaxValue);
			rBody.velocity = vel;

			// We can jump vertically while touching a wal from the floor
			onWall = true;
		}

		// If the character is attached to a floor...
		if (col.tag == "Floor")
			onFloor = true;
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		// If you've left a floor, you cannnot jump
		if (col.tag == "Floor")
			onFloor = false;

		// If you've left a wall, you cannnot jump
		if (col.tag == "Wall")
			onWall = false;
	}

	public void setStart(Vector2 pos)
	{
		transform.position = pos;
	}
}
