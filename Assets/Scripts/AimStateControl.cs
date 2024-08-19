using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStateControl : MonoBehaviour
{
    PlayerController controller;

    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

    private Quaternion rotation;
    private float x;
    private float y;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

    }


    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);

        /*// Get current value of localEulerAngles
        Vector3 currentEulerAngles = camFollowPos.localEulerAngles;

        // Update x value based on yAxis.Value
        currentEulerAngles.x = yAxis.Value;

        // Reassign the updated value to camFollowPos.localEulerAngle
        camFollowPos.localEulerAngles = currentEulerAngles;

        // Same for transform.eulerAngles
        Vector3 currentTransformEulerAngles = transform.eulerAngles;

        // Update y value based on xAxis.Value
        currentTransformEulerAngles.y = xAxis.Value;

        // Reassign the updated value to transform.eulerAngles
        transform.eulerAngles = currentTransformEulerAngles;
        //rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0);
        rotation = Quaternion.Euler(currentEulerAngles.x, currentTransformEulerAngles.y, 0);

        //rotation.y = 0;
        rotation.x = 0;
        rotation.z = 0;

        target.rotation = rotation;*/

        Quaternion camRotation = camFollowPos.rotation;
        Quaternion targetRotation = target.rotation;
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, camRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        target.rotation = targetRotation;
        Debug.Log(target.rotation);
        Debug.Log("Forward direction: " + target.forward);
    }
}
