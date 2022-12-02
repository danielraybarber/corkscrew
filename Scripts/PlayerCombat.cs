using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
   
    // Using vars under to detect which way the player is attacking
    public Vector3 attackPosition;
    public MovementController mc;
    public Transform playerPosition;
    public float hOffset;
    public float vOffset;
    
    //When you swing horizontal what should attackPoints Y coordinate be
    public float hAttkYCord = -1f;
    //When you swing vertically what should the attackPoints X coord be;
    public float vAttkXCord = 0f;

    
    

    void Start(){
        attackPoint.position = playerPosition.position + new Vector3(0, -1, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Attack();
        }
    }

    void Attack(){
        //play an attack animation
        animator.SetTrigger("Attack");
    }

    void AttackEvent(){
        AttackDirection();
        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //damage them
        foreach(Collider2D hitbox in hitEnemies){
            hitbox.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }


     void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void AttackDirection(){
        //Facing Left
        if(mc.playerAnim.GetFloat("lastX") < 0 ){
            if(hOffset > 0){
                hOffset *= -1f;
            }
            attackPosition = new Vector3(playerPosition.position.x + mc.playerAnim.GetFloat("lastX") + hOffset, playerPosition.position.y + hAttkYCord, 0f);
        }

        //Facing Right
        if(mc.playerAnim.GetFloat("lastX") > 0 ){
            if(hOffset < 0){
                hOffset *= -1f;
            }
            attackPosition = new Vector3(playerPosition.position.x + mc.playerAnim.GetFloat("lastX") + hOffset, playerPosition.position.y + hAttkYCord, 0f);
        }

        //Facing Down
        if(mc.playerAnim.GetFloat("lastY") < 0 ){
            if(vOffset > 0){
                vOffset *= -1f;
            }
            attackPosition = new Vector3(playerPosition.position.x + vAttkXCord, playerPosition.position.y + mc.playerAnim.GetFloat("lastY") + vOffset, 0f);
        }

        //Facing Up
        if(mc.playerAnim.GetFloat("lastY") > 0 ){
            if(vOffset < 0){
                vOffset *= -1f;
            }
            attackPosition = new Vector3(playerPosition.position.x + vAttkXCord, playerPosition.position.y + mc.playerAnim.GetFloat("lastY") + vOffset, 0f);
        }

        attackPoint.position = attackPosition;
    }
}
