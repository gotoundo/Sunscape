using UnityEngine;
using System.Collections;

public class DemiPortalScript : MonoBehaviour {

    public string DestinationWorld;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameManager.Main.LoadLevel(DestinationWorld);
            
        }
    }
}
