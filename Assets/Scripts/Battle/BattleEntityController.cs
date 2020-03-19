using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntityController : MonoBehaviour {
	public int currentHealth;

	public BattleEntity battleEntity;
	public HealthBar healthBar;
	public SpriteRenderer spriteRenderer;
	public TurnOrderTrackerObject turnTracker;
	public StatusEffect status;
	public int statusDuration = 0;

	private System.Random rand = new System.Random();

	void Start() {
		int maxHealth = rand.Next(battleEntity.minHealth, battleEntity.maxHealth + 1);

		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);

		spriteRenderer.sprite = battleEntity.sprite;
	}

	public bool TakeDamage(int damage) {
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);

		return currentHealth <= 0;
	}

	public void SetStatusEffect(StatusEffect effect, int duration) {
		// Place any status effect interations here

		status = effect;
		statusDuration = duration;
	}
}
