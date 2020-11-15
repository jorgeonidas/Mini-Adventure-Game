using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {
	private SpriteRenderer swordPointer;
	private Vector3 currentLookTarget;
	[SerializeField] private LayerMask layerMask;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		//dibujar el rayo
		//si el rayo hizo hit al layerMask entonces asignaremos ese punto a la posicion objetivo
		if (Physics.Raycast (ray, out hit, 500,layerMask, QueryTriggerInteraction.Ignore)) {
			if(hit.point != currentLookTarget){
				currentLookTarget = hit.point;
			}
			currentLookTarget.y = currentLookTarget.y + 1;
			transform.position = currentLookTarget;
		}
	}
}
