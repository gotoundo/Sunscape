using UnityEngine;
using System.Collections;

public class PowerDiamondScript : MonoBehaviour {

    Light myLight;
    //float defaultSpin;
    Rigidbody myRigidbody;
	// Use this for initialization
	void Start () {
        myLight = GetComponent<Light>();
        myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
       // transform.rotation = new Quaternion(270, transform.rotation.y, 0,0);
        

        myLight.range = myRigidbody.angularVelocity.magnitude;
        myLight.intensity = myRigidbody.angularVelocity.magnitude;

        transform.rotation = Quaternion.Euler(270, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        // transform.Rotate(new Vector3(0, 0, Time.deltaTime * 30));

    }
}
