using UnityEngine;
using System.Collections;

public class StasisZoneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        BasicObject freezableObject = col.GetComponent<BasicObject>();
        if (freezableObject != null)
        {
            freezableObject.InStasis++;
            StasisEffect(col);
        }
    }

    void OnTriggerStay(Collider col)
    {
        StasisEffect(col);
    }

    void StasisEffect(Collider col)
    {
        Rigidbody otherBody = col.gameObject.GetComponent<Rigidbody>();
        if(otherBody !=null)
        {
            BasicObject basicO = otherBody.GetComponent<BasicObject>();

            otherBody.velocity *= .9f;
            otherBody.angularVelocity *= .9f;

            if (otherBody.velocity.magnitude < 0.1f)
                otherBody.velocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider col)
    {
        Rigidbody otherBody = col.gameObject.GetComponent<Rigidbody>();
        BasicObject basicO = otherBody.GetComponent<BasicObject>();
        if (basicO != null)
            basicO.InStasis--;
    }
}
