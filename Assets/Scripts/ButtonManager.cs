using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	public static ButtonManager instance;
  Button cheatBtn;
  GameObject cheatScreen;
  GameObject objectiveScreen;
  GameObject objectiveTextMobile;
  GameObject objectiveTextPC;
  GameObject startButton;
  GameObject objectiveButton;
  GameObject exitButton;
  GameObject backButton;
  GameObject toggleBtnBrain;
  GameObject toggleBtnNoDmg;
  bool toggleBrain;
  bool toggleNoDmg;
  bool cheatsMenuEnabled;

  public string [] enableCheatsMenu;
  private int index;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
    SaveManager.Load ();
    objectiveScreen = GameObject.Find("ObjectiveScreen");
    objectiveTextMobile = GameObject.Find("ObjectiveTextMobile");
    objectiveTextPC = GameObject.Find("ObjectiveTextPC");
    startButton = GameObject.Find("StartGame");
    objectiveButton = GameObject.Find("Objective");
    exitButton = GameObject.Find("ExitGame");
    toggleBtnBrain = GameObject.Find ("Play as Brain");
    toggleBtnNoDmg = GameObject.Find ("Invincibility");
    toggleBrain = PlayerPrefsHelper.GetBool ("playasbrain");
    toggleNoDmg = PlayerPrefsHelper.GetBool ("nodmg");
    if(GameObject.Find ("CheatsBtn") != null)
      cheatBtn = GameObject.Find ("CheatsBtn").GetComponent<Button>();
    cheatScreen = GameObject.Find ("CheatScreen");
    cheatsMenuEnabled = PlayerPrefsHelper.GetBool ("cheatsEnabled");

    SetCheatInteractablity ();

    ToggleButtons ();




    if(objectiveScreen != null && objectiveTextMobile != null && objectiveTextPC != null)
      HideShit();
    #if UNITY_EDITOR || UNITY_WEBGL
    HideExitButton();
    #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID || UNITY_WP8
    disableButtonAnims ();
    #endif
  }

  void HideExitButton(){
    if(exitButton != null)
    exitButton.SetActive(false);
  }
//  void Update(){
//    if (enableCheatsMenu != null) {
//      if (Input.anyKeyDown) {
//        // Check if the next key in the code is pressed
//        if (Input.GetKeyDown (enableCheatsMenu [index])) {
//          // Add 1 to index to check the next key in the code
//          index++;
//        }
//      // Wrong key entered, we reset code typing
//      else {
//          index = 0;
//        }
//      }
//    
//      // If index reaches the length of the cheatCode string, 
//      // the entire code was correctly entered
//      if (index == enableCheatsMenu.Length) {
//        cheatBtn.interactable = true;
//        cheatsMenuEnabled = true;
//        PlayerPrefsHelper.SetBool ("cheatsEnabled", cheatsMenuEnabled);
//
//        index = 0;
//      }
//    }
//  }

  void ToggleButtons(){

    if (toggleBtnBrain != null && toggleBrain == true) {
      
      toggleBtnBrain.GetComponent<Toggle> ().isOn = true;
    }

    if (toggleBtnNoDmg != null && toggleNoDmg == true) {
      
      toggleBtnNoDmg.GetComponent<Toggle> ().isOn = true;
    }
  }

  public void disableButtonAnims(){
    startButton.GetComponent<Animator> ().enabled = false;
    objectiveButton.GetComponent<Animator>().enabled = false;
    exitButton.GetComponent<Animator>().enabled = false;
  }

  public void SetCheatInteractablity(){
    if (cheatBtn != null && GameManager.wins < 10) {
      cheatBtn.interactable = false;
      PlayerPrefsHelper.SetBool("cheatsEnabled",false);
    }
    if (cheatBtn != null && GameManager.wins >= 10)
      cheatBtn.interactable = true;
      PlayerPrefsHelper.SetBool("cheatsEnabled",true);
  }
  public void HideShit() {

    cheatScreen.SetActive (false);
    objectiveScreen.SetActive(false);
    objectiveTextMobile.SetActive(false);
    objectiveTextPC.SetActive(false);
  }

	public void StartGame(){
		Application.LoadLevel("Main");
	}

	public void ResumeGame(){
			Time.timeScale = 1;
		  GameManager.instance.pause = false;
		  GameManager.instance.pauseScreen.SetActive (false);
			SoundManager.instance.musicSource.UnPause();

	}

  public void Objective() {
    startButton.SetActive(false);
    objectiveButton.SetActive(false);
    exitButton.SetActive(false);
    objectiveScreen.SetActive(true);
    cheatBtn.gameObject.SetActive (false);
    #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
        objectiveTextPC.SetActive(true);

    #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID || UNITY_WP8
        objectiveTextMobile.SetActive(true);
    #endif
  }

  public void Cheats() {
    if (cheatBtn.interactable != false) {
      startButton.SetActive (false);
      objectiveButton.SetActive (false);
      exitButton.SetActive (false);
      objectiveScreen.SetActive (false);
      cheatBtn.gameObject.SetActive(false);
      cheatScreen.SetActive (true);
    }

  }

  public void ExitGameToStartScreen(){
    Destroy (GameManager.instance.gameObject);
    Destroy (SoundManager.instance.gameObject);
    Time.timeScale = 1;
    Application.LoadLevel("Start Screen");
	}

  public void ExitGameComplete() {
    Application.Quit();
  }

	public void Back(){
    startButton.SetActive(true);
    objectiveButton.SetActive(true);
    exitButton.SetActive(true);
    objectiveScreen.SetActive(false);
    cheatScreen.SetActive (false);
    if(cheatsMenuEnabled)
      cheatBtn.gameObject.SetActive(true);
    #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
      objectiveTextPC.SetActive(false);

    #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID || UNITY_WP8
      objectiveTextMobile.SetActive(false);
    #elif UNITY_WEBGL
    HideExitButton();
    #endif
	}

  public void PlayBrain(){
    toggleBrain = toggleBtnBrain.GetComponent<Toggle>().isOn;
    PlayerPrefsHelper.SetBool ("playasbrain", toggleBrain);
  }

  public void NoDmg(){
    toggleNoDmg = toggleBtnNoDmg.GetComponent<Toggle>().isOn;
    PlayerPrefsHelper.SetBool ("nodmg", toggleNoDmg);
  }

  public void PauseGame(){
    GameManager.instance.PauseGame ();
  }
  void OnLevelWasLoaded(){
  }
}
