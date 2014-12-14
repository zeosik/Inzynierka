using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	float moveSpeed = 0.7f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W))
			this.transform.position = new Vector3 (this.transform.position.x, 
			                                       this.transform.position.y, 
			                                       this.transform.position.z + moveSpeed);
		if (Input.GetKey (KeyCode.A))
			this.transform.position = new Vector3 (this.transform.position.x - moveSpeed, 
			                                       this.transform.position.y, 
			                                       this.transform.position.z);
		if (Input.GetKey (KeyCode.S))
			this.transform.position = new Vector3 (this.transform.position.x, 
			                                       this.transform.position.y, 
			                                       this.transform.position.z - moveSpeed);
		if (Input.GetKey (KeyCode.D))
			this.transform.position = new Vector3 (this.transform.position.x + moveSpeed, 
			                                       this.transform.position.y, 
			                                       this.transform.position.z);
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel("menu");
	}
}
