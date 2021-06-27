using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item;
    public Button button;
    public Text price;
    public StartTradingScript MainScript;
    private void Sell() 
    {
        var hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
        hero.money += item.Cost;
        MainScript.TradeItems.Add(item);
        if (!hero.inventory.RemoveItem(item)) 
        {
            Debug.Log("Kak??? " + item.Item_name);
        }
        MainScript.UpdateTrade();
    }
    private void Buy()
    {
        if (MainScript.hero.money > item.Cost) 
        {
            var hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
            hero.money -= item.Cost;
            hero.inventory.AddItem(item);
            MainScript.TradeItems.Remove(item);
            MainScript.UpdateTrade();
        }
    }
    public void SetBuyButton() 
    {
        button.onClick.AddListener(Buy);
        button.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Buy";
    }
    public void SetSellButton()
    {
        button.onClick.AddListener(Sell);
        button.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Sell";
    }
}
