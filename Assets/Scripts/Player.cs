using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private InputAction move;
    private Vector2 moveDirection;
    [SerializeField] private float speed = 0;

    private void Awake() {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }
    private void FixedUpdate() {
        GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
    }
}
