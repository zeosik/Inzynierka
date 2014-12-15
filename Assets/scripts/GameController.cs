using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private bool paused = false;
	private bool inMenu = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			Application.LoadLevel("menu");
//		}
		if(Input.GetKeyDown(KeyCode.P)) {
			pauseGame();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			enterMenu();
		}
	}
	void pauseGame()
	{
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("info"))
			{
				paused = !paused;
				item.enabled = paused;
				if(paused)
					BobsleighController.pause();
				else
					BobsleighController.resume();
			}
		}
	}
	void enterMenu()
	{
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("menu"))
			{
				inMenu = !inMenu;
				item.enabled = inMenu;
				pauseGame();	//Delete later on, when pausing is implemented as a new option in menu
			}
		}
	}
}
