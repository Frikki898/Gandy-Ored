using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeScript : MonoBehaviour
{
    public GameObject ogre;
    public float movementSpeed;
    private Rigidbody2D rigid;
    private BoxScript touchingBox = null;
    private BoxScript LevitatingBox = null;
    private bool holdingABox = false;
	private Animator animator = null;

    // Start is called before the first frame update
    void Start()
    {
		transform.eulerAngles = new Vector2(0, 89);

		animator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(ogre.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingABox)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            grabClosest();
        }
        if (holdingABox)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                touchingBox.GetComponent<Rigidbody2D>().velocity += Vector2.up * Time.deltaTime * 100;
            }
        }
    }

    public void grabClosest()
    {
        if (touchingBox != null)
        {
            if (!holdingABox)
            {
                if(!touchingBox.beingHeld)
                {
                    if (touchingBox.BoxType == BoxScript.BoxTypes.wood)
                    {
                        holdingABox = true;
                        LevitatingBox = touchingBox;
                        LevitatingBox.beingHeld = true;
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.steel)
                    {
                        Debug.Log("Cannot pick up steel");
                        //todo: add visual feedback that cube cant be picked up
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.magic)
                    {
                        holdingABox = true;
                        LevitatingBox = touchingBox;
                        LevitatingBox.beingHeld = true;
                    }
                    else if (touchingBox.BoxType == BoxScript.BoxTypes.wood)
                    {
                        Debug.Log("Needs help to move this");
                        //todo: add visual feedback that he needs help
                    }
                }
            }
            else
            {
                LevitatingBox.beingHeld = false;
                LevitatingBox = null;
                holdingABox = false;
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
}
