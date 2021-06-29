using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garota : MonoBehaviour
{
	public Rigidbody cr;
	public Animator anim;
	private bool correr = false;
	private bool pular = false;

	private float chanMass = 55f;
	private float gravityAceleration = 10f;

	private float moveY = 0f;	
	
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
		cr = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {	
        if(Input.GetKeyDown("0"))
			anim.Play("WAIT00");
		if (Input.GetKeyDown("1"))
			anim.Play("WAIT01");
		if (Input.GetKeyDown("2"))
			anim.Play("WAIT02");
		if(Input.GetKeyDown("3"))
			anim.Play("WAIT03");
		if (Input.GetKeyDown("4"))
			anim.Play("WAIT04");
		if (Input.GetKeyDown("space")){
			anim.Play("JUMP00");
			pular = true;
		}
		if (Input.GetKeyDown(KeyCode.L))
			anim.Play("LOSE00");
		if (Input.GetKeyDown(KeyCode.LeftAlt))
			anim.Play("SLIDE00");
		if (Input.GetMouseButtonDown(0)){
			
			int n = Random.Range(0,2);
			
			if(n==0){
				anim.Play("DAMAGED00");
			}
			else {
				anim.Play("DAMAGED01");
			}
		}
		if(Input.GetKey(KeyCode.LeftShift))
			correr = true;
		else
			correr = false;
		
		float entradaH = Input.GetAxis("Horizontal");
		float entradaV = Input.GetAxis("Vertical");
		anim.SetFloat("entradaH",entradaH);
		anim.SetFloat("entradaV",entradaV);
		anim.SetBool("correr",correr);

		// Função para fazer a chan Pular
		moveY -= gravityAceleration * Time.deltaTime; 

		// Velocidade de Movimentação andando
		float moveX = -entradaH * 400.0f * Time.deltaTime;
		float moveZ = -entradaV * 1000.0f * Time.deltaTime;
		
		if(Mathf.Abs(moveZ)<=0f)
			moveX=0f; 
		else if(correr){
			moveX *= 3f;
			moveZ *= 3f;
		}			
		else if(pular){
			moveY = 7f;
			moveX /= 5f;
			moveZ /= 5f;
			pular = false;
		}
		cr.velocity = new Vector3(moveX,moveY,moveZ);
    }
}
