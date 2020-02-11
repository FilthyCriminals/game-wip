using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour {
	[SerializeField] private Transform battleEntityPrefab;

	public List<BattleEntity> battleEntities = new List<BattleEntity>();

	private System.Random rand = new System.Random();

	private bool isPlayerTurn = false;
	private int turnOrderIndex = 0;
	private int roundCounter = 0;
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
	}

	void Update() {
		if (isPlayerTurn) return;

		BattleEntity entity = battleEntities[turnOrderIndex];
		turnOrderIndex = (++roundCounter) % battleEntities.Count;

		if (entity.isPlayerTeam) {
			isPlayerTurn = true;
		} else {
			EnemyAttack();
		}
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

	public void PlayerAttack() {
		if (!isPlayerTurn) return;

		int damage = rand.Next(player.minAttackDamage, player.maxAttackDamage + 1);
		enemy.TakeDamage(damage);

		Debug.Log("Turn " + roundCounter + ": player did: " + damage + " damage with attack!");

		isPlayerTurn = false;
	}

	public void PlayerSkill() {
		if (!isPlayerTurn) return;

		int damage = rand.Next(player.minSkillDamage, player.maxSkillDamage + 1);
		enemy.TakeDamage(damage);

		Debug.Log("Turn " + roundCounter + ": player did: " + damage + " damage with skill!");

		isPlayerTurn = false;
	}

	private void EnemyAttack() {
		if (isPlayerTurn) return;

		int damage = rand.Next(enemy.minAttackDamage, enemy.maxAttackDamage + 1);
		player.TakeDamage(damage);

		Debug.Log("Turn " + roundCounter + ": enemy did: " + damage + " damage with attack!");
	}

	private void EnemySkill() {
		if (isPlayerTurn) return;

		int damage = rand.Next(enemy.minSkillDamage, enemy.maxSkillDamage + 1);
		player.TakeDamage(damage);

		Debug.Log("Turn " + roundCounter + ": enemy did: " + damage + " damage with skill!");
	}
}
