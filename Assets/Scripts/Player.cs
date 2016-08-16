using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : Movement {
  public int wallDmg = 1;
  public int foodPts = 10;
  public int sodaPts = 15;
  public float restartDelay = .5f;
  public int attackPower;
  public bool playAsBrain;
  public bool noDmg;

  public int wins;
  
  public AudioClip death;
  public AudioClip eat1;
  public AudioClip eat2;
  public AudioClip drink1;
  public AudioClip drink2;
  public AudioClip footsteps1;
  public AudioClip footsteps2;
  [SerializeField] const int maxFood = 100;
  public Image foodImage;
  public Text damageText;
  private Animator animator;
  public int foodTotal;
  private int playerMoveNum;
  private Vector2 touchOrigin = -Vector2.one;
  private float dragDist;
  private SpriteRenderer r;

	// Use this for initialization
	protected override void Start () {
    wins = GameManager.wins;
    dragDist = Screen.width * 20 / 100;
    animator = GetComponent<Animator>();
    playAsBrain = GameManager.instance.playAsBrain;
    PlayAsBrain ();
    noDmg = GameManager.instance.noDmg;
    r = gameObject.GetComponent<SpriteRenderer> ();
    foodTotal = GameManager.instance.foodStats;
    foodImage.fillAmount = (float)foodTotal / maxFood;
    damageText = GameObject.Find ("DamageText").GetComponent<Text> ();
    damageText.text = "";
    if(playAsBrain == false)
      CheckForFatChange();
    base.Start();
	}

  public void HealthMeter(){
    foodImage.fillAmount = (float)foodTotal / maxFood;
  }

  private void PlayAsBrain(){
    if (playAsBrain == true)
      animator.SetBool ("brain", true);
  }
  private void OnDisable() {
    GameManager.instance.foodStats = foodTotal;
  }
	// Update is called once per frame
	void Update () {
	  if(GameManager.instance.playerTurn == false){
      return;
    }

    int horizontal = 0;
    int vertical = 0;

    #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
    horizontal = (int)Input.GetAxisRaw("Horizontal");
    vertical = (int)Input.GetAxisRaw("Vertical");

	// end regular input

#elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID || UNITY_WP8

		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];
			
			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began)
			{
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}
			
			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
      else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				//Set touchEnd to equal the position of this touch
        Vector2 touchEnd = myTouch.position;
				
				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;
				
				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;
				
				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;
				
        if(Mathf.Abs(x) > dragDist || Mathf.Abs(y) > dragDist){
				  //Check if the difference along the x axis is greater than the difference along the y axis.
  				if (Mathf.Abs(x) > Mathf.Abs(y))
  					//If x is greater than zero, set horizontal to 1, otherwise set it to -1
  					horizontal = x > 0 ? 1 : -1;
  				else
  					//If y is greater than zero, set horizontal to 1, otherwise set it to -1
  					vertical = y > 0 ? 1 : -1;
        }
			}
		}
		
		#endif // end mobile input
	if (GameManager.instance.pause == false) {
			if (horizontal != 0) {
				vertical = 0;
			}

			if (horizontal != 0 || vertical != 0) {
				AttemptMove<Wall, Enemy> (horizontal, vertical);
			}
		}
	}

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Exit") {
      Invoke("NextLevel", restartDelay);
      enabled = false;
    } else if (other.tag == "Food") {
      if (foodTotal < maxFood) {
        foodTotal += foodPts;
        HealthMeter();
        SoundManager.instance.RandomizeSounds(eat1, eat2);
        other.gameObject.SetActive(false);
        if(playAsBrain == false)
          CheckForFatChange(); 
        
      }
    } else if (other.tag == "Soda") {
      if (foodTotal < maxFood) {
        foodTotal += sodaPts;
        HealthMeter();
        SoundManager.instance.RandomizeSounds(drink1, drink2);
        other.gameObject.SetActive(false);
        if(playAsBrain == false)
          CheckForFatChange();
        
      }
    }


    CheckGameWon();
  }

  protected override void AttemptMove<TWall, TEnemy>(int xDir, int yDir) {
    if (playerMoveNum == 5 && noDmg == false) {
      foodTotal-=3;
      HealthMeter();
      playerMoveNum = 0;
    }
    if(playAsBrain == false)
      CheckForFatChange();


    
    base.AttemptMove<TWall, TEnemy>(xDir, yDir);
    RaycastHit2D hit;
    if (Move(xDir, yDir, out hit)) {
      SoundManager.instance.RandomizeSounds(footsteps1, footsteps2);
    }
    CheckGameOver();
    GameManager.instance.playerTurn = false;
    playerMoveNum++;
    GameManager.instance.turnDelay = .1f;
  }

  protected override void OnCantMove<T>(T component) {
    if (component.tag == "Enemy") {
      Enemy hitEnemy = component as Enemy;
      hitEnemy.EnemyDamaged (attackPower);
    } else if (component.tag == "Human") {
      Human hitHuman = component as Human;
      hitHuman.EnemyDamaged(attackPower);
    } else if (component.tag == "Wall") {
      Wall hitWall = component as Wall;
      hitWall.DamageWall(wallDmg);
    }
    //animator.SetTrigger("playerAttack");
  }

  private void NextLevel() {
    GameManager.instance.NextLevel();
  }

  private void RestartGame() {
    foodTotal = GameManager.instance.foodStats;
    GameManager.instance.RestartGame();
  }

  IEnumerator SimpleWait(float secsToWait){
    yield return new WaitForSeconds (secsToWait);
    damageText.text = "";
  }
  
  IEnumerator DamageFlash(int numOfFlashes){
    for (int i = 0; i < numOfFlashes; i++) {
      yield return new WaitForSeconds(.1f);
      r.color = new Color32 (223, 18, 18, 255);
      yield return new WaitForSeconds (.1f);
      r.color = new Color32 (255, 255, 255, 255);
    }
    
  }
  
  public void LoseItems(int loss) {
    if (noDmg == false) {
      //animator.SetTrigger("playerHit");
      damageText.text = "-" + loss + " Damage!";
      StartCoroutine(DamageFlash(3));
      StartCoroutine (SimpleWait (.25f));
      foodTotal -= loss;
      HealthMeter();
      CheckGameOver ();
    }
      if (playAsBrain == false)
        CheckForFatChange ();
      

  }

  public void CheckForFatChange() {
    if (foodTotal < 25) {
      // set sprite to withered
      animator.SetBool("normal", false);
      animator.SetBool("fat", false);
      animator.SetBool("wither", true);
    } else if (foodTotal > 24 && foodTotal < 75){
      // set sprite to regular
      animator.SetBool("wither", false);
      animator.SetBool("fat", false);
      animator.SetBool("normal", true);
    } else {
      // set sprite to fat
      animator.SetBool("wither", false);
      animator.SetBool("normal", false);
      animator.SetBool("fat", true);
    }
  }

  public void CheckGameWon() {
    if (foodTotal > maxFood)
      foodTotal = maxFood;
    if (foodTotal == maxFood) {
      
      wins++;
      GameManager.wins = wins;
      SaveManager.Save();
      SoundManager.instance.musicSource.Stop();
      GameManager.instance.GameWon();
    }
  }
  private void CheckGameOver() {
    if (foodTotal <= 0) {
      SoundManager.instance.PlaySingle(death);
      SoundManager.instance.musicSource.Stop();
      GameManager.instance.GameOver();
    }
  }
}
