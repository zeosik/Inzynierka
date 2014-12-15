using UnityEngine;
using System.Collections;

public class EndGameController : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		collider.SendMessage("gameWon");
	}
}
