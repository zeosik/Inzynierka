using UnityEngine;
using System.Collections;

public class BobsleighController : MonoBehaviour {
	
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;
	
	int steer_max = 10;
	private float steer = 0.0f;
	
	void Update()
	{
		steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		WheelFL.steerAngle = steer_max * steer;
		WheelFR.steerAngle = steer_max * steer;
	}
}
