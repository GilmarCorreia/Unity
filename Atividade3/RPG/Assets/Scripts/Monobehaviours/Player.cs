using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere
{
    public Inventario inventarioPrefab; // refer�ncia ao objeto prefab criado do invent�rio
    
    [HideInInspector]
    public Inventario inventario;
    public HealthBar healthBarPrefab; // refer�ncia ao objeto prefab criado da HealthBar
    HealthBar healthBar;

    public PontosDano pontosDano; // tem o valor da "sa�de" do player

    private AudioSource audioSource; // audio source para tocar a música do tiro
    public AudioClip audioClip; // audio do tiro

    private void Start()
    {
        inventario = Instantiate(inventarioPrefab);
        pontosDano.valor = inicioPontosDano;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;

        audioSource = gameObject.AddComponent<AudioSource>(); // pegando o audio source na hierarquia
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            audioSource.PlayOneShot(audioClip);

            StartCoroutine(FlickerCaractere());
            pontosDano.valor = pontosDano.valor - dano;

            if(pontosDano.valor <= float.Epsilon)
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

    public override void KillCaractere()
    {
        base.KillCaractere();
        Destroy(healthBar.gameObject);
        Destroy(inventario.gameObject);
    }

    public override void ResetCaractere()
    {
        inventario = Instantiate(inventarioPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;
        pontosDano.valor = inicioPontosDano;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;
            if(DanoObjeto != null)
            {
                bool DeveDesaparecer = false;
                //print("Acertou: " + DanoObjeto.NomeObjeto);
                switch (DanoObjeto.tipoItem)
                {
                    case Item.TipoItem.MOEDA:
                        // DeveDesaparecer = true;
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        break;
                    case Item.TipoItem.CRISTAL:
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        break;
                    case Item.TipoItem.HEALTH:
                        DeveDesaparecer = AjustePontosDano(DanoObjeto, DanoObjeto.quantidade);
                        break;
                    default:
                        break;
                }

                if (DeveDesaparecer)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool AjustePontosDano(Item item, int quantidade)
    {
        if (pontosDano.valor < MaxPontosDano)
        {
            pontosDano.valor = pontosDano.valor + quantidade;
            //print("Ajustando PD por: " + quantidade + ". Novo Valor = " + pontosDano.valor);

            // se o som do item não for nulo, execute
            if(item.audioClip != null)
                inventario.audioSource.PlayOneShot(item.audioClip);

            return true;
        }
        else
            return false;
    }
}
