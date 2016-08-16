using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : Movement {
  public int leastPlayerDmg;
  public int maxPlayerDmg;
  public int enemyHealth;
  public AudioClip attack1;
  public AudioClip attack2;
  public int healthBoost;

  private Animator animator;
  private Transform target;
  private bool skipMove;
  private Text damageTaken;
  private int randomDmg;
  public int staticDmg;
  public int wallDmg;
  private int dmg;
  private SpriteRenderer r;
  private Player playerObj;
	protected override void Start () {
    playerObj = GameObject.Find("Player").GetComponent<Player>();
    GameManager.instance.AddEnemyToList(this);
    r = gameObject.GetComponent<SpriteRenderer> ();
    animator = GetComponent<Animator>();
    damageTaken = gameObject.GetComponentInChildren<Text>();//GameObject.Find ("DamageText").GetComponent<Text> ();
    target = GameObject.FindGameObjectWithTag("Player").transform;
    damageTaken.text = "";
    base.Start();

	}

  protected override void AttemptMove<TWall, TEnemy>(int xDir, int yDir) {
    if (skipMove) {
      skipMove = false;
      return;
    }


    base.AttemptMove<TWall, TEnemy>(xDir, yDir);
    skipMove = true;
  }

  public virtual void MoveEnemy() {
    int xDir = 0;
    int yDir = 0;

    if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
      yDir = target.position.y > transform.position.y ? 1 : -1;
    } else {
      xDir = target.position.x > transform.position.x ? 1 : -1;
    }
    AttemptMove<Wall, Player>(xDir, yDir);

  }

  protected override void OnCantMove<T>(T component) {
    if (gameObject.activeInHierarchy == true) {

      if (component.tag == "Player") {
        Player hitPlayer = component as Player;
        if(gameObject.tag == "Human"){
          dmg = staticDmg;
        } else{
          randomDmg = Random.Range (leastPlayerDmg, maxPlayerDmg);
          dmg = randomDmg;
        }

        hitPlayer.LoseItems (dmg);
              animator.SetTrigger("enemyAttack");
        SoundManager.instance.RandomizeSounds (attack1, attack2);
      
      }  else if (component.tag == "Wall") {
        Wall hitWall = component as Wall;
        hitWall.DamageWall (wallDmg);
      }

    }
  }

  IEnumerator SimpleWait(float secsToWait){
    yield return new WaitForSeconds (secsToWait);
    damageTaken.text = "";
  }

  IEnumerator DamageFlash(int numOfFlashes){
    for (int i = 0; i < numOfFlashes; i++) {
      yield return new WaitForSeconds(.1f);
      r.color = new Color32 (223, 18, 18, 255);
      yield return new WaitForSeconds (.1f);
      r.color = new Color32 (255, 255, 255, 255);
    }

  }

  public void EnemyDamaged(int dmgAmt){
    damageTaken.text = "-" + dmgAmt + " damage";
    StartCoroutine (DamageFlash(3));
    StartCoroutine(SimpleWait (.25f));

    enemyHealth -= dmgAmt;

    if (enemyHealth <= 0){
      if (gameObject.tag == "Human") {
        playerObj.foodTotal += healthBoost;
        playerObj.HealthMeter();
      }
      gameObject.SetActive(false);
      playerObj.CheckForFatChange();
      playerObj.CheckGameWon();
    }
  }
}