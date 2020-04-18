using UnityEngine;

public class Bee : Entity 
{

    //public float verticalSpeed;
    //public float amplitude;
    private bool hasPollinated;
    public int maxPollinationCount;
    private int pollinationCount;
    public Rigidbody beeRigidBody;
    //public float speed;
    public float forceMultiplier;
    


    private Vector3 tempPosition;
    private Vector3 target;

    void Start()
    {
        hasPollinated = false;
        pollinationCount = 0;
        tempPosition = transform.position;
        findTargetPosition();
    }

    void Update()
    {

        //Creates an upward and downward movement for the bee.
        //Quaternion.RotateTowards(transform.rotation.eulerAngles, )
        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target, step);
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forceMultiplier;
        //tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed)*amplitude;
        //transform.position = new Vector3(transform.position.x, tempPosition.y,transform.position.z);

    }

    Vector3 findTargetPosition()
    {
        if (pollinationCount < maxPollinationCount)
        {
            pollinationCount++;
            target = GameObject.FindGameObjectWithTag("Plant").transform.position;
            return target;
        } else
        {
            target = GameObject.Find("Hive").transform.position;
            return target;
        }
        
    }

    void SpawnBee()
    {

    }
}
