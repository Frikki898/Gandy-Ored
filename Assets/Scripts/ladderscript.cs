using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
