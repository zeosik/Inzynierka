using UnityEngine;
using System.Collections;

public class BobsleighController : MonoBehaviour {
	
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;
	
	int steer_max = 10;
	private float steer = 0.0f;
	void Start()
	{
		OVRDevice.ResetOrientation();
	}
	void Update()
	{
		if(OVRDevice.IsHMDPresent())
		{
			Vector3 p = new Vector3();
			Quaternion Ovrinput=Quaternion.identity;
			OVRDevice.GetCameraPositionOrientation (ref p, ref Ovrinput, 0);
			float ovrZAngle = Ovrinput.eulerAngles.z;
			if(ovrZAngle < 5.0f || ovrZAngle > 355.0f)
			{
				steer = 0.0f;
			}
			else if(ovrZAngle < 180.0f)
			{
				if(ovrZAngle > 60.0f)
					steer = -1.0f;
				else
					steer = ovrZAngle / -60.0f;
			}
			else if(ovrZAngle > 180.0f)
			{
				if(ovrZAngle < 300.0f)
					steer = 1.0f;
				else
					steer = (360.0f - ovrZAngle) / 60.0f;
			}
		}
		else
		{
			steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		}

		WheelFL.steerAngle = steer_max * steer;
		WheelFR.steerAngle = steer_max * steer;
	}
}
