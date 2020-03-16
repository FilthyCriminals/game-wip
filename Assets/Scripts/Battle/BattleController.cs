using System.Collections;
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
	[SerializeField] private GameObject turnOrderTrackerObject;

	public Text battleText;
	public Transform turnOrderTracker;

	public List<BattleEntityController> battleEntities = new List<BattleEntityController>();

	private System.Random rand = new System.Random();

	private int turnOrderIndex = 0;
	private int roundCounter = 0;
	private BattleState battleState;
	private BattleEntityController player;
	private BattleEntityController enemy;
	private BattleEntityController currentEntity;
	private Coroutine typeText;

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

		SetupTurnOrderTrackerForEntity(entityController);

		return entityController;
	}

	private void SetupTurnOrderTrackerForEntity(BattleEntityController battleEntity) {

		TurnOrderTrackerObject trackerObject = Instantiate(turnOrderTrackerObject, turnOrderTracker).GetComponent<TurnOrderTrackerObject>();

		trackerObject.Setup(battleEntity.battleEntity);

		battleEntity.turnTracker = trackerObject;
	}

	private void NextTurn() {
		if(battleState != BattleState.START)
			turnOrderIndex = (++roundCounter) % battleEntities.Count;

		if(currentEntity) currentEntity.turnTracker.SetActive(false);

		currentEntity = battleEntities[turnOrderIndex];

		currentEntity.turnTracker.SetActive(true);

		if (currentEntity.battleEntity.isPlayerTeam) {
			PlayerTurn();
		} else {
			battleState = BattleState.ENEMYTURN;
			EnemyTurn();
		}
	}

	private void PlayerTurn() {
		// Put setup logic for player turn here
		// Such as changing the skills available for this character
		// Or for determining status effects

		SetBattleText("Choose your action.");
		battleState = BattleState.PLAYERTURN;
	}

	private void SetBattleText(string text) {
		if (typeText != null) {
			StopCoroutine(typeText);
			typeText = null;
		}

		battleText.text = "";
		typeText = StartCoroutine(TypeText(text));
	}

	IEnumerator TypeText(string text) {
		foreach (char character in text.ToCharArray()) {
			battleText.text += character;
			yield return null;
			yield return null;
		}
	}

	public void OnAttackButton() {
		if (battleState != BattleState.PLAYERTURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerAttack());
	}

	public void OnSkillButton() {
		if (battleState != BattleState.PLAYERTURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(0));
	}

	public void OnSkillButton1() {
		if (battleState != BattleState.PLAYERTURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(1));
	}

	public void OnSkillButton2() {
		if (battleState != BattleState.PLAYERTURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(2));
	}

	public void OnSkillButton3() {
		if (battleState != BattleState.PLAYERTURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(3));
	}

	public IEnumerator PlayerAttack() {
		int damage = rand.Next(player.battleEntity.minAttackDamage, player.battleEntity.maxAttackDamage + 1);
		bool isDead = enemy.TakeDamage(damage);
		SetBattleText("Turn " + roundCounter + " " + player.battleEntity.name + " did: " + damage + " damage with attack!");

		yield return new WaitForSeconds(1f);

		if (isDead && !IsEnemyAlive()) {
			battleState = BattleState.WON;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	public IEnumerator PlayerSkill(int numSkill) {

		//player.battleEntity.skills[numSkill].Cast(enemy);

		//int damage = rand.Next(player.battleEntity.minSkillDamage, player.battleEntity.maxSkillDamage + 1);
		//bool isDead = enemy.TakeDamage(damage);

		SetBattleText("Turn " + roundCounter + " " + player.battleEntity.name + " cast: " + player.battleEntity.skills[0].name + "!");

		int healthBefore = enemy.currentHealth;

		yield return StartCoroutine(player.battleEntity.skills[numSkill].Cast(player, enemy));

		SetBattleText("Turn " + roundCounter + " " + player.battleEntity.name + " did: " + (healthBefore - enemy.currentHealth) + " damage with " + player.battleEntity.skills[0].name + "!");

		yield return new WaitForSeconds(0.5f);

		if (!IsEnemyAlive()) {
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
		battleState = BattleState.WAITING;

		StartCoroutine(EnemyAttack());
	}

	private IEnumerator EnemyAttack() {
		yield return new WaitForSeconds(1f);

		int damage = rand.Next(enemy.battleEntity.minAttackDamage, enemy.battleEntity.maxAttackDamage + 1);
		bool isDead = player.TakeDamage(damage);

		SetBattleText("Turn " + roundCounter + " " + enemy.battleEntity.name + " did: " + damage + " damage with attack!");

		yield return new WaitForSeconds(2f);

		if (isDead && !IsPlayerAlive()) {
			battleState = BattleState.LOST;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	private void EndBattle() {

		currentEntity.turnTracker.SetActive(false);

		if(battleState == BattleState.WON) {
			battleText.text = "You won!";
		} else if (battleState == BattleState.LOST) {
			battleText.text = "You lost...";
		}
	}
}
