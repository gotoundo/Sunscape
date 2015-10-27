using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
public class TeleportBlockScript : MonoBehaviour
{
    public static List<GameObject> TeleportBlocks;
    public static List<GameObject> TeleportedObjects;
    public GameObject ParentObject;
    public GameObject TeleportSurface;

    const int maxBlockCount = 2;
    // Use this for initialization
    void Start()
    {
        if (TeleportBlocks == null)
        {
            TeleportBlocks = new List<GameObject>();
            TeleportedObjects = new List<GameObject>();
        }

        TeleportBlocks.Add(gameObject);
        if (TeleportBlocks.Count > maxBlockCount)
        {
            GameObject blockToDestroy = TeleportBlocks[0];
            TeleportBlocks.Remove(blockToDestroy);
            Destroy(blockToDestroy.GetComponent<TeleportBlockScript>().ParentObject);
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter(Collider teleportedObject)
    {
        Vector3 relativePosition = teleportedObject.transform.position - TeleportSurface.transform.position;
        if (teleportedObject.gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody otherBody = teleportedObject.gameObject.GetComponent<Rigidbody>();

            if (TeleportedObjects.Contains(teleportedObject.gameObject))
                TeleportedObjects.Remove(teleportedObject.gameObject);
            else
            {
                TeleportedObjects.Add(teleportedObject.gameObject);

                TeleportBlockScript destinationObject = GetComponent<TeleportBlockScript>();
                Vector3 teleportDestination = teleportedObject.transform.position;
                foreach (GameObject o in TeleportBlocks)
                    if (o != gameObject)
                        destinationObject = o.GetComponent<TeleportBlockScript>();
                teleportDestination = destinationObject.TeleportSurface.transform.position;

                teleportedObject.transform.position = teleportDestination + relativePosition;
                Vector3 turnangle = destinationObject.ParentObject.transform.rotation.eulerAngles - ParentObject.transform.rotation.eulerAngles + new Vector3(0,180,0);
                teleportedObject.transform.Rotate(turnangle);
                if (teleportedObject.gameObject.tag == "Player")
                {
                 //   UnityStandardAssets.Characters.FirstPerson.MouseLook.CameraRotationOffset += turnangle;
                    //GameManager.Main.MainCamera.transform.Rotate(turnangle);
                }
                // Vector3 TurnAngle = (ParentObject.transform.rotation - otherTeleporter.ParentObject.transform.rotation);
                // other.transform.Rotate(TurnAngle);
                // otherBody.velocity = Vector3.RotateTowards(otherBody.velocity, TurnAngle, 10000, 0f);
                //otherBody.velocity = Vector3.RotateTowards(otherBody.velocity,ParentObject.transform.right,2000,2000)* otherBody.velocity.magnitude;
                // otherBody.transform.rotation = ParentObject.transform.rotation;

                //  Vector3 v = transform.rotation.eulerAngles;
                //                transform.rotation = Quaternion.Euler(-90, v.y, v.z);
            }
        }
    }
}
