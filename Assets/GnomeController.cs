using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public GameObject ogre;
    public float sideWaysSpeed;
    private Rigidbody2D rigid;

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
            this.transform.position += Vector3.left * Time.deltaTime * sideWaysSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * Time.deltaTime * sideWaysSpeed;
        }
    }
}
