using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
	public int selectLevel;
	public bool canExit = true;
	public float transactionSpeed;
	public float usingSystem = 1;
	public float newXpos = 0;

	public GameObject level1;
	public GameObject level2;
	public GameObject level3;

	public GameObject cam;

	private bool gnomeExit = false;
	private bool ogreExit = false;
	private bool exiting = false;

	private GameObject ogre = null;
	private GameObject gnome = null;

	public GameObject haltingCollider;
	public GameObject exitCollider;

	void Start()
	{
		if(usingSystem == 1)
		{
			level1.SetActive(true);
			level2.SetActive(false);
			level3.SetActive(false);
		}
		if (usingSystem == 2)
		{
			//haltingCollider = GameObject.Find("haltingCollider");
			//exitCollider = GameObject.Find("exitCollider");

			//dodo: set collider to progress to active
			exitCollider.SetActive(true);
			haltingCollider.SetActive(false);
		}
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (canExit)
		{
			if (collision.gameObject.name == "gnome")
			{
				Debug.Log("gnome can now exit the level");
				gnome = collision.gameObject;
				gnomeExit = true;
			}

			if (collision.gameObject.name == "ogre")
			{
				Debug.Log("ogre can now exit the level");
				ogre = collision.gameObject;
				ogreExit = true;
			}

			if (gnomeExit && ogreExit)
			{
				Debug.Log("We can switch scene");
				exiting = true;
				setNextLevel();
			}
		}
	}

	void Update()
	{

		if (Input.GetKey(KeyCode.R))
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}
		if (usingSystem == 1)
		{
			if (Input.GetKey(KeyCode.Alpha1))
			{
				level1.SetActive(true);
				level2.SetActive(false);
				level3.SetActive(false);
			}

			if (Input.GetKey(KeyCode.Alpha2))
			{
				level1.SetActive(false);
				level2.SetActive(true);
				level3.SetActive(false);
			}

			if (Input.GetKey(KeyCode.Alpha3))
			{
				level1.SetActive(false);
				level2.SetActive(false);
				level3.SetActive(true);
			}
		}

		else if (usingSystem == 2)
		{
			if (exiting)
			{
				//todo: set collider to progress to inactive
				//todo: set collider to backtrac to active
				haltingCollider.SetActive(true);
				exitCollider.SetActive(false);

				Vector3 newPosition = cam.transform.position;
				newPosition.x = newXpos;
				//cam.transform.position = newPosition;//Vector2.Lerp(cam.transform.position, newPosition, transactionSpeed * Time.deltaTime);
				cam.transform.position = Vector3.Lerp(cam.transform.position, newPosition, transactionSpeed * Time.deltaTime);
				
			}
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject == gnome)
		{
			gnome = null;
			gnomeExit = false;
		}
		if (collision.gameObject == ogre)
		{
			ogre = null;
			ogreExit = false;
		}
	}

	void setNextLevel()
	{
		if (usingSystem == 1)
		{
			if (selectLevel == 1)
			{
				level1.SetActive(false);
				level2.SetActive(true);
				level3.SetActive(false);

		}
		else if (selectLevel == 2)
		{
			level1.SetActive(false);
			level2.SetActive(false);
			level3.SetActive(true);

			}
		}
		
	}
}
