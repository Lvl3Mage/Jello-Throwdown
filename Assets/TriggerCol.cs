using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCol : MonoBehaviour
{
	public bool colliding {
		get { return cols.Count != 0;}
	}
	public int colCount {
		get { return cols.Count;}
	}
	public Collider2D[] overlapCols{
		get { return cols.ToArray();}
	}
	List<Collider2D> cols = new List<Collider2D>();
	void OnTriggerEnter2D(Collider2D other){
		bool contains = false;
		for (int i = 0; i < cols.Count; i++){
			if(cols[i] == other){
				return;
			}
		}
		cols.Add(other);

	}
	void OnTriggerExit2D(Collider2D other){
		for (int i = 0; i < cols.Count; i++){
			if(cols[i] == other){
				cols.RemoveAt(i);
				return;
			}
		}
		
	}
}
