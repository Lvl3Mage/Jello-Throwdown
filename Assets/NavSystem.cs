using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavSystem : MonoBehaviour
{
	Transform[] NavPoints;
	[SerializeField] LayerMask blockingLayers;
	const int playerLayerIndex = 6;

	Transform[] players;
	void Awake()
	{
		List<Transform> children = new List<Transform>();
		foreach(Transform child in transform){
			children.Add(child);
		}
		NavPoints = children.ToArray();
	}
	void Start(){
		UpdatePlayers(PlayerManager.instance.GetPlayers());
		PlayerManager.instance.OnPlayersChanged += UpdatePlayers;
		ComputeNavPoints();
	}
	void UpdatePlayers(Player[] activePlayers){
		players = new Transform[activePlayers.Length];
		for(int i = 0; i < players.Length; i++){
			players[i] = activePlayers[i].gameObject.transform;
		}
	}
	void LateUpdate()
	{
		Color colorA = new Color(1, 0, 0),colorB = new Color(0, 1, 0);
		ComputeNavPoints();

	}
	List<Transform> SanitizePlayers(List<Transform> sanPlayers){ // this is theoretically unnecesarry, however sometimes a null transform can appear so this stays here for now
		for(int i = 0; i < sanPlayers.Count; i++){
			if(!sanPlayers[i]){
				sanPlayers.RemoveAt(i);
			}
		}
		return sanPlayers;
	}
	List<List<Transform>> NavLayers = new List<List<Transform>>{};
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
				// Debug.LogError("Unreachable nav point cluster!");
				break;
			}
			NavLayers.Add(newNavLayer);
		}

	}
	public Vector2 NavToPlayer(Vector2 startPoint){
		Transform closestTarget = null;
		float closestDistance = Mathf.Infinity;
		for(int i = 0; i < NavLayers.Count; i++){
			List<Transform> navLayer = NavLayers[i];
			foreach(Transform navPoint in navLayer){
				if(isInView(startPoint, navPoint.position)){
					// Debug.DrawLine(startPoint, navPoint.position, Color.red);
					float distance = ((Vector2)navPoint.position - startPoint).magnitude + PointPathDistance(navPoint.position, i-1);
					
					if(closestDistance > distance){
						closestDistance = distance;
						closestTarget = navPoint;
					}
				}
			}
		}
		if(closestTarget == null){
			return Vector2.zero;
		}
		return closestTarget.position;
	}

	float PointPathDistance(Vector2 startPoint, int startPointDepth){
		float totalDistance = 0;
		Vector2 searchPoint = startPoint;
		for(int i = startPointDepth; i >= 0; i--){

			float minLayerDistance = Mathf.Infinity;
			Transform closestLayerPoint = null;


			List<Transform> currentSearchLayer = NavLayers[i];
			foreach(Transform point in currentSearchLayer){
				if(isInView(searchPoint, point.position)){
					float distance = ((Vector2)point.position - searchPoint).magnitude;
					if(distance < minLayerDistance){

						minLayerDistance = distance;
						closestLayerPoint = point;
					}
				}
			}
			if(!closestLayerPoint){
				// Debug.LogError("Cannot form path! Point inaccesible.");
				return Mathf.Infinity;
			}
			searchPoint = closestLayerPoint.position;
			totalDistance += minLayerDistance;
		}
		return totalDistance;
	}
	bool isInView(Vector2 startPoint, Vector2 targetPoint){
		RaycastHit2D hit = Physics2D.Raycast(startPoint, targetPoint- startPoint, (targetPoint - startPoint).magnitude, blockingLayers);

		return hit.collider == null;
	}
}
