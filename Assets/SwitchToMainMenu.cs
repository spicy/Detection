using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
        SceneManager.UnloadSceneAsync(4);
    }
}
