using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlayerController : MonoBehaviour
{
	[SerializeField] TriggerCol grabZone;
	[SerializeField] Transform grabPoint;
	[SerializeField] float maxDistance;
	[SerializeField] Vector2 bulletVelocity;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Rigidbody2D PlayerBullet;
	[SerializeField] KeyCode throwKey;
	[SerializeField] Player selfPlayer;
	[SerializeField] GameObject SmallPlayerPrefab;
	[SerializeField] Animator animator;
	[SerializeField] Vector2 xSpawnVelRange, ySpawnVelRange;
	[SerializeField] float stompRadius = 5;
	[SerializeField] LayerMask enemyLayers;
	const int playerLayerIndex = 6;
	bool collided = false;
	Player grabbedPlayer;
	void Start()
	{
		selfPlayer.OnDestruction += PlayerDestroyed;
		PlayerManager.instance.OnPlayersChanged += UpdatePlayers;

		Vector2 spawnVel = new Vector2(Random.Range(xSpawnVelRange.x, xSpawnVelRange.y),Random.Range(ySpawnVelRange.x, ySpawnVelRange.y));
		rb.velocity = spawnVel;
		selfPlayer.invulnerable = true;

		UpdatePlayers(PlayerManager.instance.GetPlayers()); // in case the player is spawned when no players are left

	}
	void PlayerDestroyed(){
		if(grabbedPlayer){
			grabbedPlayer.DestroyPlayer();
		}
	}
	void UpdatePlayers(Player[] players){
		if(players.Length == 1 && players[0] == selfPlayer){
			//await animation
			animator.SetTrigger("transform");
			// ReplacePlayer();
		}
	}
	void ReplacePlayer(){
		Instantiate(SmallPlayerPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = rb.velocity;
		// Debug.Log(PlayerManager.instance.GetPlayers().Length);
		selfPlayer.Despawn();
		Destroy(gameObject);
	}
	void LateUpdate()
	{
		if(!grabbedPlayer){
			CheckGrabOverlap();
		}
		else{
			GrabLerp();
			if(Input.GetKeyDown(throwKey)){
				StartThrow();
			}
		}
	}
	void CheckGrabOverlap(){
		if(grabZone.colCount == 0){
			return;
		}
		Collider2D[] cols = grabZone.overlapCols;

		for(int i = 0; i < cols.Length; i++){
			if(cols[i].gameObject.layer == playerLayerIndex){
				GrabPlayer(cols[i].attachedRigidbody.gameObject);
				return;
			}
		}
	}
	void GrabLerp(){
		// grabbedPlayer.position =grabPoint.position;
		Vector2 lerpDelta = grabPoint.position - grabbedPlayer.transform.position;
		if(lerpDelta.magnitude > maxDistance){

			grabbedPlayer.transform.position = (Vector2)grabPoint.position - lerpDelta.normalized*maxDistance;
		}
		// grabbedPlayer.position = Vector2.Lerp(grabbedPlayer.position, grabPoint.position, Time.deltaTime*lerpSpeed);
	}
	void GrabPlayer(GameObject player){
		SmallPlayerController controller = player.GetComponent<SmallPlayerController>();
		controller.EnableStruggleMode();
		grabbedPlayer = player.GetComponent<Player>();
		animator.SetTrigger("grab");
	}
	void StartThrow(){
		//Handle animator
		animator.SetTrigger("throw");
		// ThrowPlayer(); // remove call when animating
	}
	void ThrowPlayer(){

		
		Rigidbody2D bulletRB = Instantiate(PlayerBullet, grabPoint.position, grabPoint.rotation);
		Vector2 vel = bulletVelocity;
		vel.x *= Mathf.Sign(rb.velocity.x);
		bulletRB.velocity = vel;
		BulletController bullet = bulletRB.gameObject.GetComponent<BulletController>();
		bullet.SetTeam(grabbedPlayer.team);
		grabbedPlayer.Despawn();
		Destroy(grabbedPlayer.gameObject);

		CameraShake.instance.Shake(50,100);
	}
	void OnCollisionEnter2D(Collision2D col){
		if(!collided){
			collided = true;
			selfPlayer.invulnerable = false;
			Stomp();
		}
	}
	void Stomp(){
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, stompRadius, enemyLayers);
		foreach(Collider2D col in cols){
			col.attachedRigidbody.gameObject.GetComponent<EnemyController>().DestroyEnemy(100);
		}
		CameraShake.instance.Shake(50,100);
	}
}
