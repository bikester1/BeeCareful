using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, CollisionCallable
{
    public float speed;
    public float jumpHeight;

    private CapsuleCollider myCollider;
    private Camera myCam;
    private Rigidbody myRigidbody;
    private Canvas myCanvas;
    public Text myDebugText;
    private bool isGrounded;
    public bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponentInChildren<CapsuleCollider>();
        myCam = GetComponentInChildren<Camera>();
        myRigidbody = GetComponentInChildren<Rigidbody>();
        myCanvas = myCam.GetComponentInChildren<Canvas>();
        myDebugText = myCanvas.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ProcessPhysics();
    }

    private void LateUpdate()
    {

    }


    void ProcessInput()
    {
        // Sum inputs and then finanly normalize for consitent movement speed
        Vector3 inputDir = Vector3.zero;

        #region Horizontal Movement
        if (Input.GetKey(KeyCode.A))
        {
            inputDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputDir += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            inputDir += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputDir += Vector3.back;
        }
        #endregion

        #region Vetical Movement
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
        {
            inputDir += Vector3.down;
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            inputDir += Vector3.up;
        }
        #endregion

        // Make input Dir Magnitude 1
        inputDir.Normalize();

        // represents the input direction rotated to match the camera.
        Vector3 inputDirCam = Quaternion.Euler(0, myCam.transform.rotation.eulerAngles.y, myCam.transform.rotation.eulerAngles.z) * inputDir;

        // fin
        Vector3 inputVelocity = inputDirCam * speed;

        // move player
        myRigidbody.velocity = new Vector3(inputVelocity.x, myRigidbody.velocity.y + (inputVelocity.y * jumpHeight), inputVelocity.z);


        if (debugMode) Debug.DrawLine(myRigidbody.position, myRigidbody.position + (inputVelocity));
        myDebugText.text = "Position: " + myRigidbody.position.y;
        myDebugText.text += "\nInputDir: " + inputVelocity;

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(this.transform.position);
        }
    }

    void ProcessPhysics()
    {
        if (!isGrounded)
        {
            myRigidbody.velocity += Vector3.down * 1; 
        }
    }

    void CollisionCallable.OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        for(int i = 0; i < collision.contactCount; i++)
        {
            if (contacts[i].thisCollider != null && contacts[i].point.y < myRigidbody.position.y - 0.5) {
                isGrounded = true;
                Debug.DrawLine(myRigidbody.position, contacts[i].point, Color.white, 10);
            }
            
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (contacts[i].thisCollider != null && contacts[i].point.y < myRigidbody.position.y - 0.5)
            {
                isGrounded = true;
                Debug.DrawLine(myRigidbody.position, contacts[i].point, Color.white, 10);
            }

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        List<ContactPoint> contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        isGrounded = false;
    }

}
