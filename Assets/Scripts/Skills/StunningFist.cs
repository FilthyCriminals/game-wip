using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StunningFist", menuName = "Skill/StunningFist")]
public class StunningFist : Skill {

	private System.Random rand = new System.Random();

	public override IEnumerator Cast(BattleEntityController caster, BattleEntityController[] targets) {

		if (targets.Length != 1) yield break;

		BattleEntityController target = targets[0];

		yield return new WaitForSeconds(1.5f);

		int damage = rand.Next(this.minPower, this.maxPower + 1);

		target.TakeDamage(damage);
		target.SetStatusEffect(this.status);
	}
}