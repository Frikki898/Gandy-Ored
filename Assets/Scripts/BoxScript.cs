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
}
