using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BobsleighMapChooser : MonoBehaviour {

	public string levelName;
	public Slider loadingBar;

	private AsyncOperation async;

	void OnTriggerEnter(Collider collider) 
	{
		Canvas[] list = FindObjectsOfType<Canvas>();
		foreach(Canvas item in list)
		{
			if(item.tag.Equals("info"))
				item.enabled = true;
		}
	}

	void OnTriggerStay(Collider collider)
	{
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			StartCoroutine(LoadLevel());
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
				item.enabled = false;
		}
	}
}
