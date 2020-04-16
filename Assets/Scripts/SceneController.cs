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

		DontDestroyOnLoad(gameObject);
	}

	public void LoadBattleScene(string sceneToLoad) {
		mainCamera.gameObject.SetActive(false);
		currentBattleScene = sceneToLoad;
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
	}

	public void UnloadBattleScene() {
		SceneManager.UnloadSceneAsync(currentBattleScene);
		mainCamera.gameObject.SetActive(true);
	}
}
