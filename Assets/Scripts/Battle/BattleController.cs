using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {
	START,
	PLAYER_TURN,
	TARGETING,
	ENEMY_TURN,
	WAITING,
	WON,
	LOST
}

public class BattleController : MonoBehaviour {
	[SerializeField] private Transform battleEntityPrefab;
	[SerializeField] private GameObject turnOrderTrackerObject;

	public Text battleText;
	public BattleUI battleUI;
	private EnemyContainer enemyContainer;

	public List<BattleEntityController> battleEntities = new List<BattleEntityController>();
	public List<BattleEntityController> players = new List<BattleEntityController>();
	public List<BattleEntityController> enemies = new List<BattleEntityController>();

	private System.Random rand = new System.Random();

	public int minNumEnemies = 1;
	public int maxNumEnemies = 5;
	public float entitySpacing = 2.5f;

	private int turnOrderIndex = 0;
	private int roundCounter = 0;
	public BattleState battleState;
	private BattleEntityController player;
	private BattleEntityController enemy;
	private BattleEntityController target;
	private BattleEntityController currentEntity;
	private Coroutine typeText;

	void Start() {

		enemyContainer = GetComponent<EnemyContainer>();

		// Spawn player and enemy
		SpawnPlayers();
		SpawnEnemies();

		// Randomize turn order
		battleEntities = battleEntities.OrderBy(x => rand.Next()).ToList<BattleEntityController>();

		foreach(BattleEntityController entityController in battleEntities) {
			battleUI.SetupTurnOrderTrackerForEntity(entityController);
		}

		battleState = BattleState.START;
		battleText.text = "Start!";
		NextTurn();
	}

	private void SpawnPlayers() {
		int numPlayers = 0;

		foreach (BattleEntity _entity in CharacterManager.instance.battleEntities) {
			if (_entity != null) numPlayers++;
		}

		BattleEntity entity;

		float startingPosition = (entitySpacing / 2) * (numPlayers - 1);
		for (int i = 0; i < numPlayers; i++) {
			entity = CharacterManager.instance.battleEntities[i];

			if (entity != null) {
				BattleEntityController tmp = SpawnEntity(entity, new Vector3(-3, startingPosition - i * entitySpacing), true);
				battleEntities.Add(tmp);
				players.Add(tmp);
			}
		}
	}

	private void SpawnEnemies() {
		int numEnemies = rand.Next(minNumEnemies, maxNumEnemies);
		BattleEntity entity;

		float startingPosition = (entitySpacing / 2) * (numEnemies - 1);
		for (int i = 0; i < numEnemies; i++) {
			entity = enemyContainer.enemies[rand.Next(enemyContainer.enemies.Length)];

			BattleEntityController tmp = SpawnEntity(entity, new Vector3(3, startingPosition - i * entitySpacing), false);
			battleEntities.Add(tmp);
			enemies.Add(tmp);
		}
	}

	private BattleEntityController SpawnEntity(BattleEntity entity, Vector3 position, bool isPlayerTeam) {
		BattleEntityController entityController;

		entityController = Instantiate(battleEntityPrefab, position, Quaternion.identity).GetComponent<BattleEntityController>();
		entityController.battleEntity = entity;
		entityController.isPlayerTeam = isPlayerTeam;
		entityController.battleController = this;

		return entityController;
	}


	// BattleState WAITING
	private void NextTurn() {
		if(battleState != BattleState.START)
			turnOrderIndex = (++roundCounter) % battleEntities.Count;

		if(currentEntity) currentEntity.turnTracker.SetActive(false);

		currentEntity = battleEntities[turnOrderIndex];

		currentEntity.turnTracker.SetActive(true);

		// Skip this turn
		if (currentEntity.status == StatusEffect.STUN) {
			currentEntity.statusDuration -= 1;

			if (currentEntity.statusDuration == 0) currentEntity.status = StatusEffect.NONE;

			StartCoroutine(NextTurnWithText(currentEntity.battleEntity.name + " is stunned!"));
			return;
		}

		if (currentEntity.isPlayerTeam) {
			player = currentEntity;
			PlayerTurn();
		} else {
			enemy = currentEntity;
			battleState = BattleState.ENEMY_TURN;
			EnemyTurn();
		}
	}

	IEnumerator NextTurnWithText(string text) {
		yield return SetBattleText(text);
		yield return new WaitForSeconds(1.5f);
		NextTurn();
	}

	private void PlayerTurn() {
		// Put setup logic for player turn here
		// Such as changing the skills available for this character
		// Or for determining status effects

		battleUI.SetupUIForPlayer(player);

		SetBattleText("Choose your action.");
		battleState = BattleState.PLAYER_TURN;
	}

	private Coroutine SetBattleText(string text) {
		if (typeText != null) {
			StopCoroutine(typeText);
			typeText = null;
		}

		battleText.text = "";
		typeText = StartCoroutine(TypeText(text));
		return typeText;
	}

	IEnumerator TypeText(string text) {
		foreach (char character in text.ToCharArray()) {
			battleText.text += character;
			yield return null;
			yield return null;
		}
	}

	public void OnAttackButton() {
		if (battleState != BattleState.PLAYER_TURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerAttack());
	}

	public void OnSkillButton() {
		if (battleState != BattleState.PLAYER_TURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(0));
	}

