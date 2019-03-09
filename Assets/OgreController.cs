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
    public float movementSpeed;
    private GameObject touchingBox = null;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(gnome.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ASDF");
        if (Input.GetKey(KeyCode.A))
        {
            rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            isGrounded = false;
            rigid.velocity += Vector2.up * jumpForce;
            Debug.Log(isGrounded);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            findClosestBox();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BoxScript>() != null)
        {
            Debug.Log("Touching a box");

            Vector3 center = collision.collider.bounds.center;
            Vector3 contactPoint = collision.contacts[0].point;

            Debug.Log(center.x + collision.gameObject.transform.localScale.x / 2);

            if (contactPoint.x >= center.x + collision.gameObject.transform.localScale.x/2)
            {
                touchingBox = collision.gameObject;
                Debug.Log("To the right");
            }
            if (contactPoint.x <= center.x - collision.gameObject.transform.localScale.x / 2)
            {
                touchingBox = collision.gameObject;
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

    public void findClosestBox()
    {
        BoxScript[] boxes = (BoxScript[])GameObject.FindObjectsOfType(typeof(BoxScript));
        Debug.Log(boxes.Length);
        foreach(BoxScript b in boxes)
        {
            var distance = Vector3.Distance(b.GetComponent<Transform>().position, this.transform.position);
            Debug.Log(distance);
        }
    }

    void OnCollisionStay2D()
    {
        isGrounded = true;
    }
}
