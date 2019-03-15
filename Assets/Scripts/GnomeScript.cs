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

    // Start is called before the first frame update
    void Start()
    {
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
                holdingABox = true;
                LevitatingBox = touchingBox;
                //touchingBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                LevitatingBox = null;
                holdingABox = false;
                //touchingBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
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
