using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCol : MonoBehaviour
{
	public bool colliding {
		get { return colAmount != 0;}
	}
	public int colCount {
		get { return colAmount;}
	}
	int colAmount = 0;
	void OnTriggerEnter2D(Collider2D other){
		colAmount++;
	}
	void OnTriggerExit2D(Collider2D other){
		colAmount--;
	}
}
