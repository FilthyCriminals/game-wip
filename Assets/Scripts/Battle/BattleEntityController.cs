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

	public StatusEffect status;
	public int statusDuration = 0;
	public bool isPlayerTeam = false;
	public bool isTargeting = false;

	private System.Random rand = new System.Random();

	void Start() {
		targetingImage.enabled = false;

		int maxHealth = rand.Next(battleEntity.minHealth, battleEntity.maxHealth + 1);

		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);

		spriteRenderer.sprite = battleEntity.sprite;
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;

		if (currentHealth < 0) currentHealth = 0;

		healthBar.SetHealth(currentHealth);


		// Play hurt and death animation here
		if (currentHealth == 0) {
			battleController.battleEntities.Remove(this);
			Destroy(this.gameObject);
		}
	}

	private void OnMouseUp() {
		if (battleController.battleState != BattleState.TARGETING || !isTargeting) return;

		battleController.OnEntitySelected(this);
	}

	public void SetStatusEffect(StatusEffect effect, int duration) {
		// Place any status effect interations here

		status = effect;
		statusDuration = duration;
	}

	public void OnTargeting() {
		isTargeting = true;
		targetingImage.enabled = true;
	}

	public void EndTargeting() {
		isTargeting = false;
		targetingImage.enabled = false;
	}
}
