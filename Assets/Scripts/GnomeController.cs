using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public GameObject ogre;
    public float movementSpeed;
    public float levitationSpeed;
    public float climbSpeed;
    private Rigidbody2D rigid;
    private BoxScript touchingBox = null;
    private BoxScript floatingBox;
    private bool holdingABox = false;
    private bool nextPressWillDrop = false;
    private bool onLadder = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(ogre.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("arrows");
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
            if(holdingABox)
            {
                grabClosest();
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
            if (holdingABox)
            {
                grabClosest();
            }
        }
        
        if(Input.GetKey(KeyCode.UpArrow)) {
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
                floatingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;    
            }
        }
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            grabClosest();
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

    public void grabClosest()
    {
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
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.wood)
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
