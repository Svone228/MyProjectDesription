using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory {

    public GameObject Canvas;
    
    DataBase data;  // тут хранится всё
    public GameObject Hero; //герой

    Hero self;
    public int Slots;//Количество слотов
    public Item[] items; //все айтемы в инвентаре и на персонаже, создается само
    //Первые 10 ячеек соответствуют 10 предметам на персонаже, виды предметов смотреть в database
    GameObject[] empityObjects; // Объекты слотов в инвентаре , создается само

    GameObject Swap; //свапобъект - не трогать
    public GameObject itemShow; //Пустой слот - тоже не трогать ток читать

    GameObject inventory; //Отец всех слотов, место к которому они прикреплены
    GameObject character; //Часть инвентаря к которой прикреплены надетые шмотки
    GameObject BG_inventory;//Весь инвентарь

    GameObject Left_Hand;//Дети Character, не обращать внимание
    GameObject Right_Hand;
    GameObject Ring1;
    GameObject Ring2;
    GameObject Amulet;
    GameObject Emblem;
    GameObject Boots;
    GameObject Pans;
    GameObject Chest;
    GameObject Helm;
    public GameObject Info_Panel;
    //public Image Swap_obj;

    public Inventory(int slots, GameObject itemShow) 
    {
        Slots = slots;
        this.itemShow = itemShow;
        Info_Panel = GameObject.FindGameObjectWithTag("ItemDescription");
        Hero = GameObject.FindGameObjectWithTag("Hero");
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        data = DataBase.Instance(Hero);
        Initialization();
        FillEmpity();
        AddItem(data.items[1]);
        AddItem(data.items[2]);
        AddItem(data.items[3]);
        self = Hero.GetComponent<Hero>();
        self.items = new Item[10];
    }
    public void OnTik() 
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].Attack!=null)
            {
                items[i].Attack.Cooldown_remain -= Time.deltaTime;

            }
        }
    }
    void Initialization() // получает из канваса нужные предметы, не трогать
    {
        
        data = DataBase.Instance(Hero);
        inventory = Canvas.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        character = Canvas.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).gameObject;
        BG_inventory = Canvas.transform.GetChild(1).gameObject;
        Left_Hand = character.transform.GetChild(0).gameObject;
        Right_Hand = character.transform.GetChild(1).gameObject;
        Ring1 = character.transform.GetChild(2).gameObject;
        Ring2 = character.transform.GetChild(3).gameObject;
        Amulet = character.transform.GetChild(4).gameObject;
        Emblem = character.transform.GetChild(5).gameObject;
        Boots = character.transform.GetChild(6).gameObject;
        Pans = character.transform.GetChild(7).gameObject;
        Chest = character.transform.GetChild(8).gameObject;
        Helm = character.transform.GetChild(9).gameObject;
        Info_Panel = BG_inventory.transform.GetChild(2).gameObject;
    }
    GameObject[] FillCharacterEquipments()//заполняет себя нужными ссылками на нужные объекты
    {
        GameObject[] CharacterEquipments = new GameObject[10];//вот тут написаны предметы в соответствии со слотами
        CharacterEquipments[0] = Right_Hand;
        CharacterEquipments[1] = Left_Hand;
        CharacterEquipments[2] = Helm;
        CharacterEquipments[3] = Chest;
        CharacterEquipments[4] = Pans;
        CharacterEquipments[5] = Boots;
        CharacterEquipments[6] = Amulet;
        CharacterEquipments[7] = Ring1;
        CharacterEquipments[8] = Ring2;
        CharacterEquipments[9] = Emblem;
        for (int i = 0; i < CharacterEquipments.Length; i++) 
        {
            CharacterEquipments[i].GetComponent<ItemInventory>().SetHero(Hero);
            CharacterEquipments[i].GetComponent<ItemInventory>().Info_Panel = Info_Panel;
        }
        return CharacterEquipments;
    }
    public void SetSwapObject(GameObject obj) //для скрипта inventory object
    {
        Swap = obj;
    }

    public bool IsEmpity(string name) // dont use 
    {
        for (int i = 0; i < empityObjects.Length; i++) 
        {
            if (empityObjects[i].name == name) 
            {
                if (items[i] == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        Debug.Log("KAK-TO NE NESHLO");
        return false;
    }

    public bool MoveToCurrentSlot(GameObject first)// dont use
    {
        if (Swap != null)
        {
            //Debug.Log("Я свапнул два предмета местами" + first.name + " " + Swap.name);
            int i = FindSlotForName(first.name); //это то что находится в SwapObject 
            int j = FindSlotForName(Swap.name); //Это тот предмет на который мы навелись
            if (j>= empityObjects.Length - 10) 
            {
                
                return Equip(i, j);
            }
            if (i >= empityObjects.Length - 10) 
            {
                return Unequip(i,j);
            }
            if (i != -1)
            {
                if (j != -1)
                {
                    /// ТУТ ДОБАВИТЬ ПОЗЖЕ ЧТО ЕСЛИ АЙТЕМЫ СТАКАЮТСЯ ТО СТАКНУТЬ ИХ!
                    SwapItems(i, j);
                    return true;



                }
                else 
                {
                    Debug.Log("KAK YA TUT OKAZALSYA");
                    return true;
                }
            }
            else 
            {
                Debug.Log("KAK YA TUT OKAZALSYA");
                return true;
            }
            
        }
        else 
        {
            //тут будет дроп
            return false;
        }
    }
    public bool Unequip(int i, int j) //Снимает вещь из слота i в слот j
    {
        if (items[j] == null)//если слот свободный то мы снимаем шмоткку
        {
            SwapItems(i, j);
            Hero.GetComponent<Hero>().StatsUpdate();
            return true;
        }
        else //если слот не свободный то пробуем одеть то что в занятом слоте
        {
            return Equip(j, i);
        }
    }
    public bool Equip(int i, int j) //Одеваем вещь из слота i в слот j
    {
        
        //сделать эквип в отдельные слоты
        int type = items[i].Type;//тип предмета - подробнее в database или creature
        int a = 10 - (empityObjects.Length - j); //Слот для шмотки - подробнее в database или creature
        
        if (type == 10 && a == 0)//Надеваем двуручное оружие 
        {
            if (UnequipWeapons())
            {
                SwapItems(i, j);
                return true;
            }
            else 
            {
                return false;
            }
        }
        if (type == 1 && a == 1) //надеваем оружие в левую руку
        {

            if (items[j-1] != null && items[j-1].Type == 10) //если надета двуруч то снимаем её
            {
                if (UnequipWeapons())//Если можем снять оружие то
                {
                    SwapItems(i, j); // надеваем оружие в айтемы
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else 
            {
                SwapItems(i, j);
                return true;
            }
        }
        bool ring = false;
        if ((type == 7) && (a == 8)) // Если мы пытаемся одеть кольцо что типа 7 в слот 8.
        {
            ring = true; //Костыль какой-то но главное чтобы работало.
        }
        if (a == type || ring)
        {
            
            SwapItems(i, j);
            return true;
        }
        return false;
    }
    public bool Unequip(int j)
    {
        int i = FindEmpitySlot();
        if (i != -1)
        {
            SwapItems(i, j);
            return true;
        }
        else
        {
            return false;
        }
        
    }
    bool UnequipWeapons()//тут мы точно должно снять всё оружие в оно точно поместится в инвентарь
    {
        //items[empityObjects.Length - 10] == null
        int i, j;
        if (items[empityObjects.Length - 10] != null && items[empityObjects.Length - 10 + 1] != null)
        {
            if (HaveTwoSlots())
            {
                i = FindEmpitySlot();
                j = empityObjects.Length - 10;
                SwapItems(i, j);
                i = FindEmpitySlot();
                j = empityObjects.Length - 10 + 1;
                SwapItems(i, j);
                return true;
            }
            else 
            {
                return false; 
            }
        }
        else
        {
            if (items[empityObjects.Length - 10] != null)
            {
                i = FindEmpitySlot();
                if (i != -1)
                {
                    SwapItems(i, empityObjects.Length - 10);
                }
            }
            else
            {
                if (items[empityObjects.Length - 10 + 1] != null)
                {
                    i = FindEmpitySlot();
                    if (i != -1)
                    {
                        SwapItems(i, empityObjects.Length - 10 + 1);
                    }
                }
            }
            UpdateSelf();
            return true;
        }
        
    }
    bool HaveTwoSlots()
    {
        bool first = false;
        for (int i = 0; i < empityObjects.Length - 10; i++)
        {
            if (items[i] == null && first) 
            {
                return true;
            }
            if (items[i] == null)
            {
                first = true;
            }
        }
        return false;
    }
    void SwapItems(int i, int j)  
    {
            var item = items[i];//Свапнули в массиве
            items[i] = items[j];
            items[j] = item;
            UpdateInventory();
            UpdateSelf();
            Hero.GetComponent<Hero>().StatsUpdate();
            Hero.GetComponent<Hero>().UpdateWeapon();
    }

    public int FindSlotForName(string name) //if you wanna find slot
    {
        for (int i = 0; i < empityObjects.Length; i++) 
        {
            if (empityObjects[i].name == name) 
            {
                return i;
            }
        }
        return -1;
    }

    void FillEmpity() //dont use
    {
        empityObjects = new GameObject[Slots+10];
        for (int i = 0; i < empityObjects.Length-10; i++)
        {
            empityObjects[i] = GameObject.Instantiate(itemShow) as GameObject;
            empityObjects[i].transform.SetParent(inventory.transform);
            empityObjects[i].transform.localScale = new Vector3(1, 1, 1);
            empityObjects[i].name = "Slot" + i.ToString();
            empityObjects[i].GetComponent<ItemInventory>().Info_Panel = Info_Panel;
            var script =  empityObjects[i].GetComponent<ItemInventory>();
            script.SetHero(Hero);
        }
        var temp = FillCharacterEquipments();
        for (int i = 0; i < 10; i++) 
        {
            empityObjects[empityObjects.Length - 10+i] = temp[i];
        }
        items = new Item[empityObjects.Length];

    }

    public bool AddItem(Item item) // 
    {
        int i = FindEmpitySlot();
        if (i == -1)
        {
            return false;
        }
        else 
        {
            items[i] = item;
            UpdateInventory();
            return true;
        }
        
    }
    public bool RemoveItem(Item item) 
    {
        int i = 0;
        for (i = 0; i < items.Length; i++)
        {
            if(item == items[i])
            {
                break;
            }
        }
        
        if (i != items.Length) 
        {
            items[i] = null;
            UpdateInventory();
            return true;
        }
        else 
        {
            return false;
        }
    }
    void UpdateInventory() // use after charnge items array 
    {
        for (int i = 0; i < items.Length; i++) 
        {
            if (items[i] != null)
            {
                empityObjects[i].GetComponent<Image>().sprite = items[i].Inventory_Sprite; // основная тема
                empityObjects[i].GetComponent<ItemInventory>().item = items[i];
            }
            else 
            {
                empityObjects[i].GetComponent<Image>().sprite = itemShow.GetComponent<Image>().sprite;
                empityObjects[i].GetComponent<ItemInventory>().item = null;
            }
            
        }
    }
    
    void UpdateSelf() 
    {
        int j = 0;
        for (int i = items.Length - 10; i < items.Length; i++)
        {
            self.items[j] = items[i];
            j++;
        }
    }

    int FindEmpitySlot() 
    {
        for (int i = 0; i < empityObjects.Length-10; i++) 
        {
            if(items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}