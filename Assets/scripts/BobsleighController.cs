using UnityEngine;
using System.Collections;

public class BobsleighController : MonoBehaviour {
	
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;

	static BobsleighController bobsleigh;
	Vector3 velocity;
	Vector3 angularVelocity;
	int steer_max = 10;
	float steer = 0.0f;

	void Start()
	{
		bobsleigh = gameObject.transform.GetComponent<BobsleighController>();

		OVRDevice.ResetOrientation();
		//print (this.rigidbody.centerOfMass);
		this.rigidbody.centerOfMass = new Vector3 (0, 0, -0.95f);
		//print (this.rigidbody.centerOfMass);
	}
	void Update()
	{
		this.rigidbody.AddForce(this.transform.up * 100.0f, ForceMode.Force);
		if(OVRDevice.IsHMDPresent())
		{
			Vector3 p = new Vector3();
			Quaternion Ovrinput=Quaternion.identity;
			OVRDevice.GetCameraPositionOrientation (ref p, ref Ovrinput, 0);
			float ovrZAngle = Ovrinput.eulerAngles.z;
			if(ovrZAngle < 10.0f || ovrZAngle > 350.0f)
			{
				steer = 0.0f;
			}
			else if(ovrZAngle < 180.0f)
			{
				if(ovrZAngle > 70.0f)
					steer = -1.0f;
				else
					steer = (ovrZAngle - 10.0f) / -60.0f;
			}
			else if(ovrZAngle > 180.0f)
			{
				if(ovrZAngle < 290.0f)
					steer = 1.0f;
				else
					steer = (350.0f - ovrZAngle) / 60.0f;
			}
		}
		else
		{
			steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		}

		WheelFL.steerAngle = steer_max * steer;
		WheelFR.steerAngle = steer_max * steer;
	}

	static public void pause()
	{
		bobsleigh.velocity = bobsleigh.rigidbody.velocity;
		bobsleigh.angularVelocity = bobsleigh.rigidbody.angularVelocity;
		bobsleigh.rigidbody.isKinematic = true;
	}

	static public void resume()
	{
		bobsleigh.rigidbody.isKinematic = false;
		bobsleigh.rigidbody.velocity = bobsleigh.velocity;
		bobsleigh.rigidbody.angularVelocity = bobsleigh.angularVelocity;
	}

	static public void gameWon()
	{
		//this.rigidbody.velocity = this.rigidbody.velocity * 2f;
		pause();
		//this.rigidbody.useGravity = false;
		//this.rigidbody.Sleep();
	}
}
