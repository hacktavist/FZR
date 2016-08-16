using UnityEngine;
using System.Collections;

public class SmoothCam : MonoBehaviour {

  public Transform target;
  public  float speed = .01f;
  private Camera playerCam;

  void Start() {
    playerCam = GetComponent<Camera>();
  }

  void Update() {
    playerCam.orthographicSize = (Screen.height / 100f) / 2f;

    if (target) {
      transform.position = Vector3.Lerp(transform.position, target.position, speed) + new Vector3(0f, 0f, -10f);
    }
  }

}
