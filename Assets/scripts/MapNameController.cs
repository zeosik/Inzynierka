using UnityEngine;
using System.Collections;

public class MapNameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(FindObjectOfType<OVRPlayerController>().transform.position);
		this.transform.Rotate(Vector3.up, 180f);
	}
}
