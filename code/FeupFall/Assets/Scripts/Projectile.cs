using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum proType
{
    boot, megaboot
}

public class Projectile : MonoBehaviour {

    [SerializeField]
    private int attackStrenght;
    [SerializeField]
    private int speed;
    [SerializeField]
    private proType projectileType;

    private Vector2 targetPoint;
   
    public int AttackStrenght
    {
        get
        {
            return attackStrenght;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }
    }

    public proType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }

    public Vector2 TargetPoint {
        get {
            return targetPoint;
        }

        set {
            targetPoint = value;
        }
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D() {
        Destroy(gameObject);
    }

}
