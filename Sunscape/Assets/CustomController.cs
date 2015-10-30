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
    // Use this for initialization
    void Start () {
        //m_CharacterController = GetComponent<CharacterController>();
        m_Jumping = false;
        myBody = GetComponent<Rigidbody>();
        myCamera = GameManager.Main.MainCamera;
        m_MouseLook = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
        m_MouseLook.Init(transform, GameManager.Main.MainCamera.transform);

    }
	
	// Update is called once per frame
	void Update () {
        RotateView();
        if (!m_Jump)
        {
            m_Jump = Input.GetKey(KeyCode.Space);
        }

        //Just landed
        if (!m_PreviouslyGrounded)//&&m_CharacterController.isGrounded
        {
           // StartCoroutine(m_JumpBob.DoBobCycle());
           // PlayLandingSound();
            //m_MoveDir.y = 0f;
            m_Jumping = false;
        }

        //Falling without having jumped first
        if ( !m_Jumping && m_PreviouslyGrounded) //!m_CharacterController.isGrounded &&
        {
            //m_MoveDir.y = 0f;
        }

      //  m_PreviouslyGrounded = m_CharacterController.isGrounded;


       // if (m_CharacterController.isGrounded)
       // {
            //m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                //m_MoveDir.y = m_JumpSpeed;
                //PlayJumpSound();
                Debug.Log("Jump!");
                myBody.AddForce(0, 100, 0);
                m_Jump = false;
                m_Jumping = true;
            }
       // }



        

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
        m_MouseLook.LookRotation(transform, GameManager.Main.MainCamera.transform);
    }
}
