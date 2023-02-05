using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Triggers")]
	[SerializeField] TriggerCol groundedCol;
	[SerializeField] TriggerCol extendedCol;
	[SerializeField] TriggerCol wallCol;

	[Header("Controlls")]
	[SerializeField] KeyCode left;
	[SerializeField] KeyCode right;
	[SerializeField] KeyCode jump;

	[Header("Movement")]
	[SerializeField] float moveSpeed;
	[SerializeField] float groundAcceleration;
	[SerializeField] float airAcceleration;
	[SerializeField] float jumpSpeed;
	[SerializeField] float jumpDelayTime;
	[SerializeField] int maxAirJumps;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] SpriteRenderer sr;
	[SerializeField] Animator animator;
	

	int airJumpCount = 0;

	// bool jumpFlag = false;
	// Coroutine jumpFlagRoutine;
	bool jumpDelayed = false;

	void Start()
	{
		
	}

	void Update()
	{
		sr.flipX = Mathf.Sign(rb.velocity.x) < 0;
		animator.SetFloat("velXAbs", Mathf.Abs(rb.velocity.x));
		animator.SetBool("inAir", !canJump());
		float acceleration = groundAcceleration;
		if(!groundedCol.colliding){
			acceleration = airAcceleration;
		}
		rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, moveSpeed*GetHAxis(), Time.deltaTime*acceleration), rb.velocity.y);
		if(canJump()){
			airJumpCount = 0;
		}
		if(Input.GetKeyDown(jump) && !jumpDelayed){
			if(canJump() || airJumpCount < maxAirJumps){
				StartCoroutine(JumpDelay(jumpDelayTime));
				rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
			}
			// else if(airJumpCount < maxAirJumps){
			// 	StartCoroutine(JumpDelay(jumpDelayTime));
			// 	rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed);
			// 	airJumpCount++;
			// }
			
		}
	}
	float GetHAxis(){
		if(Input.GetKey(right)){
			return 1;
		}
		else if(Input.GetKey(left)){
			return -1;
		}
		return 0;
	}
	
	bool canJump(){
		return groundedCol.colliding || extendedCol.colliding && !wallCol.colliding;
	}
	IEnumerator JumpDelay(float delaySeconds){
		jumpDelayed = true;
		yield return new WaitForSeconds(delaySeconds);
		jumpDelayed = false;
	}
	public void Disable(){
		Collider2D[] cols = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D col in cols){
			col.enabled = false;
		}
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		this.enabled = false;
	}
	// IEnumerator RaiseJumpFlag(float delaySeconds){
	// 	jumpFlag = true;
	// 	yield return new WaitForSeconds(delaySeconds);
	// 	jumpFlag = false;
	// }
}