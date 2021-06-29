using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numTentativas; // Amrmazena as tentativas válidas da rodada;
    private int maxNumTentativas; // # Máximo de tentativas para Forca ou  Salvação
    int score = 0;

    public GameObject letra;  // prefab da letra no Game
    public GameObject centro; // objeto de texto de indica o centro da tela
    public GameObject canvas; // objeto da camera principal

    private string palavraOculta = ""; // palavra a ser adivinhada 
    private string[] palavrasOcultas = new string[] { "carro", "elefante", "futebol" };
    
    private int tamanhoPalavraOculta;  // tamanho da palavra oculta
    char[] letrasOcultas;              // letras da palavra oculta
    bool[] letrasDescobertas;          // indicador de quais letras foram descobertas
    char[] letrasUsadas = new char[24]; // array que salva as letras que já foram usadas
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");
        canvas = GameObject.Find("Canvas");
        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;

        float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;

        float lettersWidth = 21;
        float spaceBetweenLetters = 80;

        float maxWidth = (numLetras * lettersWidth) + ((numLetras-1) * spaceBetweenLetters);
        float lateralSpaces = (canvasWidth - maxWidth) / 2.0f;
        Debug.Log(maxWidth);

        float x_pos = 0;

        for (int i=0; i < numLetras; i++)
        {
            Vector3 novaPosicao;

            if (i == 0)
                x_pos = lateralSpaces;//- (canvasWidth / 2.0f);
            else
                x_pos += lettersWidth + spaceBetweenLetters;

            novaPosicao = new Vector3(x_pos, centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1); //nomeia na hierarquia a GameObject com letra (iésima +1), i = 1 .. numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform); // posiciona-se como filho do GameObject Canvas
        }
    }

    void InitGame()
    {
        // palavraOculta = "pneumoultramicroscopicossilicovulcanoconiótico"; // definição da palavra a ser descoberta (Usado no Lab1 - Parte A)

        //int numeroAleatorio = Random.Range(0, palavrasOcultas.Length); // Sorteamos um # dentro do # de palavras do array
        //palavraOculta = palavrasOcultas[numeroAleatorio]; // selecionamos uma palavra sorteada

        palavraOculta = PegaUmaPalavraDoArquivo();
        tamanhoPalavraOculta = palavraOculta.Length; // determina-se o número de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper(); // transforma-se a palavra em maiúscula
        letrasOcultas = new char[tamanhoPalavraOculta]; // instanciamos o array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalavraOculta]; // instanciamos o array bool do indicador de letras certas
        letrasOcultas = palavraOculta.ToCharArray(); // copia-se a palavra no array de palavras
    }

    void checkTeclado()
    {
        if (Input.anyKeyDown)
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);
            
            if(letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)
            {

                letrasUsadas[count] = System.Char.ToUpper(letraTeclada); //salva a letra teclada num array
                GameObject.Find("letrasUsadas").GetComponent<Text>().text = GameObject.Find("letrasUsadas").GetComponent<Text>().text + letrasUsadas[count] + ", "; // mostra para o usuário as letras tecladas
                count++;

                numTentativas++;
                UpdateNumTentativas();

                if (numTentativas > maxNumTentativas)
                    SceneManager.LoadScene("Lab1_forca");

                for (int i=0; i<=tamanhoPalavraOculta;i++)
                {
                    if (!letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        
                        if(letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            VerificaSePalavraDescoberta(); 
                        }
                    }
                }
            }
        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score " + score;
    }

    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;
        for (int i = 0; i < tamanhoPalavraOculta; i++)
            condicao = condicao && letrasDescobertas[i];

        if (condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("Lab1_salvo");
        }
    }

    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length + 1);
        return (palavras[palavraAleatoria]);
    }
}
