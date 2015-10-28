using UnityEngine;
using System.Collections;

public class CustomController : MonoBehaviour {

    Rigidbody myBody;
	// Use this for initialization
	void Start () {
        myBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -60 * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, 60 * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.W))
            transform.position += 10*transform.forward*Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position -= 10 * transform.forward * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
            myBody.AddForce(0, 50, 0);
    }
}
