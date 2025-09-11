using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject mainPauseMenu;

    private Stack<GameObject> menuStack = new Stack<GameObject>();
    private bool isPaused = false;

    private void Start()
    {
        CloseAllMenus();
        hud.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                GoBack();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        hud.SetActive(false);
        OpenMenu(mainPauseMenu);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        hud.SetActive(true);
        CloseAllMenus();
    }

    public void OpenMenu(GameObject menu)
    {
        if (menuStack.Count > 0)
            menuStack.Peek().SetActive(false);

        menuStack.Push(menu);
        menu.SetActive(true);
    }

    public void GoBack()
    {
        if (menuStack.Count > 0)
        {
            GameObject current = menuStack.Pop();
            current.SetActive(false);

            if (menuStack.Count > 0)
            {
                menuStack.Peek().SetActive(true);
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void CloseAllMenus()
    {
        while (menuStack.Count > 0)
        {
            GameObject menu = menuStack.Pop();
            menu.SetActive(false);
        }
    }
}

