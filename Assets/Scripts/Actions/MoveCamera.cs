using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour 
{

	public bool podeNavegar;
	public float velocidadeTranslacao = 10f;
	public float SensibilidaeDoMouseVertical = 250f;
	public float SensibilidaeDoMouseHorizontal = 250f;
	public float alturaMaxima = 10f;
	public float distanciaMinColisao = 1f;

	public Transform planeta;

	Vector3 posicaoInicial;
	

	// Use this for initialization
	void Start () {
		podeNavegar  = false;
		posicaoInicial = transform.position;
		
	}
	
	// Update is called once per frame
	void Update ()
 	{
		//Entrada
		if (podeNavegar)
		{
			Vector3 entradaDoTeclado = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			float h = Input.GetAxis("Mouse X");
			float v = Input.GetAxis("Mouse Y");
			
			if(Input.GetKey(KeyCode.F))
			{
				transform.LookAt(planeta);
			}

			if(Input.GetKey(KeyCode.R))
			{
				transform.position = posicaoInicial;
				transform.LookAt(planeta);
			}

			RaycastHit bateu;
			Debug.DrawLine(transform.position, Vector3.forward * 5, Color.red);

			if(Physics.Raycast(transform.position, entradaDoTeclado, out bateu, 5f ))
			{	

				if ((bateu.point - transform.position).magnitude <= distanciaMinColisao)
				{
					print((bateu.point - transform.position).magnitude);
					entradaDoTeclado *= -1;
				}			
			}
			Vector3 posicaoDesejada = transform.position + entradaDoTeclado * velocidadeTranslacao * Time.deltaTime;
			if((posicaoDesejada - planeta.position).magnitude >= alturaMaxima)
				{				
					entradaDoTeclado *= -1;
				}
			transform.Translate(-entradaDoTeclado * velocidadeTranslacao * Time.deltaTime);
			transform.Rotate(-v * SensibilidaeDoMouseVertical * Time.deltaTime, h * SensibilidaeDoMouseHorizontal * Time.deltaTime, 0);
			
		}
	}
}
