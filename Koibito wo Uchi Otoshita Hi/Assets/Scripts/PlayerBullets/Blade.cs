using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public float damage;
    private bool active;
    private void Start()
    {
        active = true;
    }
    private void delete()
    {
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (active)
            {
                active = false;
                enemy.health -= damage;
                enemy.anim.SetBool("Hurt", true);
            }
        }
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
