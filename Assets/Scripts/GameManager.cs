using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {






  public static GameManager instance = null;
  public GameObject playerObj;
  public int foodStats = 25;
  public float startLevelDelay = 2f;
  public float winOrLoseDelay = 3f;
  public float turnDelay = .01f;
  [HideInInspector]
  public bool playerTurn = true;

  public static int wins;
  public bool playAsBrain = false;
  public bool noDmg = false;


  private Text levelText;
  private GameObject levelImage;
  public GameObject pauseScreen;
  private GameObject pauseButton;
  private BoardManager boardInitScript;
  private bool doingSetup = true;
  private int lvl = 1;
  private List<Enemy> enemies;
  private bool enemiesMoving;
  public bool pause;
  public bool restarting = true;

  public void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }
    playAsBrain = PlayerPrefsHelper.GetBool ("playasbrain");
    noDmg = PlayerPrefsHelper.GetBool ("nodmg");
    enemies = new List<Enemy>();
    DontDestroyOnLoad(gameObject);
    boardInitScript = GetComponent<BoardManager>();
    DontDestroyOnLoad (pauseScreen);
    SaveManager.Load ();
  }


  private void OnLevelWasLoaded(int index) {
    if (restarting == false) {
	  lvl++;
	} 

    InitGame();
  }

  void OnApplicationPause(){
    PauseGame ();
  }
  public void PauseGame(){
		
			pause = !pause;
			if (pause == true) {
				Time.timeScale = 0;
				pause = true;
				pauseScreen.SetActive (true);
				SoundManager.instance.musicSource.Pause();
			} else {
				Time.timeScale = 1;
				pause = false;
				pauseScreen.SetActive (false);
				SoundManager.instance.musicSource.UnPause();
			}
		

  }
  void Update() {
	if (doingSetup == false) {
    if(Input.GetButtonDown("Cancel")){
	    PauseGame();
    }
	}
    if (playerTurn || enemiesMoving || doingSetup) {
      return;
    }

    StartCoroutine(MoveEnemies());

  }
  
  public void AddEnemyToList(Enemy script){
    enemies.Add(script);
  }

  public void InitGame(){
  	restarting = false;
    doingSetup = true;
    pauseScreen = GameObject.FindGameObjectWithTag("Pause");
    pauseButton = GameObject.Find ("PauseButton");
    levelImage = GameObject.Find("LevelImage");
    levelText = GameObject.Find("LevelText").GetComponent<Text>();
    levelText.text = "Feast " + lvl;
    levelImage.SetActive(true);
    pauseButton.SetActive (true);
    pauseScreen.SetActive(true);

    Invoke("HideShit", startLevelDelay);
    enemies.Clear();
    boardInitScript.SetupScene(lvl);
  }

  private void HideShit() {
    levelImage.SetActive(false);
    pauseScreen.SetActive(false);
    doingSetup = false;
    #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
      pauseButton.SetActive(false);
    #endif

  }

  public void GameWon() {

    levelText.text = "You have become so fat \nyou have busted your gut. \nCongrats!";
    levelImage.SetActive(true);
    enabled = false;


    Invoke("RestartGame", winOrLoseDelay);
  }

  public void GameOver() {
    levelText.text = "You became famished during feast " + lvl;
    levelImage.SetActive(true);
    enabled = false;
    Invoke("RestartGame", winOrLoseDelay);
  }

  public void NextLevel() {
    Application.LoadLevel(Application.loadedLevel);
  }

  public void RestartGame() {
	Destroy (gameObject);
	Destroy (SoundManager.instance.gameObject);
	restarting = true;
	Application.LoadLevel ("Main");
  }

  IEnumerator MoveEnemies() {
    enemiesMoving = true;
    yield return new WaitForSeconds(turnDelay);
    if (enemies.Count == 0) {
      yield return new WaitForSeconds(turnDelay);
    }
    for (int i = 0; i < enemies.Count; i++) {
      enemies[i].MoveEnemy();
      yield return new WaitForSeconds(enemies[i].moveTime);
    }
    playerTurn = true;
    enemiesMoving = false;
  }


}