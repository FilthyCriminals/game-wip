using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderTrackerObject : MonoBehaviour
{
	public Image characterSprite;
	public Image background;
	public Image activeBorder;

	public void Setup(BattleEntityController entityController) {
		characterSprite.sprite = entityController.battleEntity.sprite;

		background.color = entityController.isPlayerTeam ? Color.green : Color.red;
	}

	public void SetActive(bool isActive) {
		activeBorder.enabled = isActive;
	}
}
