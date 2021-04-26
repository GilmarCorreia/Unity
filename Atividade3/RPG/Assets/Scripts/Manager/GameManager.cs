using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instanciaCompartilhada = null;
    public RPGCameraManager cameraManager;

    public PontoSpawn playerPontoSpawn;

    [HideInInspector]
    public int countConsumables = 0;

    private GameObject player;

    private void Awake()
    {
        if(instanciaCompartilhada != null && instanciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanciaCompartilhada = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
        CreatePrefabs();
    }

    void CreatePrefabs(){
        // Ajusta os prefabs do jogo
        PlayerPrefs.SetInt("moedas", 0);
        PlayerPrefs.SetInt("cristaisAzuis", 0);
        PlayerPrefs.SetInt("cristaisRosas", 0);
        PlayerPrefs.SetInt("cristaisPretos", 0);
        PlayerPrefs.SetInt("cristaisVerdes", 0);
        PlayerPrefs.SetInt("cristaisAmarelos", 0);
        PlayerPrefs.SetInt("cristaisLaranjas", 0);
    }

    public void SetupScene()
    {
        SpawnPlayer();
        CountColectables(); // Função que conta quantos itens coletáveis existem na cena e podem ser coletados pelo player
    }

    public void SpawnPlayer()
    {
        if(playerPontoSpawn != null)
        {
            player = playerPontoSpawn.SpawnO();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }

    /// <summary>
    /// Função utilizada para contar quantos elementos existem na cena que o player pode coletar. Coletando todos o player passa de nível
    /// </summary>

    public void CountColectables(){
        GameObject[] objs ;
        
        objs = GameObject.FindGameObjectsWithTag("Coletavel");
        
        foreach(GameObject colectable in objs) {

            if (colectable.GetComponent<Consumable>().item.tipoItem != Item.TipoItem.HEALTH ){
                countConsumables++;
            }
        }

        print(countConsumables);

    }

    // Update is called once per frame
    void Update()
    {
        int itensColetados = 0;
        foreach(Item item in player.GetComponent<Player>().inventario.itens){
            if (item != null)
                itensColetados += item.quantidade;
        }

        print(itensColetados);
        if(itensColetados == countConsumables){

            updatePrefabs();
            SceneManager.LoadScene("Nivel2");
        }
    }

    /// <summary>
    /// Atualiza os prefabs do jogo para o próximo nível.
    /// </summary>
    void updatePrefabs(){
        GameObject[] objs ;
        
        objs = GameObject.FindGameObjectsWithTag("Coletavel");
        
        foreach(GameObject colectable in objs) {

            Item item = colectable.GetComponent<Consumable>().item;
            switch (item.NomeObjeto){

                case "Moeda":
                    PlayerPrefs.SetInt("moedas", item.quantidade);
                    break;
                case "Cristal Azul":
                    PlayerPrefs.SetInt("cristaisAzuis", item.quantidade);
                    break;
                case "Cristal Preto":
                    PlayerPrefs.SetInt("cristaisPretos", item.quantidade);
                    break;
                case "Cristal Verde":
                    PlayerPrefs.SetInt("cristaisVerdes", item.quantidade);
                    break;
                case "Cristal Amarelo":
                    PlayerPrefs.SetInt("cristaisAmarelos", item.quantidade);
                    break;
                case "Cristal Laranja":
                    PlayerPrefs.SetInt("cristaisLaranjas", item.quantidade);
                    break;
                case "Cristal Rosa":
                    PlayerPrefs.SetInt("cristaisRosas", item.quantidade);
                    break;

            }
        }
    }
}
