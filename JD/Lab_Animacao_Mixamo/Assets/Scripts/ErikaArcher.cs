using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErikaArcher : MonoBehaviour
{
    public Animator anim;
    public GameObject BowMesh;
    public CharacterController controller;
    private Vector3 playerVelocity;

    public GameObject mainCamera;

    private bool armed = false;
    private bool run = false;
    private bool jump = false;
    private float cameraTranslationF = 0f;
    private float cameraTranslationB = 0f;


    private float playerSpeed = 2.0f;
    private float jumpHeight = 2.5f;
    private float gravityValue = -9.81f;

    float moveX, moveZ;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        ShowBow(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) { // Se apertar 1, equipa e desequipa o arco
            if (!armed)
                StartCoroutine("EquipBow");
            else
                StartCoroutine("DisarmBow");             

            armed = !armed;
            anim.SetBool("armed", armed);
        }
        if (Input.GetKey(KeyCode.Mouse1)) // Se clicar com o botão direito do mouse, executa o Aim
            Aim(true);
        else
            Aim(false);
        if (Input.GetKeyDown("space")){
            jump = true;
        }

        walkCharacter();
        runCharacter();

        float runSpeed = 1f;
        if (run)
            runSpeed = 3f;

        moveCharacter(runSpeed);
    }

    IEnumerator EquipBow()
    {
        anim.Play("equip_bow");
        yield return new WaitForSeconds(0.15f);
        ShowBow(true);
    }
    IEnumerator DisarmBow()
    {
        anim.Play("disarm_bow");
        yield return new WaitForSeconds(0.5f);
        ShowBow(false);
    }

    void runCharacter()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            run = true;
        else
            run = false;

        anim.SetBool("running", run);
    }
    void walkCharacter()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        anim.SetFloat("moveH", moveX);
        anim.SetFloat("moveV", moveZ);

        if (moveX > 0.1f || moveX < -0.1f || moveZ > 0.1f || moveZ < -0.1f)
            anim.SetBool("walking", true);
        else
            anim.SetBool("walking", false);
    }

    void moveCharacter(float runSpeed)
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * runSpeed * playerSpeed * Time.deltaTime);

        // Changes the height position of the player..
        if (jump)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            jump = false;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        

    }

    // Função que oculta e mostra o arco na cena.
    void ShowBow(bool show)
    {
        SkinnedMeshRenderer render = BowMesh.GetComponentInChildren<SkinnedMeshRenderer>();

        render.enabled = show;
    }

    void Aim(bool aiming)
    {
        anim.SetBool("aiming", aiming);
        if (aiming)
        {
            cameraTranslationB = 0f;
            cameraTranslationF += (Vector3.back * Time.deltaTime).magnitude;
            if(cameraTranslationF < 0.1f)
                mainCamera.transform.Translate(-Vector3.back * cameraTranslationF);
        }
        else
        {
            if (cameraTranslationF > 0f)
            {
                cameraTranslationB -= (Vector3.back * Time.deltaTime).magnitude;
                if (cameraTranslationB > -0.1f)
                    mainCamera.transform.Translate(-Vector3.back * cameraTranslationB);
                else
                    cameraTranslationF = 0f;
            }
            
        }
    }
}
