using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    [SerializeField] private GameObject gameObjectToPool;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> objectPool;
    // Awake is called before the first frame update and before other objects may try to use the pool.
    void Awake()
    {
        SharedInstance = this;
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(gameObjectToPool);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            objectPool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        GameObject obj = Instantiate(gameObjectToPool);
        obj.SetActive(false);
        obj.transform.parent = this.transform;
        objectPool.Add(obj);
        return obj;
    }
    
}
