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
	}

}
