using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuScript : MonoBehaviour
{
    public bool firstTime = false;
    public TextMeshProUGUI textToChange;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(firstTime)
        {
            textToChange.text = "Continue";
        }
    }

    public void use()
    {
        firstTime = true;
    }
}
