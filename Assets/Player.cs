using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	void Start()
	{
		// Debug.Log(PlayerManager.instance);
		PlayerManager.instance.AddPlayer(this);
	}

	void Update()
	{
		
	}
	public void RemovePlayer(){
		PlayerManager.instance.RemovePlayer(this);
	}
}
