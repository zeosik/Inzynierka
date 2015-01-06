using UnityEngine;
using System.Collections;

public class BobsleighOutOfTrackTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		BobsleighController.gameWon();
	}
}
