using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadBuildDeck : MonoBehaviour
{
    // public TextMeshProUGUI LoadingText;
    public GameObject menu;
    public GameObject loadingBackground;

    private void change(){
        menu.SetActive(false);
        loadingBackground.SetActive(true);
        ChangeScene.LoadScene("BuildDeck");
    }


}
