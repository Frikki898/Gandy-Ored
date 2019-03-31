using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
	public bool canExit = true;
	public float transactionSpeed;
	//public float usingSystem = 1;
	public float newXpos = 0;

	//public GameObject level1;
	//public GameObject level2;
	//public GameObject level3;

	public GameObject cam;

	private bool gnomeExit = false;
	private bool ogreExit = false;
	private bool exiting = false;
	private bool canMoveCamera = true;

	private GameObject ogre = null;
	private GameObject gnome = null;
	private GameManager gameManager;

	public GameObject haltingCollider;
	public GameObject exitCollider;

	private Vector3 newCamPosition;

	void Start()
	{
		//if(usingSystem == 1)
		//{
		//	level1.SetActive(true);
		//	level2.SetActive(false);
		//	level3.SetActive(false);
		//}
		//if (usingSystem == 2)
		//{
			//haltingCollider = GameObject.Find("haltingCollider");
			//exitCollider = GameObject.Find("exitCollider");
			
		exitCollider.SetActive(true);
		haltingCollider.SetActive(false);
		//}
		gameManager = FindObjectOfType<GameManager>();

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
			}
		}
	}

	void Update()
	{

		//if (Input.GetKey(KeyCode.R))
		//{
		//	Scene scene = SceneManager.GetActiveScene();
		//	SceneManager.LoadScene(scene.name);
		//}
		//if (usingSystem == 1)
		//{
		//	if (Input.GetKey(KeyCode.Alpha1))
		//	{
		//		level1.SetActive(true);
		//		level2.SetActive(false);
		//		level3.SetActive(false);
		//	}

		//	if (Input.GetKey(KeyCode.Alpha2))
		//	{
		//		level1.SetActive(false);
		//		level2.SetActive(true);
		//		level3.SetActive(false);
		//	}

		//	if (Input.GetKey(KeyCode.Alpha3))
		//	{
		//		level1.SetActive(false);
		//		level2.SetActive(false);
		//		level3.SetActive(true);
		//	}
		//}

		//else if (usingSystem == 2)
		//{
		if (exiting)
		{

			haltingCollider.SetActive(true);
			exitCollider.SetActive(false);

			newCamPosition = cam.transform.position;
			newCamPosition.x = newXpos;

			if (canMoveCamera)
				cam.transform.position = Vector3.Lerp(cam.transform.position, newCamPosition, transactionSpeed * Time.deltaTime);

			if (newCamPosition.x - cam.transform.position.x < 0.08)
			{
				if(canMoveCamera)
					setNextLevel();

				canMoveCamera = false;
				//Debug.Log("shutting down");
				//GetComponent<LevelExit>().enabled = false;
			}
		}
        if(!exiting)
        {

        }
		//}
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
		gameManager.cameraPosition = newCamPosition;
		gameManager.ogreResetPositions = GameObject.Find("ogre").transform.position;
		gameManager.gnomeResetPositions = GameObject.Find("gnome").transform.position;

		//if (usingSystem == 1)
		//{
		//	if (selectLevel == 1)
		//	{
		//		level1.SetActive(false);
		//		level2.SetActive(true);
		//		level3.SetActive(false);

		//}
		//else if (selectLevel == 2)
		//{
		//	level1.SetActive(false);
		//	level2.SetActive(false);
		//	level3.SetActive(true);

		//	}
		//}

	}
}
