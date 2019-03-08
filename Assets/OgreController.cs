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
    }

    void OnCollisionStay2D()
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
