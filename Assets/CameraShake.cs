using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public static CameraShake instance;
	[SerializeField] Transform followPoint;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] TargetJoint2D tj;
	void Awake()
	{
		if(CameraShake.instance == null){
			CameraShake.instance = this;
		}
		else{
			Debug.LogError("Cannot initialize camara shake");
			Destroy(this);
		}
	}

	void FixedUpdate()
	{
		tj.target = followPoint.position;
	}
	public void Shake(float lineaForce, float angularForce){
		Vector2 offsetVector = Random.insideUnitCircle.normalized;
		if(offsetVector == Vector2.zero){
			offsetVector = Vector2.down;
		}
		offsetVector*= lineaForce;

		float angularOffset = (Random.Range(0,2)*2 - 1)*angularForce;
		rb.velocity += offsetVector;
		rb.angularVelocity = angularOffset;
	}
}
