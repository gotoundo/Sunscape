using UnityEngine;
using System.Collections;

public class ReplicatorCellScript : MonoBehaviour {


    public float ReplicationTime;
    public int ChildrenCount;

    float remainingReplicationTime;

    public float ChildrenScale= .75f;
    public int Generations= 4;

    Rigidbody myBody;
    /*
    public bool DestroyOnDeath;
    public bool Temporary;
    public float TemporaryLifetime;*/


	// Use this for initialization
	void Start () {
        remainingReplicationTime = ReplicationTime;
        myBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        remainingReplicationTime -= Time.deltaTime;
        if(remainingReplicationTime <= 0 && Generations > 0)
        {
            remainingReplicationTime = ReplicationTime;
            transform.localScale *= ChildrenScale;
            myBody.mass *= ChildrenScale;
            Generations--;
            
            
            for (int i = 0; i < ChildrenCount; i++)
            {
                GameObject child = Instantiate(gameObject);
                child.transform.position += Random.Range(0f,1f)<.5? new Vector3(i - (ChildrenCount / 2), 0, 0) * transform.localScale.x : new Vector3(0, 0, i - (ChildrenCount / 2) * transform.localScale.x);
            }
            Destroy(gameObject);
        }
	}
}
