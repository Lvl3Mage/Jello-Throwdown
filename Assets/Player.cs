using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] PlayerTeam playerTeam;
	[SerializeField] GameObject DestructionEffect;
	const float reviveTime = 5f;
	public PlayerTeam team
	{
		get{
			return playerTeam;
		}
	}
	void Start()
	{
		PlayerManager.instance.AddPlayer(this);
	}
	public void Despawn(){
		PlayerManager.instance.RemovePlayer(this);
	}
	public void DestroyPlayer(){
		PlayerManager.instance.RemovePlayer(this);
		PlayerManager.instance.SpawnPlayer(reviveTime, team);
		// Instantiate(DestructionEffect,transform.position,transform.rotation);
		Destroy(gameObject);
	}
}
public enum PlayerTeam{
	A,
	B
}
