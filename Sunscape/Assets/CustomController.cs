using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class CustomController : MonoBehaviour {
    public UnityStandardAssets.Characters.FirstPerson.MouseLook m_MouseLook;
    //public CharacterController m_CharacterController;
    public Rigidbody myBody;
    public float speed = 1;
    public Camera myCamera;
    public bool m_Jumping;
    public bool m_Jump;
    public bool m_PreviouslyGrounded;
    public bool m_Grounded;

    int exludePlayerLevelMask;
    float velocityCap = 10;

    public Vector3 velocity;
    public float velocityMagnitude;

    // Use this for initialization
    void Start () {
        exludePlayerLevelMask = ~(1 << 10);

        m_Jumping = false;
        myBody = GetComponent<Rigidbody>();
        myCamera = GameManager.Main.MainCamera;
        m_MouseLook = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
        m_MouseLook.Init(transform, GameManager.Main.MainCamera.transform);

    }

    bool hovering = true;
    float hoverHeight = 4f;


    public float HeightAboveGround()
    {
        RaycastHit hit;
        float heightAboveGround = 100;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity,exludePlayerLevelMask))
        {
            heightAboveGround = hit.distance;
        }
        return heightAboveGround;
    }

    // Update is called once per frame
    void Update () {
        velocity = myBody.velocity;
        velocityMagnitude = myBody.velocity.magnitude >0.01? myBody.velocity.magnitude:0;
        if (velocityMagnitude < 0.3)
            velocity = Vector3.zero;
        
        RotateView();
        m_Grounded = HeightAboveGround() < 2;

        if(hovering)
        {
          //  myBody.position += new Vector3(0, hoverHeight - HeightAboveGround(), 0);
            /*
            if (HeightAboveGround() < hoverHeight)
            {
                myBody.useGravity = false;
                myBody.position += new Vector3(0, hoverHeight - HeightAboveGround(), 0);
            }
            else if(HeightAboveGround() > hoverHeight * 2)
            {
                myBody.useGravity = true;
            }*/
        }




        if (!m_Jump)
        {
            m_Jump = Input.GetKey(KeyCode.Space);
        }

        //Just landed
        if (!m_PreviouslyGrounded && m_Grounded)
        {
            m_Jumping = false;
        }

        m_PreviouslyGrounded = m_Grounded;
        if (m_Grounded)
        {
            if (m_Jump)
            {
                Debug.Log("Jump!");
                myBody.AddForce(0, 100, 0);
                m_Jump = false;
                m_Jumping = true;
            }
        }



        if (Input.GetKey(KeyCode.A))
            myBody.AddForce((myCamera.transform.rotation * Vector3.left) * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D))
            myBody.AddForce((myCamera.transform.rotation*Vector3.right) * Time.deltaTime * speed);

        Vector3 forwardVector = new Vector3(myCamera.transform.forward.x,0, myCamera.transform.forward.z).normalized;

        if (Input.GetKey(KeyCode.W))
            myBody.AddForce(forwardVector * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.S))
            myBody.AddForce(-forwardVector * Time.deltaTime* speed);


       // myBody.AddForce(Physics.gravity);
        //if (Input.GetKey(KeyCode.Space))
        //    myBody.AddForce(0, 50, 0);
    }

    void RotateView()
    {

        //transform.rotation = myCamera.transform.rotation;
        m_MouseLook.LookRotation(transform, GameManager.Main.MainCamera.transform);
    }
}
