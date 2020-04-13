﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{

	public GameObject turnOrderTracker;

	public TurnOrderTrackerObject turnOrderTrackerObject;

	public Text battleText;
	public ScrollRect scrollRect;
	public GameObject cancelTargetingButton;
	public GameObject attackButton;
	public GameObject[] skillButtons;
	public Text[] skillTexts;

	private void Awake() {
		battleText.text = "Battle start.";
		ClearUI(false);
	}

	public void SetupTurnOrderTrackerForEntity(BattleEntityController battleEntity) {

		TurnOrderTrackerObject trackerObject = Instantiate(turnOrderTrackerObject, turnOrderTracker.transform).GetComponent<TurnOrderTrackerObject>();

		trackerObject.Setup(battleEntity);

		battleEntity.turnTracker = trackerObject;
	}

	public void SetupUIForPlayer(BattleEntityController player) {

		cancelTargetingButton.SetActive(false);
		attackButton.SetActive(true);

		foreach (GameObject button in skillButtons) {
			button.SetActive(false);
		}

		for (int i = 0; i < player.battleEntity.skills.Count; i++) {
			skillButtons[i].SetActive(true);
			skillTexts[i].text = player.battleEntity.skills[i].name;
		}
	}

	public void ClearUI(bool showCancel) {

		cancelTargetingButton.SetActive(showCancel);
		attackButton.SetActive(false);

		foreach (GameObject button in skillButtons) {
			button.SetActive(false);
		}
	}

	public void DisplayEndScreen(bool hasWon) {
		return;
	}

	public void LogUpdate(string text) {
		battleText.text += text;
		StartCoroutine(UpdateScrollPosition());
	}


	IEnumerator UpdateScrollPosition() {
		yield return new WaitForEndOfFrame();
		scrollRect.verticalNormalizedPosition = 0f;
	}
}
