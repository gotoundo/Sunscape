using UnityEngine;
using System.Collections;

public class ForceWallScript : MonoBehaviour {

    public static ForceWallScript Main;

    Rigidbody myRigidbody;
	// Use this for initialization
	void Start () {
        if (Main != null)
            Destroy(Main.gameObject);
        Main = this;

        myRigidbody = GetComponent<Rigidbody>();
       // myRigidbody.freezeRotation = true;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
