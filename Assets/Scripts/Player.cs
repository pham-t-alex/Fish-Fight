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
    [SerializeField] private int health = 100;
    [SerializeField] private float attackCooldown = 0.25f;
    private float timer = 0.0f;
    private Rigidbody2D rb;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private Fish fish;
    [SerializeField] private int fishUses;
    [SerializeField] private float fishExpiration;
    private bool movedRightLast = true; // by default, the player is facign towards the center, which would be right
    //private bool movedLeftLast = false;
    // all disabled colliders
    private HashSet<Collider2D> disabledColliders = new HashSet<Collider2D>();
    
    // tracks current objects colliding
    private HashSet<Collider2D> currentCollisions = new HashSet<Collider2D>();

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
        timer += Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        moveDirection.x = direction.x;
        if (moveDirection.x < 0) {
            //movedLeftLast = true;
            movedRightLast = false;
            Debug.Log("Last moved left");
        } else if (moveDirection.x > 0) {
            //movedLeftLast = false;
            movedRightLast = true;
            Debug.Log("Last moved right");
        }
        // be able to move through platform
        if (direction.y < 0 && Mathf.Abs(direction.y) >= Mathf.Abs(direction.x))
        {
            foreach (Collider2D collider in currentCollisions)
            {
                if (!disabledColliders.Contains(collider))
                {
                    Debug.Log("Disabling!");
                    StartCoroutine(DisableCollision(collider, 0.5f));
                }
            }
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
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Platform")
        {
            if (tag == "Platform")
            {
                currentCollisions.Add(collision.collider);
            }
            Vector3 normal = collision.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                jumpCount = maxJumps;
                Debug.Log("Jumps reset");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            currentCollisions.Remove(collision.collider);
        }
    }

    public void Attack(InputAction.CallbackContext context) {
        if (timer > attackCooldown) {
            GameObject attackRange = Instantiate(attackObject);
            timer = 0.0f;
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
    }
    public void Hurt(int damage) {
        this.health -= damage;
        if (health <= 0) {
            Debug.Log("I died. ;-;");
            Destroy(this.gameObject);
        }
    }

    private IEnumerator DisableCollision(Collider2D collider, float time)
    {
        disabledColliders.Add(collider);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
        disabledColliders.Remove(collider);
    }

    // returns whether pick up was successful
    public bool PickupFish(Fish f)
    {
        if (fish != null)
        {
            return false;
        }
        fish = f;
        return true;
    }
}
