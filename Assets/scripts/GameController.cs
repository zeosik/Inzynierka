using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private bool paused = false;
	private bool inMenu = false;
	static GameController gameController;
	// Use this for initialization
	void Start () {
		gameController = gameObject.transform.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			Application.LoadLevel("menu");
//		}
		if(Input.GetKeyDown(KeyCode.P)) {
			togglePauseGame();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			toggleMenu();
		}
	}
	public static void togglePauseGame()
	{
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("info"))
			{
				gameController.paused = !gameController.paused;
				item.enabled = gameController.paused;
				if(gameController.paused)
					BobsleighController.pause();
				else
					BobsleighController.resume();
			}
		}
	}
	public static bool isPaused()
	{
		return gameController.paused;
	}
	public static void toggleMenu()
	{
		BobsleighMenuController.toggleMenu();
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("menu"))
			{
				gameController.inMenu = !gameController.inMenu;
				item.enabled = gameController.inMenu;
				//pauseGame();	//Delete later on, when pausing is implemented as a new option in menu
			}
		}
	}
}
