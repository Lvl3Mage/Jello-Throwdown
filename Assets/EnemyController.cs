using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	NavSystem navigation;
	const int JumpTriggerLayer = 7, PlayerLayerIndex = 6, BulletLayerIndex = 10;
	int activeJumpTriggers = 0;
	bool jumpDelayed = false;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] float speed;
	[SerializeField] float acceleration;
	[SerializeField] float jumpStrength;
	[SerializeField] float leapStrength;
	[SerializeField] float jumpDelayTime;
	[SerializeField] float airspeedFactor;
	[SerializeField] float minJumpHeightDif;
	[SerializeField] SpriteRenderer sr;
	[SerializeField] GameObject DestructionEffect;
	// Start is called before the first frame update
	void Start()
	{
		navigation = GameObject.FindGameObjectWithTag("NavSystem").GetComponent<NavSystem>();
		StartCoroutine(TargetSearch());
	}

	Vector2 target = Vector2.zero;
	void Update()
	{
		sr.flipX = Mathf.Sign(rb.velocity.x) < 0;
		float airAccelMultiplier = 1;
		if(jumpDelayed){
			airAccelMultiplier = airspeedFactor;
		}
		// Vector2 target = navigation.NavToPlayer(transform.position);
		Debug.DrawLine(transform.position, target, Color.green);
		//Get Nav Target
		Vector2 targetDelta = target - (Vector2)transform.position;
		//Horizontal Movement
		rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, Mathf.Sign(targetDelta.x)*speed, Time.deltaTime*acceleration*airAccelMultiplier), rb.velocity.y);

		if(targetDelta.y > minJumpHeightDif && activeJumpTriggers > 0 && !jumpDelayed){
			rb.velocity = new Vector2(rb.velocity.x*leapStrength, jumpStrength);
			StartCoroutine(JumpDelay(jumpDelayTime));
		}
		//Check height differrence
			//if grounded and contacting jump trigger => jump
	}
	IEnumerator JumpDelay(float delaySeconds){
		jumpDelayed = true;
		yield return new WaitForSeconds(delaySeconds);
		jumpDelayed = false;
	}
	IEnumerator TargetSearch(){
		while(true){
			
			yield return new WaitForSeconds(0.2f);
			target = navigation.NavToPlayer(transform.position);

		}
		
	}
	void OnCollisionEnter2D(Collision2D col){
		if(col.collider.gameObject.layer == PlayerLayerIndex){
			PlayerCollision(col);
		}
		
	}
	void PlayerCollision(Collision2D col){
		if(col.collider.attachedRigidbody == null){
			return;
		}
		Player player = col.collider.attachedRigidbody.gameObject.GetComponent<Player>();
		if(player == null){
			return;
		}
		player.DestroyPlayer();
	}
	public void DestroyEnemy(){
		Destroy(gameObject);
		Instantiate(DestructionEffect, transform.position, Quaternion.identity);
	}
	public void AddJumpTrigger(){
		activeJumpTriggers++;
	}
	public void RemoveJumpTrigger(){
		activeJumpTriggers--;
	}
}
