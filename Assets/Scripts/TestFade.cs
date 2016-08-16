using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TestFade : MonoBehaviour {

	// Use this for initialization
	void Start () {
    GameObject.Find ("Title").GetComponent<Image> ().canvasRenderer.SetAlpha (-1.0f);
    StartCoroutine( test ());
     
	}
  void Update(){
    if (Input.anyKeyDown) {
      Application.LoadLevel (Application.loadedLevel + 1);
    }
  }
  IEnumerator test(){

    float fade = GameObject.Find ("Main Camera").GetComponent<Fade>().Fader(1);
    yield return new  WaitForSeconds(fade);
    fade = GameObject.Find ("Main Camera").GetComponent<Fade> ().Fader(0);
    yield return new WaitForSeconds(fade);
    Application.LoadLevel (Application.loadedLevel + 1);
  }
}
