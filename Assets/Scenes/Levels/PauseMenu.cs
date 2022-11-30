using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool pauseActive = true;
    void Start()
    {
        showPauseMenu();
        pauseMenu.SetActive(false);
    }
    public void pauseButtonPressed(InputAction.CallbackContext menuButton)
    {
        if(menuButton.performed)
        
            showPauseMenu();
        
    }
    
    public void showPauseMenu()
    {
        if(pauseActive)
        {
           pauseMenu.SetActive(false);
           pauseActive = false;
           Time.timeScale = 1;
        }
        else if (!pauseActive)
        {
           pauseMenu.SetActive(true);
           pauseActive = true;
           Time.timeScale = 0;
        }
    }
  


}
