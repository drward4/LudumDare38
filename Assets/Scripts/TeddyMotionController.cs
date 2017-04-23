using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyMotionController : MonoBehaviour
{
    public float Sensitivity;
    public float MaxSpeed;
    public float MaxAirSpeed;
    public float InAirMoveSpeed;
    public float JumpStrength;
    public float CameraFollowDistance;
    public GameObject LookRotationObject;
    Animator Animator;

    public Rigidbody RigidBody;
    private Quaternion StartOrientation;
    private Vector3 LookObjectStartPositionOffset;
    private float RotationX, RotationY;
    public bool IsGrounded;
    public int CollisionCount;
    public bool HasKey;
    public bool HasHammer;
    private bool InRangeOfKey;
    private bool InRangeOfHammer;
    private bool InRangeOfToyChest;
    private bool InRangeOfWindow;

    void Start()
    {
        this.StartOrientation = this.LookRotationObject.transform.localRotation;
        this.LookObjectStartPositionOffset = this.LookRotationObject.transform.position - this.transform.position;
        this.Animator = this.GetComponent<Animator>();
    }


    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (this.InRangeOfKey)
            {
                this.HasKey = true;
            }

            if (this.InRangeOfHammer)
            {
                this.HasHammer = true;
            }
            
            if (this.InRangeOfToyChest && this.HasKey) // Check during process input in case of picking up key while near toy chest
            {
                GameController.ShowMessage("Open Toy Chest - E");
            }

            if (this.InRangeOfWindow && this.HasHammer)  // Check during process input in case hammer somehow lands next to window
            {
                GameController.ShowMessage("Smash Window - E");
            }
        }

        // Move Forward
        // Can move partially if in the air as long as not colliding with anything
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = this.transform.forward.normalized;

            if (this.IsGrounded)
            {
                direction *= this.MaxSpeed;
                // Don't mess with gravity
                this.RigidBody.velocity = new Vector3(direction.x, this.RigidBody.velocity.y, direction.z);
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
                if (collision.contacts[0].normal == Vector3.up)
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



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)  // Key
        {
            this.InRangeOfKey = true;
            GameController.ShowMessage("Pick up - E");
        }
        else if (other.gameObject.layer == 14)  // Hammer
        {
            this.InRangeOfHammer = true;
            GameController.ShowMessage("Pick up - E");
        }
        else if (other.gameObject.layer == 15)  // Toy Chest
        {
            this.InRangeOfToyChest = true;
            if (this.HasKey)
            {
                GameController.ShowMessage("Open Toy Chest - E");
            }
        }
        else if (other.gameObject.layer == 16)  // Window
        {
            this.InRangeOfWindow = true;
            if (this.HasHammer)
            {
                GameController.ShowMessage("Smash Window - E");
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        GameController.HideMessage();

        if (other.gameObject.layer == 13)  // Key
        {
            this.InRangeOfKey = false;
        }
        else if (other.gameObject.layer == 14)  // Hammer
        {
            this.InRangeOfHammer = false;
        }
        else if (other.gameObject.layer == 15)  // Toy Chest
        {
            this.InRangeOfToyChest = false;
        }
        else if (other.gameObject.layer == 16)  // Window
        {
            this.InRangeOfWindow = false;
        }
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
        this.LookRotationObject.transform.position = this.transform.position + this.LookObjectStartPositionOffset;

        Camera.main.transform.position = this.LookRotationObject.transform.position - this.LookRotationObject.transform.forward * this.CameraFollowDistance;
        Camera.main.transform.LookAt(this.LookRotationObject.transform);

        //float magnitude = this.RigidBody.velocity.sqrMagnitude;
        //if (magnitude > this.MaxSpeed)
        //{
        //    this.RigidBody.velocity = this.RigidBody.velocity.normalized * this.MaxSpeed;
        //}
    }
}
