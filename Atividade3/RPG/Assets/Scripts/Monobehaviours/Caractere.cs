using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; //Vers�o anterior do valor de "dano"
    public PontosDano pontosDano; // novo tipo que tem o valor do objeto script
    // public int MaxPontosDano; //Vers�o anterior do valor m�ximo de "dano"
    public float inicioPontosDano; // valor m�nimo inicial de "sa�de" do player
    public float MaxPontosDano; // novo tipo que tem o valor m�ximo do objeto script
}
