using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BobsleighMenuController : MonoBehaviour {

	public Button[] menuItems;
	public string mapChoosingSceneName;
	static BobsleighMenuController menu;

	bool menuActive = false;
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
		if (menuActive) 
		{
			gamePaused = GameController.isPaused();
			menuItems[pauseResumeButtonIndex].GetComponentInChildren<Text>().text = (gamePaused ? "resume " : "pause ") + "game";
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				selectedIndex = (selectedIndex == (menuItems.Length - 1) ? selectedIndex = 0 : ++selectedIndex);
				menuItems[selectedIndex].Select();
				//menuItems[selectedIndex].
			} 
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				selectedIndex = (selectedIndex == 0 ? menuItems.Length - 1 : --selectedIndex);
				menuItems[selectedIndex].Select();
			} 
			else if(Input.GetKeyDown(KeyCode.Return))
			{
				executeAction();
			}
		}
	}
	void executeAction()
	{
		if (menuItems [selectedIndex].name.ToLower ().Contains ("pause") ||
		    menuItems [selectedIndex].name.ToLower ().Contains ("resume"))
		{
			gamePaused = !gamePaused;
			GameController.togglePauseGame();
		}
		else if (menuItems[selectedIndex].name.ToLower().Contains("exit"))
		{
			Application.LoadLevel(mapChoosingSceneName);
		}
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
}
