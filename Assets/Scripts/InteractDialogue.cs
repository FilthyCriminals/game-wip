using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractDialogue : Interactable
{

	public Dialogue dialogue;
	public string battleSceneToLoad;

	public override void Interact() {
		base.Interact();

		DialogueManager.instance.interactable = this;

		StartCoroutine(DialogueManager.instance.StartDialogue(dialogue));
	}
}
