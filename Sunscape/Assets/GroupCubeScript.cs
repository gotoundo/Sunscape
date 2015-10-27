using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupCubeScript : MonoBehaviour {

    public GameObject LaserDrawerTemplate;
    Dictionary<RecievesPowerScript, LineRenderer> LaserDirectory;
   // List<LineDrawer> Lasers;
   // List<GameObject> Targets;
    float searchRange = 10f;
    float pushForce = -30f;
    //public bool showLasers = false;
    Rigidbody myRigidbody;
    // Use this for initialization
    void Awake()
    {
        LaserDirectory = new Dictionary<RecievesPowerScript, LineRenderer>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRange);
        foreach (Collider c in hitColliders)
        {
            RecievesPowerScript objectToShoot = c.gameObject.GetComponent<RecievesPowerScript>();
            if (objectToShoot && objectToShoot.gameObject != gameObject)
            {
                Vector3 angle = (objectToShoot.transform.position - transform.position).normalized;
                float distanceNormal = (Vector3.Distance(objectToShoot.transform.position, transform.position) / searchRange) - 0.5f;
                objectToShoot.GetComponent<Rigidbody>().velocity += angle * Time.deltaTime * pushForce * distanceNormal;
            }
            // && !LaserDirectory.ContainsKey(objectToShoot)
            //LaserDirectory.Add(objectToShoot, Instantiate(LaserDrawerTemplate).GetComponent<LineRenderer>());
        }

        /*foreach(RecievesPowerScript target in new List<RecievesPowerScript>(LaserDirectory.Keys))
        {
            if (Vector3.Distance(transform.position, target.transform.position) < searchRange)
            {
                LaserDirectory[target].SetPosition(0, transform.position);
                LaserDirectory[target].SetPosition(1, target.transform.position);

                Vector3 angle = (target.transform.position - transform.position).normalized;
                float distanceNormal = (Vector3.Distance(target.transform.position, transform.position)/ searchRange)-0.5f;

                target.GetComponent<Rigidbody>().velocity+= angle * Time.deltaTime* pushForce* distanceNormal;

                LaserDirectory[target].gameObject.SetActive(showLasers);
            }
            else
            {
                Destroy(LaserDirectory[target].gameObject);
                LaserDirectory.Remove(target);
            }
        }*/
    }
}
