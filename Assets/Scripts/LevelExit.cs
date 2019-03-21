using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
	public int selectLevel;
	public bool canExit = true;

	public GameObject cam1;

	private bool gnomeExit = false;
	private bool ogreExit = false;

	private GameObject ogre = null;
	private GameObject gnome = null;

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
				setNextLevel();
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
		if (selectLevel == 1)
		{
			Debug.Log("should relocate");
			gnome.transform.position = new Vector3(-16.85f, -14.73015f, -40.3f);
			ogre.transform.position = new Vector3(-25.21f, -14.71885f, -38.5f);

			cam1.transform.position = new Vector3(-5.989776f, -1.957116f, -45);
		}
		//switch (selectLevel)
		//{
		//	case 1:
		//		Debug.Log("should relocate");
		//		gnome.transform.position = new Vector3(-16.85f, -14.73015f, -40.3f);
		//		ogre.transform.position = new Vector3(-25.21f, -14.71885f, -38.5f);
		//		break;
		//	case 2:
		//		break;
		//	default:

		//		break;
		//}
	}
}
