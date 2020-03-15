using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Vector2 movementInput;

	public float moveSpeed = 5f;
	public Rigidbody2D rb;
	public Animator animator;

	private void Update() {

		movementInput.x = Input.GetAxisRaw("Horizontal");
		movementInput.y = Input.GetAxisRaw("Vertical");

		animator.SetFloat("Horizontal", movementInput.x);
		animator.SetFloat("Vertical", movementInput.y);
		animator.SetFloat("Speed", movementInput.sqrMagnitude);
	}

	private void FixedUpdate() {
		Debug.Log(movementInput);

		rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
	}
}
