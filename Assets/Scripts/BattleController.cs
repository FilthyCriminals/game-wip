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

	public List<BattleEntity> battleEntities = new List<BattleEntity>();

	private System.Random rand = new System.Random();

	private int turnOrderIndex = 0;
	private int roundCounter = 0;
	private BattleState battleState;
	private BattleEntity player;
	private BattleEntity enemy;

	void Start() {
		// Spawn player and enemy
		player = SpawnEntity(true);
		battleEntities.Add(player);

		enemy = SpawnEntity(false);
		battleEntities.Add(enemy);

		// Randomize turn order
		battleEntities = battleEntities.OrderBy(x => rand.Next()).ToList<BattleEntity>();

		battleState = BattleState.START;
		battleText.text = "Start!";
		NextTurn();
	}

	private BattleEntity SpawnEntity(bool isPlayerTeam) {
		Vector3 position;
		Transform entityTransform;
		BattleEntity entity;

		if (isPlayerTeam) {
			position = new Vector3(-3, 0);
		} else {
			position = new Vector3(3, 0);
		}

		entityTransform = Instantiate(battleEntityPrefab, position, Quaternion.identity);
		entity = entityTransform.GetComponent<BattleEntity>();

		entity.isPlayerTeam = isPlayerTeam;

		if (!isPlayerTeam)
			PopulateEnemyStats(entity);

		return entity;
	}

	private void PopulateEnemyStats(BattleEntity enemy) {
		enemy.maxHealth = rand.Next(90, 100);
		enemy.currentHealth = enemy.maxHealth;

		enemy.minAttackDamage = 1;
		enemy.maxAttackDamage = 2;
	}


	private void NextTurn() {
		if(battleState != BattleState.START)
			turnOrderIndex = (++roundCounter) % battleEntities.Count;

		BattleEntity entity = battleEntities[turnOrderIndex];

		if (entity.isPlayerTeam) {
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
		int damage = rand.Next(player.minAttackDamage, player.maxAttackDamage + 1);
		bool isDead = enemy.TakeDamage(damage);

		battleText.text = "Turn " + roundCounter + " player did: " + damage + " damage with attack!";
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
		int damage = rand.Next(player.minSkillDamage, player.maxSkillDamage + 1);
		bool isDead = enemy.TakeDamage(damage);

		battleText.text = "Turn " + roundCounter + " player did: " + damage + " damage with skill!";
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
			if (!entity.isPlayerTeam && entity.currentHealth > 0)
				isEnemyAlive = true;
		});

		return isEnemyAlive;
	}

	private bool IsPlayerAlive() {
		bool isPlayerAlive = false;

		battleEntities.ForEach(entity => {
			if (entity.isPlayerTeam && entity.currentHealth > 0)
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

		int damage = rand.Next(enemy.minAttackDamage, enemy.maxAttackDamage + 1);
		bool isDead = player.TakeDamage(damage);

		battleText.text = "Turn " + roundCounter + " enemy did: " + damage + " damage with attack!";
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
