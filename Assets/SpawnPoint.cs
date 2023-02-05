using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	[SerializeField] Animator animator;
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	[SerializeField] GameObject cameraTargeter;
	GameObject curTargeter;
	//very shitty way of doing this but I don't care
	GameObject playerToSpawn;
	public void SpawnPlayer(GameObject playerPrefab){
		playerToSpawn = playerPrefab;
		curTargeter = Instantiate(cameraTargeter, transform.position, Quaternion.identity);
		animator.SetTrigger("shoot");
	}
	void ShootPlayer(){
		Destroy(curTargeter);
		Instantiate(playerToSpawn, transform.position, Quaternion.identity);
	}
}
