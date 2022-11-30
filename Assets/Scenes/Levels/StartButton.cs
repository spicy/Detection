using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuConfig : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Lab");
    }
}
