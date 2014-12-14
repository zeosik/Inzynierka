using UnityEngine;
using System.Collections;

public class GlobalSettings : MonoBehaviour {

	static Vector3 mainMenuUserPosition;

	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName.ToLower().Equals("menu"))
		{
			if(mainMenuUserPosition.Equals(Vector3.zero))
			{
				mainMenuUserPosition = this.transform.position;
			}
			else
			{
				this.transform.position = mainMenuUserPosition;
			}
		}
	}

	static public void setMenuUserPosition()
	{
		OVRCameraController camera = FindObjectOfType<OVRCameraController>();
		mainMenuUserPosition = camera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
