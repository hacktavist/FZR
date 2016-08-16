using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

  [Serializable]
  public class Count {
    public int min, max;

    public Count(int minimum, int maximum) {
      min = minimum;
      max = maximum;
    }
  }

  public int row = 8, col = 8;
  public Count wallCount = new Count(5, 9);
  public Count foodCount = new Count(1, 5);
  public GameObject exit;
  public GameObject empty;
  
  public GameObject [] floorTiles;
  public GameObject[] wallTiles;
  public GameObject[] foodTiles;
  public GameObject[] enemyTiles;
  public GameObject[] outerWallTiles;

  private Transform boardHolder;
  private List<Vector3> gridPositions = new List<Vector3>();
  private Vector3 exitPlacement;
  public Count enemyCount = new Count(1, 3);

  void InitList() {
    gridPositions.Clear();

    /*
     * create coordinates to place tiles;
     *  using col and row -1 so we leave a border
     *  around the sides to not have any blocks 
     *  that will block player completely
     */
    for (int x = 1; x < col - 1; x++) {
      for (int y = 1; y < row - 1; y++) {
        gridPositions.Add(new Vector3(x,y,0f));
      } // end for loop rows
    } // end for loop columns
  }

  void BoardSetup() {
    boardHolder = new GameObject("Board").transform;
    exitPlacement = new Vector3(Random.Range(0, col - 1), row, 0f);
    for (int x = -15; x < col + 15; x++) {
      for (int y = -15; y < row + 15; y++) {
        // create a random floor tile
        GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
        // check if tile needs to be an edge tile, if so toInstantiate gets an outerwall tile
        if(x < 0 || x >= col || y < 0 || y >= row){
          if ((x == exitPlacement.x && y == exitPlacement.y)) {
            toInstantiate = exit;
          } else if (x == exitPlacement.x && y > exitPlacement.y) {
            toInstantiate = empty;
          } else {
            toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
          }
        } // end if
        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardHolder);
      } // end for loop rows
    } // end for loop columns
  }

  Vector3 RandomPos() {
    int randomIndex = Random.Range(0, gridPositions.Count);
    Vector3 randomPos = gridPositions[randomIndex];
    gridPositions.RemoveAt(randomIndex);
    return randomPos;
  }

  void LayoutRandomObj(GameObject [] tileArr, int min, int max) {
    int objCount = Random.Range(min, max + 1);
    for (int i = 0; i < objCount; i++) {
      Vector3 randomPosition = RandomPos();
      GameObject tileChoice = tileArr[Random.Range(0, tileArr.Length)];
      Instantiate(tileChoice, randomPosition, Quaternion.identity);
    } // end for loop placing objects
  }

  public void SetupScene(int lvl) {
    
    BoardSetup();
    InitList();
    LayoutRandomObj(wallTiles, wallCount.min, wallCount.max);
    LayoutRandomObj(foodTiles, foodCount.min, foodCount.max);
    LayoutRandomObj (enemyTiles, enemyCount.min, enemyCount.max);
    
    
   }


}
