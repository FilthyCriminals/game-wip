using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattleEntity", menuName ="Battle Entity")]
public class BattleEntity : ScriptableObject {
	public new string name;

	public int minHealth = 10;
	public int maxHealth = 10;

	public bool isPlayerTeam;

	public int minAttackDamage = 5;
	public int maxAttackDamage = 10;

	public int minSkillDamage = 5;
	public int maxSkillDamage = 10;

	public Sprite sprite;

	public List<Skill> skills;
}