	public void OnSkillButton1() {
		if (battleState != BattleState.PLAYER_TURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(1));
	}

	public void OnSkillButton2() {
		if (battleState != BattleState.PLAYER_TURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(2));
	}

	public void OnSkillButton3() {
		if (battleState != BattleState.PLAYER_TURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(PlayerSkill(3));
	}

	public IEnumerator PlayerAttack() {

		battleUI.ClearUI(true);

		// Enters BattleState for targeting
		yield return StartCoroutine(Targeting());

		battleUI.ClearUI(false);

		int damage = rand.Next(player.battleEntity.minAttackDamage, player.battleEntity.maxAttackDamage + 1);
		target.TakeDamage(damage);

		SetBattleText("Turn " + roundCounter + " " + target.battleEntity.name + " took " + damage + " damage!");

		target = null;

		yield return new WaitForSeconds(1f);

		if (!IsEnemyAlive()) {
			battleState = BattleState.WON;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	public IEnumerator PlayerSkill(int numSkill) {

		battleUI.ClearUI(true);

		Skill skill = player.battleEntity.skills[numSkill];

		// Enters BattleState for targeting
		yield return StartCoroutine(Targeting(skill));

		battleUI.ClearUI(false);

		SetBattleText("Turn " + roundCounter + " " + player.battleEntity.name + " used " + skill.name + " on " + target.battleEntity.name + "!");

		int healthBefore = target.currentHealth;

		if(skill.isSingleTarget)
			yield return StartCoroutine(skill.Cast(player, new BattleEntityController[] { target }));
		else {
			yield return StartCoroutine(skill.Cast(player, battleEntities.Where(x => !x.isPlayerTeam).ToArray()));
		}

		int healthDiff = healthBefore - target.currentHealth;

		if(healthDiff == 0)
			SetBattleText("Turn " + roundCounter + " " + target.battleEntity.name + " was " + skill.statusEffect.ToString().ToLower() + "ed!");
		else if(healthDiff > 0)
			SetBattleText("Turn " + roundCounter + " " + target.battleEntity.name + " took " + (healthBefore - target.currentHealth) + " damage from " + skill.name + "!");
		else if(healthDiff < 0)
			SetBattleText("Turn " + roundCounter + " " + target.battleEntity.name + " healed for " + (target.currentHealth - healthBefore) + " health!");

		target = null;

		yield return new WaitForSeconds(1.5f);

		if (!IsEnemyAlive()) {
			battleState = BattleState.WON;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	private IEnumerator Targeting(Skill skill) {
		battleState = BattleState.TARGETING;
		SetBattleText("Select a target");

		TargetAll(skill.isFriendly);

		while (target == null) yield return new WaitForSecondsRealtime(0.25f);

	}

	// Targeting for attack
	private IEnumerator Targeting() {
		battleState = BattleState.TARGETING;
		SetBattleText("Select a target");

		TargetAll(false);

		while (target == null) yield return new WaitForSecondsRealtime(0.25f);

	}

	public void CancelTarget() {
		StopAllCoroutines();
		EndTargetingAll();
		PlayerTurn();
	}

	private void TargetAll(bool isPlayerTeam) {
		foreach (BattleEntityController entity in battleEntities) {
			if (entity.isPlayerTeam == isPlayerTeam)
				entity.OnTargeting();
		}
	}

	//private BattleEntityController GetFirstEntity(bool isPlayerTeam) {
	//	foreach (BattleEntityController entity in battleEntities) {
	//		if (entity.isPlayerTeam == isPlayerTeam)
	//			return entity;
	//	}
	//	return null;
	//}

	private bool IsEnemyAlive() {
		bool isEnemyAlive = false;

		foreach (BattleEntityController entity in battleEntities) {
			if (!entity.isPlayerTeam && entity.currentHealth > 0)
				isEnemyAlive = true;
		}

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
		if (battleState != BattleState.ENEMY_TURN) return;
		battleState = BattleState.WAITING;

		StartCoroutine(EnemyAttack());
	}

	private IEnumerator EnemyAttack() {
		yield return new WaitForSeconds(1f);

		BattleEntityController player = players[rand.Next(players.Count)];

		int damage = rand.Next(enemy.battleEntity.minAttackDamage, enemy.battleEntity.maxAttackDamage + 1);
		player.TakeDamage(damage);

		SetBattleText("Turn " + roundCounter + " " + enemy.battleEntity.name + " did: " + damage + " damage with attack!");

		yield return new WaitForSeconds(2f);

		if (!IsPlayerAlive()) {
			battleState = BattleState.LOST;
			EndBattle();
		} else {
			NextTurn();
		}
	}

	private void EndBattle() {

		currentEntity.turnTracker.SetActive(false);

		if (battleState == BattleState.WON) {
			battleText.text = "You won!";
			battleUI.DisplayEndScreen(true);
		} else if (battleState == BattleState.LOST) {
			battleText.text = "You lost...";
			battleUI.DisplayEndScreen(false);
		}
	}

	public void OnEntitySelected(BattleEntityController entityController) {
		target = entityController;

		EndTargetingAll();
	}

	private void EndTargetingAll() {
		foreach (BattleEntityController entity in battleEntities) {
			entity.EndTargeting();
		}
	}
}
