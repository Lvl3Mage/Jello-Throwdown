using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;
	
	public delegate void PlayerChangeHandler(Player[] player);
	public event PlayerChangeHandler OnPlayersChanged;

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
	public void RemovePlayer(Player player){
		for(int i = 0; i < players.Count; i++){
			if(players[i] == player){
				players.RemoveAt(i);
				return;
			}
		}
		UpdatePlayers();
	}
	public Player[] GetPlayers(){
		return players.ToArray();
	}
	void UpdatePlayers(){
		if(OnPlayersChanged != null){
			OnPlayersChanged.Invoke(players.ToArray());
		}
	}
	// public void SpawnPlayer(){

	// }
}
