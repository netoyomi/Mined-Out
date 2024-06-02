using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class LevelHandler: MonoBehaviour
{
    public GameObject[] levels;
    public GameObject[] starsImages;
    public Text textField;
    public Text buttonText;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject homeButton;
    public Game game;
    public int totalLevels;
    public string[] lines = new string[9];
    private int level = 1;


    void Awake()
    {
        gameObject.SetActive(false);
        SetLevelsActive(false);
        lines = FileHandler.LoadData(FileHandler.resultsFile);
        totalLevels = lines.Length - 1;
        homeButton.SetActive(false);
        levels[0].SetActive(true);
        SetStarsActive(false, 3);
    }

    public void CallLevelMessage(int level, int stars = 0)
    {
        SetStarsActive(false, 3);
        HandleTextAndSave(level, stars);
        gameObject.SetActive(true);
        SetStarsActive(true, stars);
        Time.timeScale = 0f;
        this.level = level;
        game.EndGame();
    }
    public void Restart()
    {
        game.EndGame();
        ToNextLevel();
    }
  
    private void HandleTextAndSave(int level, int stars)
    {
        if (level != this.level )
        {
            textField.text = "LEVEL COMPLETED!";
            buttonText.text = "CONTINUE";

            SaveLevel(this.level, stars);
        }
        else if (this.level == totalLevels)
        {
            textField.text = "YOU'RE HOME!";
            buttonText.text = "HOME";
            homeButton.SetActive(true);
            nextButton.SetActive(false);
            backButton.SetActive(false);
            SaveLevel(this.level, 3);
        }
        else
        {
            textField.text = "YOU DIED!";
            buttonText.text = "RESTART";
        }
    }
    private void SaveLevel(int level, int starsCount)
    {
        var parts = lines[level].Split(',');

        if (!int.TryParse(parts[0], out int currentStars))
        {
            currentStars = 0;
        }

        if (starsCount > currentStars)
        {
            lines[level] = $"{starsCount},{parts[1]}";
        }
        if (parts[1] == "False")
        {
            lines[level] = $"{currentStars},{true}";
        }
        FileHandler.SaveData(FileHandler.resultsFile, lines);
    }

    public void ToNextLevel()
    {
        Debug.Log("Next level in handler: " + level);
        if(level <= totalLevels)
        {
            SetLevelsActive(false);
            levels[level - 1].SetActive(true);
            game.NewGame();

            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
        }
           
     
   
    }
    public void LoadLevel(int level)
    {
        SetLevelsActive(false);
        levels[level - 1].SetActive(true); 
        this.level = level;

    }
    private void SetStarsActive(bool active, int count)
    {
        for(int i = 0; i < count; i++)
        {
            starsImages[i].SetActive(active);
        }
    }
    private void SetLevelsActive(bool active)
    {
        foreach (var level in levels)
        {
            level.SetActive(active);
        }
    }

    public void UpdateCoins(int coins)
    {
        lines[0] = coins.ToString();
        FileHandler.SaveData(FileHandler.resultsFile, lines);
    }
    public void GameEnd()
    {
        game.gameObject.SetActive(false);
        SetLevelsActive(false);
        SceneManager.LoadScene("Main Menu");
    }
    public void BackHome()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main Menu");
    }
}