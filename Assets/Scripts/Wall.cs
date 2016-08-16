using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

  public Sprite wallDmg;
  public int hp = 4;
  public AudioClip chopSound;
  public AudioClip chopSound2;

  private SpriteRenderer spriteRenderer;

	void Awake () {
    spriteRenderer = GetComponent<SpriteRenderer>();
	}

  public void DamageWall(int dmgTaken) {
    SoundManager.instance.RandomizeSounds(chopSound, chopSound2);
    spriteRenderer.sprite = wallDmg;
    hp -= dmgTaken;
    if (hp <= 0) {
      gameObject.SetActive(false);
    }
  }
	

}
