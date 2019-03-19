using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OgreMeshController : MonoBehaviour
{
	public GameObject gnome;

	public float walkSpeed = 65;
	public float runSpeed = 65;
	public float jumpForce = 80;
	//public float smoothTurnTime = 0.4f;

	public bool isGrounded;

	private float turnVelocity;
	private float animSpeed = 1;
	private float movementSpeed;

	private bool holdingABox = false;
	private bool facingLeft = false;

	private Animator animator = null;
	private BoxScript touchingBox = null;
	private BoxScript holdingBox = null;
	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
	{
		transform.eulerAngles = new Vector2(0, 100);

		Physics2D.IgnoreCollision(gnome.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
		animator = GetComponent<Animator>();
		movementSpeed = runSpeed;

		rb = this.GetComponent<Rigidbody2D>();
		rb.centerOfMass = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

		//Debug.Log(touchingBox);
		if (touchingBox != null)
		{
			if (holdingBox != null)
			{
				if (Mathf.Abs(holdingBox.GetComponent<Rigidbody2D>().velocity.y) >= 0.01)
				{
					Debug.Log("releaseng box");
					movementSpeed = runSpeed;
					holdingBox = null;
					touchingBox.transform.parent = null;
					touchingBox.beingHeld = false;
					touchingBox.rb.mass = 10;
					holdingABox = false;
				}
			}
		}

		if (Input.GetKey(KeyCode.A))
		{
			if (!facingLeft)
			{
				if (holdingBox != null)
				{
					holdingBox.rb.velocity += Vector2.left * Time.deltaTime * movementSpeed;
					animSpeed = -0.6f;
				}
				else
				{
					facingLeft = true;
					rb.transform.eulerAngles = new Vector2(0, -80);
					animSpeed = 1f;
				}
			}
			else
			{
				if (holdingBox != null)
				{
					holdingBox.rb.velocity += Vector2.left * Time.deltaTime * movementSpeed;
					animSpeed = 0.6f;
				}
			}
			
			rb.velocity += Vector2.left  * Time.deltaTime * movementSpeed;

			if (isGrounded)
			{
				animator.SetFloat("mainSpeed", animSpeed);
			}
			else
			{
				//TODO: in-air and falling animation
				animator.SetFloat("mainSpeed", 1.5f);
			}

		}
		else if (Input.GetKey(KeyCode.D))
		{
			if (facingLeft)
			{
				if (holdingBox != null)
				{
					holdingBox.rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;
					animSpeed = -0.6f;
				}
				else
				{
					facingLeft = false;
					rb.transform.eulerAngles = new Vector2(0, 100);
				}
			}
			else
			{
				if (holdingBox != null)
				{
					holdingBox.rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;
					animSpeed = 0.6f;
				}
			}
			rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;

			if (isGrounded)
			{
				animator.SetFloat("mainSpeed", animSpeed);
			}
			else
			{
				//TODO: in-air and falling animation
				animator.SetFloat("mainSpeed", 1.5f);
			}
		}
		
		if(!Input.anyKey)
		{
			if(isGrounded)
				animator.SetFloat("mainSpeed", 0);
		}

		if (Input.GetKeyDown(KeyCode.W) && isGrounded)
		{
			if (holdingBox == null)
			{
				isGrounded = false;
				rb.velocity += Vector2.up * jumpForce;
			}
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			grabClosest();
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
		//if (holdingBox != null)
		//{
			if (collision.gameObject.GetComponent<BoxScript>() != null)
			{
				Debug.Log("stopped touching box");
				touchingBox = null;
			}
		//}
        
    }

	public void grabClosest()
	{
		if (touchingBox != null)
		{
			if (holdingBox == null)
			{
				movementSpeed = walkSpeed;
				holdingBox = touchingBox;
				touchingBox.transform.parent = this.transform;
				touchingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				touchingBox.beingHeld = true;
				touchingBox.rb.mass = 1;
				holdingABox = true;
			}
			else
			{
				movementSpeed = runSpeed;
				holdingBox = null;
				touchingBox.transform.parent = null;
				touchingBox.beingHeld = false;
				touchingBox.rb.mass = 10;
				holdingABox = false;
			}
		}
	}
	void OnCollisionStay2D()
	{
		isGrounded = true;
	}
}
