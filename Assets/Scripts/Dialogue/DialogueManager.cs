using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	private Queue<string> sentences;

	public Text dialogueText;
	public Text nameText;

	public Animator dialogueBoxAnimator;
	public Animator dialogueCharacterAnimator;

	void Start() {
		sentences = new Queue<string>();

		dialogueText.text = "";
		nameText.text = "";
	}

	public IEnumerator StartDialogue(Dialogue dialogue) {

		dialogueBoxAnimator.SetBool("IsOpen", true);
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
		}
	}

	void EndDialogue() {
		dialogueBoxAnimator.SetBool("IsOpen", false);
		dialogueCharacterAnimator.SetBool("IsOpen", false);

		Debug.Log("End");
	}
}
