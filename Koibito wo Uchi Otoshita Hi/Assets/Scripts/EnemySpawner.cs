using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float level;
    public float time;
    void Start()
    {
        time = 12.0f;
        level = 0.0f;
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 8.0f - level)
        {
            time = 0.0f;
            if (level < 2.0f)
            {
                level += 0.4f;
            }
            int a = Random.Range(0, 3);
            if (a == 0)
            {
                time = 3.0f;
            }
            if (a == 1)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ChargeBug"), transform.position, transform.rotation);
            }
            else if (a == 2)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ShootBug"), transform.position, transform.rotation);
            }

        }
    }
}
