using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    public float Sensitivity;
    public float MovementSpeed;
    public float CameraFollowDistance;

    private Rigidbody RigidBody;
    private Quaternion StartOrientation;
    private float RotationX, RotationY;

	void Start ()
    {
        this.StartOrientation = this.transform.localRotation;
        this.RigidBody = this.GetComponent<Rigidbody>();
	}
	

	void Update ()
    {
        this.RotationX += Input.GetAxis("Mouse X") * Sensitivity;
        this.RotationY += Input.GetAxis("Mouse Y") * Sensitivity;

        Quaternion x = Quaternion.AngleAxis(this.RotationX, Vector3.up);
        Quaternion y = Quaternion.AngleAxis(this.RotationY, Vector3.left);

        this.transform.localRotation = this.StartOrientation * x * y;

        this.RigidBody.velocity = Vector3.zero;

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

        Camera.main.transform.position = this.transform.position - this.transform.forward * this.CameraFollowDistance;
        Camera.main.transform.LookAt(this.transform);
    }
}
