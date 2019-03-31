﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBubble : MonoBehaviour
{
	public enum setText
	{
		help,
		noStr,
		noMag
	}
	public enum setPlayer
	{
		ogre,
		gnome
	}

	public setPlayer setPlayerTo;

	private string help = "Hmm, maybe if I could get help with that!";
	private string noStrength = "Ugh.. This is bound with some kind of magic";
	private string noMagic = "No magic is powerful enought to lift this";

	private string text = "";

	private Vector3 pos;

	private float time = 0;
	public float setActiveTimeSec;

	// Start is called before the first frame update
	void Start()
    {
		this.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		Debug.Log("should be moving with " + setPlayerTo);

		if (time < setActiveTimeSec)
		{
			setposition();
			time += Time.deltaTime;
		}
		else
		{
			time = 0;
			this.gameObject.SetActive(false);
		}
    }

	void setposition()
	{
		if (setPlayerTo == setPlayer.ogre)
		{
			pos = GameObject.Find("ogre").transform.position;
			pos.x += 2;
			pos.y += 5;
			pos.z -= 10;
		}
		else if(setPlayerTo == setPlayer.gnome)
		{
			pos = GameObject.Find("gnome").transform.position;
			pos.x += 1.8f;
			pos.y += 3.6f;
			pos.z -= 10;
		}

		this.transform.position = pos;
	}

	public void setTextBubble(setText set)
	{
		setposition();
		if (set == setText.help)
			this.text = help;
		else if (set == setText.noMag)
			this.text = noMagic;
		else if (set == setText.noStr)
			this.text = noStrength;

		TextMeshPro textMesh = this.transform.Find("text").GetComponent<TextMeshPro>();
		textMesh.text = this.text;
		this.gameObject.SetActive(true);
	}
}
