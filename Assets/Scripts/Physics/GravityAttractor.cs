using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour {

	public float gravidade = -10f;

	public  void Attract(Transform corpo)
	{
		Vector3 targetDir = (corpo.position - transform.position).normalized;
		Vector3 bodyUp = corpo.up;

		corpo.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * corpo.rotation;
		corpo.GetComponent<Rigidbody>().AddForce(targetDir * gravidade);
	}
}
