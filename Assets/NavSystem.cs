using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavSystem : MonoBehaviour
{
	[SerializeField] Transform[] NavPoints;
	[SerializeField] LayerMask blockingLayers;
	const int playerLayerIndex = 6;
	// Start is called before the first frame update
	[SerializeField] Transform[] players;
	void Start()
	{
		ComputeNavPoints();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		// Color colorA = new Color(1, 0, 0),colorB = new Color(0, 1, 0);
		ComputeNavPoints();
		// for (int i = 1; i < NavLayers.Count; i++){
		// 	List<Transform> prevLayer = NavLayers[i-1];
		// 	List<Transform> curLayer = NavLayers[i];
		// 	Color layerColor = Color.Lerp(colorB, colorA, i/(float)NavLayers.Count);
		// 	foreach(Transform curPoint in curLayer){
		// 		foreach(Transform targetPoint in prevLayer){
		// 			if(isInView(curPoint.position, targetPoint.position)){
		// 				Debug.DrawLine(curPoint.position, targetPoint.position,layerColor);
		// 			}
		// 		}
		// 	}
		// }
	}
	List<List<Transform>> NavLayers;
	void ComputeNavPoints(){
		List<Transform> navPointBuffer = new List<Transform>(NavPoints);
		NavLayers = new List<List<Transform>>();

		NavLayers.Add(new List<Transform>(players));// adding the players as the closest nav points

		while (navPointBuffer.Count > 0){
			List<Transform> newNavLayer = new List<Transform>();
			List<Transform> lastNavLayer = NavLayers[NavLayers.Count-1];
			bool clusterUnreachable = true;
			for (int i = 0; i < navPointBuffer.Count; i++){
				Transform curNavPoint = navPointBuffer[i];
				bool reachable = false;
				for(int j = 0; j < lastNavLayer.Count; j++){
					if(isInView(curNavPoint.position, lastNavLayer[j].position)){
						reachable = true;
						break;
					}
				}
				if(reachable){// point reachable
					clusterUnreachable = false;
					newNavLayer.Add(curNavPoint);

					//can be removed from buffer now
					navPointBuffer.RemoveAt(i);
					i--;
				}

				// clusterUnreachable = clusterUnreachable && !reachable; // unreachable cluster endcheck
			}
			if(clusterUnreachable){
				Debug.LogError("Unreachable nav point cluster!");
				break;
			}
			NavLayers.Add(newNavLayer);
		}

	}
	public Vector2 NavToPlayer(Vector2 startPoint){
		Transform closestTarget = null;
		float closestDistance = Mathf.Infinity;
		foreach(List<Transform> navLayer in NavLayers){

			foreach(Transform navPoint in navLayer){
				if(isInView(startPoint, navPoint.position)){
					float distance = ((Vector2)navPoint.position - startPoint).magnitude;
					if(closestDistance > distance){
						closestDistance = distance;
						closestTarget = navPoint;
					}
				}
			}
			if(closestTarget != null){
				break;
			}
		}
		// if(closestTarget == null){
		// 	return null;
		// }
		return closestTarget.position;
	}
	bool isInView(Vector2 startPoint, Vector2 targetPoint){
		RaycastHit2D hit = Physics2D.Raycast(startPoint, targetPoint- startPoint, (targetPoint - startPoint).magnitude, blockingLayers);

		return hit.collider == null;
	}
}
