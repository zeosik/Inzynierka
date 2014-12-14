using UnityEngine;
using System.Collections;

public class GameMenuCollisionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {

		RaycastHit hit;

		//print ("this: " + this.transform.position + " " + this.transform.forward);

		Debug.DrawRay(this.transform.position, this.transform.forward);
		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit)) {

			//print("Hit something " + hit.distance + " " + hit.collider.name);
			if(hit.collider.tag.Equals("menu")) {
				hit.collider.SendMessage("menuControl");
				//print ("znalazlem");
				//if(Input.GetMouseButtonDown(0)) {
				//	hit.collider.SendMessage("TestFunc");
				//}
			}
		}
	}
}
