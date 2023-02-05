using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
	[SerializeField] TextAutowritter text;
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	public void AnimationOver(){
		Destroy(this);
	}
	public void SetScore(float score){
		text.SetString(score.ToString());
	}
}
