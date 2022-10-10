using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    private GameObject targetToLook;
    private Vector3 initialLocation;
    // Start is called before the first frame update
    void Start()
    {
        initialLocation = new Vector3(0, 25, -5);
    }

    public void SetTarget(GameObject target)
    {
        targetToLook = target;
        transform.localRotation = Quaternion.LookRotation(target.transform.localPosition - transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(targetToLook)
        {
            transform.localPosition = targetToLook.transform.localPosition + initialLocation;
        }
    }
}
