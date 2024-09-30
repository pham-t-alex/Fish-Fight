using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jumpAction;
    private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private float speed = 0;
    [SerializeField] private float jump = 0;
    [SerializeField] private int jumpCount = 0;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private int health = 100;
    private Rigidbody2D rb;

    private void Awake() {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
        jumpAction = playerControls.Player.Jump;
        jumpAction.started += Jump;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = move.ReadValue<Vector2>().x;
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
}
