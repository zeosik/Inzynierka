using UnityEngine;
using System.Collections;

public class BobsleighOutOfTrackTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.name.ToLower().Contains("checkpoint"))
			BobsleighController.checkpoint ();
		else
			BobsleighController.gameWon();
	}
}
