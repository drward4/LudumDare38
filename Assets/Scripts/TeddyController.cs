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
        this.Possessable.BeginPossession += this.BeginPossession;
        this.Possessable.EndPossession += this.EndPossession;
    }


    public void BeginPossession(Vector3 position)
    {
        Debug.Log("begin possession");
        this.TeddyRagdoll.SetActive(false);

        // Move animated teddy to same location as ragdoll teddy without messing up the y axis
        this.TeddyAnimated.transform.position = new Vector3(
            position.x, 
            this.TeddyAnimated.transform.position.y,
            position.z);

        this.TeddyAnimated.SetActive(true);
    }


    public void EndPossession()
    {
        Debug.Log("end possession");
        this.TeddyAnimated.SetActive(false);

        this.TeddyRagdoll.transform.position = new Vector3(
            this.TeddyAnimated.transform.position.x,
            this.TeddyAnimated.transform.position.y,
            this.TeddyAnimated.transform.position.z);

        this.TeddyRagdoll.SetActive(true);
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && this.IsPossessed())
        {
            GameController.EndPossession(this.Possessable);
        }
    }
}
