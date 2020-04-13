using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Strength", menuName = "Skill/Strength")]
public class Strength : Skill {

	private System.Random rand = new System.Random();
	public float damageMultiplier = 0.5f;

	public override IEnumerator Cast(BattleEntityController caster, BattleEntityController[] targets) {

		if (targets.Length != 1) yield break;

		BattleEntityController target = targets[0];

		yield return new WaitForSeconds(1.5f);

		this.status.callback = OnEndStatus;
		target.SetStatusEffect(this.status);
		target.damageMultiplier += damageMultiplier;
	}

	public void OnEndStatus(BattleEntityController battleEntityController) {
		battleEntityController.damageMultiplier -= damageMultiplier;
	}
}