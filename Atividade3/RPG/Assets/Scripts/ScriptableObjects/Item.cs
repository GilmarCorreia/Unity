using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    public string NomeObjeto;
    public Sprite sprite;
    public int quantidade;
    public bool empilhavel;
    public AudioClip audioClip; // audio do coletável

    public enum TipoItem
    {
        MOEDA,
        CRISTAL_AZUL,
        CRISTAL_ROSA,
        CRISTAL_LARANJA,
        CRISTAL_PRETO,
        CRISTAL_AMARELO,
        CRISTAL_VERDE,
        HEALTH
    }

    public TipoItem tipoItem;
}
