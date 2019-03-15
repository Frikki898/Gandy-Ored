using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public GameObject ogre;
    public float movementSpeed;
    public float levitationSpeed;
    private Rigidbody2D rigid;
    private GameObject touchingBox = null;
    private GameObject floatingBox;
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
        //Debug.Log("arrows");
        if(!holdingABox) {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigid.velocity += Vector2.left * Time.deltaTime * movementSpeed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rigid.velocity += Vector2.right * Time.deltaTime * movementSpeed;        
            }
        }
        
        if(Input.GetKey(KeyCode.UpArrow)) {
            if(holdingABox) {
                floatingBox = touchingBox;
                touchingBox.GetComponent<Rigidbody2D>().velocity += Vector2.up * Time.deltaTime * levitationSpeed;
            }
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            if(holdingABox) {
                touchingBox.GetComponent<Rigidbody2D>().velocity += Vector2.down * Time.deltaTime * levitationSpeed;
            }
        }
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            grabClosest();
        }
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BoxScript>() != null)
        {
            Debug.Log("Touching a box");
            touchingBox = collision.gameObject;
        }
    }

    public void grabClosest()
    {
        if(touchingBox != null)
        {
            if(!holdingABox)
            {
                if(floatingBox) {
                    floatingBox.GetComponent<Rigidbody2D>().gravityScale = 25;
                }
                holdingABox = true;
                touchingBox.GetComponent<Rigidbody2D>().gravityScale = 0;
                Debug.Log("grabbed " + touchingBox);
            }
            else
            {
                holdingABox = false;
                Debug.Log("dropped " + touchingBox);
                touchingBox.transform.parent = null;
                touchingBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                touchingBox = null;
            }
        }
        else {
            if(floatingBox) {
                    floatingBox.GetComponent<Rigidbody2D>().gravityScale = 25;
            }
        }
    }
}
