using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Recarrega o jogo
    public void RestartMundoGame()
    {
        SceneManager.LoadScene("Lab3");
    }

    // Fecha o jogo
    public void QuitGame()
    {
        // Fecha o Jogo ao ser clicado -> Se estiver usando o unity ele irá calcelar a variável de jogar do Unity 
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
  
}
