﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreController : MonoBehaviour
{
    public float sideWaysSpeed;
    public GameObject gnome;
    private Rigidbody2D rigid;
    public float jumpForce;
    public bool isGrounded;
    public float movementSpeed;
    public float runSpeed;
    public float carrySpeed;
	private Animator animator = null;
	private BoxScript touchingBox = null;
    private BoxScript holdingBox = null;
    private float ychange = 1;
	private float animSpeed = 1;
	private bool facingLeft = false;

	// Start is called before the first frame update
	void Start()
    {
		transform.eulerAngles = new Vector2(0, 100);

		Physics2D.IgnoreCollision(gnome.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
		animator = GetComponent<Animator>();
		rigid = this.GetComponent<Rigidbody2D>();
        movementSpeed = runSpeed;
        rigid.centerOfMass = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingBox != null)
        {
            if (Mathf.Abs(holdingBox.GetComponent<Rigidbody2D>().velocity.y) >= ychange || Mathf.Abs(rigid.velocity.y) >= ychange)
            {
                Debug.Log("break from here");
                movementSpeed = runSpeed;
                holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                holdingBox.beingHeld = false;
                holdingBox.ogreSelection.SetActive(false);
                holdingBox = null;
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
					rigid.transform.eulerAngles = new Vector2(0, -80);
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
				else
				{
					animSpeed = 1f;
				}
			}

			rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;

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
					rigid.transform.eulerAngles = new Vector2(0, 100);
					animSpeed = 1f;
				}
			}
			else
			{
				if (holdingBox != null)
				{
					holdingBox.rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;
					animSpeed = 0.6f;
				}
				else
				{
					animSpeed = 1f;
				}
			}
			rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
		}

		if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            if(holdingBox == null)
            {
                isGrounded = false;
                rigid.velocity += Vector2.up * jumpForce;
                Debug.Log(isGrounded);
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
			animSpeed = 0;
			grabClosest();
        }

		if (!Input.anyKey)
		{
			if (isGrounded)
				animSpeed = 0;
		}

		if (isGrounded)
		{
			animator.SetFloat("mainSpeed", animSpeed);
		}
		else
		{
			animator.SetFloat("mainSpeed", 1.5f);
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
        BoxScript b = collision.gameObject.GetComponent<BoxScript>();
        if (b != null)
        {
            Debug.Log("getting in here");
            if (b == holdingBox)
            {
                Debug.Log("not getting in here");
                //releasing the box if i move away form it
                grabClosest();
            }
            Debug.Log("stopped touching box");
            touchingBox = null;
        }
    }

    public void grabClosest()
    {
        if(holdingBox == null)
        {
            if (touchingBox != null)
            {
                if(touchingBox.BoxType == BoxScript.BoxTypes.wood)
                {
                    movementSpeed = carrySpeed;
                    holdingBox = touchingBox;
                    holdingBox.beingHeld = true;
                    holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    holdingBox.rb.mass = 1;
                    holdingBox.ogreSelection.SetActive(true);
                } 
                else if(touchingBox.BoxType == BoxScript.BoxTypes.steel)
                {
                    movementSpeed = carrySpeed;
                    holdingBox = touchingBox;
                    holdingBox.beingHeld = true;
                    holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    holdingBox.rb.mass = 1;
                    holdingBox.ogreSelection.SetActive(true);
                }
                else if(touchingBox.BoxType == BoxScript.BoxTypes.magic)
                {
                    Debug.Log("Cannot pick up magic");
                    //todo: add visual feedback that cube cant be picked up
                }
                else if(touchingBox.BoxType == BoxScript.BoxTypes.wood)
                {
                    Debug.Log("Needs help to move this");
                    //todo: add visual feedback that he needs help
                }
            }
        }
        else
        {
            movementSpeed = runSpeed;
            holdingBox.beingHeld = false;
            holdingBox.rb.mass = 10;
            holdingBox.ogreSelection.SetActive(false);
            holdingBox = null;
        }
    }


    private float lastFrameVelo;
    void OnCollisionStay2D()
    {
        if (rigid.velocity.y == 0 && lastFrameVelo == 0)
        {
            isGrounded = true;
        }
        lastFrameVelo = rigid.velocity.y;
    }
}
