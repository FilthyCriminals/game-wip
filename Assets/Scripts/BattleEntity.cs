using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : MonoBehaviour {
	public int maxHealth = 10;
	public int currentHealth;
	public bool isPlayerTeam;

	public int minAttackDamage = 5;
	public int maxAttackDamage = 10;

	public int minSkillDamage = 5;
	public int maxSkillDamage = 10;

	public HealthBar healthBar;

	void Start() {
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}
}
