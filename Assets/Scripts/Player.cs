using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player instance;

	Vector2 movementInput;

	public float moveSpeed = 5f;
	public Rigidbody2D rb;
	public Animator animator;
	public Interactable closestInteractable;

	private void Awake() {
		if(instance != null) {
			Debug.LogWarning("More than one Player");
			Destroy(gameObject);
			return;
		}

		instance = this;
	}

	private void Update() {

		movementInput.x = Input.GetAxisRaw("Horizontal");
		movementInput.y = Input.GetAxisRaw("Vertical");

		animator.SetFloat("Horizontal", movementInput.x);
		animator.SetFloat("Vertical", movementInput.y);
		animator.SetFloat("Speed", movementInput.sqrMagnitude);

		if(Input.GetKey(KeyCode.Space) && closestInteractable != null) {
			closestInteractable.Interact();
		}
	}

	private void FixedUpdate() {
		Debug.Log(movementInput);

		rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
	}
}
