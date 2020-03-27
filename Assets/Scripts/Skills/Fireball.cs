using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Skill/Fireball")]
public class Fireball : Skill {

	private System.Random rand = new System.Random();

	public override IEnumerator Cast(BattleEntityController caster, BattleEntityController[] targets) {

		yield return new WaitForSeconds(1.5f);

		int damage = rand.Next(this.minPower, this.maxPower + 1);

		foreach(BattleEntityController target in targets) {
			target.TakeDamage(damage);
		}
	}
}