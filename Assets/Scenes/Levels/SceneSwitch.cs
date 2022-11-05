using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitch : MonoBehaviour
{
    public int currentSceneIndex;
    public int sceneIndexToLoad;
    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneIndexToLoad);
        SceneManager.UnloadSceneAsync(currentSceneIndex);
    }
}
