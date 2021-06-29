using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]

public class Perambular : MonoBehaviour
{

    public float velocidadePerseguicao; // Velocidade do "Inimigo" na �rea de Spot
    public float velocidadePerambular;  // velocidade do "Inimigo" passeando
    float velocidadeCorrente;           // velocidade do "Inimigo" atribu�da

    public float intervaloMudancaDirecao; // Tempo para alterar direcao
    public bool perseguePlayer;           // Indicador de perseguidor ou n�o

    Coroutine moverCoroutine;

    Rigidbody2D rb2D;                     // armazena o componente rigidbody2D
    Animator animator;                    // armazena o componente Animator

    Transform alvoTransform = null;       // armazena o componente Transform do Alvo

    Vector3 posicaoFinal;
    float anguloAtual = 0;                // Angulo atribuido

    CircleCollider2D circleCollider;   // armazena o coponente de Spot

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocidadeCorrente = velocidadePerambular;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(RotinaPerambular());
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    public IEnumerator RotinaPerambular()
    {
        while (true)
        {
            EscolheNovoPontoFinal();
            if(moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }

            moverCoroutine = StartCoroutine(Mover(rb2D,velocidadeCorrente));
            yield return new WaitForSeconds(intervaloMudancaDirecao);
        }

    }

    void EscolheNovoPontoFinal()
    {
        anguloAtual += Random.Range(0, 360);
        anguloAtual = Mathf.Repeat(anguloAtual, 360);

        Vector2 newPos = (rb2D.position + Vector3ParaAngulo(anguloAtual));
        posicaoFinal =  new Vector3(newPos.x,newPos.y,0);
    }

    Vector2 Vector3ParaAngulo(float anguloEntradaGraus)
    {
        float anguloEntradaRadianos = anguloEntradaGraus * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(anguloEntradaRadianos), Mathf.Sin(anguloEntradaRadianos));
    }

    public IEnumerator Mover(Rigidbody2D rBParaMover, float velocidade)
    {
        float distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
        while(distanciaFaltante > float.Epsilon)
        {
            if(alvoTransform != null)
            {
                posicaoFinal = alvoTransform.position;
            }

            if(rBParaMover != null)
            {
                animator.SetBool("Caminhando", true);
                Vector3 novaPosicao = Vector3.MoveTowards(rBParaMover.position, posicaoFinal, velocidade* Time.deltaTime);
                rb2D.MovePosition(novaPosicao);
                distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("Caminhando", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && perseguePlayer)
        {
            velocidadeCorrente = velocidadePerseguicao;
            alvoTransform = collision.gameObject.transform;

            if(moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }

            moverCoroutine = StartCoroutine(Mover(rb2D, velocidadeCorrente));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Caminhando", false);
            velocidadeCorrente = velocidadePerambular;
            if(moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }

            alvoTransform = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2D.position, posicaoFinal, Color.red);   
    }
}
