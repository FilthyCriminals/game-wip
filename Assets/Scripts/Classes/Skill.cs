using System.Collections;
using UnityEngine;

public abstract class Skill : ScriptableObject {

	//[SerializeField] public new string name;
	[SerializeField] protected int minPower;
	[SerializeField] protected int maxPower;
	[SerializeField] protected int cost;
	[SerializeField] protected float speed;

	[SerializeField] protected GameObject VFXPrefab;

	public abstract IEnumerator Cast(BattleEntityController caster, BattleEntityController target);
}