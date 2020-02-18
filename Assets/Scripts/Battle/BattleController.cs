﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {
	START,
	PLAYERTURN,
	ENEMYTURN,
	WAITING,
	WON,
	LOST
}

public class BattleController : MonoBehaviour {
	[SerializeField] private Transform battleEntityPrefab;
	public Text battleText;

	public List<BattleEntityController> battleEntities = new List<BattleEntityController>();

	private System.Random rand = new System.Random();

	private int turnOrderIndex = 0;
	private int roundCounter = 0;
	private BattleState battleState;
	private BattleEntityController player;
	private BattleEntityController enemy;

	void Start() {
		// Spawn player and enemy
		player = SpawnEntity(true);
		battleEntities.Add(player);

		enemy = SpawnEntity(false);
		battleEntities.Add(enemy);

		// Randomize turn order
		battleEntities = battleEntities.OrderBy(x => rand.Next()).ToList<BattleEntityController>();

		battleState = BattleState.START;
		battleText.text = "Start!";
		NextTurn();
	}

	private BattleEntityController SpawnEntity(bool isPlayerTeam) {
		Vector3 position;
		BattleEntityController entityController;
		BattleEntity entity;

		if (isPlayerTeam) {
			position = new Vector3(-3, 0);
			entity = Resources.Load("BattleEntities/Players/Chris") as BattleEntity;
		} else {
			position = new Vector3(3, 0);
			entity = Resources.Load("BattleEntities/Enemies/Gobbo") as BattleEntity;
		}

		entityController = Instantiate(battleEntityPrefab, position, Quaternion.identity).GetComponent<BattleEntityController>();
		entityController.battleEntity = entity;

		return entityController;
	}

	private void NextTurn() {
		if(battleState != BattleState.START)
			turnOrderIndex = (++roundCounter) % battleEntities.Count;

		BattleEntityController entity = battleEntities[turnOrderIndex];

		if (entity.battleEntity.isPlayerTeam) {
			battleState = BattleState.PLAYERTURN;
			PlayerTurn();
		} else {
			battleState = BattleState.ENEMYTURN;
			EnemyTurn();
		}
	}

	private void PlayerTurn() {
		// Put setup logic for player turn here

		battleText.text = "Choose your action.";
	}

	public void OnAttackButton() {
		if (battleState != BattleState.PLAYERTURN) return;

		StartCoroutine(PlayerAttack());
	}

	public void OnSkillButton() {
		if (battleState != BattleState.PLAYERTURN) return;

		StartCoroutine(PlayerSkill());
	}

	public IEnumerator PlayerAttack() {
		int damage = rand.Next(player.battleEntity.minAttackDamage, player.battleEntity.maxAttackDamage + 1);
		bool isDead = enemy.TakeDamage(damage);

		battleText.text = "Turn " + roundCounter + " " + player.battleEntity.name + " did: " + damage + " damage with attack!";
		battleState = BattleState.WAITING;

		yield return new WaitForSeconds(1f);

		if (isDead && !IsEnemyAlive()) {
			battleState = BattleState.WON;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	public IEnumerator PlayerSkill() {
		int damage = rand.Next(player.battleEntity.minSkillDamage, player.battleEntity.maxSkillDamage + 1);
		bool isDead = enemy.TakeDamage(damage);

		battleText.text = "Turn " + roundCounter + " " + player.battleEntity.name + " did: " + damage + " damage with skill!";
		battleState = BattleState.WAITING;

		yield return new WaitForSeconds(1f);

		if (isDead && !IsEnemyAlive()) {
			battleState = BattleState.WON;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	private bool IsEnemyAlive() {
		bool isEnemyAlive = false;

		battleEntities.ForEach(entity => {
			if (!entity.battleEntity.isPlayerTeam && entity.currentHealth > 0)
				isEnemyAlive = true;
		});

		return isEnemyAlive;
	}

	private bool IsPlayerAlive() {
		bool isPlayerAlive = false;

		battleEntities.ForEach(entity => {
			if (entity.battleEntity.isPlayerTeam && entity.currentHealth > 0)
				isPlayerAlive = true;
		});

		return isPlayerAlive;
	}

	private void EnemyTurn() {
		if (battleState != BattleState.ENEMYTURN) return;

		StartCoroutine(EnemyAttack());
	}

	private IEnumerator EnemyAttack() {
		yield return new WaitForSeconds(1f);

		int damage = rand.Next(enemy.battleEntity.minAttackDamage, enemy.battleEntity.maxAttackDamage + 1);
		bool isDead = player.TakeDamage(damage);

		battleText.text = "Turn " + roundCounter + " " + enemy.battleEntity.name + " did: " + damage + " damage with attack!";
		battleState = BattleState.WAITING;

		yield return new WaitForSeconds(2f);

		if (isDead && !IsPlayerAlive()) {
			battleState = BattleState.LOST;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	private void EndBattle() {

		if(battleState == BattleState.WON) {
			battleText.text = "You won!";
		} else if (battleState == BattleState.LOST) {
			battleText.text = "You lost...";
		}
	}
}
