using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public float movementSpeed;
    public float levitationSpeed;
    public float climbSpeed;
    public float jumpForce;

    private Rigidbody2D rigid;
    private BoxScript touchingBox = null;
    private BoxScript floatingBox;
	private Animator animator = null;
	private bool holdingABox = false;
    private bool nextPressWillDrop = false;
    private bool onLadder = false;
	private bool facingLeft = false;
    private bool isGrounded;
	private float ychange = 1;
    public GameObject deathAnim;



    public Rigidbody2D getRigid()
    {
        return rigid;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider2D col1 in FindObjectOfType<OgreController>().GetComponents<Collider2D>())
        {
            foreach (Collider2D col2 in this.GetComponents<Collider2D>())
            {
                Physics2D.IgnoreCollision(col1, col2);
            }
        }
        rigid = this.GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
		if (floatingBox != null)
		{
			if (floatingBox.BoxType == BoxScript.BoxTypes.led)
			{ 
				if (Mathf.Abs(floatingBox.GetComponent<Rigidbody2D>().velocity.y) >= ychange || Mathf.Abs(rigid.velocity.y) >= ychange)
				{
                    Debug.Log("altering gravity");
					floatingBox.GetComponent<Rigidbody2D>().gravityScale = 25;
					floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
					floatingBox.beingHeld = false;
					floatingBox.gnomeSelection.SetActive(false);
					floatingBox.gnomeHolding = false;

					BoxCollider2D collider = floatingBox.GetComponent<BoxCollider2D>();
					collider.offset = new Vector2(0, 0);
					collider.size = new Vector2(1, 1);

					floatingBox = null;
					nextPressWillDrop = false;
				}
			}
		}

		//Debug.Log("arrows");
		//if (Input.GetKey(KeyCode.LeftArrow))
        isGrounded = false;
        if (rigid.velocity.y == 0 || touchingBox != null)
        {
            isGrounded = true;
        }

        //Debug.Log("arrows");
        if (Input.GetKey(KeyCode.LeftArrow))
		{
		if (!facingLeft)
		{
			facingLeft = true;
			rigid.transform.eulerAngles = new Vector2(0, -89);
		}


            /*if (rigid.gravityScale == 0)
            {
                rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed * 0.4f;
            }
            else */if(isGrounded && !onLadder)
            {
                rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
            }
            else
            {
                Debug.Log("laddermove");
                rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed * 0.5f;
            }
			

            if(holdingABox && touchingBox != floatingBox)
            {
                grabClosest(false);
            }
			animator.SetFloat("animation", 1);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			if (facingLeft)
			{
				facingLeft = false;
				rigid.transform.eulerAngles = new Vector2(0, 89);
			}

            /*if (rigid.gravityScale == 0)
            {
                rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed * 0.4f;
            }
            else */if (isGrounded && !onLadder)
            {
                rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
            }
            else
            {
                rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed * 0.5f;
            }

            if (holdingABox && touchingBox != floatingBox)
            {
                grabClosest(false);
            }
			animator.SetFloat("animation", 1);
		}
		else
		{
			animator.SetFloat("animation", 0);
		}

		if (floatingBox != null)
		{

			if (floatingBox.ogreHolding == true && holdingABox)
			{
				grabClosest(false);
			}
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
            if(holdingABox)
			{
				if (floatingBox.BoxType != BoxScript.BoxTypes.led)
				{
					floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
					floatingBox.GetComponent<Rigidbody2D>().velocity += Vector2.up * Time.deltaTime * levitationSpeed;
				}
            }
            else if(onLadder)
            {
                Debug.Log("ASDF");
                rigid.velocity += Vector2.up * Time.deltaTime * climbSpeed;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    isGrounded = false;
                    rigid.velocity += Vector2.up * jumpForce;
                }
            }
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            if(holdingABox)
			{
				if (floatingBox.BoxType != BoxScript.BoxTypes.led)
				{
					floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
					floatingBox.GetComponent<Rigidbody2D>().velocity += Vector2.down * Time.deltaTime * levitationSpeed;
				}
			}
            else if (onLadder)
            {
                rigid.velocity += Vector2.down * Time.deltaTime * climbSpeed;
            }
        }
        if (floatingBox) {
            if(!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) {

				if (floatingBox.ogreHolding == false && floatingBox.BoxType != BoxScript.BoxTypes.led)
				{
					floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
				}
            }
        }
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            grabClosest(true);
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        BoxScript b = collision.gameObject.GetComponent<BoxScript>();
        if (b != null)
        {
            //Debug.Log("Touching a box");
            touchingBox = collision.gameObject.GetComponent<BoxScript>();

            Vector3 center = collision.collider.bounds.center;
            Vector3 contactPoint = collision.contacts[0].point;

            if (contactPoint.x > center.x + collision.gameObject.transform.localScale.x / 2)
            {
                
            }
            else if (contactPoint.x < center.x - collision.gameObject.transform.localScale.x / 2)
            {
                
            }
            else
            {
                //rigid.gravityScale = 0;
                //Debug.Log("on top of box");
            }

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ladder")
        {
            Debug.Log("onLadder");
            onLadder = true;
            rigid.gravityScale = 0;
            movementSpeed = 100;
        }
        else if(collision.gameObject.tag == "Box")
        {
            GameObject anim = Instantiate(deathAnim);
            anim.transform.position = this.transform.position + Vector3.up * 1;
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ladder")
        {
            Debug.Log("onLadder");
            onLadder = false;
            movementSpeed = 150;
            //rigid.velocity = Vector2.zero;
            Debug.Log("altering gravity");
            rigid.gravityScale = 25;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        BoxScript b = collision.gameObject.GetComponent<BoxScript>();
        if (b != null)
        {
            Debug.Log("altering gravity");
            if(!onLadder)
            {
                rigid.gravityScale = 25;
            }
            touchingBox = null;
        }
    }

    public void grabClosest(bool pressActivate)
    {
		if (pressActivate && holdingABox) {
			nextPressWillDrop = true;
			holdingABox = false;
		}

		if (nextPressWillDrop && floatingBox == null)
		{
			nextPressWillDrop = false;
		}
        else if(nextPressWillDrop)
        {
            Debug.Log("altering gravity");
            floatingBox.GetComponent<Rigidbody2D>().gravityScale = 25;
            floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            floatingBox.beingHeld = false;
            floatingBox.gnomeSelection.SetActive(false);
			floatingBox.gnomeHolding = false;

			BoxCollider2D collider = floatingBox.GetComponent<BoxCollider2D>();
			collider.offset = new Vector2(0, 0);
			collider.size = new Vector2(1, 1);
            floatingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			floatingBox = null;
            nextPressWillDrop = false;
            
        }

        if(!holdingABox)
        {
            if(touchingBox != null)
            {
                if(!touchingBox.beingHeld)
                {
                    if (touchingBox.BoxType == BoxScript.BoxTypes.wood)
                    {
                        floatingBox = touchingBox;
                        floatingBox.beingHeld = true;
                        holdingABox = true;
                        touchingBox.GetComponent<Rigidbody2D>().gravityScale = 0;
                        floatingBox.gnomeSelection.SetActive(true);
                        floatingBox.gnomeHolding = true;
                        //Debug.Log("grabbed " + touchingBox);
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.steel)
                    {
                        Debug.Log("Cannot pick up steel");
                        //todo: add visual feedback that cube cant be picked up
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.magic)
                    {
                        floatingBox = touchingBox;
                        floatingBox.beingHeld = true;
                        holdingABox = true;
                        touchingBox.GetComponent<Rigidbody2D>().gravityScale = 0;
                        floatingBox.gnomeSelection.SetActive(true);
                        floatingBox.gnomeHolding = true;
                        //Debug.Log("grabbed " + touchingBox);
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.led)
                    {
                        Debug.Log("Needs help to move this");

						floatingBox = touchingBox;
						floatingBox.beingHeld = true;
						holdingABox = true;
						floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
						floatingBox.gnomeSelection.SetActive(true);
						floatingBox.gnomeHolding = true;

						BoxCollider2D collider = floatingBox.GetComponent<BoxCollider2D>();

						collider.offset = new Vector2(0, -0.05f);
						collider.size = new Vector2(1, 1.1f);
						//floatingBox.transform.position = new Vector3(floatingBox.transform.position.x, floatingBox.transform.position.y + 0.1f, floatingBox.transform.position.z);
						//todo: add visual feedback that he needs help
						if (floatingBox.ogreHolding)
						{
							floatingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
							floatingBox.rb.mass = 1;
						}
					}
                }
            }
        }
        else {
            holdingABox = false;
            nextPressWillDrop = true;
        }
    }
}
