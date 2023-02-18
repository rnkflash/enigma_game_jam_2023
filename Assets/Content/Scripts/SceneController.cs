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
		await LoadSceneAsync("Level_MegaStructure_01_Prologue", LoadSceneMode.Single);
	}

	public async void LoadLoseScreen()
	{
		await LoadSceneAsync("lose", LoadSceneMode.Single);
	}

	public async void LoadWinScreen()
	{
		await LoadSceneAsync("win", LoadSceneMode.Single);
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