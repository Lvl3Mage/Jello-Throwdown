using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSetter : MonoBehaviour
{
	[SerializeField] TextAutowritter text;
	void Start()
	{
		PlayerManager.instance.OnGameOver += SetScore;
	}

	void SetScore()
	{
		text.SetString(ScoreUtility.instance.GetScore().ToString());
	}
}
