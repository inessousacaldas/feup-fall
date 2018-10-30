using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SINFBehaviour : MonoBehaviour
{
    [SerializeField]
    private float time;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            var playerPos = Player.playerPosition;

            foreach (var gObj in gos)
            {
                gObj.setImmortal(time);
            }
        }
    }
}
