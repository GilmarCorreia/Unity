using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageCartas : MonoBehaviour
{
    public GameObject carta; // A carta a ser descartada
    private bool primeiraCartaSelecionada, segundaCartaSelecionada, terceiraCartaSelecionada, quartaCartaSelecionada; // indicadores para cada carta escolhida em cada linha
    private GameObject carta1, carta2, carta3, carta4; // gameObjects das cartas selecionadas
    private string linhaCarta1, linhaCarta2, linhaCarta3, linhaCarta4; // linha da carta selecionada

    bool timerPausado, timerAcionado; // indicador de pausa no Timer ou Start Timer
    float timer; // variável de tempo

    float initialTime, tempoAtual; // utilizado para medir o tempo da rodada
    int[] tempoRecorde = new int[3]{ 99,99,999};

    int numTentativas = 0; // # de tentativas na rodada
    int numAcertos = 0; // # de match de pares acertados
    AudioSource somOK; // som de acerto

    int ultimoJogo = 0; // Quantas tentativas foram feitas no último jogo
    int recorde = 100; // Menor número de tentativas de todos os jogos

    int[] randomNaipes; // Vetor de naipes randomicos
    int[] linhaSelecionada = { -1, -1, -1, -1}; //Guarda no vetor a linha atual selecionada
    int countLinha = 0;

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

        GameObject.Find("tempo").GetComponent<Text>().text = minutos.ToString("00") + ":" + segundos.ToString("00") + ":" + mili.ToString("000"); // Converte o número para uma string personalizada

        if (timerAcionado)
        {
            timer += Time.deltaTime;
            //print(timer);

            if (timer > 1.5)
            {
                timerPausado = true;
                timerAcionado = false;

                bool cartasIguais = (carta1.tag == carta2.tag) && (carta1.tag == carta3.tag) && (carta1.tag == carta4.tag); // Verifica se todas as 4 cartas selecionadas são iguais

                if(cartasIguais)
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    Destroy(carta3);
                    Destroy(carta4);
                    numAcertos++;
                    somOK.Play();

                    if (numAcertos == 13)
                    {
                        PlayerPrefs.SetInt("Jogadas", numTentativas);

                        // Verifica se o número de tentativas é menor que o número recorde
                        if (numTentativas < recorde)
                        {
                            PlayerPrefs.SetInt("Recorde", numTentativas); // salva o novo valor caso seja inferior ao recorde

                            VerificaTempoRecorde(minutos, segundos, mili, numTentativas);

                            SceneManager.LoadScene("congratulations"); // carrega a tela de congratulações
                        }

                        VerificaTempoRecorde(minutos, segundos, mili, numTentativas);

                        SceneManager.LoadScene("credits");
                    }

                    for(int i = 0; i < 4; i++)
                        linhaSelecionada[i] = -1;

                    countLinha = 0;
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                    carta3.GetComponent<Tile>().EscondeCarta();
                    carta4.GetComponent<Tile>().EscondeCarta();

                    for (int i = 0; i < 4; i++)
                        linhaSelecionada[i] = -1;

                    countLinha = 0;
                }

                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                terceiraCartaSelecionada = false;
                quartaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                carta3 = null;
                carta4 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                linhaCarta3 = "";
                linhaCarta4 = "";
                timer = 0;
            }
        }
    }


    // Método que salva o tempo recorde e o numTentativas recorde
    void VerificaTempoRecorde(int minutos,int segundos,int mili, int numTentativas)
    {

        PlayerPrefs.SetInt("Recorde", numTentativas);

        long tempoTotal = (minutos * 60 * 1000) + (segundos * 1000) + mili; // Caso tenha acabado o jogo computa o tempoTotal da jogada
        long tempoRecordeTotal = (tempoRecorde[0] * 60 * 1000) + (tempoRecorde[1] * 1000) + tempoRecorde[2]; // Pega o tempo recorde e converte em milissegundos

        // Verifica se o tempo da jogada é menor que o tempo recorde, se sim, o novo tempo é salvo
        if (tempoTotal < tempoRecordeTotal)
        {
            PlayerPrefs.SetInt("TempoRecordeMinutos", minutos); // salva o tempo recorde em minutos
            PlayerPrefs.SetInt("TempoRecordeSegundo", segundos); // salva o tempo recorde em segundos
            PlayerPrefs.SetInt("TempoRecordeMilis", mili); // salva o tempo recorde em milissegundos

            SceneManager.LoadScene("congratulations"); // carrega a tela de congratulações
        }
    }

    void MostraCartas()
    {
        randomNaipes = CriaNaipesRandomicos();

        int[] arrayEmbaralhado = CriaArrayEmbaralhado();
        int[] arrayEmbaralhado2 = CriaArrayEmbaralhado();
        int[] arrayEmbaralhado3 = CriaArrayEmbaralhado(); // Cria o array embaralhado para a terceira linha
        int[] arrayEmbaralhado4 = CriaArrayEmbaralhado(); // Cria o array embaralhado para a quarta linha

        for (int i = 0; i < 13; i++)
        {
            AddUmaCarta(0, i, arrayEmbaralhado[i]);
            AddUmaCarta(1, i, arrayEmbaralhado2[i]);
            AddUmaCarta(2, i, arrayEmbaralhado3[i]); // adiciona cartas na linha 3
            AddUmaCarta(3, i, arrayEmbaralhado4[i]); // adiciona cartas na linha 4
        }
    }

    void AddUmaCarta(int linha, int rank, int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = ((650*escalaCartaOriginal)/110.0f);
        float fatorEscalaY = ((945 * escalaCartaOriginal) / 110.0f);

        Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank - 13/2) * fatorEscalaX),
                                          centro.transform.position.y + ((linha - 2) * fatorEscalaY),
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
        for (int i = 0; i < 4; i++)
        {
            int naipe = randomNaipes[linha];
            if (naipe == 0)
                nomeDaCarta = numeroCarta + "_of_clubs";
            else if (naipe == 1)
                nomeDaCarta = numeroCarta + "_of_diamonds";
            else if (naipe == 2)
                nomeDaCarta = numeroCarta + "_of_spades";
            else if (naipe == 3)
                nomeDaCarta = numeroCarta + "_of_hearts";
        }

        

        Sprite s1 = (Sprite) (Resources.Load<Sprite>(nomeDaCarta));

        //print("S1: " + s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().SetCartaOriginal(s1);

    }

    // Função que cria randomicamente qual naípe será escolhido para cada linha
    public int[] CriaNaipesRandomicos()
    {
        int escolha = UnityEngine.Random.Range(0, 8);

        // listando todas as possibilidades
        if(escolha == 0)
            return new int[] { 0, 1, 2, 3 };
        else if (escolha == 1)
            return new int[] { 0, 3, 2, 1 };
        else if (escolha == 2)
            return new int[] { 2, 3, 0, 1 };
        else if (escolha == 2)
            return new int[] { 2, 1, 0, 3 };
        else if (escolha == 4)
            return new int[] { 3, 0, 1, 2};
        else if (escolha == 5)
            return new int[] { 1, 0, 3, 2 };
        else if (escolha == 6)
            return new int[] { 1, 2, 3, 0 };
        else
            return new int[] { 3, 2, 1, 0 };

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

            string linha = carta.name.Substring(0, 1);

            // Método para verificar se é possível adicionar a carta a linha selecionada
            int linhaClicada = int.Parse(linha);
            bool podeAdicionar = true;

            for (int i = 0; i < 4; i++)
            {
                if (linhaClicada == linhaSelecionada[i])
                    podeAdicionar = false;
            }

            // Se a linha da carta não tiver sido clicado, revela a carta
            if (podeAdicionar)
            {
                linhaSelecionada[countLinha] = linhaClicada;
                countLinha++;

                linhaCarta1 = linha;

                primeiraCartaSelecionada = true;
                carta1 = carta;
                carta1.GetComponent<Tile>().RevelaCarta();
            }



        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            // Método para verificar se é possível adicionar a carta a linha selecionada
            int linhaClicada = int.Parse(linha);
            bool podeAdicionar = true;

            for (int i = 0; i < 4; i++)
            {
                if (linhaClicada == linhaSelecionada[i])
                    podeAdicionar = false;
            }

            // Se a linha da carta não tiver sido clicado, revela a carta
            if (podeAdicionar)
            {
                linhaSelecionada[countLinha] = linhaClicada;
                countLinha++;
                linhaCarta2 = linha;

                segundaCartaSelecionada = true;
                carta2 = carta;
                carta2.GetComponent<Tile>().RevelaCarta();
            }

        }
        else if (primeiraCartaSelecionada && segundaCartaSelecionada && !terceiraCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);

            // Método para verificar se é possível adicionar a carta a linha selecionada
            int linhaClicada = int.Parse(linha);
            bool podeAdicionar = true;

            for (int i = 0; i < 4; i++)
            {
                if (linhaClicada == linhaSelecionada[i])
                    podeAdicionar = false;
            }

            // Se a linha da carta não tiver sido clicado, revela a carta
            if (podeAdicionar)
            {
                linhaSelecionada[countLinha] = linhaClicada;
                countLinha++;

                linhaCarta3 = linha;

                terceiraCartaSelecionada = true;
                carta3 = carta;
                carta3.GetComponent<Tile>().RevelaCarta();
            }

        }

        else if (primeiraCartaSelecionada && segundaCartaSelecionada && terceiraCartaSelecionada && !quartaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            // Método para verificar se é possível adicionar a carta a linha selecionada
            int linhaClicada = int.Parse(linha);
            bool podeAdicionar = true;

            for (int i = 0; i < 4; i++)
            {
                if (linhaClicada == linhaSelecionada[i])
                    podeAdicionar = false;
            }

            // Se a linha da carta não tiver sido clicado, revela a carta
            if (podeAdicionar)
            {
                linhaSelecionada[countLinha] = linhaClicada;
                countLinha++;

                linhaCarta4 = linha;

                quartaCartaSelecionada = true;
                carta4 = carta;
                carta4.GetComponent<Tile>().RevelaCarta();

                VerificaCartas();
            }
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
