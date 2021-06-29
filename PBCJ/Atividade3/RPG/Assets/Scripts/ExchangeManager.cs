using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ExchangeManager : MonoBehaviour
{
    public Item[] itens;
    public int[] qtd;

    Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
        }
    }

    public void Exchange(){

        bool exchange = true;
        
        for(int i=0; i< itens.Length; i++){

            foreach(Item itemPlayer in player.inventario.itens){

                if(itemPlayer != null){
                    if (itemPlayer.tipoItem == itens[i].tipoItem){
                        if(itemPlayer.quantidade >= qtd[i]){
                            exchange = exchange && true;
                        }
                        else{
                            exchange = exchange && false;
                        }
                    }
                }

            }
        }

        if(exchange){
            for(int i=0; i< itens.Length; i++){

                foreach(Item itemPlayer in player.inventario.itens){

                    if(itemPlayer != null){
                        if (itemPlayer.tipoItem == itens[i].tipoItem){
                            if(itemPlayer.quantidade >= qtd[i]){
                                itemPlayer.quantidade -= qtd[i];

                                 switch (itemPlayer.tipoItem){
                                    case Item.TipoItem.MOEDA:
                                        PlayerPrefs.SetInt("moedas", PlayerPrefs.GetInt("moedas") - qtd[i]);
                                        break;
                                    case Item.TipoItem.CRISTAL_AZUL:
                                        PlayerPrefs.SetInt("cristaisAzuis", PlayerPrefs.GetInt("cristaisAzuis") - qtd[i]);
                                        break;
                                    case Item.TipoItem.CRISTAL_PRETO:
                                        PlayerPrefs.SetInt("cristaisPretos", PlayerPrefs.GetInt("cristaisPretos") - qtd[i]);
                                        break;
                                    case Item.TipoItem.CRISTAL_VERDE:
                                        PlayerPrefs.SetInt("cristaisVerdes", PlayerPrefs.GetInt("cristaisVerdes") - qtd[i]);
                                        break;
                                    case Item.TipoItem.CRISTAL_AMARELO:
                                        PlayerPrefs.SetInt("cristaisAmarelos", PlayerPrefs.GetInt("cristaisAmarelos") -qtd[i] );
                                        break;
                                    case Item.TipoItem.CRISTAL_LARANJA:
                                        PlayerPrefs.SetInt("cristaisLaranjas", PlayerPrefs.GetInt("cristaisLaranjas")-qtd[i]);
                                        break;
                                    case Item.TipoItem.CRISTAL_ROSA:
                                        PlayerPrefs.SetInt("cristaisRosas", PlayerPrefs.GetInt("cristaisRosas") - qtd[i]);
                                        break;

                                }
                            }
                        }
                    }

                }
            }

            Item item = (Item) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Itens/CristalPreto.asset", typeof(Item));
            bool teste = player.inventario.AddItem(item);
            bool teste2 = player.inventario.AddItem(item);

            player.inventario.UpdateSlots();
        }
        else{
            print("não dá");
        }
    }
}
