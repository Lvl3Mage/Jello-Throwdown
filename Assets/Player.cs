using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] PlayerTeam playerTeam;
	const float respawnTime = 1f, reviveTime = 5f;
	public PlayerTeam team
	{
		get{
			return playerTeam;
		}
	}
	void Start()
	{
		// Debug.Log(PlayerManager.instance);
		PlayerManager.instance.AddPlayer(this);
	}

	void Update()
	{
		
	}
	public void Despawn(){
		PlayerManager.instance.RemovePlayer(this, respawnTime);
	}
	public void DestroyPlayer(){
		PlayerManager.instance.RemovePlayer(this, reviveTime);
	}
}
public enum PlayerTeam{
	A,
	B
}
