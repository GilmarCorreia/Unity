using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Inventario : MonoBehaviour
{
    public GameObject slotPrefab; // objeto que recebe o prefab slot
    public const int numSlots = 5; //numero fixo de slots
    public AudioSource audioSource; // audio source para tocar a música do coletável

    Image[] itemImagens = new Image[numSlots]; // array de imagens

    [HideInInspector]
    public Item[] itens = new Item[numSlots]; // array de itens
    GameObject[] slots = new GameObject[numSlots]; // array de Slots

    void Start()
    {
        CriaSlots();
        audioSource = gameObject.AddComponent<AudioSource>();  

        UpdateSlots();
        
    }
    public void CriaSlots()
    {
        if(slotPrefab != null)
        {
            for(int i = 0; i < numSlots; i++)
            {
                GameObject novoSlot = Instantiate(slotPrefab);
                novoSlot.name = "ItemSlot_" + i;
                novoSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = novoSlot;
                itemImagens[i] = novoSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    ///<summary>
    /// adiciona a quantidade de itens após cada nível
    /// </summary>
    public void UpdateSlots(){

        //print(PlayerPrefs.GetInt("moedas"));
        if(PlayerPrefs.GetInt("moedas") != 0){
            Item item = (Item) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Itens/Moeda.asset", typeof(Item));
            //print(item.tipoItem);
            AddItemInChangeLevels(item, PlayerPrefs.GetInt("moedas"),0);
        }
        
        if(PlayerPrefs.GetInt("cristaisAzuis") != 0){
            Item item = (Item) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Itens/CristalAzul.asset", typeof(Item));
            //print(item.tipoItem);
            AddItemInChangeLevels(item, PlayerPrefs.GetInt("cristaisAzuis"),1);
        }

        if(PlayerPrefs.GetInt("cristaisRosas") != 0){
            Item item = (Item) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Itens/CristalRosa.asset", typeof(Item));
            //print(item.tipoItem);
            AddItemInChangeLevels(item, PlayerPrefs.GetInt("cristaisRosas"),2);
        }

        if(PlayerPrefs.GetInt("cristaisLaranjas") != 0){
            Item item = (Item) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Itens/CristalLaranja.asset", typeof(Item));
            //print(item.tipoItem);
            AddItemInChangeLevels(item, PlayerPrefs.GetInt("cristaisLaranjas"),3);
        }

        if(PlayerPrefs.GetInt("cristaisPretos") != 0){
            Item item = (Item) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Itens/CristalPreto.asset", typeof(Item));
            //print(item.tipoItem);
            AddItemInChangeLevels(item, PlayerPrefs.GetInt("cristaisPretos"),4);
        }
    }

    ///<summary>
    /// adiciona a quantidade de itens após cada nível
    /// </summary>

    void AddItemInChangeLevels(Item itemToAdd, int qtd, int i){

        if(itens[i] == null)
        {
            itens[i] = Instantiate(itemToAdd);
            itens[i].quantidade = qtd;
            itemImagens[i].sprite = itemToAdd.sprite;
            itemImagens[i].enabled = true;
        }

        Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
        Text quantidadeTexto = slotScript.qtdTexto;
        quantidadeTexto.enabled = true;
        quantidadeTexto.text = itens[i].quantidade.ToString("000");
    }

    public bool AddItem(Item itemToAdd)
    {
        for(int i=0; i<itens.Length; i++)
        {

            if(itens[i]!=null && itens[i].tipoItem == itemToAdd.tipoItem && itemToAdd.empilhavel == true)
            {
                itens[i].quantidade = itens[i].quantidade + 1;
                
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = itens[i].quantidade.ToString("000");
                // se o som do item não for nulo, execute
                if(itens[i].audioClip != null)
                    audioSource.PlayOneShot(itens[i].audioClip);

                return true;
            }
            else if(itens[i] == null)
            {
                itens[i] = Instantiate(itemToAdd);
                itens[i].quantidade = 1;
                itemImagens[i].sprite = itemToAdd.sprite;
                itemImagens[i].enabled = true;

                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = itens[i].quantidade.ToString("000");

                // se o som do item não for nulo, execute
                if(itens[i].audioClip != null)
                    audioSource.PlayOneShot(itens[i].audioClip);
                    
                return true;
            }

            
        }

        return false;
    }
}
