using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

	public static DialogueManager instance;

	private Queue<string> sentences;

	public Text dialogueText;
	public Text nameText;
	public GameObject dialogueContainer;

	private SceneController sceneController;

	[HideInInspector] public InteractDialogue interactable;

	public Animator dialogueBoxAnimator;
	public Animator dialogueCharacterAnimator;

	private void Awake() {
		if(instance != null) {
			Debug.LogWarning("More than one DialogueManager");
			Destroy(gameObject);
			return;
		}

		if (dialogueContainer != null) dialogueContainer.SetActive(false);

		instance = this;
	}

	void Start() {
		sentences = new Queue<string>();

		dialogueText.text = "";

		nameText.text = "";

		sceneController = SceneController.instance;
	}

	public IEnumerator StartDialogue(Dialogue dialogue) {

		if (dialogueContainer != null) dialogueContainer.SetActive(true);

		if(dialogueBoxAnimator != null)
			dialogueBoxAnimator.SetBool("IsOpen", true);

		if(dialogueCharacterAnimator != null)
			dialogueCharacterAnimator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;
		dialogueText.text = "";

		sentences.Clear();

		foreach(string sentence in dialogue.sentences) {
			sentences.Enqueue(sentence);
		}

		yield return new WaitForSeconds(1f);

		DisplayNextSentence();
	}

	public void DisplayNextSentence() {
		if(sentences.Count == 0) {
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();

		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence) {

		dialogueText.text = "";

		foreach(char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return null;
			yield return null;
		}
	}

	void EndDialogue() {
		if (dialogueContainer != null) dialogueContainer.SetActive(false);

		if (interactable != null) interactable.hasInteracted = false;

		if(dialogueBoxAnimator != null)
			dialogueBoxAnimator.SetBool("IsOpen", false);

		if(dialogueCharacterAnimator != null)
			dialogueCharacterAnimator.SetBool("IsOpen", false);

		if (interactable != null && interactable.battleSceneToLoad != null) {
			sceneController.LoadBattleScene(interactable.battleSceneToLoad);
		}
	}
}
