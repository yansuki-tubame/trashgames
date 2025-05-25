using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int initialSize;
    }

    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools) {
            Queue<GameObject> queue = new();

            for(int i = 0; i < pool.initialSize; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, queue);
        }
    }

    public GameObject GetObject(string tag, Vector3 position)
    {
        if(!poolDictionary.ContainsKey(tag)) {
            return null;
        }

        Queue<GameObject> queue = poolDictionary[tag];

        if(queue.Count == 0) {
            return null;
        }

        GameObject obj = queue.Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        if (poolDictionary.ContainsKey(obj.tag)) {
            poolDictionary[obj.tag].Enqueue(obj);
        }
        else {
            Destroy(obj);
        }
    }
}
