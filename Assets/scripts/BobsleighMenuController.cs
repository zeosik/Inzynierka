using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BobsleighMenuController : MonoBehaviour {

	public Button[] menuItems;

	bool menuActive = false;
	int selectedIndex = 0;
	bool gamePaused = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (menuActive) 
		{
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				selectedIndex = (selectedIndex == (menuItems.Length - 1) ? selectedIndex = 0 : ++selectedIndex);
				menuItems[selectedIndex].Select();
			} else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				selectedIndex = (selectedIndex == 0 ? menuItems.Length - 1 : --selectedIndex);
				menuItems[selectedIndex].Select();
			} else if(Input.GetKeyDown(KeyCode.Return))
			{
				executeAction();
			}
		}
	}
	void executeAction()
	{
		if (menuItems [this.selectedIndex].name.ToLower ().Contains ("pause") ||
		    menuItems [this.selectedIndex].name.ToLower ().Contains ("resume"))
		{
			gamePaused = !gamePaused;
			if(gamePaused)
				BobsleighController.pause();
			else
				BobsleighController.resume();
		}
	}
}
