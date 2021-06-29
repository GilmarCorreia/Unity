using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instanciaCompartilhada = null;
    public RPGCameraManager cameraManager;

    public PontoSpawn playerPontoSpawn;

    public string nextScene;

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

            Item item = colectable.GetComponent<Consumable>().item;
            if (item.tipoItem != Item.TipoItem.HEALTH ){
                countConsumables++;
            }
        }

        PlayerPrefs.SetInt("consumables", PlayerPrefs.GetInt("consumables") + countConsumables);
        print(PlayerPrefs.GetInt("consumables"));
        print("consumíveis: " + countConsumables);

    }

    // Update is called once per frame
    void Update()
    {
        int itensColetados = 0;
        foreach(Item item in player.GetComponent<Player>().inventario.itens){
            if (item != null)
                itensColetados += item.quantidade;
        }

        //print(itensColetados);
        if(itensColetados == PlayerPrefs.GetInt("consumables")){
            
            foreach(Item item in player.GetComponent<Player>().inventario.itens){

                if (item != null){
                    switch (item.tipoItem){
                        case Item.TipoItem.MOEDA:
                            PlayerPrefs.SetInt("moedas", PlayerPrefs.GetInt("moedas") + item.quantidade);
                            break;
                        case Item.TipoItem.CRISTAL_AZUL:
                            PlayerPrefs.SetInt("cristaisAzuis", PlayerPrefs.GetInt("cristaisAzuis") + item.quantidade);
                            break;
                        case Item.TipoItem.CRISTAL_PRETO:
                            PlayerPrefs.SetInt("cristaisPretos", PlayerPrefs.GetInt("cristaisPretos") + item.quantidade);
                            break;
                        case Item.TipoItem.CRISTAL_VERDE:
                            PlayerPrefs.SetInt("cristaisVerdes", PlayerPrefs.GetInt("cristaisVerdes") + item.quantidade);
                            break;
                        case Item.TipoItem.CRISTAL_AMARELO:
                            PlayerPrefs.SetInt("cristaisAmarelos", PlayerPrefs.GetInt("cristaisAmarelos") + item.quantidade );
                            break;
                        case Item.TipoItem.CRISTAL_LARANJA:
                            PlayerPrefs.SetInt("cristaisLaranjas", PlayerPrefs.GetInt("cristaisLaranjas")+item.quantidade);
                            break;
                        case Item.TipoItem.CRISTAL_ROSA:
                            PlayerPrefs.SetInt("cristaisRosas", PlayerPrefs.GetInt("cristaisRosas") + item.quantidade);
                            break;

                    }
                }
            }
            SceneManager.LoadScene(nextScene);

        }
    }

}
