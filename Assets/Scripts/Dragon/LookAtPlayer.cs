using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform cam;

    public void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
