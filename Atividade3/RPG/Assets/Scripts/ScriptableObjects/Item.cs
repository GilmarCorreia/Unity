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
        CRISTAL,
        HEALTH
    }

    public TipoItem tipoItem;
}
