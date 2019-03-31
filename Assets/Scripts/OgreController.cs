using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreController : MonoBehaviour
{
    public float sideWaysSpeed;
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
    private float fromFloatingBox;
    public GameObject deathAnim;
	public GameObject chatBubble;
	private bool helpChat = true;

    private enum grabSide
    {
        left,
        right,
        none
    };

    private grabSide gSide = grabSide.none;

	// Start is called before the first frame update
	void Start()
    {
		transform.eulerAngles = new Vector2(0, 100);

        ignoreCollision();
		
		animator = GetComponent<Animator>();
		rigid = this.GetComponent<Rigidbody2D>();
        movementSpeed = runSpeed;
        rigid.centerOfMass = Vector3.zero;
    }

    public void ignoreCollision()
    {
        foreach (Collider2D col1 in FindObjectOfType<GnomeController>().GetComponents<Collider2D>())
        {
            foreach (Collider2D col2 in this.GetComponents<Collider2D>())
            {
                Physics2D.IgnoreCollision(col1, col2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingBox)
        {
            if(Vector3.Distance(holdingBox.transform.position, this.transform.position) > fromFloatingBox + 0.2f)
            {
				Debug.Log("in Break");
                grabClosest();
            }
        }

        if(holdingBox != null)
        {
            if (Mathf.Abs(holdingBox.GetComponent<Rigidbody2D>().velocity.y) >= ychange || Mathf.Abs(rigid.velocity.y) >= ychange)
            {
                Debug.Log("break from here");
                movementSpeed = runSpeed;
                holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                holdingBox.beingHeld = false;
                holdingBox.ogreSelection.SetActive(false);
				holdingBox.ogreHolding = false;
				holdingBox = null;
            }
        }
        //Debug.Log(gSide);

        if (Input.GetKey(KeyCode.A))
		{
            if(holdingBox)
            {
                if(gSide == grabSide.left)
                {
                    if(holdingBox.gnomeHolding && holdingBox.BoxType != BoxScript.BoxTypes.led)
                    {
                        holdingBox.rb.velocity += Vector2.left * Time.deltaTime * movementSpeed;
                    }
                    else
                    {
                        holdingBox.rb.velocity += Vector2.left * Time.deltaTime * movementSpeed*2;
                    }
                }
                else
                {
                    if (holdingBox.gnomeHolding && holdingBox.BoxType != BoxScript.BoxTypes.led)
                    {
                        rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
                    }
                    else
                    {
                        rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed * 2;
                    }
                }
                if(!facingLeft)
                {
                    animSpeed = -0.6f;
                }
                else
                {
                    facingLeft = true;
                    rigid.transform.eulerAngles = new Vector2(0, -80);
                    animSpeed = 0.6f;
                }
            }
            else
            {
                if(isGrounded)
                {
                    rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
                }
                else
                {
                    rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed * 0.5f;
                }
                facingLeft = true;
                rigid.transform.eulerAngles = new Vector2(0, -80);
                animSpeed = 1f;
            }
		}
		else if (Input.GetKey(KeyCode.D))
		{
            if (holdingBox)
            {
                if (gSide == grabSide.right)
                {
                    if (holdingBox.gnomeHolding && holdingBox.BoxType != BoxScript.BoxTypes.led)
                    {
                        holdingBox.rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;
                    }
                    else
                    {
                        holdingBox.rb.velocity += Vector2.right * Time.deltaTime * movementSpeed * 2;
                    }
                }
                else
                {
                    if (holdingBox.gnomeHolding && holdingBox.BoxType != BoxScript.BoxTypes.led)
                    {
                        rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
                    }
                    else
                    {
                        rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed * 2;
                    }
                }
                if (!facingLeft)
                {
                    animSpeed = 0.6f;
                }
                else
                {
                    facingLeft = true;
                    rigid.transform.eulerAngles = new Vector2(0, -80);
                    animSpeed = -0.6f;
                }
            }
            else
            {
                if (isGrounded)
                {
                    rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
                }
                else
                {
                    rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed * 0.5f;
                }
                facingLeft = false;
                rigid.transform.eulerAngles = new Vector2(0, 100);
                animSpeed = 1f;
            }
        }
		else
		{
			if (isGrounded)
				animSpeed = 0;
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

		if (isGrounded)
		{
			animator.SetFloat("mainSpeed", animSpeed);
		}
		else
		{
			animator.SetFloat("mainSpeed", 1.5f);
		}

		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            GameObject anim = Instantiate(deathAnim);
            anim.transform.position = this.transform.position + Vector3.up * 2;
            this.gameObject.SetActive(false);
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
                if (holdingBox == null)
                {
                    gSide = grabSide.right;
                }
                Debug.Log("To the right");
            }
            if (contactPoint.x < center.x - collision.gameObject.transform.localScale.x / 2)
            {
                touchingBox = collision.gameObject.GetComponent<BoxScript>();
                if(holdingBox == null)
                {
                    gSide = grabSide.left;
                }
                Debug.Log("To the left");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("exiting");
        isGrounded = false;
        BoxScript b = collision.gameObject.GetComponent<BoxScript>();
        if(b != null)
        {
            if (b == holdingBox)    
            {
                if (Vector3.Distance(this.transform.position, holdingBox.transform.position) > fromFloatingBox + 1)
                {
                    grabClosest();
                }
            }
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
					holdingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    holdingBox.beingHeld = true;
                    holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    holdingBox.rb.mass = 1;
                    holdingBox.ogreSelection.SetActive(true);
                    fromFloatingBox = Vector3.Distance(this.transform.position, holdingBox.transform.position);
					holdingBox.ogreHolding = true;

                    if(holdingBox.gnomeHolding)
                    {
                        holdingBox.rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    }
                } 
                else if(touchingBox.BoxType == BoxScript.BoxTypes.steel)
                {
                    movementSpeed = carrySpeed;

                    holdingBox = touchingBox;
                    holdingBox.beingHeld = true;
                    holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    holdingBox.rb.mass = 1;
                    holdingBox.ogreSelection.SetActive(true);
                    fromFloatingBox = Vector3.Distance(this.transform.position, holdingBox.transform.position);
                }
                else if(touchingBox.BoxType == BoxScript.BoxTypes.magic)
                {
                    Debug.Log("Cannot pick up magic");
					TextBubble bubble = chatBubble.GetComponent<TextBubble>();
					bubble.setTextBubble(TextBubble.setText.noStr);
				}
                else if(touchingBox.BoxType == BoxScript.BoxTypes.led)
                {
                    Debug.Log("Needs help to move this");
					movementSpeed = carrySpeed;

					holdingBox = touchingBox;
					holdingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
					//holdingBox.beingHeld = true;
					holdingBox.ogreSelection.SetActive(true);
					holdingBox.ogreHolding = true;
					fromFloatingBox = Vector3.Distance(this.transform.position, holdingBox.transform.position);

					if (touchingBox.gnomeHolding)
					{
						holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
						holdingBox.rb.mass = 1;

					}
					else if (helpChat)
					{
						TextBubble bubble = chatBubble.GetComponent<TextBubble>();
						bubble.setTextBubble(TextBubble.setText.help);
						helpChat = false;
					}
                }
            }
        }
        else
        {
			Debug.Log("releasing");
			movementSpeed = runSpeed;
            holdingBox.beingHeld = false;
            holdingBox.rb.mass = 10;
            holdingBox.ogreSelection.SetActive(false);
			holdingBox.ogreHolding = false;
			holdingBox = null;
        }
    }


    private float lastFrameVelo;
    void OnCollisionStay2D()
    {
        if (rigid.velocity.y == 0)
        {
            isGrounded = true;
        }
        lastFrameVelo = rigid.velocity.y;
    }
}

