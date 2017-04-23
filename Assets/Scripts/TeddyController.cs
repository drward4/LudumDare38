using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyController : MonoBehaviour
{
    public GameObject TeddyRagdoll;
    public GameObject TeddyAnimated;
    public Possessable Possessable;
    public bool HasKey;
    public bool HasHammer;
    private bool InRangeOfKey;
    private bool InRangeOfHammer;
    private bool InRangeOfToyChest;
    private bool InRangeOfWindow;


    public bool IsPossessed()
    {
        return this.TeddyAnimated.activeSelf;
    }


    void Start()
    {
        this.Possessable.ObjectName = "Teddy";
        this.Possessable.BeginPossession += this.BeginPossession;
        this.Possessable.EndPossession += this.EndPossession;
    }


    public void BeginPossession(Vector3 position)
    {
        this.TeddyRagdoll.SetActive(false);

        // Move animated teddy to same location as ragdoll teddy without messing up the y axis
        this.TeddyAnimated.transform.position = new Vector3(
            position.x, 
            this.TeddyAnimated.transform.position.y,
            position.z);

        this.TeddyAnimated.SetActive(true);

        GameController.ShowControls(this.Possessable.ObjectName + " Controls\n\nMove Forward - W\nTurn / Look - Mouse\nLeave Body - X");
    }


    public void EndPossession()
    {
        this.TeddyAnimated.SetActive(false);

        this.TeddyRagdoll.transform.position = new Vector3(
            this.TeddyAnimated.transform.position.x,
            this.TeddyAnimated.transform.position.y,
            this.TeddyAnimated.transform.position.z) 
            + Vector3.up * 1.5f;

        this.TeddyRagdoll.SetActive(true);
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
                if (!GameController.IsChestOpen())
                {
                    GameController.ShowMessage("Open Toy Chest - E");
                }
            }
            else
            {
                GameController.ShowMessage("Needs Key");
            }
        }
        else if (other.gameObject.layer == 16)  // Window
        {
            this.InRangeOfWindow = true;
            if (this.HasHammer)
            {
                GameController.ShowMessage("Smash Window - E");
            }
            else
            {
                GameController.ShowMessage("Need something to smash it");
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

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (this.InRangeOfKey)
            {
                this.HasKey = true;
                GameController.PickUpKey();
            }

            if (this.InRangeOfHammer)
            {
                this.HasHammer = true;
                GameController.PickUpHammer();
            }

            if (this.InRangeOfToyChest && this.HasKey && !GameController.IsChestOpen()) // Check during process input in case of picking up key while near toy chest
            {
                GameController.OpenToyChest();
            }

            if (this.InRangeOfWindow && this.HasHammer)  // Check during process input in case hammer somehow lands next to window
            {
                GameController.SmashWindow();
            }
        }

        if (Input.GetKeyUp(KeyCode.X) && this.IsPossessed())
        {
            GameController.EndPossession(this.Possessable);
        }
    }

    void Update()
    {
        this.ProcessInput();
    }
}
