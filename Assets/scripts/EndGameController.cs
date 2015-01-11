using UnityEngine;
using System.Collections;

public class EndGameController : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		BobsleighController.gameWon();
		BobsleighController.checkpoint ();
	}
}
