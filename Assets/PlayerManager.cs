using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;
	
	public delegate void PlayerChangeHandler(Player[] player);
	public event PlayerChangeHandler OnPlayersChanged;
	[SerializeField] SpawnPoint spawnPoint;
	[SerializeField] GameObject TeamACharacter;
	[SerializeField] GameObject TeamBCharacter;
	[SerializeField] Animator gameOverMenu;
	[SerializeField] RespawnHUDManager respawnHUDManager;
	[SerializeField] float slowMotionSpeed, slowMotionLerpTime;

	List<Player> players = new List<Player>();
	void Awake()
	{
		if(PlayerManager.instance == null){
			PlayerManager.instance = this;
		}
		else{
			Debug.LogError("Cannot initialize player manager");
			Destroy(this);
		}
		
	}
	public void AddPlayer(Player newPlayer){
		players.Add(newPlayer);
		UpdatePlayers();
	}
	// public void ReplacePlayer
	public void SpawnPlayer(float respawnTime, PlayerTeam team){
		StartCoroutine(RespawnPlayer(respawnTime, team));
	}
	public void RemovePlayer(Player player){
		for(int i = 0; i < players.Count; i++){
			if(players[i] == player){
				players.RemoveAt(i);
				UpdatePlayers();
				
				return;
			}
		}
		
	}
	public Player[] GetPlayers(){
		return players.ToArray();
	}
	void UpdatePlayers(){
		if(players.Count == 0){
			GameOver();
		}
		if(OnPlayersChanged != null){
			OnPlayersChanged.Invoke(players.ToArray());
		}
	}
	void GameOver(){
		gameOverMenu.SetTrigger("enable");
		SlowMotion.LerpSlowDown(slowMotionSpeed, slowMotionLerpTime, this);
	}
	IEnumerator RespawnPlayer(float respawnTime, PlayerTeam team){
		respawnHUDManager.AddRespawn(team, respawnTime);
		yield return new WaitForSeconds(respawnTime);
		if(players.Count != 0){
			spawnPoint.SpawnPlayer(GetPlayerPrefab(team));
			// Instantiate(GetPlayerPrefab(team), spawnPoint.position, Quaternion.identity);
		}
		
	}
	GameObject GetPlayerPrefab(PlayerTeam team){
		switch(team){
			case PlayerTeam.A:
				return TeamACharacter;
				break;
			case PlayerTeam.B:
				return TeamBCharacter;
				break;
			default:
				return null;
				break;
		}
	}
	// public void SpawnPlayer(){

	// }
}
