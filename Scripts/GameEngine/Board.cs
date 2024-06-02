using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;
using System.Collections;
public class Board : MonoBehaviour
{
    public Cell minePrefab;
    public Cell emptyPrefab;
    public Cell wallPrefab;
    public Cell playerPrefab;
    public Cell starPrefab;
    public Cell enemyPrefab;
    public Cell coinPrefab;
    public Cell highlightPrefab;
    public Cell chestPrefab;
    public GameObject exitArea;
    public Star[] stars;
    private string filePath;
    private Vector2 startPosition;
    private Vector2 cellSize = new Vector2(3f, 3f);
    public Dictionary<Vector2, Cell> cellMap = new Dictionary<Vector2, Cell>(); // Dictionary to store cell positions and corresponding objects
    private char[,] grid;


    public void Draw(Vector2 start, int width, int height, string path)
    {
     
        startPosition = start;
        filePath = path;
        stars = new Star[3];
       
        int starCount = 0;
        grid = new char[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);
                Vector2 worldPosition = startPosition + new Vector2(cellPosition.x * cellSize.x, cellPosition.y * cellSize.y);

                Cell instantiatedCell;

                if (x == width / 2 && y == 1)
                {
                    instantiatedCell = Instantiate(playerPrefab, new Vector2(worldPosition.x, worldPosition.y), Quaternion.identity);
                    grid[x, y] = 'E';

                }
                else if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    instantiatedCell = Instantiate(wallPrefab, worldPosition, Quaternion.identity);
                    grid[x, y] = 'W';
                }
                else
                {
                    char cellType = ReadCellTypeFromTextFile(x, y);

                    switch (cellType)
                    {
                        case 'N': // Enemy cell
                            instantiatedCell = Instantiate(enemyPrefab, worldPosition, Quaternion.identity);
                            break;
                        case 'M': // Mine cell
                            instantiatedCell = Instantiate(minePrefab, worldPosition, Quaternion.identity);
                            break;
                        case 'S': // Star cell
                            instantiatedCell = Instantiate(starPrefab, worldPosition, Quaternion.identity);
                            stars[starCount] = (Star)instantiatedCell;
                            starCount++;
                            break;
                        case 'C':
                            instantiatedCell = Instantiate(coinPrefab, worldPosition, Quaternion.identity);
                            break;
                    
                        case 'T':
                            instantiatedCell = Instantiate(chestPrefab, worldPosition, Quaternion.identity);
                            break;
                        default:
                            instantiatedCell = Instantiate(emptyPrefab, worldPosition, Quaternion.identity);
                            break;
                    }
                    grid[x, y] = cellType;
                }
                instantiatedCell.position = worldPosition;
                cellMap.Add(worldPosition, instantiatedCell);
                
            }


        }
    }
        private char ReadCellTypeFromTextFile(int x, int y)
        {
            string[] lines = File.ReadAllLines(filePath);

            if (y >= 0 && y < lines.Length)
            {
                string line = lines[y];

                if (x >= 0 && x < line.Length)
                {
                    return line[x];
                }
            }

            return 'E';
        } 
    
   public Vector2Int FromWorldToCell(Vector2 position) {

        Vector2 relativePos = position - startPosition;
        int cellX = Mathf.FloorToInt(relativePos.x / cellSize.x);
        int cellY = Mathf.FloorToInt(relativePos.y / cellSize.y);
        return new Vector2Int(cellX, cellY);
    }
   
    public List<MineCell> FindAdjacentMines(Vector2 position)
    {
        Vector2Int cellPosition = FromWorldToCell(position);
        List<MineCell> mines = new List<MineCell>();

        List<Vector2Int> neighbors = new List<Vector2Int>
         {
                new Vector2Int(cellPosition.x + 1, cellPosition.y),
                new Vector2Int(cellPosition.x - 1, cellPosition.y),
                new Vector2Int(cellPosition.x, cellPosition.y + 1),
                new Vector2Int(cellPosition.x, cellPosition.y - 1)
         };
        List<Vector2> newNeighbors = TranslateToWorld(neighbors); 

        foreach (var neighbor in newNeighbors)
        {
            if (cellMap.ContainsKey(neighbor) && cellMap[neighbor] is MineCell)
            {
                if(!((MineCell)cellMap[neighbor]).revealed) mines.Add((MineCell)cellMap[neighbor]);

            }
        }

        return mines;
    }
    public void ClearBoard()
    {
   
        foreach (Cell cell in cellMap.Values)
        {
            Destroy(cell.gameObject);
  
        }
        foreach (Star star in stars)
        {
            Destroy(star);
        }
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = 'E';
            }
        }

        cellMap.Clear();
    }
    public void MovePlayer(Vector2 currentPosition, Vector2 targetPosition, Player player, float duration)
    {
        if (IsValidCell(targetPosition))
        {
            if (cellMap.ContainsKey(targetPosition))
            {

                if (cellMap[targetPosition] is Highlight)
                {
                    Destroy(cellMap[targetPosition].gameObject);
                    cellMap[targetPosition] = Instantiate(emptyPrefab, targetPosition, Quaternion.identity);
                }
                else if (cellMap[targetPosition] is Star)
                {
                    Star star = (Star)cellMap[targetPosition];
                    star.Collect();
                    Destroy(cellMap[targetPosition].gameObject);
                    cellMap[targetPosition] = Instantiate(emptyPrefab, targetPosition, Quaternion.identity);
                }
                else if (cellMap[targetPosition] is Coin)
                {
                    Coin coin = (Coin)cellMap[targetPosition];
                    coin.Collect();
                    player.CollectCoin();
                    Destroy(cellMap[targetPosition].gameObject);
                    cellMap[targetPosition] = Instantiate(emptyPrefab, targetPosition, Quaternion.identity);

                }

                StartCoroutine(MovePlayerCoroutine(currentPosition, targetPosition, player, duration));
            }

        }
        else if (cellMap[targetPosition] is Chest)
        {
            Chest chest = (Chest)cellMap[targetPosition];
            if (!chest.isOpened)
            {
                chest.Open();
            }
           
        }
        else if (cellMap[targetPosition] is MineCell && !((MineCell)cellMap[targetPosition]).revealed)
        {
            MineCell mine = (MineCell)cellMap[targetPosition];
            mine.Reveal();
            player.OnMineEnter();
        }
       
    }
    public void MoveEnemy(Vector2 currentPosition, Vector2 targetPosition, Dino enemy)
    {
        if (cellMap.ContainsKey(targetPosition) && IsValidCell(targetPosition))
        {
            StartCoroutine(MoveEnemyCoroutine(currentPosition, targetPosition, enemy));
        }
    }
    private IEnumerator MovePlayerCoroutine(Vector2 startPosition, Vector2 targetPosition, Player player, float duration)
    {
        float elapsedTime = 0f;
        Vector2 initialPosition = player.position;
      
            while (elapsedTime < duration)
            {
            elapsedTime += Time.deltaTime;

            if (player.rb != null)
            {

                float t = Mathf.Clamp01(elapsedTime / duration);

                player.position = Vector2.Lerp(initialPosition, targetPosition, t);
                player.rb.MovePosition(player.position);
                Vector2 movementDirection = (targetPosition - startPosition).normalized;
                player.UpdateAnimator(movementDirection);
                yield return null;

            }
           
            }
       if(player.rb != null) { 
            player.position = targetPosition;
            player.rb.MovePosition(player.position);
            player.animator.SetFloat("Speed", 0f);
            Cell value = cellMap[targetPosition];

            Cell playerCell = cellMap[startPosition];
            cellMap.Remove(startPosition);
            cellMap.Add(startPosition, value);
            cellMap.Remove(targetPosition);
            cellMap.Add(targetPosition, playerCell);
        }
            
       
    }

    private IEnumerator MoveEnemyCoroutine(Vector2 startPosition, Vector2 targetPosition, Dino enemy)
    {
        float duration = 0.5f; 
        float elapsedTime = 0f;
        Vector2 initialPosition = enemy.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (enemy.rb != null)
            {
                float t = Mathf.Clamp01(elapsedTime / duration); 

                enemy.position = Vector2.Lerp(initialPosition, targetPosition, t);
                enemy.rb.MovePosition(enemy.position);
                Vector2 movementDirection = (targetPosition - startPosition).normalized;
                enemy.UpdateAnimator(movementDirection);
                yield return null;
            }
        }

      
        enemy.position = targetPosition;
        if(enemy.rb != null)
        {
            enemy.rb.MovePosition(enemy.position);
            enemy.animator.SetFloat("Speed", 0f);
            Cell value = cellMap[targetPosition];

            Cell playerCell = cellMap[startPosition];
            cellMap.Remove(startPosition);
            cellMap.Add(startPosition, value);
            cellMap.Remove(targetPosition);
            cellMap.Add(targetPosition, playerCell);
        }
        
     
    }
   
    private bool IsValidCell(Vector2 position)
    {
     
        if (cellMap.ContainsKey(position))
        {
      
            if (cellMap[position] is WallCell || cellMap[position] is MineCell || cellMap[position] is Chest )
            {
                return false;
            }
        }

     
        return true;
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2Int target)
    {
        Vector2Int start = FromWorldToCell(startPos);
    
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        bool[,] visited = new bool[width, height];
        Vector2Int[,] cameFrom = new Vector2Int[width, height];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current.Equals(target))
            {
                List<Vector2Int> path = new List<Vector2Int>();
                while (current != start)
                {
                    path.Add(current);
                    current = cameFrom[current.x, current.y];
                }
                path.Add(start);
                path.Reverse();
                return TranslateToWorld(path);
            }
            if (grid[current.x, current.y] != 'W' && grid[current.x, current.y] != 'M' && grid[current.x, current.y] != 'T')
            {
                foreach (var neighbor in GetUnvisited(current, visited))
                {

                    visited[neighbor.x, neighbor.y] = true;
                    cameFrom[neighbor.x, neighbor.y] = current;
                    queue.Enqueue(neighbor);

                }
            }
           
        }
        return null;
    }

    private List<Vector2> TranslateToWorld(List<Vector2Int> path)
    {
        List<Vector2> result = new List<Vector2>();
        foreach (var pos in path)
        {
            result.Add(startPosition + new Vector2(pos.x * cellSize.x, pos.y * cellSize.y));
        }
        return result;
    }

    private List<Vector2Int> GetUnvisited(Vector2Int cell, bool[,] visited)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        int[,] directions = new int[,]
        {
            {-1, 0 },
            { 1, 0 },
            { 0, -1 },
            { 0, 1 }
        };

        int rowCount = directions.GetLength(0);

        for (int i = 0; i < rowCount; i++)
        {
            int neighborX = cell.x + directions[i, 0] /** 2*/;
            int neighborY = cell.y + directions[i, 1] /** 2*/;

            if (neighborX >= 0 && neighborX < visited.GetLength(0) && neighborY >= 0 && neighborY < visited.GetLength(1) &&
               !visited[neighborX, neighborY] )
            {
                neighbors.Add(new Vector2Int(neighborX, neighborY));
            }
        }

        return neighbors;
    }

}
