using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdScript : MonoBehaviour {

  public string gameId;

  void Start(){
    if (Advertisement.isSupported) {
      Advertisement.Initialize (gameId);
    }
  }
  public void ShowAd () {
    
    var showOptions = new ShowOptions {
      resultCallback = HandleShowResult
    };
    if (Advertisement.IsReady ()) {
      GameManager.instance.PauseGame ();
      Advertisement.Show (null, showOptions);

    }
	}


  private void HandleShowResult(ShowResult result)
  {
    switch (result)
    {
    case ShowResult.Finished:
      Debug.Log ("The ad was successfully shown.");
      GameManager.instance.PauseGame ();
      break;

    case ShowResult.Skipped:
      Debug.Log ("The ad was skipped before reaching the end.");
      GameManager.instance.PauseGame ();
      break;

    case ShowResult.Failed:
      Debug.LogError ("The ad failed to be shown.");
      GameManager.instance.PauseGame ();
      break;
    }
  }
	

}
