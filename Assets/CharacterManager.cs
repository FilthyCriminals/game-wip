using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

	public static CharacterManager instance;

	public BattleEntity[] battleEntities = new BattleEntity[4];

	void Start() {

		if(instance != null) {
			Debug.LogWarning("Two instances of CharacterManager found");
			Destroy(gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(gameObject);

		for (int i = 0; i < battleEntities.Length; i++) {
			if (battleEntities[i] == null) continue;
			battleEntities[i] = Instantiate(battleEntities[i]);

			for (int j = 0; j < battleEntities[i].skills.Count; j++) {

				if (battleEntities[i].skills[j] == null) continue;
				battleEntities[i].skills[j] = Instantiate(battleEntities[i].skills[j]);

			}
		}
	}

}
