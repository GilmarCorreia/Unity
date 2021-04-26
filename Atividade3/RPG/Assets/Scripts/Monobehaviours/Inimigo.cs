using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : Caractere
{

    float pontosVida; //equivalente � saude do inimigo
    public int forcaDano; // poder de dano
    private AudioSource audioSource; // audio source para tocar a música do tiro
    public AudioClip audioClip; // audio do tiro

    Coroutine danoCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // pegando o audio source na hierarquia
    }

    private void OnEnable()
    {
        ResetCaractere();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (danoCoroutine == null)
            {
                danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(danoCoroutine != null)
            {
                StopCoroutine(danoCoroutine);
                danoCoroutine = null;
            }
        }
    }



    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(FlickerCaractere());
            pontosVida = pontosVida - dano;

            if(pontosVida <= float.Epsilon)
            {
                KillCaractere();
                break;
            }
            
            if(intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }

            else
            {
                break;
            }
        }
    }

    public override void ResetCaractere()
    {
        pontosVida = inicioPontosDano;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
