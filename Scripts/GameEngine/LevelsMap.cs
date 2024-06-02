using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelsMap: MonoBehaviour
{
    public MapLevel[] levels;

    private Game game;
    private string[] lines = new string[11];
    private bool gameIs = false;

    private void Start()
    {
        lines = FileHandler.LoadData(FileHandler.resultsFile);
        DontDestroyOnLoad(gameObject);

        SetStarsActive(false);
        LoadGameData();
    }
    private void LoadGameData()
    {
        
        for(int i = 0; i < levels.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i + 1]))
            {
                var parts = lines[i + 1].Split(',');
                int starCount = int.Parse(parts[0]);
                bool isUnlocked = bool.Parse(parts[1]);

                levels[i].SetStars(starCount);
                levels[i].gameObject.SetActive(isUnlocked);
            }

        }
    }
    private void SetStarsActive(bool active)
    {
        for (int i = 0; i < levels.Length; i++)
        {
          
            for(int j =0; j < levels[i].mapStars.Length; j++)
            {

                levels[i].mapStars[j].SetActive(active);
            }
            
        }
    }

    public void LoadChosenLevel(MapLevel level)
    {

        SceneManager.LoadScene("Game");

        SceneManager.sceneLoaded += (scene, mode) => OnGameSceneLoaded(scene, mode, level);
        gameIs = true;
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode, MapLevel level)
    {
        if (gameIs)
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
                            game.LoadGame(level.levelIndex); 
                            gameIs = false;
                        }
                    }
                }
            }
        }
      
    }

}