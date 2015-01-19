using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BobsleighMenuController : MonoBehaviour {

	public Button[] menuItems;
	public string mapChoosingSceneName;
	public AudioSource click;
	static BobsleighMenuController menu;

	bool menuActive = false;
	bool networkMenuActive = false;
	int selectedIndex = 0;
	bool gamePaused = false;
	int pauseResumeButtonIndex;
	// Use this for initialization
	void Start () {
		menu = gameObject.transform.GetComponent<BobsleighMenuController>();
		foreach(Button btn in menuItems)
		{
			if(btn.name.ToLower().Contains("pause") ||
			   btn.name.ToLower().Contains("resume"))
			{
				break;
			}
			++pauseResumeButtonIndex;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (menuActive && !networkMenuActive) 
		{
			if(Network.isClient)
			{
				menuItems[pauseResumeButtonIndex].interactable = false;
				if(selectedIndex == pauseResumeButtonIndex)
					selectNext();
			}
			else
			{
				menuItems[pauseResumeButtonIndex].interactable = true;
			}
			gamePaused = GameController.isPaused();
			menuItems[pauseResumeButtonIndex].GetComponentInChildren<Text>().text = (gamePaused ? "resume " : "pause ") + "game";
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				click.Play();
				selectNext();
			} 
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				click.Play();
				selectPrev();
			} 
			else if(Input.GetKeyDown(KeyCode.Return))
			{
				click.Play();
				executeAction();
			}
			else if(Input.GetKeyDown(KeyCode.Escape))
			{
				click.Play();
			}
		}
	}
	void selectNext()
	{
		selectedIndex = (selectedIndex == (menuItems.Length - 1) ? selectedIndex = 0 : ++selectedIndex);
		if(menuItems[selectedIndex].interactable)
			menuItems[selectedIndex].Select();
		else
			selectNext();
	}
	void selectPrev()
	{
		selectedIndex = (selectedIndex == 0 ? menuItems.Length - 1 : --selectedIndex);
		if(menuItems[selectedIndex].interactable)
			menuItems[selectedIndex].Select();
		else
			selectPrev();
	}
	void executeAction()
	{
		if (menuItems [selectedIndex].name.ToLower ().Contains ("pause") ||
		    menuItems [selectedIndex].name.ToLower ().Contains ("resume"))
		{
			gamePaused = !gamePaused;
			if(Network.isServer)
				GameObject.Find("bobslej").networkView.RPC("togglePauseGame", RPCMode.All, null);
			else if(!Network.isClient)
				togglePauseGame();
			else
			{
				menuItems[selectedIndex].interactable = false;
				selectNext();
			}
		}
		else if (menuItems[selectedIndex].name.ToLower().Contains("network"))
		{
			GameController.toggleNetworkMenu();
		}
		else if (menuItems[selectedIndex].name.ToLower().Contains("restart"))
		{
			if(Network.isServer)
				GameObject.Find("bobslej").networkView.RPC("restartGame", RPCMode.All, null);
			else if(!Network.isClient)
				BobsleighController.restartGame();
		}
		else if (menuItems[selectedIndex].name.ToLower().Contains("exit"))
		{
			Application.LoadLevel(mapChoosingSceneName);
		}
	}
	void togglePauseGame()
	{
		GameController.togglePauseGame();
	}
	public static void toggleMenu()
	{
		menu.menuActive = !menu.menuActive;
		if(menu.menuActive)
		{
			menu.selectedIndex = 0;
			menu.menuItems[menu.selectedIndex].Select();
		}
	}
	public static void toggleNetworkMenu()
	{
		menu.networkMenuActive = !menu.networkMenuActive;
		GameNetworkManager.toggleNetworkMenu();
	}
}
