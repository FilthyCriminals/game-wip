using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public static SceneController instance;

	public Camera mainCamera;

	private string currentBattleScene;

	private void Awake() {
		if(instance != null) {
			Debug.LogWarning("More than one SceneController");
			Destroy(gameObject);
		}

		instance = this;
	}

	public void LoadBattleScene(string sceneToLoad) {
		mainCamera.enabled = false;
		currentBattleScene = sceneToLoad;
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
	}

	public void UnloadBattleScene() {
		SceneManager.UnloadSceneAsync(currentBattleScene);
		mainCamera.enabled = true;
	}
}
