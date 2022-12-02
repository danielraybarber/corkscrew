using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public int speed;
    public Rigidbody2D playerRb;
    public Animator playerAnim;

    public float hInput;
    public float vInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        AnimateMovement();
    }

    void FixedUpdate(){
        playerRb.velocity = new Vector2(hInput * speed, vInput * speed);      
    }

    private void AnimateMovement (){
        playerAnim.SetFloat("Horizontal", playerRb.velocity.x);
        playerAnim.SetFloat("Vertical", playerRb.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1){
            playerAnim.SetFloat("lastX", Input.GetAxisRaw("Horizontal"));
            playerAnim.SetFloat("lastY", Input.GetAxisRaw("Vertical"));
        }
    }
}
