using UnityEngine;
using System.Collections;

public class BasicObject : MonoBehaviour
{

    Light myLight;
    public float defaultSpin;
    public Rigidbody myRigidbody;
    public int InStasis;
    bool startingGravityUsage;
    public float maxIntegrity = 0;
    public float integrity;
    // Use this for initialization

    bool stasisSwitchedGravity;

    void Start()
    {
        InStasis = 0;
        myLight = GetComponent<Light>();
        myRigidbody = GetComponent<Rigidbody>();
        startingGravityUsage = myRigidbody.useGravity;

        if (maxIntegrity == 0)
            maxIntegrity = myRigidbody.mass;
        integrity = maxIntegrity;

    }

    // Update is called once per frame

    void Update()
    {
        if (defaultSpin != 0 && InStasis<=0)
            myRigidbody.angularVelocity += new Vector3(0, Time.deltaTime * defaultSpin, 0);

        if (InStasis > 0)
        {
            if (startingGravityUsage == true && myRigidbody.useGravity == true)
            {
                stasisSwitchedGravity = true;
                myRigidbody.useGravity = false;
            }
        }
        else if (stasisSwitchedGravity)
        {
            stasisSwitchedGravity = false;
            myRigidbody.useGravity = startingGravityUsage;
        }


        //InStasis = false;
    }





    //Glass Breaking stuff
    Vector3 vel;
    BreakGlass g;
    void FixedUpdate()
    {
        vel = GetComponent<Rigidbody>().velocity;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<BreakGlass>() != null)
        {
            g = col.gameObject.GetComponent<BreakGlass>();
            GetComponent<Rigidbody>().velocity = vel * g.SlowdownCoefficient;

            if (g.BreakByVelocity)
            {
                if (col.relativeVelocity.magnitude >= g.BreakVelocity)
                {
                    col.gameObject.GetComponent<BreakGlass>().BreakIt();
                    return;
                }
            }

            if (g.BreakByImpulse)
            {
                if (col.relativeVelocity.magnitude * GetComponent<Rigidbody>().mass >= g.BreakImpulse)
                {
                    col.gameObject.GetComponent<BreakGlass>().BreakIt();
                    return;
                }
            }

        }
    }
}
