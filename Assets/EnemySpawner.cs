using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] GameObject enemy;
	[SerializeField] int enemyIncrease = 4;
	[SerializeField] float timeIncrease = 4;
	[SerializeField] float waveWaitTime = 2;
	void Start()
	{
		StartCoroutine(NewWave(1));
	}

	void Update()
	{
		
	}
	List<GameObject> enemies = new List<GameObject>();
	IEnumerator WaitForEnemiesOver(){
		while(enemies.Count > 0){
			yield return null;
			for(int i = 0; i < enemies.Count; i++){
				if(enemies[i] == null){
					enemies.RemoveAt(i);
				}
			}
		}
		yield return new WaitForSeconds(waveWaitTime);
	}
	IEnumerator NewWave(int id){
		int enemyAmount = id*enemyIncrease;
		float timespan = id*timeIncrease;
		yield return SpawnEnemies(enemyAmount, timespan);
		yield return WaitForEnemiesOver();
		StartCoroutine(NewWave(id+1));
	}
	IEnumerator SpawnEnemies(int amount, float timespan){
		float delta = timespan / amount;
		while(amount > 0){
			SpawnEnemy();
			amount--;
			yield return new WaitForSeconds(delta);
		}

	}
	void SpawnEnemy(){
		Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
		enemies.Add(Instantiate(enemy, spawnPoint.position, Quaternion.identity));
	}
}
