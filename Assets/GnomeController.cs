using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public GameObject ogre;
    public float movementSpeed;
    private Rigidbody2D rigid;
    private GameObject touchingBox = null;
    private bool holdingABox = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(ogre.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        rigid = this.GetComponent<Rigidbody2D>();
        touchingBox.transform.parent = null;
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
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            grabClosest();
        }
        else {
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                touchingBox.GetComponent<Rigidbody2D>().velocity += Vector2.up * Time.deltaTime * movementSpeed;
            }
        }
    }

    public void grabClosest()
    {
        if(touchingBox != null)
        {
            if(!holdingABox)
            {
                holdingABox = true;
                touchingBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                holdingABox = false;
                touchingBox.transform.parent = null;
                touchingBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }
}
