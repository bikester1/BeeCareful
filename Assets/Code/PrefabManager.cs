using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject flowerBasic;
    public GameObject bee;
    public GameObject hive;
    public GameObject seedIcon;
    [SerializeField]
    public List<GameObject> objects;

    private Dictionary<string, GameObject> items;

    public Dictionary<string, GameObject> Items { get => items; }


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] itemArray = Resources.LoadAll<GameObject>("Items");
        items = new Dictionary<string, GameObject>();

        foreach(GameObject obj in itemArray)
        {
            items.Add(obj.name, obj);
        }

        Debug.Log(items);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
