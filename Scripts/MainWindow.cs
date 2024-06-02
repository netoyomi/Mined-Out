using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainWindow: MonoBehaviour
{
    public GameObject sideMenuUI;
    public GameObject sideMenuIcon;
    public LevelHandler levelHandler;
    void Awake()
    {
        sideMenuUI.SetActive(false);
    }
    public void OpenSideMenu()
    {
        Time.timeScale = 0f;

        sideMenuUI.SetActive(true);
        sideMenuIcon.SetActive(false);
    }
    public void ToMainMenu()
    {
        Game game = FindObjectOfType<Game>();
        game.EndGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    public void RestartLevel()
    {
        levelHandler.Restart();
        sideMenuUI.SetActive(false );
        sideMenuIcon.SetActive(true);
    }
    public void ContinueGame()
    {
        sideMenuUI.SetActive(false);
        sideMenuIcon.SetActive(true);
        Time.timeScale = 1f;
    
    }
}