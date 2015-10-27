using UnityEngine;
using System.Collections;

public class PortalFacePlayer : MonoBehaviour {

   // public Vector3 AdditionalRotation = new Vector3(270, 0, 0);


    void Start()
    {

    }
    // float zRotation = 0;
    void Update()
    {
        transform.LookAt(transform.position + GameManager.Main.MainCamera.transform.rotation * Vector3.forward, GameManager.Main.MainCamera.transform.rotation * Vector3.up);
      //      transform.Rotate(AdditionalRotation);

        Vector3 v = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(-90, v.y, v.z);
    }
}
