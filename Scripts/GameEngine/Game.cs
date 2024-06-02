using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
public class Game : MonoBehaviour
{
    private int width = 27;
    private int height = 16;
    private Board board;
    public LifeHandler lifeHandler;
    public LevelHandler levelHandler;
    private Player player;
    public Stopwatch stopwatch;
    public CoinCounter coinCounter;
    private int currentLevel = 1;
    private Vector2 lastPlayerPosition;
    private float timeSinceLastPosition;
    private Dino dino;
    private bool GameIsOn = false;
    public MineHints mineHints;

    private void Update()
    {
        if (GameIsOn)
        {
            if (player.position != lastPlayerPosition)
            {
                lastPlayerPosition = player.position;
                timeSinceLastPosition = 0f;
            }
            else
            {
                timeSinceLastPosition += Time.deltaTime;

                if (timeSinceLastPosition >= 10f)

                {
                    timeSinceLastPosition = -10f;
                    HighlightPath();
                   
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if(mineHints.amount > 0)
                {
                    bool used = UseBonus();
                    if(used)
                    {
                        mineHints.amount -= 1;
                        mineHints.UpdateCount();
                    }
                  
                }
                else
                {
                    coinCounter.CallCoinMessage("No hints left!");
                }
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Vector2 targetPosition = player.position + Vector2.up * new Vector2(3f, 3f);

                board.MovePlayer(player.position, targetPosition, player, player.stepSize);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Vector2 targetPosition = player.position + Vector2.left * new Vector2(3f, 3f);

                board.MovePlayer(player.position, targetPosition, player, player.stepSize);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Vector2 targetPosition = player.position + Vector2.down * new Vector2(3f, 3f);

                board.MovePlayer(player.position, targetPosition, player, player.stepSize);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Vector2 targetPosition = player.position + Vector2.right * new Vector2(3f, 3f);

                board.MovePlayer(player.position, targetPosition, player, player.stepSize);
            }
            FindMines();
        }
      

    }
    private bool UseBonus()
    {
            if (player.adjacentMines.Count > 0)
            {
                int randomIndex = Random.Range(0, player.adjacentMines.Count);
                MineCell mineToOpen = player.adjacentMines[randomIndex];
                mineToOpen.Open();
                return true;
            }
            else
            {
                coinCounter.CallCoinMessage("No mines around!");
                return false;
              
            }
           
    }
    public void NewGame()
    {
        board = GetComponentInChildren<Board>();
        if (currentLevel == levelHandler.levels.Length)
        {
            board.exitArea.SetActive(false);
        }
        Debug.Log(currentLevel);
        Vector2 startPosition = new Vector2(-38.8f, -22.4f);
        board.Draw(startPosition, width, height, GetLevelFilePath(currentLevel));
        lifeHandler.SetHearts();
        WatchPlayers();
        stopwatch.ResetStopwatch();
        GameIsOn = true;


    }
    public void LoadGame(int level)
    {
        currentLevel = level;
        levelHandler.LoadLevel(level);
        NewGame();
    }
    private void WatchPlayers()
    {
        dino = FindObjectOfType<Dino>();
        if (dino != null)
        {
            dino.PlayerCaughtByDinoEvent += LoseLevel;
            dino.MoveDino += FindPlayer;
        }
        player = FindObjectOfType<Player>();
        player.Wallet = int.Parse(levelHandler.lines[0]);
        coinCounter.UpdateCoinCounter(player.Wallet);
        player.PlayerUpdatedWallet += UpdateWallet;
        player.PlayerTookAward += AwardMessage;
        player.PlayerExplodedMine += DecreaseHealthHandle;
        player.PlayerReachedTheEndEvent += CompleteLevel;
        player.PlayerDied += LoseLevel;
    }
    private void UpdateWallet()
    {
        coinCounter.UpdateCoinCounter(player.Wallet);
        levelHandler.UpdateCoins(player.Wallet);
    }
    public void EndGame()
    {;
        board.ClearBoard();
        GameIsOn = false;
        stopwatch.StopStopwatch();  
    }
    private void DecreaseHealthHandle()
    {
        lifeHandler.DecreaseHearts(player.Health);
    }
    private string GetLevelFilePath(int levelNumber)
    {
        string fileName = "level_" + levelNumber + ".txt";

        string directoryPath = Path.Combine(Application.streamingAssetsPath, "GameData", "Levels", fileName);
        return directoryPath;
    }
    public void CompleteLevel()
    {
        int starCount = 0;
        if(currentLevel != levelHandler.totalLevels)
        {
            foreach (Star star in board.stars)
            {
                if (star.IsCollected)
                {
                    starCount++;
                }
            }
        }
        
        if(stopwatch.elapsedTime <= 90f)
        {
            
            player.Wallet += 10;
            
            UpdateWallet();
        }
            currentLevel++;
        
        player.reachedEnd = true;
        levelHandler.CallLevelMessage(currentLevel, starCount);
    }
    private void AwardMessage()
    {
        coinCounter.CallCoinMessage("+ " + player.award + " coins!");
    }
    public void LoseLevel()
    {
        levelHandler.CallLevelMessage(currentLevel);
    }

    public void FindMines()
    {
        player.adjacentMines = board.FindAdjacentMines(player.position);
        GameObject minesCounterObject = GameObject.Find("MinesCounter");
        MinesCounter minesCounter = minesCounterObject.GetComponent<MinesCounter>();
        minesCounter.UpdateAdjacentMines(player.adjacentMines.Count);

    }
    private void FindPlayer()
    {
        Vector2 relativePos = player.position - new Vector2(-38.8f, -22.4f); ;
        int cellX = Mathf.FloorToInt(relativePos.x / 3f);
        int cellY = Mathf.FloorToInt(relativePos.y / 3f);
        Vector2Int playerPos = new Vector2Int(cellX, cellY);
        List<Vector2> path = board.FindPath(dino.position, playerPos);
        if (path != null)
        {
            board.MoveEnemy(dino.position, path[1], dino);
        }
    }
    private void HighlightPath()
    {
        Vector2Int exitPosition = new Vector2Int((width / 2), (height - 1));

        List<Vector2> path = board.FindPath(player.position, exitPosition);
        if (path != null && path.Count >= 2)
        {
            Cell previousCell = board.cellMap[path[1]];
          
            Cell highlightObject = Instantiate(board.highlightPrefab, path[1], Quaternion.identity);
            board.cellMap[path[1]] = highlightObject;
            StartCoroutine(HighlightCellForSeconds(previousCell, highlightObject, 5f));
        }
    }

    private IEnumerator HighlightCellForSeconds(Cell previousCell, Cell highlightObject, float highlightTime)
    {
        yield return new WaitForSeconds(highlightTime);
        if (highlightObject != null)
        {
            Destroy(highlightObject.gameObject);
            board.cellMap[previousCell.position] = previousCell;
        }
       
       
    }
}
