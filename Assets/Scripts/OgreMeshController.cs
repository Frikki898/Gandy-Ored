using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OgreMeshController : MonoBehaviour
{
	public GameObject gnome;

	public float walkSpeed = 65;
	public float runSpeed = 120;
	public float jumpForce = 80;
	//public float smoothTurnTime = 0.2f;

	public bool isGrounded;

	private float turnVelocity;
	private float movementSpeed;

	private bool holdingABox = false;
	private bool facingLeft = false;

	private Animator animator = null;
	private BoxScript touchingBox = null;
	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
	{
		Physics2D.IgnoreCollision(gnome.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
		transform.eulerAngles = new Vector2(0, 100);
		animator = GetComponent<Animator>();
		movementSpeed = runSpeed;

		rb = this.GetComponent<Rigidbody2D>();
		rb.centerOfMass = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.A))
		{
			if (!facingLeft)
			{
				facingLeft = true;
				rb.transform.eulerAngles = new Vector2(0, -80);
			}
			
			rb.velocity += Vector2.left  * Time.deltaTime * movementSpeed;

			if (isGrounded)
			{
				animator.SetFloat("Speed", 1);
			}

		}
		else if (Input.GetKey(KeyCode.D))
		{
			if (facingLeft)
			{
				facingLeft = false;
				rb.transform.eulerAngles = new Vector2(0, 100);
			}
			rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;

			if (isGrounded)
			{
				animator.SetFloat("Speed", 1);
			}
		}
		
		if(!Input.anyKey)
		{
			animator.SetFloat("Speed", 0);
		}

		if (Input.GetKeyDown(KeyCode.W) && isGrounded)
		{
			if (!holdingABox)
			{
				isGrounded = false;
				rb.velocity += Vector2.up * jumpForce;
				Debug.Log(isGrounded);
			}
		}	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<BoxScript>() != null)
		{
			Debug.Log("Touching a box");

			Vector3 center = collision.collider.bounds.center;
			Vector3 contactPoint = collision.contacts[0].point;

			if (contactPoint.x > center.x + collision.gameObject.transform.localScale.x / 2)
			{
				touchingBox = collision.gameObject.GetComponent<BoxScript>();
				Debug.Log("To the right");
			}
			if (contactPoint.x < center.x - collision.gameObject.transform.localScale.x / 2)
			{
				touchingBox = collision.gameObject.GetComponent<BoxScript>();
				Debug.Log("To the left");
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		isGrounded = false;
		if (collision.gameObject.GetComponent<BoxScript>() != null)
		{
			if (!holdingABox)
			{
				Debug.Log("stopped touching box");
				touchingBox = null;
			}
		}
	}

	public void grabClosest()
	{

		if (touchingBox != null)
		{
			if (!holdingABox)
			{
				movementSpeed = walkSpeed;
				holdingABox = true;
				touchingBox.transform.parent = this.transform;
				touchingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				touchingBox.beingHeld = true;
				touchingBox.rb.mass = 1;
			}
			else
			{
				movementSpeed = runSpeed;
				holdingABox = false;
				touchingBox.transform.parent = null;
				touchingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
				touchingBox.beingHeld = false;
			}
		}
	}
	void OnCollisionStay2D()
	{
		isGrounded = true;
	}
}
