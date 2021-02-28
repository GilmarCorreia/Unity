using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageCartas : MonoBehaviour
{
    public GameObject carta; // A carta a ser descartada
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; // indicadores para cada carta escolhida em cada linha
    private GameObject carta1, carta2; // gameObjects da 1 e 2 carta selecionada
    private string linhaCarta1, linhaCarta2; // linha da carta selecionada

    bool timerPausado, timerAcionado; // indicador de pausa no Timer ou Start Timer
    float timer; // variável de tempo

    float initialTime, tempoAtual; // utilizado para medir o tempo da rodada
    int[] tempoRecorde = new int[3]{ 99,99,999};

    int numTentativas = 0; // # de tentativas na rodada
    int numAcertos = 0; // # de match de pares acertados
    AudioSource somOK; // som de acerto

    int ultimoJogo = 0; // Quantas tentativas foram feitas no último jogo
    int recorde = 100; // Menor número de tentativas de todos os jogos

    bool jogoTerminado = false;

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas();
        UpdateTentativas();
        somOK = GetComponent<AudioSource>();
        ultimoJogo = PlayerPrefs.GetInt("Jogadas", 0);
        recorde = PlayerPrefs.GetInt("Recorde", 0); // Pega o recorde do jogo

        tempoRecorde[0] = PlayerPrefs.GetInt("TempoRecordeMinutos", 0);
        tempoRecorde[1] = PlayerPrefs.GetInt("TempoRecordeSegundos", 0);
        tempoRecorde[2] = PlayerPrefs.GetInt("TempoRecordeMilis", 0); // Salva em um array o tempo recorde em Minutos, Segundos e Milissegundos

        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Jogo Anterior = " + ultimoJogo;
        GameObject.Find("recorde").GetComponent<Text>().text = "Recorde = " + recorde + " - " + tempoRecorde[0].ToString("00")+":"+tempoRecorde[1].ToString("00") + ":"+tempoRecorde[2].ToString("000"); // Mostra o recorde do jogo em pontuação e tempo

        initialTime = Time.deltaTime;
        tempoAtual = initialTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Pega o tempo atual e divide em minutos, segundos e milisegundos
        tempoAtual += Time.deltaTime;

        int segundos = (int) (tempoAtual - initialTime);
        int minutos = segundos / 60;
        int mili = ((int) ((tempoAtual - initialTime) * 1000.0f)) - (segundos*1000) ;

        GameObject.Find("tempo").GetComponent<Text>().text = minutos.ToString("00") + ":" + segundos.ToString("00") + ":" + mili.ToString("000");

        if (timerAcionado)
        {
            timer += Time.deltaTime;
            //print(timer);

            if (timer > 1.5)
            {
                timerPausado = true;
                timerAcionado = false;

                if(carta1.tag == carta2.tag)
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    somOK.Play();

                    if (numAcertos == 13)
                    {
                        PlayerPrefs.SetInt("Jogadas", numTentativas);
                        
                        if(numTentativas < recorde)
                            PlayerPrefs.SetInt("Recorde", numTentativas);


                        long tempoTotal = (minutos * 60 * 1000) + (segundos * 1000) + mili;
                        long tempoRecordeTotal = (tempoRecorde[0] * 60 * 1000) + (tempoRecorde[1] * 1000) + tempoRecorde[2];

                        if (tempoTotal < tempoRecordeTotal)
                        {
                           PlayerPrefs.SetInt("TempoRecordeMinutos", minutos);
                           PlayerPrefs.SetInt("TempoRecordeSegundo", segundos);
                           PlayerPrefs.SetInt("TempoRecordeMilis", mili);
                        }

                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }

                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    void MostraCartas()
    {
        int[] arrayEmbaralhado = CriaArrayEmbaralhado();
        int[] arrayEmbaralhado2 = CriaArrayEmbaralhado();

        for (int i = 0; i < 13; i++)
        {
            AddUmaCarta(0,i, arrayEmbaralhado[i]);
            AddUmaCarta(1, i, arrayEmbaralhado2[i]);
        }
    }

    void AddUmaCarta(int linha, int rank, int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = ((650*escalaCartaOriginal)/110.0f);
        float fatorEscalaY = ((945 * escalaCartaOriginal) / 110.0f);

        Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank - 13/2) * fatorEscalaX),
                                          centro.transform.position.y + ((linha - 1.0f/2.0f) * fatorEscalaY),
                                          centro.transform.position.z);

        GameObject c = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity));

        c.tag = "" + (valor);
        c.name = "" + linha + "_" + valor;

        string nomeDaCarta = "";
        string numeroCarta = "";

        if (valor == 0)
            numeroCarta = "ace";
        else if (valor == 10)
            numeroCarta = "jack";
        else if (valor == 11)
            numeroCarta = "queen";
        else if (valor == 12)
            numeroCarta = "king";
        else
            numeroCarta = "" + (valor + 1);

        // Incluindo linhas com diferentes naipes
        if(linha == 0)
            nomeDaCarta = numeroCarta + "_of_clubs";
        else if(linha == 1)
            nomeDaCarta = numeroCarta + "_of_diamonds";

        Sprite s1 = (Sprite) (Resources.Load<Sprite>(nomeDaCarta));

        //print("S1: " + s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().SetCartaOriginal(s1);

    }

    public int[] CriaArrayEmbaralhado()
    {
        int[] novoArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;

        for(int t=0; t < 13; t++)
        {
            temp = novoArray[t];
            int r = UnityEngine.Random.Range(t, 13);
            novoArray[t] = novoArray[r];
            novoArray[r] = temp;
        }

        return novoArray;

    }

    public void CartaSelecionada(GameObject carta)
    {
        if (!primeiraCartaSelecionada)
        {

            string linha = carta.name.Substring(0,1);
            linhaCarta1 = linha;

            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();


        }
        else if(primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta2 = linha;

            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();

            VerificaCartas();
        }
    }

    public void VerificaCartas()
    {
        DisparaTimer();
        numTentativas++;
        UpdateTentativas();
    }

    public void DisparaTimer()
    {
        timerPausado = false;
        timerAcionado = true;
    }

    void UpdateTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
    }
}
