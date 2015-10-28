using UnityEngine;
using System.Collections;

public class GGDroneScript : MonoBehaviour {


    LineRenderer lineRender;
    GameObject targetObject;
    Transform destination;
    Rigidbody body;
    float laserRange = 10;
    float stopRange = 6;
    float giveUpRange = 50f;

	void Start () {
        lineRender = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody>();
	}
    
	// Update is called once per frame
	void Update () {

        if(targetObject == null || Vector3.Distance(targetObject.transform.position,transform.position)> giveUpRange)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2000f);
            GameObject closestBody = gameObject;
            foreach (Collider c in hitColliders)
            {
                BasicObject temp = c.gameObject.GetComponent<BasicObject>();
                if (temp != null && temp.gameObject != gameObject)
                {
                    if (closestBody == gameObject || Vector3.Distance(temp.transform.position, transform.position) < Vector3.Distance(closestBody.transform.position, transform.position))
                        closestBody = temp.gameObject;
                }   
            }
            targetObject = closestBody;
            destination = closestBody.transform;
        }

        if (Vector3.Distance(destination.position, transform.position) > stopRange)
            body.AddForce((destination.position - transform.position + new Vector3(0, 5, 0)).normalized * 1000 * Time.deltaTime);

        if (Vector3.Distance(destination.position, transform.position) < laserRange)
        {
            lineRender.enabled = true;
            lineRender.SetPosition(0, transform.position);
            lineRender.SetPosition(1, destination.position);

            BasicObject enemyBody = targetObject.GetComponent<BasicObject>();
            enemyBody.integrity -= Time.deltaTime;
            if (enemyBody.integrity <= 0.1f)
                Destroy(enemyBody.gameObject);
        }
        else
            lineRender.enabled = false;
    }
}
