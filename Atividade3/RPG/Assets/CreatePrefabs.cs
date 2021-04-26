using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePrefabs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        createPrefabs();
    }

    void createPrefabs(){
        // Ajusta os prefabs do jogo
        PlayerPrefs.SetInt("moedas", 0);
        PlayerPrefs.SetInt("cristaisAzuis", 0);
        PlayerPrefs.SetInt("cristaisRosas", 0);
        PlayerPrefs.SetInt("cristaisPretos", 0);
        PlayerPrefs.SetInt("cristaisVerdes", 0);
        PlayerPrefs.SetInt("cristaisAmarelos", 0);
        PlayerPrefs.SetInt("cristaisLaranjas", 0);
    }

}
