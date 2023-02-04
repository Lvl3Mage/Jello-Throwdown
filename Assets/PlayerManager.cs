using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;
	
	public delegate void PlayerChangeHandler(Player[] player);
	public event PlayerChangeHandler OnPlayersChanged;
	[SerializeField] Transform spawnPoint;
	[SerializeField] GameObject TeamACharacter;
	[SerializeField] GameObject TeamBCharacter;

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
	public void RemovePlayer(Player player, float respawnTime){
		for(int i = 0; i < players.Count; i++){
			if(players[i] == player){
				players.RemoveAt(i);
				UpdatePlayers();
				StartCoroutine(RespawnPlayer(respawnTime, player.team));
				return;
			}
		}
		
	}
	public Player[] GetPlayers(){
		return players.ToArray();
	}
	void UpdatePlayers(){
		if(OnPlayersChanged != null){
			OnPlayersChanged.Invoke(players.ToArray());
		}
	}
	public IEnumerator RespawnPlayer(float respawnTime, PlayerTeam team){
		yield return new WaitForSeconds(respawnTime);
		Instantiate(GetPlayerPrefab(team), spawnPoint.position, Quaternion.identity);
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
