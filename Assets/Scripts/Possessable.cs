using System;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    public Action<Vector3> BeginPossession;
    public Action EndPossession;
}
