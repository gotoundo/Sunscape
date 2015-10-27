using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour
{
    float maxForce = 600f;
    float maxRange = 30f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxRange);
        foreach (Collider c in hitColliders)
        {
            Rigidbody temp = c.gameObject.GetComponent<Rigidbody>();
            if (temp != null)
            {
                Vector3 Direction = (transform.position - temp.position);
                temp.AddForce(Direction.normalized * maxForce / Mathf.Max(1,Direction.sqrMagnitude));
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        TouchBlackHole(collisionInfo);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        TouchBlackHole(collisionInfo);
    }

    void TouchBlackHole(Collision collisionInfo)
    {
        Rigidbody temp = collisionInfo.gameObject.GetComponent<Rigidbody>();
        if (temp != null)
        {
            temp.transform.localScale *= .95f;
            temp.velocity *= 0f;
            if (temp.transform.localScale.magnitude < .2f)
                Destroy(collisionInfo.gameObject);
        }
    }
}
