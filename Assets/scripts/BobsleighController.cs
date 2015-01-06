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
		this.rigidbody.centerOfMass = new Vector3 (0, 0, -0.95f);
	}
	void FixedUpdate()
	{
		this.rigidbody.AddForce(this.transform.up * 100.0f, ForceMode.Force);
		print(WheelFL.motorTorque + " " + WheelFL.rpm + " " + this.rigidbody.velocity.sqrMagnitude);
		if(this.rigidbody.velocity.sqrMagnitude < 1500f)
		{
			WheelFL.motorTorque = 10f;
			WheelFR.motorTorque = 10f;
		}
		else
		{
			WheelFL.motorTorque = 0;
			WheelFR.motorTorque = 0;
		}
		if(OVRDevice.IsHMDPresent())
		{
			Vector3 p = new Vector3();
			Quaternion Ovrinput=Quaternion.identity;
			OVRDevice.GetCameraPositionOrientation (ref p, ref Ovrinput, 0);

			if(Network.isClient)
			{
				GameObject.Find ("Player2").transform.localEulerAngles = Ovrinput.eulerAngles;//new Vector3(Ovrinput.eulerAngles.z, Ovrinput.eulerAngles.x, Ovrinput.eulerAngles.y);
			}
			else
			{
				//GameObject.Find ("Player1").transform.localEulerAngles = new Vector3(Ovrinput.eulerAngles.z, Ovrinput.eulerAngles.x, Ovrinput.eulerAngles.y);
				GameObject.Find ("Player1").transform.localEulerAngles = Ovrinput.eulerAngles;
			}
		}


		if (Network.isClient)
			return;
		else
			updateRotation ();
	}
	void updateRotation()
	{
		float headAngle = GameObject.Find ("Player1").transform.localRotation.eulerAngles.z;
		if(headAngle < 10.0f || headAngle > 350.0f)
		{
			steer = 0.0f;
		}
		else if(headAngle < 180.0f)
		{
			if(headAngle > 70.0f)
				steer = -1.0f;
			else
				steer = (headAngle - 10.0f) / -60.0f;
		}
		else if(headAngle > 180.0f)
		{
			if(headAngle < 290.0f)
				steer = 1.0f;
			else
				steer = (350.0f - headAngle) / 60.0f;
		}
		
		headAngle = GameObject.Find ("Player2").transform.localRotation.eulerAngles.z;
		if(headAngle < 10.0f || headAngle > 350.0f)
		{
			steer += 0.0f;
		}
		else if(headAngle < 180.0f)
		{
			if(headAngle > 70.0f)
				steer += (-1.0f)/3f;
			else
				steer += ((headAngle - 10.0f) / -60.0f)/3f;
		}
		else if(headAngle > 180.0f)
		{
			if(headAngle < 290.0f)
				steer += (1.0f)/3f;
			else
				steer += ((350.0f - headAngle) / 60.0f)/3f;
		}

		//TODO: delete for final version - steering with arrows shouldn't be allowed
		if(!OVRDevice.IsHMDPresent())
			steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);

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
		if(!GameController.isPaused())
			GameController.togglePauseGame();
	}

}
