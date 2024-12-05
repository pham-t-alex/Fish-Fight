using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
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
    [SerializeField] private float xDirectionDash = 800.0f;
    [SerializeField] private float yDirectionDash = 10.0f;
    public int Health
    {
        get
        {
            return health;
        }
    }

    private bool moving = false;

    [SerializeField] private float attackCooldown = 0.25f; // how much cooldown time there is between attacks
    private float attackCooldownTimer = 0.0f; // a timer that keeps track of when another attack can be initiated

    //[SerializeField] private float stunDuration = 0.25f; // how much time a character is stunned after an attack
    private float stunnedTimer = 0.0f; // a timer keeping track of when another attack can be initiated. 
                                       // if the timer isn't zero, then the player is stunned.
    public bool Stunned
    {
        get
        {
            return stunnedTimer > 0;
        }
    }
    [SerializeField] private float useBlockTimeLimit = 3.0f;

    private float blockingTimer = 0;
    
    private float busyTimer = 0;
    public bool Busy
    {
        get
        {
            return busyTimer > 0;
        }
    }
    
    private enum Action
    {
        None,
        Attack,
        Use,
        Throw
    }
    private Action action = Action.None;
    [SerializeField] private float attackDelay = 0.2f; // delay for attacks
    [SerializeField] private float useDelay = 0.2f; // delay for fish use (could vary from fish to fish)
    [SerializeField] private float throwDelay = 0.2f; // delay for throw
    private float actionDelayTimer = 0;
    public bool WaitingToAct
    {
        get
        {
            return actionDelayTimer > 0;
        }
    }

    [SerializeField] private float dashTime = 2f;

    private Rigidbody2D rb;
    [SerializeField] private GameObject attackObject;
    private bool blocking = false;
    [SerializeField] private GameObject counterObject;
    [SerializeField] private Fish fish;
    [SerializeField] private int fishUses;
    [SerializeField] private float fishExpiration;
    private bool movedRightLast = true; // by default, the player is facign towards the center, which would be right
    public bool MovedRightLast
    {
        get
        {
            return movedRightLast;
        }
    }
    //private bool movedLeftLast = false;
    // all disabled colliders
    private HashSet<Collider2D> disabledColliders = new HashSet<Collider2D>();
    
    // tracks current objects colliding
    private HashSet<Collider2D> currentCollisions = new HashSet<Collider2D>();

    public delegate void HealthChangeEventHandler(int health);
    public event HealthChangeEventHandler HealthChangeEvent;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BattleUI.Instance.AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownTimer += Time.deltaTime;
        if (stunnedTimer < 0) stunnedTimer = 0; // fixes a bug where the stunnedTimer goes below 0. This compensates for that case.
        
        if (stunnedTimer > 0) {
            stunnedTimer -= Time.deltaTime;
            //Debug.Log("Stunned timer: " + stunnedTimer);
        }

        if (busyTimer > 0)
        {
            busyTimer -= Time.deltaTime;
            if (busyTimer <= 0)
            {
                if (!Stunned)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                busyTimer = 0;
            }
        }

        if (fishExpiration > 0)
        {
            fishExpiration -= Time.deltaTime;
            if (fishExpiration <= 0 && !WaitingToAct)
            {
                fish = null;
                GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.255f);
            }
        }

        if (actionDelayTimer > 0)
        {
            actionDelayTimer -= Time.deltaTime;
            if (actionDelayTimer <= 0)
            {
                TriggerAction();
            }
        }
        if (!blocking) blockingTimer = 0;
        else blockingTimer += Time.deltaTime;

        if (blockingTimer > useBlockTimeLimit) {
            blocking = false;
            Debug.Log("blocking changed to false");
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (moving)
            {
                moving = false;
                InterruptMovement();
            }
            return;
        }
        moving = true;
        Vector2 direction = context.ReadValue<Vector2>();
        moveDirection.x = direction.x;
        if (moveDirection.x < 0)
        {
            //movedLeftLast = true;
            movedRightLast = false;
            Debug.Log("Last moved left");
        }
        else if (moveDirection.x > 0)
        {
            //movedLeftLast = false;
            movedRightLast = true;
            Debug.Log("Last moved right");
        }
        if (WaitingToAct || Busy || Stunned || blocking)
        {
            return;
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

    public void InterruptMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        return;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (WaitingToAct || Busy || Stunned || blocking)
        {
            return;
        }
        if (context.started && jumpCount > 0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            Debug.Log("Jumped!");
        }
    }

    private void FixedUpdate() {
        if (moving && !(WaitingToAct || Busy || Stunned || blocking)) {
            if (blocking) {
                rb.velocity = new Vector2(moveDirection.x * speed / 2, rb.velocity.y);
                //Debug.Log("speed: " + (speed / 2));
            } else {
                rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
                //Debug.Log("speed: " + speed);
            }
        }
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

    public void TriggerAction()
    {
        if (Busy || Stunned || blocking)
        {
            return;
        }
        switch (action)
        {
            case Action.Attack:
                GameObject attackRange = Instantiate(attackObject);

                //attackRange.setDirectionFacing(movedRightLast);
                if (attackRange.TryGetComponent(out AttackArea attackArea))
                {
                    attackArea.setDirectionFacing(movedRightLast);
                    attackArea.ThisObjectCreator(this);
                }

                if (movedRightLast)
                { // moved right last
                //GameObject attackRange = Instantiate(attackObject);
                    attackRange.transform.position = new Vector2((this.transform.position.x + 1), this.transform.position.y);
                    Debug.Log("Instantiated attack to the right!");
                }
                else
                { // moved left last
                //GameObject attackRange = Instantiate(attackObject);
                    attackRange.transform.position = new Vector2((this.transform.position.x - 1), this.transform.position.y);
                    Debug.Log("Instantiated attack to the left!");
                }
                Destroy(attackRange, 0.1f /* This number is how long the attack will last*/);
                break;

            case Action.Use:
                fish.Use();
                fishUses--;
                if (fishUses <= 0 || fishExpiration <= 0)
                {
                    fishUses = 0;
                    fish = null;
                    GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.255f);
                }
                break;

            case Action.Throw:
                fish.Throw();
                fishUses = 0;
                fish = null;
                GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.255f);
                break;
        }
    }

    public void Attack(InputAction.CallbackContext context) {
        if (WaitingToAct || Busy || Stunned || blocking)
        {
            return;
        }
        if (context.started && attackCooldownTimer > attackCooldown) {
            InterruptMovement();
            attackCooldownTimer = 0.0f;

            if (fish != null)
            {
                action = Action.Use;
                actionDelayTimer = useDelay;
            }
            else
            {
                action = Action.Attack;
                actionDelayTimer = attackDelay;
            }
        }
        Debug.Log("hm 2");
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (WaitingToAct || Busy || Stunned || blocking)
        {
            return;
        }
        if (context.started && fish != null)
        {
            InterruptMovement();
            action = Action.Throw;
            actionDelayTimer = throwDelay;
        }
    }

    public void Counter(InputAction.CallbackContext context)
    {
        if (WaitingToAct || Busy || Stunned || blocking)
        {
            return;
        }
        if (context.started)
        {
            InterruptMovement();
            GameObject counter = Instantiate(counterObject);

            if (counter.TryGetComponent(out CounterArea counterArea))
            {
                counterArea.setDirectionFacing(movedRightLast);
                counterArea.ThisObjectCreator(this);
            }

            if (movedRightLast)
            { // moved right last
              //GameObject attackRange = Instantiate(attackObject);
                counter.transform.position = new Vector2((this.transform.position.x + 1), this.transform.position.y);
                Debug.Log("Instantiated attack to the right!");
            }
            else
            { // moved left last
              //GameObject attackRange = Instantiate(attackObject);
                counter.transform.position = new Vector2((this.transform.position.x - 1), this.transform.position.y);
                Debug.Log("Instantiated attack to the left!");
            }
            Destroy(counter, 0.5f /* This number is how long the attack will last*/);
        }
    }

    public void Disarm()
    {
        action = Action.None;
        actionDelayTimer = 0;
        fishUses = 0;
        fish = null;
        GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.255f);
    }

    public void Hurt(int damage, Vector2 knockback) {
        //Debug.Log("Damage taken: " + damage);
        if (!blocking) {
            this.health -= damage;
            HealthChangeEvent(health);
            if (health <= 0) {
                Debug.Log("I died ;-;");
                Destroy(this.gameObject);
            }
            InterruptMovement();
            rb.velocity = Vector2.zero;
            rb.AddForce(knockback, ForceMode2D.Impulse);
            //moveDirection.x = knockback;
        } else {
            rb.AddForce(new Vector2(knockback.x / 2.0f, knockback.y / 2.0f), ForceMode2D.Impulse); // if blocking is true, there is still knockback, but less than the knockback vector
        }
    }
    public void Block(InputAction.CallbackContext context) {
        if (WaitingToAct || Busy || Stunned)
        {
            return;
        }
        if (context.started)
        {
            blocking = true;
            Debug.Log("blocking true");
            InterruptMovement();
        } else if (context.canceled) {
            blocking = false;
            Debug.Log("blocking false");
        }
    }
    public void Stun(float stunDuration)
    {
        if (blocking) stunDuration /= 2;
        //Debug.Log("Stun duration: " + stunDuration);
        stunnedTimer = Mathf.Max(stunnedTimer, stunDuration);
    }
    public void Dash(InputAction.CallbackContext context) {
        if (WaitingToAct || Busy || Stunned || blocking)
        {
            return;
        }
        if (context.started) {
            InterruptMovement();
            busyTimer = dashTime;
            Debug.Log("Dash initiated");
            if (movedRightLast) rb.AddForce(new Vector2(xDirectionDash, yDirectionDash), ForceMode2D.Impulse);
            else rb.AddForce(new Vector2(xDirectionDash * -1, yDirectionDash), ForceMode2D.Impulse);
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
        fishUses = f.GetMaxUses();
        fishExpiration = f.GetMaxTime();
        f.SetPlayer(this);
        GetComponent<SpriteRenderer>().color = new Color(0, 1, 1);
        return true;
    }
}
