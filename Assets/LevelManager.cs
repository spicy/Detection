using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int sceneUnlocked;
    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        sceneUnlocked = PlayerPrefs.GetInt("sceneUnlocked", 1);
        // start by making every button unusable
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        //loops to reveal level select buttons
        for (int i = 0; i < sceneUnlocked; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    // Update is called once per frame
    void Update()
    {

    }
}