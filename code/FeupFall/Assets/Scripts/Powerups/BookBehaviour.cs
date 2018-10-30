using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player script = (Player)player.GetComponent(typeof(Player));
            script.increaseHP();
        }
       
    }
}
