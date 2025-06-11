using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public void summonBullet(Vector2 firePoint,Quaternion direction,string filepath)
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>(filepath), firePoint, direction);
    }
}
