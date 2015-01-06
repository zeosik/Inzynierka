using UnityEngine;
using System.Collections;

public class BobsleighOnTrackTrigger : MonoBehaviour {

	private int count = 0;

	void OnTriggerExit(Collider collider)
	{
		--count;
		if(count == 0)
		{
			BobsleighController.gameWon();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		++count;
	}
}
