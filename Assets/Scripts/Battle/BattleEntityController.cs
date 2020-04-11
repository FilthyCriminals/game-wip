using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEntityController : MonoBehaviour {
	public int currentHealth;

	public BattleController battleController;
	public BattleEntity battleEntity;
	public HealthBar healthBar;
	public SpriteRenderer spriteRenderer;
	public TurnOrderTrackerObject turnTracker;
	public Image targetingImage;


	public List<Status> statuses = new List<Status>();
	public int maxHealth;
	public bool isPlayerTeam = false;
	public bool isTargeting = false;

	public float damageMultiplier = 1f;

	private System.Random rand = new System.Random();

	void Start() {
		targetingImage.enabled = false;

		maxHealth = rand.Next(battleEntity.minHealth, battleEntity.maxHealth + 1);

		if (!isPlayerTeam) healthBar.gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1f) };

		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);

		spriteRenderer.sprite = battleEntity.sprite;
	}

	public void TakeDamage(int damage) {

		UpdateHealth(damage);

		// Play hurt and death animation here
		if (currentHealth == 0) {
			battleController.battleEntities.Remove(this);
			Destroy(turnTracker.gameObject);
			Destroy(this.gameObject);
		}
	}

	public void RegainHealth(int heal) {
		UpdateHealth(-heal);

		// Play healed animation
	}

	private void UpdateHealth(int damage) {
		currentHealth -= damage;

		if (currentHealth < 0) currentHealth = 0;
		if (currentHealth > maxHealth) currentHealth = maxHealth;

		healthBar.SetHealth(currentHealth);
		turnTracker.background.transform.localScale = new Vector3(turnTracker.background.transform.localScale.x, currentHealth / (float)maxHealth);
	}

	private void OnMouseUp() {
		if (battleController.battleState != BattleState.TARGETING || !isTargeting) return;

		battleController.OnEntitySelected(this);
	}

	public void SetStatusEffect(Status status) {
		// Place any status effect interations here


		statuses.Add(status);
	}

	public void RemoveStatusEffect(Status status) {
		statuses.Remove(status);
		status.callback(this);
	}

	public void OnTargeting() {
		isTargeting = true;
		targetingImage.enabled = true;
	}

	public void EndTargeting() {
		isTargeting = false;
		targetingImage.enabled = false;
	}

	// Returns whether or not it takes it's turn
	public bool TickStatuses() {

		bool shouldTakeTurn = true;

		for (int i = 0; i < statuses.Count; i++) {
			Status status = statuses[i];
			status.duration -= 1;
			if (status.effect == StatusEffect.STUN) {
				shouldTakeTurn = false;
			}
		}

		return shouldTakeTurn;
	}

	public void CleanUpStatuses() {
		List<Status> statusesToRemove = new List<Status>();

		for (int i = 0; i < statuses.Count; i++) {
			Status status = statuses[i];

			if (status.duration == 0) statusesToRemove.Add(status);
		}

		foreach(Status status in statusesToRemove) {
			RemoveStatusEffect(status);
		}
	}
}
