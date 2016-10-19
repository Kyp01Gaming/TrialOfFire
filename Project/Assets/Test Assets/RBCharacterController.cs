﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RBCharacterController : MonoBehaviour {

	Rigidbody rb = null;
	public Transform cameraTransform = null;
	public float movementSpeed = 10;
	public float rotateSpeed = 2;

	bool horizontalInput;
	bool verticalInput;

	Vector3 forward;

	public int spriteCount;
	public int maxSpriteCount;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) 
		{
			Vector3 dir = (cameraTransform.forward * Input.GetAxis ("Vertical")) + (cameraTransform.right * Input.GetAxis ("Horizontal"));
			transform.TransformDirection (dir);
			transform.rotation = Quaternion.LookRotation (dir);
			rb.velocity = dir * movementSpeed;
		}
	}

	/*
	void Update ()
	{
		horizontalInput = Input.GetAxis ("Horizontal") != 0 ? true : false;
		verticalInput = Input.GetAxis ("Vertical") != 0 ? true : false;

		if (verticalInput) 
		{
			Vector3 rot = transform.localEulerAngles;
			rot.y = cameraTransform.localEulerAngles.y;
			transform.localEulerAngles = rot;
			forward = rot;
		}

		if (horizontalInput) 
		{
			Vector3 rot = transform.localEulerAngles;
			rot.y = Input.GetAxis ("Horizontal") > 0 ? 90 : -90;// forward.y + (90 * Input.GetAxis ("Horizontal"));
			transform.localEulerAngles = rot;
		}

		if (horizontalInput || verticalInput) {
			rb.velocity = transform.forward * movementSpeed;
		} else {
			rb.velocity = Vector3.zero;
		}
	}
	*/
}