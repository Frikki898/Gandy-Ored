using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreController : MonoBehaviour
{
    public float sideWaysSpeed;
    public GameObject gnome;
    private Rigidbody2D rigid;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(gnome.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * Time.deltaTime * sideWaysSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * Time.deltaTime * sideWaysSpeed;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            rigid.AddForce(Vector2.up * jumpForce);
        }
    }
}
