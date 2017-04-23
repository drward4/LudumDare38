using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMotionController : MonoBehaviour
{
    public float Sensitivity;
    public float MaxSpeed;
    public float TurnDegreesPerSecond;
    public float CameraFollowDistance;
    public Possessable Possessable;

    public Rigidbody RigidBody;
    private bool IsPossessed;

    void Start()
    {
        this.Possessable.ObjectName = "Truck";
        this.Possessable.BeginPossession += this.BeginPossession;
        this.Possessable.EndPossession += this.EndPossession;
    }

    //public bool IsPossessed()
    //{
    //    return this.TeddyAnimated.activeSelf;
    //}



    public void BeginPossession(Vector3 position)
    {
        GameController.ShowControls(this.Possessable.ObjectName + " Controls\n\nMove WASD\nLeave Body - X");
        this.IsPossessed = true;
    }


    public void EndPossession()
    {
        this.IsPossessed = false;
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

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(Vector3.up, -this.TurnDegreesPerSecond * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(Vector3.up, this.TurnDegreesPerSecond * Time.deltaTime);
        }

        // Move Forward
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = this.transform.forward;
            direction = direction.normalized * this.MaxSpeed;
            this.RigidBody.velocity = new Vector3(direction.x, this.RigidBody.velocity.y, direction.z);
        }

        // Move Backward
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 direction = -this.transform.forward;
            direction = direction.normalized * this.MaxSpeed;
            this.RigidBody.velocity = new Vector3(direction.x, this.RigidBody.velocity.y, direction.z);
        }
    }


    void Update()
    {
        if (!this.IsPossessed)
        {
            return;
        }

        this.ProcessInput();

        Camera.main.transform.position = this.transform.position + Vector3.up * 0.4f;
        Camera.main.transform.LookAt(this.transform.position + this.transform.forward * 5f);
    }
}
