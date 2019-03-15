using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OgreController : MonoBehaviour
{
    public float sideWaysSpeed;
    public GameObject gnome;
    private Rigidbody2D rigid;
    public float jumpForce;
    public bool isGrounded;
    private float movementSpeed;
    public float runSpeed;
    public float carrySpeed;
    private BoxScript touchingBox = null;
    private BoxScript holdingBox = null;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(gnome.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        rigid = this.GetComponent<Rigidbody2D>();
        movementSpeed = runSpeed;
        rigid.centerOfMass = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(touchingBox != null)
        {
            if(holdingBox != null)
            {
                if (Mathf.Abs(holdingBox.GetComponent<Rigidbody2D>().velocity.y) >= 0.01)
                {
                    movementSpeed = runSpeed;
                    holdingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    holdingBox.beingHeld = false;
                    holdingBox = null;
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if(holdingBox != null)
            {
                holdingBox.rb.velocity += Vector2.left * Time.deltaTime * movementSpeed;
            }
            rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (holdingBox != null)
            {
                holdingBox.rb.velocity += Vector2.right * Time.deltaTime * movementSpeed;
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
        if (collision.gameObject.GetComponent<BoxScript>() != null)
        {
            Debug.Log("stopped touching box");
            touchingBox = null;
        }
    }

    public void grabClosest()
    {

        if(touchingBox != null)
        {
            if(holdingBox == null)
            {
                movementSpeed = carrySpeed;
                holdingBox = touchingBox;
                touchingBox.transform.parent = this.transform;
                touchingBox.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                touchingBox.beingHeld = true;
                touchingBox.rb.mass = 1;
            }
            else
            {
                movementSpeed = runSpeed;
                holdingBox = null;
                touchingBox.transform.parent = null;
                touchingBox.beingHeld = false;
                touchingBox.rb.mass = 10;
            }
        }
    }

    void OnCollisionStay2D()
    {
        isGrounded = true;
    }
}
