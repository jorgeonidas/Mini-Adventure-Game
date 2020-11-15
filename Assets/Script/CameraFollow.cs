using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float smoothing = 5f;
	Vector3 offset;

	void Awake(){
		Assert.IsNotNull (target);
	}

	// Use this for initialization
	void Start () {
		//distancia entre player y camara
		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetCameraPosition = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCameraPosition, smoothing * Time.deltaTime);
	}
}
