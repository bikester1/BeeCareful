using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Entity

    
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation.SetEulerAngles(Vector3.RotateTowards(transform.rotation.eulerAngles, GameObject.Find("Flower").transform.position, .2f, .32f));
        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Flower").transform.position, .2f);
    }
}
