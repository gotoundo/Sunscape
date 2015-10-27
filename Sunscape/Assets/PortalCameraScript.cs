using UnityEngine;
using System.Collections;

public class PortalCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
       // transform.localRotation = Quaternion.identity;
        //transform.rotation = GameManager.Main.MainCamera.transform.rotation;


        //Quaternion.AngleAxis(0,Vector3.zero);
         transform.rotation = GameManager.Main.MainCamera.transform.rotation;
        transform.Rotate(new Vector3(0, 0, 180));

    }
}
