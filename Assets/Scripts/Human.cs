using UnityEngine;
using System.Collections;

public class Human : Enemy {


  Player p;
  Component playerComponent;
  Transform currentPos;
  Transform targetPos;
  float diffDistX;
  float diffDistY;
  Vector2 startHit;
  Vector2 endHit;

  void Awake(){
    playerComponent = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
    p = playerComponent as Player;
    targetPos = GameObject.FindGameObjectWithTag ("Player").transform;
  }

  protected override void Start () {

    base.Start ();
	}



   public override void MoveEnemy(){
    if (gameObject.activeInHierarchy == true) {
      // overriding Enemy class method MoveEnemy to give humans a "shooting" 
      //  style functionality
      diffDistX = Mathf.Abs (targetPos.position.x - transform.position.x);
      diffDistY = Mathf.Abs (targetPos.position.y - transform.position.y);
      // figure this part out! need a line cast to check if a wall is between
      //  the human and the player

      //startHit = transform.position;
      //endHit = new Vector2(transform.position.x, transform.position.y);

      if (diffDistX < float.Epsilon) {
        if (diffDistY == -3 || diffDistY == 3)
          p.LoseItems (staticDmg);
      } else if (diffDistY < float.Epsilon) {
        if (diffDistX == -3 || diffDistX == 3)
          p.LoseItems (staticDmg);
      } else {
        base.MoveEnemy ();
      }

    }
  }
}
