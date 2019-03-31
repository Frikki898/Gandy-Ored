using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameTrigger : MonoBehaviour
{
    private bool startCounting = false;
    private float timePassed = 0;
    bool ogreEnter;
    bool gnomeEnter;
    public GameObject endSceen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startCounting)
        {
            timePassed += Time.deltaTime;
        }

        if(timePassed > 4)
        {
            endSceen.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "gnome")
        {
            gnomeEnter = true;
        }

        if (collision.gameObject.name == "ogre")
        {
            ogreEnter = true;
        }

        if (gnomeEnter && ogreEnter)
        {
            startCounting = true;
        }
        
    }
}
