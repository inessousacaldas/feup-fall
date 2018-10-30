using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCalendar : Enemy {

    private float pos;
    private float currentSpeed;
    private bool increasing = true;
    private bool decreasing = false;

    // Use this for initialization
    void Start () {
        currentSpeed = MinSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        pos += Time.deltaTime * currentSpeed;
        transform.position = new Vector2(Mathf.PingPong(pos, 5.2f) - 2.6f,transform.localPosition.y);
       
        if (currentSpeed > MaxSpeed) {
            decreasing = true;
            increasing = false;
        } else if (currentSpeed < MinSpeed) {
            decreasing = false;
            increasing = true;
        }

        if (increasing) {
            currentSpeed += IncreasingSpeedAcceleration;
        } else if (decreasing){
            currentSpeed -= DecreasingSpeedAcceleration;
        }
        

    }
}
