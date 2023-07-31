using DG.Tweening;
using UnityEngine;

public class BootController : MonoBehaviour
{
	private void Start()
	{
		Application.targetFrameRate = 60;
		DOTween.Init();
		SceneController.Instance.StartGame();
		SceneController.Instance.LoadLoseScreen();
	}
}