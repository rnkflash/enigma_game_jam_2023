using System.Threading.Tasks;
using SaveSystem;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
	public async void StartGame()
	{
		await LoadSceneAsync("menu", LoadSceneMode.Single);
	}

	public async void LoadMainMenu()
	{
		await LoadSceneAsync("menu", LoadSceneMode.Single);
	}

	public async void LoadGameplay()
	{
		await LoadSceneAsync("game", LoadSceneMode.Single);
	}

	public async void LoadAnyScene(string scene)
	{
		SaveLoadSystem.Instance.SaveScene();
		await LoadSceneAsync(scene, LoadSceneMode.Single);
		SaveLoadSystem.Instance.LoadScene();
	}

	private async Task LoadSceneAsync(string sceneName, LoadSceneMode mode)
	{
		var loadOp = SceneManager.LoadSceneAsync(sceneName, mode);

		while (!loadOp.isDone)
		{
			await Task.Delay(60);
		}
	}
}