using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	PlayerInputActions inputActions;
	Vector2 movementInput;

	public float moveSpeed = 5f;
	public Rigidbody2D rb;
	public Animator animator;

	private void Awake() {
		inputActions = new PlayerInputActions();
		inputActions.PlayerControls.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
	}

	private void Update() {
		animator.SetFloat("Horizontal", movementInput.x);
		animator.SetFloat("Vertical", movementInput.y);
		animator.SetFloat("Speed", movementInput.sqrMagnitude);
	}

	private void FixedUpdate() {
		rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
	}

	private void OnEnable() {
		inputActions.Enable();
	}

	private void OnDisable() {
		inputActions.Disable();
	}
}
