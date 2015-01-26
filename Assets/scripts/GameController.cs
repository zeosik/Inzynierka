using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	private bool paused = false;
	private bool inMenu = false;
	private bool inNetworkMenu = false;

	private static float popupTimer = 0f;
	private static Canvas pauseInfoCanvas;
	private static Canvas popupCanvas;

	static GameController gameController;
	// Use this for initialization
	void Start () {
		gameController = gameObject.transform.GetComponent<GameController>();
		foreach(Canvas item in FindObjectsOfType<Canvas>())
		{
			if(item.tag.Equals("info"))
			{
				pauseInfoCanvas = item;
			}
			else if(item.tag.Equals("popup"))
			{
				popupCanvas = item;
			}
		}
	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			Application.LoadLevel("menu");
//		}

		if (popupCanvas.enabled) 
		{
			if (popupTimer > 0) 
			{
				popupTimer -= Time.deltaTime;
			}
			else
			{
				popupCanvas.enabled = false;
			}
		}

		if(Input.GetKeyDown(KeyCode.P)) {
			//togglePauseGame();
			if(Network.isServer)
			{
				togglePauseGame();
				if(paused)
					//GameObject.Find("bobslej").networkView.RPC("togglePauseGame", RPCMode.All, null);
					GameObject.Find("bobslej").networkView.RPC("pauseGame", RPCMode.Others, null);
				else
					GameObject.Find("bobslej").networkView.RPC("unpauseGame", RPCMode.Others, null);
		    }
			else if(!Network.isClient)
				togglePauseGame();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(inNetworkMenu)
				toggleNetworkMenu();
			else
				toggleMenu();
		}
	}

	public static void newPopupInfo(string info)
	{
		popupTimer = 2f;
		popupCanvas.enabled = true;
		popupCanvas.GetComponentInChildren<Text>().text = info;
	}

	public static void togglePauseGame()
	{
		gameController.paused = !gameController.paused;
		pauseInfoCanvas.enabled = gameController.paused;
		if(gameController.paused)
			BobsleighController.pause();
		else
			BobsleighController.resume();

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
			}
		}
	}

	public static void toggleNetworkMenu()
	{
		BobsleighMenuController.toggleNetworkMenu();
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("networkMenu"))
			{
				gameController.inNetworkMenu = !gameController.inNetworkMenu;
				item.enabled = gameController.inNetworkMenu;
			}
		}
	}
}
