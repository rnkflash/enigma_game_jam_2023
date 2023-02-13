using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    
    public void OnButtonClicked(int i) {
        if (i == 0)
            SceneController.Instance.LoadGameplay();
        else
            SceneController.Instance.LoadMainMenu();
    }

}
