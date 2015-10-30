using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class AdamPlayer : MonoBehaviour {

    public GameObject Reticule;
    public GameObject[] CreatedObjects;
    public Transform Facing;
    float sprayCooldownRemaining;
    float sprayCooldown = 0.1f;
    bool createEntrance = true;
    GameObject heldObject;
    float grabRange = 4f;
    float singlePullRange = 15f;
    float singlePushRange = 15f;
    float massPullRange = 20f;
    float massPushRange = 20f;
    float letGoRange = 5f;

    public float Speed;

    //FirstPersonController fpController;
    int layerMask = -1;

    void Awake()
    {
        Instantiate(Reticule);
        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 10;
        layerMask = ~layerMask;
        // This would cast rays only against colliders in layer 8, so we just inverse the mask.



        //  fpController = GetComponent<FirstPersonController>();

        //fpController.
    }
    // Update is called once per frame
    void Update()
    {
        CheckForAbilities();
        UpdateHeldObject();
    }

    void UpdateHeldObject()
    {
        if (heldObject != null)
        {
            if (Vector3.Distance(heldObject.transform.position, transform.position) > letGoRange)
                DropObject();
            else
            {
                heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }

    void CheckForAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
            GameManager.Main.LoadLevel("World1");

        //Create Bouncy Ball
        if (Input.GetKeyDown(KeyCode.R))
            Create(CreatedObjects[0]);//).transform.position = transform.position + transform.forward;

        //Create Float Cube
        if (Input.GetKeyDown(KeyCode.Y))
            Create(CreatedObjects[1]);
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject o = Create(CreatedObjects[3]);
            //o.transform.position += Facing.forward * 2;
        }

        //Toggle Gravity
        if (Input.GetKeyDown(KeyCode.G))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f);
            foreach (Collider c in hitColliders)
            {
                Rigidbody temp = c.gameObject.GetComponent<Rigidbody>();
                if (temp != null)
                    temp.useGravity = !temp.useGravity;
            }
        }

        //Force Push
        if (Input.GetKeyDown(KeyCode.E))
        {
            DropObject();
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, singlePushRange,layerMask))
                {
                    Rigidbody rigidb = hit.collider.GetComponent<Rigidbody>();
                    if(rigidb!=null)
                        rigidb.AddExplosionForce(1000, transform.position, singlePushRange);
                }
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, massPushRange, layerMask);
                foreach (Collider c in hitColliders)
                {
                    Rigidbody temp = c.gameObject.GetComponent<Rigidbody>();
                    if (temp != null)
                        temp.AddExplosionForce(1000, transform.position, massPushRange);
                }
            }


        }

        //Grab, Single Force Pull, and Mass Force Pull
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Single Force Pull or Grab
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                bool usedForcePull = false;

                if (Physics.Raycast(ray, out hit, singlePullRange, layerMask))
                {
                    Rigidbody rigidb = hit.collider.GetComponent<Rigidbody>();
                    if (rigidb != null && hit.distance > grabRange)
                    {
                        rigidb.AddExplosionForce(-1000, transform.position, singlePullRange);
                        usedForcePull = true;
                    }
                }
                if (!usedForcePull)
                    GrabOrDrop();
            }

            else //Mass Force Pull
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, massPullRange, layerMask);
                foreach (Collider c in hitColliders)
                {
                    Rigidbody temp = c.gameObject.GetComponent<Rigidbody>();
                    if (temp != null)
                        temp.AddExplosionForce(-1000, transform.position, massPullRange);
                }
            }
        }

        //Create Telepoter
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject o;
            if (createEntrance)
                o = Create(CreatedObjects[8]);
            else
                o = Create(CreatedObjects[9]);

           // o.transform.position += Facing.forward * 2;
            o.transform.rotation = Facing.rotation;

            Vector3 v = o.transform.rotation.eulerAngles;
            o.transform.rotation = Quaternion.Euler(-90, v.y, v.z);

            createEntrance = !createEntrance;
        }

        //Black Hole
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject o = Create(CreatedObjects[6]);
        }

        //Hive Block
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameObject o = Create(CreatedObjects[10]);
        }

        //Force Wall
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject o = Create(CreatedObjects[7]);
           // o.transform.position += Facing.forward * 2;
            o.transform.rotation = Facing.rotation;
        }

        //Raise Terrain
        if (Input.GetKey(KeyCode.Z))
        {
            RaiseLowerTerrain(100, 0.02f);
        }

        //Lower Terrain
        if (Input.GetKey(KeyCode.X))
        {
            RaiseLowerTerrain(100, -0.02f);
        }

        //Mouse Inputs
        if (Input.GetMouseButton(0))
        {
            sprayCooldownRemaining -= Time.deltaTime;
            if (sprayCooldownRemaining <= 0)
            {
                sprayCooldownRemaining = sprayCooldown;
                GameObject o = Create(CreatedObjects[2],2);
                o.GetComponent<Rigidbody>().AddForce(Facing.forward * 600, ForceMode.Force);
                o.transform.rotation = Facing.rotation;
                o.transform.Rotate(new Vector3(90, 0, 0));
                Destroy(o, 30f);
            }
        }
        if (Input.GetMouseButton(1))
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f);
            foreach (Collider c in hitColliders)
            {
                Rigidbody temp = c.gameObject.GetComponent<Rigidbody>();
                if (temp != null)
                {
                    temp.velocity = Vector3.zero;
                    temp.angularVelocity = Vector3.zero;
                }
            }

        }
    }

    void RaiseLowerTerrain(float range, float altitudePerSecond)
    {
        if (GameManager.Main.terrainModification != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                GameManager.Main.terrainModification.AltitudeSquare(hit.point, altitudePerSecond * Time.deltaTime, 5, 5);
            }
        }
        else
            Debug.Log("This terrain does not have a modification component.");
    }

    void GrabOrDrop()
    {
        if (heldObject == null)
            TakeObject(grabRange);
        else
            DropObject();
    }

    void TakeObject(float range)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            BasicObject grabableObject = hit.collider.GetComponent<BasicObject>();
            if(grabableObject != null && grabableObject.gameObject!=this)
            {
                Rigidbody targetRigidbody = grabableObject.myRigidbody;
                heldObject = targetRigidbody.gameObject;
                targetRigidbody.transform.SetParent(Facing.transform);
                grabableObject.InStasis++;
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.parent = null;
            heldObject.GetComponent<BasicObject>().InStasis--;
        }
        heldObject = null;
    }


    GameObject Create(GameObject o, float offset=3)
    {
        GameObject newObject = Instantiate(o);
        newObject.transform.position = Facing.position + Facing.forward* offset;
        return newObject;
    }


}
