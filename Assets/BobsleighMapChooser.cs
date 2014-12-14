using UnityEngine;
using System.Collections;

public class BobsleighMapChooser : MonoBehaviour {

	public string levelName;

	void OnTriggerEnter(Collider collider) 
	{
		print ("press enter to load level: " + levelName);
	}

	void OnTriggerStay(Collider collider)
	{
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			print("enter");
			Application.LoadLevel (levelName);
		}
	}
}
