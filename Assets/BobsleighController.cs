using UnityEngine;
using System.Collections;

public class BobsleighController : MonoBehaviour {
	
	Vector3 v;
	Vector3 lastV;

	// Use this for initialization
	void Start () {
		v = this.rigidbody.velocity;
		lastV = this.rigidbody.velocity;
	}
	// Update is called once per frame
	void Update () {
		Vector3 vDiff = this.rigidbody.velocity - lastV;
		Vector3 d = this.transform.rotation.eulerAngles;

		if (d.y <= 180f) {						//0->180 (right)
			d.y = d.y / 90f;
		} else {								//181->360 (left)
			d.y = (360f - d.y) / -90f;
		}

		v.y += vDiff.y;
		v.x += vDiff.x;

		print ("oryginalnie: " + this.rigidbody.velocity + " zmiana: " + vDiff.z + " teraz: " + v + " " + d + " " + this.transform.rotation.eulerAngles);

		this.transform.Rotate(new Vector3(this.transform.rotation.x, Input.GetAxis("Horizontal"), this.transform.rotation.z));

		if (d.y > 1f) {
			v.z -= vDiff.z;
			this.rigidbody.velocity = new Vector3 (v.z * (2f - d.y), v.y, v.z * (1f - d.y));
		} else if (d.y < -1f) {
			v.z -= vDiff.z;
			this.rigidbody.velocity = new Vector3 (v.z * (-2f - d.y), v.y, v.z * (1f + d.y));
		} else {
			v.z += vDiff.z;
			this.rigidbody.velocity = new Vector3 (v.z * d.y, v.y, v.z * (1f - Mathf.Abs(d.y)));
		}

		lastV = this.rigidbody.velocity;
	}
}
