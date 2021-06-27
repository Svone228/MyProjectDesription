using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartTradingScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Item[] PlayerItems;
    public List<Item> TradeItems;

    public GameObject TradeWindow;
    public GameObject ItemObject;
    public int TraderNumber;
    [Header("Auto Atributes")]
    public GameObject Sell;
    public GameObject Buy;
    Text money;
    public Hero hero;
    public Button CloseButton;
    GameObject window;
   /* void Start()
    {
        TradeItems = new List<Item>();
        var dataBase = DataBase.Instance();
        TradeItems.Add(dataBase.items[2]);
        TradeItems.Add(dataBase.items[1]);
        StartTrade();
    }*/

    // Update is called once per frame
    public void StartTrade() 
    {
        if (TradeItems == null)
            TradeItems = DataBase.TraderCreate(1);
        InstantiateTradeWindow();
        PlayerItems = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>().inventory.items;
        CreateSellItems();
        CreateBuyItems();
    }
    void CreateSellItems() 
    {
        if (PlayerItems != null)
        {
            for (int i = 0; i < PlayerItems.Length; i++)
            {
                if (PlayerItems[i] != null)
                    InstantiateItem(PlayerItems[i], Sell);
            }
        }
    }
    void CreateBuyItems() 
    {
        if (TradeItems != null) 
        {
            for (int i = 0; i < TradeItems.Count; i++)
            {
                if (TradeItems[i] != null)
                    InstantiateItem(TradeItems[i], Buy);
            }
        }
        
    }
    void InstantiateTradeWindow() 
    {
        window = Instantiate(TradeWindow);
        //Debug.Log()
        window.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        window.transform.localPosition = new Vector3(0,540,0);
        window.transform.localScale = new Vector3(1, 1, 1);
        Buy  = window.transform.GetChild(1).transform.GetChild(0).gameObject;
        Sell = window.transform.GetChild(2).transform.GetChild(0).gameObject;
        money = window.transform.GetChild(3).GetComponent<Text>();
        hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
        SetMoney();
        CloseButton = window.transform.GetChild(4).gameObject.GetComponent<Button>();
        Debug.Log(CloseButton);
        CloseButton.onClick.AddListener(Close);
        Debug.Log(CloseButton.onClick);
    }
    public void Close() 
    {
        Debug.Log(window);
        Destroy(window);
    }
    void SetMoney() 
    {
        Debug.Log(money);
        money.text = "Money: " + hero.money.ToString();
    }
    void InstantiateItem(Item item, GameObject Father) 
    {
        var TempItem = Instantiate(ItemObject);
        TempItem.transform.SetParent(Father.transform);
        TempItem.transform.localScale = new Vector3(1, 1, 1);
        TempItem.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.Sprite;
        TempItem.transform.GetChild(1).GetComponent<Text>().text = item.Item_name;
        TempItem.GetComponent<TradeObject>().item = item;
        TempItem.GetComponent<TradeObject>().MainScript = this;
        TempItem.transform.GetChild(3).GetComponent<Text>().text = "Price: " + item.Cost.ToString();
        
        if (Father == Buy)
        {
            TempItem.GetComponent<TradeObject>().SetBuyButton();
            if (item.Cost > hero.money)
            {
                TempItem.GetComponent<TradeObject>().button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            }
        }
        else 
        {
            TempItem.GetComponent<TradeObject>().SetSellButton();
        }
    }
    public void RemoveAllObjects() 
    {
        for (int i = 0; i < Buy.transform.childCount; i++)
        {
            Destroy(Buy.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < Sell.transform.childCount; i++)
        {
            Destroy(Sell.transform.GetChild(i).gameObject);
        }
    }
    public void UpdateTrade() 
    {
        RemoveAllObjects();
        PlayerItems = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>().inventory.items;
        CreateSellItems();
        CreateBuyItems();
        SetMoney();
    }
}
