using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public KeyCode resetKey;
	public KeyCode hardResetKey;

	private int level = 0;

	[HideInInspector]public Vector3 gnomeResetPositions;
	[HideInInspector]public Vector3 ogreResetPositions;
	[HideInInspector]public Vector3 cameraPosition;
	private BoxScript[] resetableObjects;

	private GameObject ogre;
	private GameObject gnome;
	private GameObject camera;

	private Vector3[] objectsPosition;
	// Start is called before the first frame update
	void Start()
    {
		ogre = GameObject.Find("ogre");
		gnome = GameObject.Find("gnome");
		camera = GameObject.Find("Main Camera");

		ogreResetPositions = ogre.transform.position;
		gnomeResetPositions = gnome.transform.position;
		cameraPosition = camera.transform.position;

		resetableObjects = FindObjectsOfType<BoxScript>();
        objectsPosition = new Vector3[resetableObjects.Length];

		int i = 0;
		foreach (BoxScript obj in resetableObjects)
		{
			objectsPosition[i] = obj.transform.position;
			i++;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(resetKey))
		{
			resetLevel();
		}
		if (Input.GetKeyDown(hardResetKey))
		{
			hardReset();
		}
	}

	void resetLevel()
	{
		

		
		gnome.transform.position = gnomeResetPositions;
		ogre.transform.position = ogreResetPositions;
		camera.transform.position = cameraPosition;

		GnomeController gc = gnome.GetComponent<GnomeController>();
		gc.grabClosest(true);
		OgreController oc = ogre.GetComponent<OgreController>();
		oc.grabClosest();

        Debug.Log(resetableObjects.Length);

		int i = 0;
		foreach (BoxScript obj in resetableObjects)
		{
			obj.transform.position = objectsPosition[i];
			i++;
		}
	}

	void hardReset()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
}
