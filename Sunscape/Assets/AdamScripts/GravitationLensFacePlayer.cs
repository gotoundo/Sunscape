using UnityEngine;
using System.Collections;

public class GravitationLensFacePlayer : MonoBehaviour
{
    public Vector3 AdditionalRotation = new Vector3(0, 0, 0);


    void Start()
    {
       
    }
   // float zRotation = 0;
    void Update()
    {
        //zRotation += Time.deltaTime*20;
        transform.LookAt(transform.position + GameManager.Main.MainCamera.transform.rotation * Vector3.forward, GameManager.Main.MainCamera.transform.rotation * Vector3.up);
        transform.Rotate(AdditionalRotation);
        //transform.Rotate(0, 0, zRotation);
    }
}
