using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, CollisionCallable
{
    public float speed;

    private CapsuleCollider myCollider;
    private Camera myCam;
    private Rigidbody myRigidbody;
    private Canvas myCanvas;
    public Text myDebugText;
    private bool isGrounded;
    private int groundContactCount;
    public bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponentInChildren<CapsuleCollider>();
        myCam = GetComponentInChildren<Camera>();
        myRigidbody = GetComponentInChildren<Rigidbody>();
        myCanvas = myCam.GetComponentInChildren<Canvas>();
        Debug.Log(myCanvas.transform.childCount);
        Debug.Log(myCanvas.transform.GetChild(0).GetType());
        myDebugText = myCanvas.GetComponentInChildren<Text>();
        groundContactCount = 0;
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
        myRigidbody.velocity = new Vector3(inputVelocity.x, myRigidbody.velocity.y + inputVelocity.y, inputVelocity.z);

        //transform.position += inputVelocity * Time.deltaTime;

        if (debugMode) Debug.DrawLine(myRigidbody.position, myRigidbody.position + (inputVelocity));
        myDebugText.text = "Position: " + myRigidbody.position.y;
        myDebugText.text += "\nInputDir: " + inputVelocity;
        myDebugText.text += "\nGround Contact Count: " + groundContactCount;

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
        List<ContactPoint> contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        foreach (ContactPoint contact in contacts)
        {
            if (contact.thisCollider != null && contact.point.y < myRigidbody.position.y - 0.5) {
                isGrounded = true;
                Debug.Log("Contact: " + contact.point.y);
                Debug.Log("idl" + contact.thisCollider);
                Debug.DrawLine(myRigidbody.position, contact.point, Color.white, 10);
            }
            
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        List<ContactPoint> contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        foreach (ContactPoint contact in contacts)
        {
            if (contact.thisCollider != null && contact.point.y < myRigidbody.position.y - 0.5)
            {
                isGrounded = true;
                //groundContactCount++;
                Debug.Log("Contact: " + contact.point.y);
                Debug.Log("idl" + contact.thisCollider);
                //Debug.DrawLine(myRigidbody.position, contact.point, Color.white, 10);
            }

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        List<ContactPoint> contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        isGrounded = false;
        groundContactCount = 0;
    }

}
