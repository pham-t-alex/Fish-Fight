using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private float speed = 0;
    [SerializeField] private float jump = 0;
    [SerializeField] private int jumpCount = 0;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private double health = 100;
    private Rigidbody2D rb;
    [SerializeField] private GameObject attackObject;
    private bool movedRightLast = true; // by default, the player is facign towards the center, which would be right
    //private bool movedLeftLast = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection.x = context.ReadValue<Vector2>().x;
        if (moveDirection.x < 0) {
            //movedLeftLast = true;
            movedRightLast = false;
            Debug.Log("Last moved left");
        } else if (moveDirection.x > 0) {
            //movedLeftLast = false;
            movedRightLast = true;
            Debug.Log("Last moved right");
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && jumpCount > 0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jump);
            Debug.Log("Jumped!");
        }
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = collision.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                jumpCount = maxJumps;
                Debug.Log("Jumps reset");
            }
        }
    }
    public void Attack(InputAction.CallbackContext context) {
        GameObject attackRange = Instantiate(attackObject);
        if (movedRightLast) { // moved right last
            //GameObject attackRange = Instantiate(attackObject);
            attackRange.transform.position = new Vector2((this.transform.position.x + 1), this.transform.position.y);
            Debug.Log("Instantiated attack to the right!");
        } else { // moved left last
            //GameObject attackRange = Instantiate(attackObject);
            attackRange.transform.position = new Vector2((this.transform.position.x - 1), this.transform.position.y);
            Debug.Log("Instantiated attack to the left!");
        }
        Destroy(attackRange, 0.5f /* This number is how long the attack will last*/);
    }
    public void Hurt(double damage) {
        damage /= 3;
        this.health -= damage;
        if (health <= 0) {
            Debug.Log("I died. ;-;");
            Destroy(this.gameObject);
        }
    }
}
