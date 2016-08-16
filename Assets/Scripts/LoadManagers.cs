using UnityEngine;
using System.Collections;

public class LoadManagers : MonoBehaviour {
  public GameObject gameManager;
  public GameObject soundManager;

	void Awake () {
    if (GameManager.instance == null)
      Instantiate(gameManager);
    if (SoundManager.instance == null)
      Instantiate(soundManager);
	}
	

}
