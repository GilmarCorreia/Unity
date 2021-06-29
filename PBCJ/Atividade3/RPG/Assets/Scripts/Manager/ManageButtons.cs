using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Close());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Lab1_Start");
    }
    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab5_RPGSetup");
    }

    public void LoadFinishScene()
    {
        // muda para a cena de créditos ao ser clicado
        SceneManager.LoadScene("credits");
    }

    public void QuitGame()
    {
        // Fecha o Jogo ao ser clicado -> Se estiver usando o unity ele irá calcelar a variável de jogar do Unity 
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
  
}
