using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StruggleController : MonoBehaviour
{
	[SerializeField] KeyCode up, down, left, right;
	[SerializeField] float struggleMagnitude = 0.5f;
	[SerializeField] float struggleForce = 4f;
	[SerializeField] PlayerController controller;
	bool struggleState = false;
	void Start()
	{
		
	}

	void FixedUpdate()
	{	
		if(!struggleState){
			return;
		}
		Vector2 axis = GetAxis();
		transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right*axis.x*struggleMagnitude + Vector3.up*axis.y*struggleMagnitude, Time.deltaTime*struggleForce);
	}
	Vector2 GetAxis(){
		Vector2 axis = Vector2.zero;
		if(Input.GetKey(up)){
			axis.y = 1;
		}
		else if(Input.GetKey(down)){
			axis.y = -1;
		}
		if(Input.GetKey(left)){
			axis.x = -1;
		}
		else if(Input.GetKey(right)){
			axis.x = 1;
		}
		return axis;
	}
	public void EnableStruggleMode(){
		controller.Disable();
		struggleState = true;
	}
}
