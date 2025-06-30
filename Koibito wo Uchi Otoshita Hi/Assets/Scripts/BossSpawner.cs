using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public float time;
    void Start()
    {
        time = 30.0f;
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 60.0f)
        {
            time = 0.0f;
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/BOSS"), transform.position, transform.rotation);
        }
    }
}
