using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	[SerializeField] float maxTravelDistance;
	[SerializeField] GameObject DestructionEffect;
	Vector2 pastPos;
	float accumDistance = 0;
	void Awake(){
		pastPos = transform.position;

	}
	void Update(){
		accumDistance += (pastPos - (Vector2)transform.position).magnitude;
		pastPos = transform.position;
	}
	void OnCollisionEnter2D(Collision2D col){
		if(accumDistance > maxTravelDistance){
			// Instantiate(DestructionEffect,transform.position, Quaternion.Identity);
			Destroy(gameObject);
		}
	}
}
