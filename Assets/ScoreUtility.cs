using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUtility : MonoBehaviour
{
	float score = 0;
	public static ScoreUtility instance;
	void Awake()
	{
		if(ScoreUtility.instance == null){
			ScoreUtility.instance = this;
		}
		else{
			Debug.LogError("Cannot initialize ScoreUtility");
			Destroy(this);
		}
	}

	public void AddScore(float val){
		score += val;
	}
	public float GetScore(){
		return score;
	}
	void Update()
	{
		
	}
}
