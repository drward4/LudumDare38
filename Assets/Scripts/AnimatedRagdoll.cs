using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedRagdoll : MonoBehaviour
{
    private Dictionary<Rigidbody, Quaternion> RagDollRootStartRotations;

    public Rigidbody RagdollRootBone;
    public Animator Animator;

    private bool _IsAnimated;
    public bool IsAnimated { get { return this._IsAnimated; } }
    public bool IsRagdoll { get { return !this._IsAnimated; } }


    void Start()
    {
        this.RagDollRootStartRotations = new Dictionary<Rigidbody, Quaternion>();
        List<Rigidbody> rigidBodies = new List<Rigidbody>();
        this.RagdollRootBone.gameObject.GetComponentsInChildren<Rigidbody>(rigidBodies);

        for (int i = 0; i < rigidBodies.Count; i++)
        {
            this.RagDollRootStartRotations.Add(rigidBodies[i], rigidBodies[i].transform.localRotation);
        }
    }


    public void SetState(bool animated)
    {
        this._IsAnimated = animated;
        this.Animator.enabled = animated;

        foreach (KeyValuePair<Rigidbody, Quaternion> item in this.RagDollRootStartRotations)
        {
            // If we switch back to the animated state, reset all bones to original rotations
            if (animated)
            {
                item.Key.transform.localRotation = item.Value;
            }

            item.Key.isKinematic = animated;
        }
    }


    public void SwitchToAnimated()
    {
        this.SetState(true);
    }


    public void SwitchToRagdoll()
    {
        this.SetState(false);
    }
}
