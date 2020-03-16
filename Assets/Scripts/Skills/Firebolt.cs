using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class Firebolt : Skill {

	private System.Random rand = new System.Random();

	public override IEnumerator Cast(BattleEntityController caster, BattleEntityController target) {

		this.VFXPrefab.transform.position = caster.transform.position;

		Transform source = Instantiate(this.VFXPrefab).transform;

		yield return new WaitForSeconds(0.75f);

		while (Vector3.Distance(source.position, target.transform.position) > 0.001f) {
			source.position = Vector3.MoveTowards(source.position, target.transform.position, this.speed * Time.deltaTime);
			yield return null;
		}

		int damage = rand.Next(this.minPower, this.maxPower + 1);

		target.TakeDamage(damage);
		Destroy(source.gameObject);
	}
}