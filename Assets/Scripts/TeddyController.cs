using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyController : MonoBehaviour
{
    public GameObject TeddyRagdoll;
    public GameObject TeddyAnimated;
    public Possessable Possessable;


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

        GameController.ShowControls(this.Possessable.ObjectName + " Controls\n\nMove Forward - W\nTurn / Look - Mouse\nLeave Body - Esc");
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


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && this.IsPossessed())
        {
            GameController.EndPossession(this.Possessable);
        }
    }
}
