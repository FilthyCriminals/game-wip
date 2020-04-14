using UnityEngine;

public class Interactable : MonoBehaviour {

	public float radius = 0.5f;

	private Player player;

	[HideInInspector]
	public bool canInteract = false;
	public bool hasInteracted = false;

	private void Start() {
		player = Player.instance;
	}

	void Update() {

		float distance = Vector3.Distance(player.transform.position, transform.position);

		if (distance < radius && (player.closestInteractable == null ||
			distance < Vector3.Distance(player.transform.position, player.closestInteractable.transform.position))) {

			canInteract = true;
			player.closestInteractable = this;

		} else if(canInteract) {
			canInteract = false;
			if (player.closestInteractable == this) player.closestInteractable = null;
		}
	}

	public virtual void Interact() {
		if (hasInteracted) return;

		hasInteracted = true;
		Debug.Log("Interacting with " + gameObject.name);
	}

	private void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
