using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Skill/Heal")]
public class Heal : Skill {

	private System.Random rand = new System.Random();

	public override IEnumerator Cast(BattleEntityController caster, BattleEntityController[] targets) {

		if (targets.Length != 1) yield break;

		BattleEntityController target = targets[0];

		yield return new WaitForSeconds(1.5f);

		int heal = rand.Next(this.minPower, this.maxPower + 1);

		target.RegainHealth(heal);
	}
}