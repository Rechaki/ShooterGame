using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
	public float distance;

	void LateUpdate() {
		transform.localPosition = player.position - transform.forward * distance;
	}
}
