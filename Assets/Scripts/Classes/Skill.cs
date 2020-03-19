using System.Collections;
using UnityEngine;

public abstract class Skill : ScriptableObject {

	//[SerializeField] public new string name;
	[SerializeField] protected int minPower;
	[SerializeField] protected int maxPower;
	[SerializeField] public new string name;
	[SerializeField] protected int cost;

	public abstract IEnumerator Cast(BattleEntityController caster, BattleEntityController target);
}

public enum StatusEffect {
	NONE,
	STUN,
	BURN,
	POISON
}