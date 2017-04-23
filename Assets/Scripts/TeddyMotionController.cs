using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyMotionController : MonoBehaviour
{
    public float Sensitivity;
    public float MovementSpeed;
    public float CameraFollowDistance;
    public GameObject LookRotationObject;
    Animator Animator;

    private Rigidbody RigidBody;
    private Quaternion StartOrientation;
    private Vector3 LookObjectStartPositionOffset;
    private float RotationX, RotationY;

    void Start()
    {
        this.StartOrientation = this.LookRotationObject.transform.localRotation;
        this.LookObjectStartPositionOffset = this.LookRotationObject.transform.position - this.transform.position;
        this.RigidBody = this.GetComponent<Rigidbody>();
        this.Animator = this.GetComponent<Animator>();
    }


    void Update()
    {
        this.RotationX += Input.GetAxis("Mouse X") * Sensitivity;
        this.RotationY += Input.GetAxis("Mouse Y") * Sensitivity;

        Quaternion x = Quaternion.AngleAxis(this.RotationX, Vector3.up);
        Quaternion y = Quaternion.AngleAxis(this.RotationY, Vector3.left);

        this.transform.localRotation = this.StartOrientation * x;

        // Important that this rotation is applied after the parent object rotation
        this.LookRotationObject.transform.localRotation = this.StartOrientation * x * y;

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = this.transform.forward;
            direction = direction.normalized * this.MovementSpeed;
            //direction.y = 0f;

            // Don't mess with gravity
            this.RigidBody.velocity = new Vector3(direction.x, this.RigidBody.velocity.y, direction.z);

            if (!this.Animator.GetBool("IsRunning"))
            {
                this.Animator.SetBool("IsRunning", true);
            }
        }
        else
        {
            //this.RigidBody.velocity = new Vector3(0f, this.RigidBody.velocity.y, 0f);

            if (this.Animator.GetBool("IsRunning"))
            {
                this.Animator.SetBool("IsRunning", false);
            }
        }

        //Debug.Log(this.RigidBody.velocity);

        //if (Input.GetKey(KeyCode.S))
        //{
        //    this.RigidBody.velocity -= this.transform.forward.normalized * this.MovementSpeed;
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    this.RigidBody.velocity += this.transform.right.normalized * this.MovementSpeed;
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    this.RigidBody.velocity -= this.transform.right.normalized * this.MovementSpeed;
        //}

        Camera.main.transform.position = this.LookRotationObject.transform.position - this.LookRotationObject.transform.forward * this.CameraFollowDistance;
        Camera.main.transform.LookAt(this.LookRotationObject.transform);
    }


    void FixedUpdate()
    {
        this.LookRotationObject.transform.position = this.transform.position + this.LookObjectStartPositionOffset;
    }
}
