﻿using System;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    public float Sensitivity;
    public float MovementSpeed;
    public float CameraFollowDistance;

    private Rigidbody RigidBody;
    private Quaternion StartOrientation;
    private float RotationX, RotationY;
    private Possessable CollidingPossessable;

	void Start ()
    {
        this.StartOrientation = this.transform.localRotation;
        this.RigidBody = this.GetComponent<Rigidbody>();
	}
	

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.RigidBody.velocity += this.transform.forward.normalized * this.MovementSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.RigidBody.velocity -= this.transform.forward.normalized * this.MovementSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.RigidBody.velocity += this.transform.right.normalized * this.MovementSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.RigidBody.velocity -= this.transform.right.normalized * this.MovementSpeed;
        }

        if (Input.GetKeyUp(KeyCode.Space) && this.CollidingPossessable !=null)
        {
            GameController.BeginPossesion(this.CollidingPossessable);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            this.CollidingPossessable = other.gameObject.GetComponent<Possessable>();
            GameController.ShowMessage("Press Space to Possess");
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            this.CollidingPossessable = null;
            GameController.HideMessage();
        }
    }


    void Update ()
    {
        this.RotationX += Input.GetAxis("Mouse X") * Sensitivity;
        this.RotationY += Input.GetAxis("Mouse Y") * Sensitivity;

        Quaternion x = Quaternion.AngleAxis(this.RotationX, Vector3.up);
        Quaternion y = Quaternion.AngleAxis(this.RotationY, Vector3.left);

        this.transform.localRotation = this.StartOrientation * x * y;

        this.RigidBody.velocity = Vector3.zero;

        this.ProcessInput();

        Camera.main.transform.position = this.transform.position - this.transform.forward * this.CameraFollowDistance;
        Camera.main.transform.LookAt(this.transform);
    }
}
