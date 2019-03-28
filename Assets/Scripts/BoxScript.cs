using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxScript : MonoBehaviour
{
    public enum BoxTypes
    {
        wood,
        steel,
        magic,
        led
    };

    public Rigidbody2D rb;

    public BoxTypes BoxType;

    public bool beingHeld = false;

    public GameObject gnomeSelection;
    public GameObject ogreSelection;
	public bool ogreHolding = false;
	public bool gnomeHolding = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		//this.transform.localPosition = new Vector3(1, 1, 1);
		//rb.velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        //rb.velocity = Vector3.zero;
    }

    void OnCollisionStay2D()
    {
        if (!beingHeld)
        {
            rb.mass = 10;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }

	void OnCollisionExit2D(Collision2D collision)
	{
		if (gnomeHolding)
		{
			if (collision.gameObject.tag.Equals("Ground"))
			{
				Debug.Log("Box Falling");
				rb.GetComponent<Rigidbody2D>().gravityScale = 25;
			}
			
		}
	}
}
