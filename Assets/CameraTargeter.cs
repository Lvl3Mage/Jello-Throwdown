using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargeter : MonoBehaviour
{
	CameraController controller;
	void Start()
	{
		controller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
		controller.AddTracker(transform);
	}
}
