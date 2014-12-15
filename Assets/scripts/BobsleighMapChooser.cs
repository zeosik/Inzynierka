using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BobsleighMapChooser : MonoBehaviour {

	public string levelName;
	public Slider loadingBar;

	string enterInfoText = "press ENTER to ";

	private AsyncOperation async;

	void OnTriggerEnter(Collider collider) 
	{
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("info"))
			{
				item.enabled = true;
				if(this.name.ToLower().Contains("map"))
					item.GetComponentInChildren<Text>().text = enterInfoText + "start";
				else if(this.name.ToLower().Contains("menu"))
					item.GetComponentInChildren<Text>().text = enterInfoText + "go back to menu";

				break;
			}
		}
	}

	void OnTriggerStay(Collider collider)
	{
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			if(this.name.ToLower().Contains("map"))
			   	StartCoroutine(LoadLevel());
		   	else if(this.name.ToLower().Contains("menu"))
	        	Application.LoadLevel("menu");
		}
	}

	IEnumerator LoadLevel()
	{
		async = Application.LoadLevelAsync(levelName);
		while(!async.isDone)
		{
			loadingBar.value = async.progress;
			yield return null;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("info"))
			{
				item.enabled = false;
				break;
			}
		}
	}
}
