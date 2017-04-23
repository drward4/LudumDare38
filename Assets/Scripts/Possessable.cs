using System;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    public string ObjectName;
    public Action<Vector3> BeginPossession;
    public Action EndPossession;
}
