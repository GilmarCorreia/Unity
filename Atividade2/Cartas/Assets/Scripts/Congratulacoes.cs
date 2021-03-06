using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Congratulacoes : MonoBehaviour
{

    float tempoAtual;
    // Start is called before the first frame update
    void Start()
    {
        int recorde = PlayerPrefs.GetInt("Recorde", 0); // Pega o recorde do jogo

        int[] tempoRecorde = new int[3];
        tempoRecorde[0] = PlayerPrefs.GetInt("TempoRecordeMinutos", 0);
        tempoRecorde[1] = PlayerPrefs.GetInt("TempoRecordeSegundos", 0);
        tempoRecorde[2] = PlayerPrefs.GetInt("TempoRecordeMilis", 0);

        GameObject.Find("novoRecorde").GetComponent<Text>().text = "Novo Recorde = " + recorde + " - " + tempoRecorde[0].ToString("00") + ":" + tempoRecorde[1].ToString("00") + ":" + tempoRecorde[2].ToString("000"); // Mostra o recorde do jogo em pontuação e tempo

        tempoAtual = Time.deltaTime; // salva o tempo atual
    }

    // Update is called once per frame
    void Update()
    {
        tempoAtual += Time.deltaTime;
        if (tempoAtual > 20)
        {
            SceneManager.LoadScene("credits");
        }
    }
}
