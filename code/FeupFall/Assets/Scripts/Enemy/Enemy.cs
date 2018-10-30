using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageHandler {

    [SerializeField]
    private int damage;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float decreasingSpeedAcceleration;
    [SerializeField]
    private float increasingSpeedAcceleration;
    [SerializeField]
    private bool shoots;
    [SerializeField]
    private Projectile projectile;
    [SerializeField]
    private float rateOfFire;
    [SerializeField]
    private Transform spawnPoint;

    private float timer = 0;
    protected bool touchesWall = false;

    public int Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    public float MinSpeed {
        get {
            return minSpeed;
        }

        set {
            minSpeed = value;
        }
    }

    public float MaxSpeed {
        get {
            return maxSpeed;
        }

        set {
            maxSpeed = value;
        }
    }

    public float DecreasingSpeedAcceleration {
        get {
            return decreasingSpeedAcceleration;
        }

        set {
            decreasingSpeedAcceleration = value;
        }
    }

    public float IncreasingSpeedAcceleration {
        get {
            return increasingSpeedAcceleration;
        }

        set {
            increasingSpeedAcceleration = value;
        }
    }


    // Use this for initialization
    void Start () {
		
	}

    void FixedUpdate() {
        if (shoots) {
            timer += Time.deltaTime;

            if (timer > rateOfFire) {
                timer = 0;
                shootProjectile();
            }
        }
        
    }

    void shootProjectile() {
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.position = spawnPoint.position;
        newProjectile.TargetPoint = Player.playerPosition;
        StartCoroutine(moveProjectile(newProjectile));
    }

    IEnumerator moveProjectile(Projectile projectileToMove) {
        while (getDistanceToTarget(projectileToMove.TargetPoint) > 0.2f && projectileToMove != null) {
            Vector2 direction = projectileToMove.TargetPoint - (Vector2) projectileToMove.transform.localPosition;
            float angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectileToMove.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectileToMove.transform.position = Vector2.MoveTowards(projectileToMove.transform.localPosition, projectileToMove.TargetPoint, projectileToMove.Speed * Time.deltaTime);
            
            if (projectileToMove == null || Vector2.Distance(projectileToMove.TargetPoint,projectileToMove.transform.position) < 0.3f) {
                Destroy(projectileToMove.gameObject);
            }
            yield return null;
        }
        
    }

    private float getDistanceToTarget(Vector2 targetPosition) {
        return Mathf.Abs(Vector2.Distance(transform.localPosition, targetPosition));
    }
}
