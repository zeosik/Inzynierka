using UnityEngine;
using System.Collections;

public class BobsleighController : MonoBehaviour {
	
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;

	private Vector3 startingPosition;
	private Quaternion startingRotation;

	static BobsleighController bobsleigh;
	Vector3 velocity;
	Vector3 angularVelocity;
	int steer_max = 10;
	float steer = 0.0f;

	void Start()
	{
		startingPosition = this.rigidbody.position;
		startingRotation = this.rigidbody.rotation;

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
		if(Network.isServer)
			GameObject.Find("bobslej").networkView.RPC("togglePauseGame", RPCMode.All, null);
		else if(!Network.isClient)
			pause();
	}

	static public void restartGame()
	{
		bobsleigh.rigidbody.position = bobsleigh.startingPosition;
		bobsleigh.rigidbody.rotation = bobsleigh.startingRotation;
		bobsleigh.rigidbody.velocity = Vector3.zero;
		bobsleigh.rigidbody.angularVelocity = Vector3.zero;
		GameController.togglePauseGame();
	}
}
