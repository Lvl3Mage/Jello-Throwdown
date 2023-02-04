using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other){
		EnemyController controller = other.gameObject.GetComponent<EnemyController>();
		if(!controller){
			return;
		}
		controller.AddJumpTrigger();
	}
	void OnTriggerExit2D(Collider2D other){
		EnemyController controller = other.gameObject.GetComponent<EnemyController>();
		if(!controller){
			return;
		}
		controller.RemoveJumpTrigger();
	}
}
