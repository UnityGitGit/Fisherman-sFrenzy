﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//this fish should be able to swim around, to be cought, and to be released
public class EndangeredFish : MonoBehaviour {

	[SerializeField]private float swimSpeed;
	[SerializeField]private float rotationSpeed;

	[SerializeField]private Transform waypointHolder;
	private Vector3[] waypoints;
	private Vector3 currentWaypoint;
	[SerializeField]private int currentWaypointIndex;
	[SerializeField]private float waypointReachedDeltaDist;

	private bool caught = false;

	private void Start () {
		//prepare our waypoints:
		List<Transform> waypointChildren = waypointHolder.GetComponentsInChildren<Transform> ().ToList();
		waypointChildren.Remove (waypointHolder);

		int waypointCount = waypointChildren.Count;
		waypoints = new Vector3[waypointCount];
		for (int i = 0; i < waypointCount; i++) {
			waypoints [i] = waypointChildren [i].position;
		}
			
		currentWaypoint = waypoints [currentWaypointIndex];
	}

	private void Update () {
		if (caught)
			return;

		Quaternion targetRot = Quaternion.LookRotation (currentWaypoint - transform.position);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

		transform.Translate (Vector3.forward * swimSpeed * Time.deltaTime);

		if (Vector3.Distance (transform.position, currentWaypoint) <= waypointReachedDeltaDist) {
			UpdateWaypoint ();
		}
	}

	private void UpdateWaypoint(){
		currentWaypointIndex++;
		currentWaypointIndex = (currentWaypointIndex == waypoints.Length) ? 0 : currentWaypointIndex;
		currentWaypoint = waypoints [currentWaypointIndex];
	}

	public void AttachToNet(){
		caught = true;
	}

	public void Escape(){
		caught = false;
	}

	public void Kill(){
		Destroy (gameObject);
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.grey;
		Gizmos.DrawLine (transform.position, currentWaypoint);
	}
}
