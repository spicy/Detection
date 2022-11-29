using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public void clickTest()
    {
        Debug.Log(message: "button clicked");
    }

    public void LevelSelected(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

}
