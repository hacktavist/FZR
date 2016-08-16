using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour {
  public float moveTime = .01f;
  public LayerMask blockingLayer;

  private BoxCollider2D boxCollider;
  private Rigidbody2D rigidBody;
  private float inverseMoveTime;
	// Use this for initialization
	protected virtual void Start () {
    boxCollider = GetComponent<BoxCollider2D>();
    rigidBody = GetComponent<Rigidbody2D>();
    inverseMoveTime = 1f / moveTime;
	}

  protected IEnumerator SmoothMovement(Vector3 end) {
    float sqRemainingDist = (transform.position - end).sqrMagnitude;

    while (sqRemainingDist > float.Epsilon) {
      Vector3 newPos = Vector3.MoveTowards(rigidBody.position, end, inverseMoveTime * Time.deltaTime);
      rigidBody.MovePosition(newPos);
      sqRemainingDist = (transform.position - end).sqrMagnitude;
      yield return null;
    }
  }

  protected bool Move(int xDir, int yDir, out RaycastHit2D rHit) {
    Vector2 start = transform.position;
    Vector2 end = start + new Vector2(xDir, yDir);
    boxCollider.enabled = false;
    rHit = Physics2D.Linecast(start, end, blockingLayer);
    boxCollider.enabled = true;
    if (rHit.transform == null && gameObject.activeInHierarchy) {
      StartCoroutine(SmoothMovement (end));
      return true;
    }

    return false;
  }

  protected virtual void AttemptMove<TWall, TEnemy> (int xDir, int yDir)
    where TWall  : Component
    where TEnemy : Component
  {
    RaycastHit2D hit;
    bool canMove = Move(xDir, yDir, out hit);
    if (hit.transform == null) {
      return;
    }

    if (hit.transform.tag == "Wall") {
      TWall hitComponent = hit.transform.GetComponent<TWall> ();
      if (!canMove && hitComponent != null) {
        OnCantMove(hitComponent);
      }
    } else if (hit.transform.tag == "Player") {
      TEnemy hitComponent = hit.transform.GetComponent<TEnemy> ();
      if (!canMove && hitComponent != null) {
        OnCantMove(hitComponent);
      }
    } else if (hit.transform.tag == "Enemy") {
      TEnemy hitComponent = hit.transform.GetComponent<TEnemy> ();
      if (!canMove && hitComponent != null) {
        OnCantMove(hitComponent);
      } 
    }  else if (hit.transform.tag == "Human") {
      TEnemy hitComponent = hit.transform.GetComponent<TEnemy> ();
      if (!canMove && hitComponent != null) {
        OnCantMove(hitComponent);
      } 
    } 




  }






  protected abstract void OnCantMove<T>(T component)
    where T : Component;

}
