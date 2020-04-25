using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, CollisionCallable
{
    public float speed;
    public float jumpHeight;
    public float jumpForgiveness;
    private float jumpTimer;
    private bool jumped = false;

    private CapsuleCollider myCollider;
    private Camera myCam;
    private Rigidbody myRigidbody;
    private Canvas myCanvas;
    public Text myDebugText;
    private bool isGrounded;
    public bool debugMode = false;
    private float groundCollisionTime;

    private Inventory myInventory;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponentInChildren<CapsuleCollider>();
        myCam = GetComponentInChildren<Camera>();
        myRigidbody = GetComponentInChildren<Rigidbody>();
        myCanvas = myCam.GetComponentInChildren<Canvas>();
        myDebugText = myCanvas.GetComponentInChildren<Text>();
        myInventory = myCam.GetComponentInChildren<Inventory>();
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

        if (Time.realtimeSinceStartup- groundCollisionTime > Time.fixedDeltaTime) isGrounded = false;

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

        inputDir.Normalize();

        #region Vetical Movement
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
        {
            inputDir += Vector3.down;
        }

        // step timer for jump
        if (jumped)
        {
            jumpTimer += Time.deltaTime;
        }

        // cancel jump if past forgiveness time
        if(jumpTimer > jumpForgiveness)
        {
            jumpTimer = 0;
            jumped = false;
        }

        // initiate jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
            jumpTimer = 0;
        }

        // jump
        if(isGrounded && jumped)
        {
            inputDir = Vector3.up;
            jumped = false;
            jumpTimer = 0;
            isGrounded = false;
        }

        #endregion

        // Make input Dir Magnitude 1


        // represents the input direction rotated to match the camera.
        Vector3 inputDirCam = Quaternion.Euler(0, myCam.transform.rotation.eulerAngles.y, myCam.transform.rotation.eulerAngles.z) * inputDir;

        // fin
        Vector3 inputVelocity = inputDirCam * speed;

        // move player
        myRigidbody.velocity = new Vector3(inputVelocity.x, myRigidbody.velocity.y + (inputVelocity.y * jumpHeight), inputVelocity.z);


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
        // Ignore flowers
        if (collision.gameObject.GetComponent<Plant>() != null)
        {
            Physics.IgnoreCollision(collision.collider, myCollider);
        }

        if(collision.gameObject.GetComponent<Item>() != null)
        {
            Item item = collision.gameObject.GetComponent<Item>();
            int slot = myInventory.FirstEmptySlot();

            // inventory full
            if (slot == -1) 
            {
                Physics.IgnoreCollision(collision.collider, myCollider);
                return; 
            }

            item.MeshRenderer.enabled = false;
            myInventory.ItemToSlot(item, slot);
        }

        // Determine if grounded.
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        for(int i = 0; i < collision.contactCount; i++)
        {
            if (contacts[i].thisCollider != null && contacts[i].point.y < myRigidbody.position.y - 0.5) {
                isGrounded = true;
                groundCollisionTime = Time.realtimeSinceStartup;
            }
            
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        // Ignore flowers
        if (collision.gameObject.GetComponent<Plant>() != null)
        {
            Physics.IgnoreCollision(collision.collider, myCollider);
        }

        if (collision.gameObject.GetComponent<Item>() != null)
        {
            Item item = collision.gameObject.GetComponent<Item>();
            int slot = myInventory.FirstEmptySlot();

            // inventory full
            if (slot == -1)
            {
                Physics.IgnoreCollision(collision.collider, myCollider);
                return;
            }

            item.MeshRenderer.enabled = false;
            myInventory.ItemToSlot(item, slot);
        }

        // Determine if grounded
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (contacts[i].thisCollider != null && contacts[i].point.y < myRigidbody.position.y - 0.5)
            {
                isGrounded = true;
                groundCollisionTime = Time.realtimeSinceStartup;
            }

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        // on exit when we are no longer in collision so we cannot be grounded
        List<ContactPoint> contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        isGrounded = false;
    }

}
