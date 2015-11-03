using UnityEngine;
using System.Collections;

public class PrisonCube : MonoBehaviour {
    public Vector3 TargetPosition;
    public float speed;
    Rigidbody body;
    public bool ExitCube = false;// = true;
	// Use this for initialization
	void Start () {
        TargetPosition = transform.position;
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        body.velocity = (TargetPosition - transform.position).normalized * speed;
     //   body.velocity = body.velocity.normalized * speed;

	}

    void OnTriggerEnter(Collider col)
    {
        if(ExitCube && col.gameObject == GameManager.Main.Player)
        {
            GameManager.Main.LoadLevel("World-BounceHouse");
        }
    }

}
