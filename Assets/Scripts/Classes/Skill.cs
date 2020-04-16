using System;
using System.Collections;
using UnityEngine;

public abstract class Skill : ScriptableObject {

	[SerializeField] protected int minPower;
	[SerializeField] protected int maxPower;
	[SerializeField] public new string name;
	[SerializeField] protected int cost;
	[SerializeField] public Status status;
	[SerializeField] public bool isSingleTarget;
	[SerializeField] public bool isFriendly;

	public abstract IEnumerator Cast(BattleEntityController caster, BattleEntityController[] targets);
}

public enum StatusEffect {
	NONE,
	STUN,
	BURN,
	POISON,
	BUFF
}

[System.Serializable]
public class Status {
	public int duration;
	public StatusEffect effect;
	public Action<BattleEntityController> callback;
}