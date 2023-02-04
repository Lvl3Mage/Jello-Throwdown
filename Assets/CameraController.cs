using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[System.Serializable]
	public struct CameraConfig
	{
		[HideInInspector] public Transform[] trackedObjects;


		[SerializeField] public float baseZoom;
		[SerializeField] public float velocityZoom;
		[SerializeField] public float lerpSpeed;
		[SerializeField] public float zoomLerpSpeed;
	}
	[SerializeField] Camera camera;
	[SerializeField] CameraConfig cameraConfig;

	void Start()
	{
		// cameraConfig.trackedObjects = new Transform[]{};
		PlayersUpdated(PlayerManager.instance.GetPlayers());
		PlayerManager.instance.OnPlayersChanged += PlayersUpdated;
	}
	void PlayersUpdated(Player[] players){
		cameraConfig.trackedObjects = new Transform[players.Length];
		for(int i = 0; i < players.Length; i++){
			cameraConfig.trackedObjects[i] = players[i].gameObject.transform;
		}
	}
	public void SetTrackedObjects(Transform[] newTrackedObjects){
		cameraConfig.trackedObjects = newTrackedObjects;
	}

	//Cam controlling
	void FixedUpdate(){
		if(cameraConfig.trackedObjects.Length == 0){
			return;
		}


		// Calclulating Avarage point
		Vector2 targetPosition = Vector2.zero;
		for (int i = 0; i < cameraConfig.trackedObjects.Length; i++){
			targetPosition += (Vector2)cameraConfig.trackedObjects[i].position;
		}
		
		targetPosition /= cameraConfig.trackedObjects.Length;
		


		//Computing Max distance from avarage point
		float maxDistSqr = 0;
		for (int i = 0; i < cameraConfig.trackedObjects.Length; i++){
			Vector2 delta = (Vector2)cameraConfig.trackedObjects[i].position - targetPosition;
			float aspectAdjustedMagnitudeSqr = Mathf.Pow(delta.y,2) + Mathf.Pow(delta.x/camera.aspect,2);
			if(maxDistSqr < aspectAdjustedMagnitudeSqr){
				maxDistSqr = aspectAdjustedMagnitudeSqr;
			}
		}
		float objectFramingZoom = Mathf.Sqrt(maxDistSqr);

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), cameraConfig.lerpSpeed*Time.unscaledDeltaTime);

		//Computing frame velocity
		Vector2 targetDelta = targetPosition - (Vector2)transform.position;
		targetDelta.x /= camera.aspect;

		
		float targetZoom = cameraConfig.baseZoom + targetDelta.magnitude*cameraConfig.velocityZoom + objectFramingZoom;

		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, cameraConfig.zoomLerpSpeed*Time.unscaledDeltaTime);


	}
}