using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public float movementSpeed;
    public float levitationSpeed;
    public float climbSpeed;
    private Rigidbody2D rigid;
    private BoxScript touchingBox = null;
    private BoxScript floatingBox;
	private Animator animator = null;
	private bool holdingABox = false;
    private bool nextPressWillDrop = false;
    private bool onLadder = false;
	private bool facingLeft = false;

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
		//Debug.Log("arrows");
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			if (!facingLeft)
			{
				facingLeft = true;
				rigid.transform.eulerAngles = new Vector2(0, -89);
			}

			rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;

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

			rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;

            if (holdingABox && touchingBox != floatingBox)
            {
                grabClosest(false);
            }
			animator.SetFloat("animation", 1);
		}

		if(floatingBox != null)
		{ 
			if (floatingBox.ogreHolding == true && holdingABox)
			{
				grabClosest(false);
			}
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
            if(holdingABox) {
                floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                floatingBox.GetComponent<Rigidbody2D>().velocity += Vector2.up * Time.deltaTime * levitationSpeed;
            }
            else if(onLadder)
            {
                rigid.velocity += Vector2.up * Time.deltaTime * climbSpeed;
            }
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            if(holdingABox) {
                floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;    
                floatingBox.GetComponent<Rigidbody2D>().velocity += Vector2.down * Time.deltaTime * levitationSpeed;
            }
            else if (onLadder)
            {
                rigid.velocity += Vector2.down * Time.deltaTime * climbSpeed;
            }
        }
        if (floatingBox) {
            if(!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) {

				if (floatingBox.ogreHolding == false)
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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ladder")
        {
            Debug.Log("onLadder");
            onLadder = false;
            //rigid.velocity = Vector2.zero;
            rigid.gravityScale = 25;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        BoxScript b = collision.gameObject.GetComponent<BoxScript>();
        if (b != null)
        {
            touchingBox = null;
        }
    }

    public void grabClosest(bool pressActivate)
    {
		if (pressActivate && holdingABox) {
			nextPressWillDrop = true;
			holdingABox = false;
		}

        if(nextPressWillDrop)
        {
            floatingBox.GetComponent<Rigidbody2D>().gravityScale = 25;
            floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            floatingBox.beingHeld = false;
            floatingBox.gnomeSelection.SetActive(false);
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
                        //Debug.Log("grabbed " + touchingBox);
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.led)
                    {
                        Debug.Log("Needs help to move this");
                        //todo: add visual feedback that he needs help
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
