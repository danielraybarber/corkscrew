using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    
    public bool canWalk = false;
    public bool isWalking = false;
    public bool isCoroutineRunning; 
    public Vector3 newPoint;

    public float xlowRange;
    public float xhighRange;
    public float ylowRange;
    public float yhighRange;
    public Transform enemy;
    public float speed;
    public float rangeModifier;

    //public Object self;
    public float deathTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); 
        enemy = GetComponent<Transform>();

        xlowRange = enemy.position.x - 5f;
        xhighRange = enemy.position.x +5;
        ylowRange = enemy.position.y - 5f;
        yhighRange = enemy.position.y +5;
        
    }

    void FixedUpdate(){
        if(canWalk){
            if(isWalking) {
                EnemyWalk();
            }
            if(isCoroutineRunning == false){
                StartCoroutine(WalkCoroutine());
                isCoroutineRunning = true;
            }
            AnimateMovement();
        }
    }


    public void TakeDamage(int damage){
        currentHealth -= damage;

        //play hurt animation
        animator.SetTrigger("HurtEnemy");

        if(currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        //play death animation
        animator.SetBool("IsDead", true);
    }

    void SelfDestruct(){
        Destroy(gameObject, deathTime);
    }

    void EnemyWalk(){
        enemy.position = Vector3.MoveTowards(enemy.position, newPoint, speed*Time.deltaTime);
        if(enemy.position == newPoint){
            isWalking = false;
            isCoroutineRunning = false;
            GenerateWalkingPoint();
        }
    }

    void GenerateWalkingPoint(){
        newPoint = new Vector3(Random.Range(xlowRange, xhighRange), Random.Range(ylowRange, yhighRange), 0f);
    }
    IEnumerator WalkCoroutine(){

        //couroutine that sets ranges of the random coords
        xlowRange = newPoint.x - rangeModifier;
        xhighRange = newPoint.x + rangeModifier;
        ylowRange = newPoint.y - rangeModifier;
        yhighRange = newPoint.y + rangeModifier;
        
        //decides how long coroutine should wait between 3 and 10 seconds
        float random = Random.Range(3f, 10f);

        //waits random amount of seconds
        yield return new WaitForSeconds(random);
        
        //start walking to the coords generated
        isWalking = true;

    
    }
    void AnimateMovement(){
        
        //This math function always returns the distance between two points as a positive value
        //find the distance between the x points
        float xDist = Mathf.Abs(enemy.position.x - newPoint.x);
        //find the distance between the y points
        float yDist = Mathf.Abs(enemy.position.y - newPoint.y);


        //Animate Left/Right only
        if(xDist > yDist){
            //manimate Left
            if(enemy.position.x > newPoint.x){
                animator.SetFloat("Horizontal", -1f);
                animator.SetFloat("Vertical", 0f);
            }
            //animate right
            if(enemy.position.x < newPoint.x){
                animator.SetFloat("Horizontal", 1f);
                animator.SetFloat("Vertical", 0f);
            }
        }

        //animate up/down only
        if(yDist > xDist){
            //Animate Down
            if(enemy.position.y > newPoint.y){
                animator.SetFloat("Vertical", -1f);
                animator.SetFloat("Horizontal", 0f);
            }
            //Animate Up
            if(enemy.position.y < newPoint.y){
                animator.SetFloat("Vertical", 1f);
                animator.SetFloat("Horizontal", 0f);
            }

        }

        //super obscure case where the distance is exact. Just allow the animatior to decide
        if(yDist == xDist){
            //AnimateDown
            if(enemy.position.y > newPoint.y){
                animator.SetFloat("Vertical", -1f);
            }
            //Animate Up
            if(enemy.position.y < newPoint.y){
                animator.SetFloat("Vertical", 1f);
            }
            if(enemy.position.x > newPoint.x){
                animator.SetFloat("Horizontal", -1f);
            }
            //animate right
            if(enemy.position.x < newPoint.x){
                animator.SetFloat("Horizontal", 1f);
            }
        }

    //if they stop walking then stop animations
        if(isWalking == false){
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }


    }
}
