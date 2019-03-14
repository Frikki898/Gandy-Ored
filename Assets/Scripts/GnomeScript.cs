using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeScript : MonoBehaviour
{
    public GameObject ogre;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(ogre.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
