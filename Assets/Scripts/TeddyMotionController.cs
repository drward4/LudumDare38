using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyMotionController : MonoBehaviour
{
    public float Sensitivity;
    public float MaxSpeed;
    public float Force;
    public float MaxAirSpeed;
    public float InAirMoveSpeed;
    public float JumpStrength;
    public float CameraFollowDistance;
    public GameObject ControllerObject;
    public GameObject LookRotationObject;
    Animator Animator;

    public Rigidbody RigidBody;
    private Quaternion StartOrientation;
    private Vector3 LookObjectStartPositionOffset;
    private float RotationX, RotationY;
    public bool IsGrounded;
    public int CollisionCount;


    void Start()
    {
        this.StartOrientation = this.LookRotationObject.transform.localRotation;
        this.LookObjectStartPositionOffset = this.LookRotationObject.transform.position - this.transform.position;
        this.Animator = this.GetComponent<Animator>();
    }


    private void ProcessInput()
    {
        // Move Forward
        // Can move partially if in the air as long as not colliding with anything
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = this.transform.forward.normalized;

            if (this.IsGrounded)
            {
                direction *= this.Force;

                if (this.RigidBody.velocity.magnitude < this.MaxSpeed)
                    this.RigidBody.AddForce(direction, ForceMode.Impulse);
            }
            else if (this.CollisionCount == 0)
            {
                Vector3 velocity = this.RigidBody.velocity;
                velocity.y = 0f;
                if (velocity.magnitude < this.MaxAirSpeed)
                {
                    direction *= this.InAirMoveSpeed;
                    this.RigidBody.AddForce(direction);
                }

            }

            if (!this.Animator.GetBool("IsRunning") && this.IsGrounded)
            {
                this.Animator.SetBool("IsRunning", true);
            }
        }
        else
        {
            if (this.Animator.GetBool("IsRunning"))
            {
                this.Animator.SetBool("IsRunning", false);
            }
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && (this.IsGrounded || this.RigidBody.velocity.y == 0f))
        {
            this.RigidBody.AddForce(Vector3.up * this.JumpStrength);
            this.IsGrounded = false;

            if (this.Animator.GetBool("IsRunning"))
            {
                this.Animator.SetBool("IsRunning", false);
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        ++CollisionCount;

        if (collision.collider.gameObject.layer == 9 || collision.collider.gameObject.layer == 12) // Can walk on Toys or Ground
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.1f)
                {
                    this.IsGrounded = true;
                    break;
                }
            }
        }
    }


    void OnCollisionExit(Collision collision)
    {
        --this.CollisionCount;
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

        this.ProcessInput();
    }


    void FixedUpdate()
    {
        this.ControllerObject.transform.position = this.transform.position;
        this.LookRotationObject.transform.position = this.transform.position + this.LookObjectStartPositionOffset;
        
        Camera.main.transform.position = this.LookRotationObject.transform.position - this.LookRotationObject.transform.forward * this.CameraFollowDistance;
        Camera.main.transform.LookAt(this.LookRotationObject.transform);
    }
}
