using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs : MonoBehaviour
{
    public Transform player;
    public GameObject orb;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        orb = gameObject;
    }
    void Update()
    {
        if (Vector2.Distance(orb.transform.position, player.position) < 0.5f)
        {
            if (orb.tag == "Red")
            {
                //
            }
            if (orb.tag == "Yellow")
            {

            }
            if (orb.tag == "Blue")
            {

            }
            GameObject.Destroy(orb);
        }
    }
}
