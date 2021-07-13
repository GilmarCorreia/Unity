using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovePlayer : MonoBehaviour
{

    public Animator anim; // Armazena o controlador da anima��o
    bool isWalkingFlag; // Armazena o estado do par�metro "IsWalking"
    bool isRunningFlag; // Armazena o estado do par�metro "IsRunning"

    PlayerControl input;
    public Camera camera;

    Vector2 Movimento = new Vector2(); // armazena os controles de dire��o
    Vector2 Orientacao = new Vector2(); // armazena os controles de orienta��o
    bool movimentoPressionado; // armazena o estado de Mover
    bool runPressionado; // armazena o estado de Correr


    // Inicializa o script para a realiza��o do player controls
    private void Awake()
    {
        input = new PlayerControl();

        input.Player.Move.performed += ctx =>
        {
            Movimento = ctx.ReadValue<Vector2>();
            movimentoPressionado = Movimento.x != 0 || Movimento.y != 0;
        };

        input.Player.Run.performed += ctx => runPressionado = ctx.ReadValueAsButton();

        input.Player.Rotate.performed += ctx =>
        {
            Orientacao = ctx.ReadValue<Vector2>();
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("IsWalking", isWalkingFlag);
        anim.SetBool("IsRunning", isRunningFlag);

    }

    // Executa a Movimenta��o do personagem no seu pr�prio eixo ao inserir a opera��o de Move
    void Mover()
    {

        anim.SetFloat("MoveX", Movimento.x);
        anim.SetFloat("MoveZ", Movimento.y);

        if (movimentoPressionado && !isWalkingFlag)
        {
            SetIsWalkingFlag(true);
        }
        if(!movimentoPressionado && isWalkingFlag)
        {
            SetIsWalkingFlag(false);
        }
        if ((movimentoPressionado && runPressionado) && !isRunningFlag)
        {
            SetIsRunningFlag(true);
        }
        if ((!movimentoPressionado || !isRunningFlag) && isRunningFlag)
        {
            SetIsRunningFlag(false);
        }
    }

    // Executa a rota��o do personagem no seu pr�prio eixo ao inserir a opera��o de rota��o
    void Rotacionar()
    {
        float HorizontalSensitivity = 30.0f;
        float VerticalSensitivity = 30.0f;

        float RotationX = HorizontalSensitivity * Orientacao.x * Time.deltaTime;
        float RotationY = VerticalSensitivity * Orientacao.y * Time.deltaTime;

        Vector3 CameraRotation = camera.transform.rotation.eulerAngles;

        CameraRotation.x -= RotationY;
        CameraRotation.y += RotationX;

        camera.transform.rotation = Quaternion.Euler(CameraRotation);
        //Vector3 atualPosition = transform.position; // Armazena a posi��o atual;
        //Vector3 novaPosition = new Vector3(Orientacao.x, 0f, Orientacao.y); // Mudan�a de posi��o
        //Vector3 positionToLookAt = atualPosition + novaPosition; // atualiza��o da posi��o
        //transform.LookAt(positionToLookAt);
    }

    // Configura a Flag IsWalking
    void SetIsWalkingFlag(bool state)
    {
        isWalkingFlag = state;
        anim.SetBool("IsWalking", state);
    }

    // Configura a Flag IsRunning
    void SetIsRunningFlag(bool state)
    {
        isRunningFlag = state;
        anim.SetBool("IsRunning", state);
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Rotacionar();
    }

    // Habilita o controlador
    private void OnEnable()
    {
        input.Player.Enable();
    }

    // Desabilita o controlador
    private void OnDisable()
    {
        input.Player.Disable();
    }
}
