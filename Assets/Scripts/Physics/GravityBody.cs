using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class GravityBody : MonoBehaviour
{
	GravityAttractor planeta;
	Rigidbody corpoRB;

	void Awake()
	{
		planeta = GameObject.FindGameObjectWithTag("Planeta").GetComponent<GravityAttractor>();
		corpoRB = GetComponent<Rigidbody>();
		corpoRB.useGravity = false;
		corpoRB.constraints = RigidbodyConstraints.FreezeRotation;
	}

	void FixedUpdate()
	{
		planeta.Attract(transform);
	}

}
