using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Fade : MonoBehaviour {
  public Image gameTitle;
  public float fadeTime = 1.5f;





  public float Fader(int direction){

    gameTitle.CrossFadeAlpha (direction, fadeTime, true);
    return (fadeTime);
  }


}
