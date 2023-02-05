using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnHUD : MonoBehaviour
{
	[SerializeField] TextAutowritter text;
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	public void StartTimer(float time){
		StartCoroutine(Timer(time));
	}
	IEnumerator Timer(float time){
		while(time >= 0){
			yield return null;
			time -= Time.deltaTime;
			text.SetString(time.ToString("F") + "s");
		}
		Destroy(gameObject);
	}
}