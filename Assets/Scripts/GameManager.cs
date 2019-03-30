using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public KeyCode resetKey;
	public KeyCode hardResetKey;

	public int level = 0;

	
	public Vector3[] gnomeResetPositions = new Vector3[1];
	public Vector3[] ogreResetPositions = new Vector3[1];
	public GameObject[] resetableObjects;
	public Vector3 initCameraPosition;

	private Vector3[] objectsPosition;
	// Start is called before the first frame update
	void Start()
    {
		objectsPosition = new Vector3[resetableObjects.Length];

		int i = 0;
		foreach (GameObject obj in resetableObjects)
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
		GameObject ogre = GameObject.Find("ogre");
		GameObject gnome = GameObject.Find("gnome");
		GameObject camera = GameObject.Find("Main Camera");

		
		gnome.transform.position = gnomeResetPositions[level];
		ogre.transform.position = ogreResetPositions[level];
		camera.transform.position = initCameraPosition;

		GnomeController gc = gnome.GetComponent<GnomeController>();
		gc.grabClosest(true);
		OgreController oc = ogre.GetComponent<OgreController>();
		oc.grabClosest();

		int i = 0;
		foreach (GameObject obj in resetableObjects)
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
