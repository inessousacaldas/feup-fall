using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenBehaviour : MonoBehaviour {

    [SerializeField]
    private float lineSight;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            var playerPos = Player.playerPosition;

            for (var i = gos.Length - 1; i >= 0; i--)
            {
                if(Mathf.Abs(playerPos.y - gos[i].transform.position.y) < lineSight)
                    Destroy(gos[i]);
            }
        }

    }
}
