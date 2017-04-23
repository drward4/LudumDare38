using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMotionController : MonoBehaviour
{
    public float Sensitivity;
    public float MovementSpeed;
    public float CameraFollowDistance;

    public Rigidbody RigidBody;
    private Quaternion StartOrientation;
    private float RotationX, RotationY;
    private bool IsPossessed;
    public Possessable Possessable;
    public Transform TopRotor;
    public Transform SideRotor;
    public float RotorRotationSpeed;

    private void ShowControls()
    {
        GameController.ShowControls("HELICOPTER CONTROLS\n\nMove - WASD\nTurn - Mouse\nLeave Body - X");
    }


    void Start()
    {
        this.Possessable.ObjectName = "Helicopter";
        this.Possessable.BeginPossession += this.BeginPossession;
        this.Possessable.EndPossession += this.EndPossession;
        this.StartOrientation = this.transform.localRotation;
        this.ShowControls();
    }


    private void ProcessInput()
    {
        if (!this.IsPossessed)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            GameController.EndPossession(this.Possessable);
        }

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
    }


    public void BeginPossession(Vector3 position)
    {
        GameController.ShowControls(this.Possessable.ObjectName + " Controls\n\nMove WASD\nLeave Body - X");
        this.RigidBody.useGravity = false;
        this.IsPossessed = true;
    }


    public void EndPossession()
    {
        this.RigidBody.useGravity = true;
        this.IsPossessed = false;
    }


    void Update()
    {
        if (!this.IsPossessed)
        {
            return;
        }

        // Rotor Motion
        if (this.IsPossessed)
        {
            this.TopRotor.Rotate(Vector3.forward, this.RotorRotationSpeed);
            this.SideRotor.Rotate(Vector3.left, this.RotorRotationSpeed);
        }

        this.RotationX += Input.GetAxis("Mouse X") * Sensitivity;
        this.RotationY += Input.GetAxis("Mouse Y") * Sensitivity;

        Quaternion x = Quaternion.AngleAxis(this.RotationX, Vector3.up);
        Quaternion y = Quaternion.AngleAxis(this.RotationY, Vector3.left);

        this.transform.localRotation = this.StartOrientation * x * y;
        this.RigidBody.velocity = Vector3.zero;

        this.ProcessInput();

        this.transform.position = this.RigidBody.transform.position;
        this.RigidBody.gameObject.transform.LookAt(this.transform.position + this.transform.forward.normalized);

        Camera.main.transform.position = this.transform.position - this.transform.forward * this.CameraFollowDistance;
        Camera.main.transform.LookAt(this.transform);
    }
}
