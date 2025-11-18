using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int objectsNumberOnStart;

    private List<GameObject> objectsPool = new List<GameObject>();

    private void Start()
    {
        CreateObjects();    }

    /// <summary>
    /// create the objects needed at the beginning of the game
    /// </summary>
    private void CreateObjects()
    {
        for (int i = 0;i < objectsNumberOnStart; i++)
        {
            CreateNewObject();
        }
    }

    /// <summary>
    /// Instantiate new object and add to the List
    /// </summary>
    /// <returns>new GameObject</returns>
    private GameObject CreateNewObject()
    {
        //Instantiate anywhere
        GameObject newObject = Instantiate(prefabObject);
        //Deactive
        newObject.SetActive(false);
        //Add to the list
        objectsPool.Add(newObject);

        return newObject;
    }

    /// <summary>
    /// Take from the list an available object
    /// if not exist create a new one and add to the list
    /// and Active the object
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        //Find in the objectsPool and object that is inactive in the game hierachy
        GameObject theObject = objectsPool.Find(x => x.activeInHierarchy == false);

        //if not exist , create one
        if (theObject == null) theObject = CreateNewObject();

        //Active gameObject
        theObject.SetActive(true);

        return theObject;
    }
}
