using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Firebolt", menuName = "Skill/Firebolt")]
public class Firebolt : Skill {

	[SerializeField] protected float speed;

	[SerializeField] protected GameObject VFXPrefab;

	private System.Random rand = new System.Random();

	public override IEnumerator Cast(BattleEntityController caster, BattleEntityController target) {

		VFXPrefab.transform.position = caster.transform.position;

		Transform source = Instantiate(VFXPrefab).transform;

		yield return new WaitForSeconds(0.75f);

		while (Vector3.Distance(source.position, target.transform.position) > 0.001f) {
			source.position = Vector3.MoveTowards(source.position, target.transform.position, speed * Time.deltaTime);
			yield return null;
		}

		int damage = rand.Next(this.minPower, this.maxPower + 1);

		target.TakeDamage(damage);
		Destroy(source.gameObject);
	}
}