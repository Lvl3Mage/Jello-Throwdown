using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour // spaghetti xd
{
	const float respawnTime = 1f;
	[SerializeField] float maxTravelDistance;
	[SerializeField] float maxLifetime = 1;
	[SerializeField] GameObject DestructionEffect;
	Vector2 pastPos;
	float accumDistance = 0;
	Coroutine lifetimeRoutine;
	bool destroyed = false;
	PlayerTeam team;
	void Awake(){
		pastPos = transform.position;
		lifetimeRoutine = StartCoroutine(LifetimeDelay(maxLifetime));
	}
	public void SetTeam(PlayerTeam _team){
		team = _team;
	}
	void Update(){
		accumDistance += (pastPos - (Vector2)transform.position).magnitude;
		pastPos = transform.position;
	}
	void OnCollisionEnter2D(Collision2D col){
		if(accumDistance > maxTravelDistance){
			DestroyBullet();
		}
	}
	IEnumerator LifetimeDelay(float lifetimeSeconds){
		yield return new WaitForSeconds(lifetimeSeconds);
		DestroyBullet();
	}
	void DestroyBullet(){
		if(destroyed){
			return;
		}
		destroyed = true;
		// Instantiate(DestructionEffect,transform.position, Quaternion.Identity);
		PlayerManager.instance.SpawnPlayer(respawnTime, team);
		Destroy(gameObject);

	}
}
