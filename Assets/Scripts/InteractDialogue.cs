using UnityEngine;

public class InteractDialogue : Interactable
{

	public Dialogue dialogue;

	public override void Interact() {
		base.Interact();

		DialogueManager.instance.interactable = this;

		StartCoroutine(DialogueManager.instance.StartDialogue(dialogue));
	}
}
