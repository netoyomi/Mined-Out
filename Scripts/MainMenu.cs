using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using static Cinemachine.DocumentationSortingAttribute;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject levelsMap;
    public GameObject settingsWindow;
    public GameObject shopWindow;
    public Shop shop;
    private bool startGame = false;
    private int lastUnlockedLevel = 1;

    private void Start()
    {
        instructionPanel.SetActive(false);
        shopWindow.SetActive(false);
        levelsMap.SetActive(false);
        settingsWindow.SetActive(false);
    }
    public void CloseMap()
    {
        levelsMap.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        LoadGameData();
        SceneManager.sceneLoaded += (scene, mode) => OnGameSceneLoaded(scene, mode, lastUnlockedLevel);
        startGame = true;
     
    }
    private void LoadGameData()
    {
        string[] lines = FileHandler.LoadData(FileHandler.resultsFile);

        for (int i = lines.Length - 1; i > 0; i--)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                var parts = lines[i].Split(',');
                bool isUnlocked = bool.Parse(parts[1]);

                if (isUnlocked)
                {
                    lastUnlockedLevel = i; 
                    break;
                }
            }
        }
    }

    public void OpenShop()
    {
        shopWindow.SetActive(true);
    }
    public void CloseShop()
    {
        shopWindow.SetActive(false);
    }
    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode, int index)
    {
        Game game;
        if(startGame)
        {
            if (scene.name == "Game")
            {
                GameObject[] gameObjects = scene.GetRootGameObjects();
                foreach (GameObject go in gameObjects)
                {
                    Canvas canvas = go.GetComponentInChildren<Canvas>();
                    if (canvas != null)
                    {
                        Game gameComponent = canvas.GetComponentInChildren<Game>();
                        if (gameComponent != null)
                        {
                            game = gameComponent;
                            game.LoadGame(index);
                            startGame = false;
                        }
                    }
                }
            }
           
        }
       

    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true); 
    }
    public void ShowSettings()
    {
        settingsWindow.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsWindow.SetActive(false);    
    }
    public void ResetData()
    {
        string[] lines = FileHandler.LoadData(FileHandler.resultsFile);
      Debug.Log(lines.Length);
        for(int i = 0; i < lines.Length; i++)
        {
            if(i == 0)
            {
                lines[i] = "0";
            }
            else 
            {
                lines[i] = "0,False";
            }

        }
        FileHandler.SaveData(FileHandler.resultsFile, lines);

        string[] lines2 = FileHandler.LoadData(FileHandler.shopInfoFile);
     
        for (int i = 0;i < lines2.Length; i++)
        {
            if (i == 0)
            {
                lines2[i] = "True,True";
            }
            else 
            {
                lines2[i] = "False,False";
            }
               
        }
        FileHandler.SaveData(FileHandler.shopInfoFile, lines2);

        string[] lines3 = new string[1];
        for (int i = 0; i < lines3.Length; i++)
        {
            lines3[i] = "0";
        }
        FileHandler.SaveData(FileHandler.hintsInfoFile, lines3);
        shop.mineHints.amount = 0;
        shop.mineHints.UpdateCount();
        shop.Wallet.text = "0";
        shop.LoadShopData();
    }
    public void OpenMap()
    {
        levelsMap.SetActive(true);
    }
    public void CloseInstruction()
    {
        instructionPanel.SetActive(false);
    }
}
