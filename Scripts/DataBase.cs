using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataBase
{
    public List<Item> items;
    //public GameObject hero;
    static DataBase data;
    static GameObject Hero;
    public static DataBase Instance(GameObject hero) 
    {
        if(data == null) 
        {
            Hero = hero;
            data = new DataBase();
        }
        return data;
    }
    public static DataBase Instance() 
    {
        if (Hero == null)
        {
            Hero = GameObject.FindGameObjectWithTag("Hero");
            if (data == null)
            {
                data = new DataBase();
            }
        }
        
        return data;
    }
    private DataBase() 
    {
        CreateItems();
    }
    static public List<Item> TraderCreate(int i) 
    {
        List<Item> list = new List<Item>();
        if (data == null)
            Instance();
        list.Add(data.items[1]);
        list.Add(data.items[2]);
        return list;
    }
    void CreateItems() /// Тут позже мы должны создавать все объекты 
    {
        items = new List<Item>();
        Sprite lukInventory = Resources.Load<Sprite>("Textures/OldTextures/Inventory_Sprites/Bow");
        Sprite lukIngame = Resources.Load<Sprite>("Textures/OldTextures/Ingame_Sprites/1");
        Sprite white = Resources.Load<Sprite>("Textures/OldTextures/Inventory_Sprites/white");
        Sprite sword = Resources.Load<Sprite>("Textures/OldTextures/Inventory_Sprites/One_Hand_Sword");
        Sprite projectile = Resources.Load<Sprite>("Textures/OldTextures/FireProjectiles/2");
        Item item = new Item(0, "block", 30, 1,null, white);
        this.items.Add(item);
        item = new Item(1, "Luk", 30, 10, lukIngame, lukInventory,100, projectile,null);
        this.items.Add(item);
        item = new Item(2, "Metch_left", 30, 1, sword, sword, 10, null);
        this.items.Add(item);
        
        item = new Item(3, "Metch_right", 30, 0, sword, sword, 10, new meleeAtack(Hero,2.5f,50,energycost: 10, cooldown: 1f));
        this.items.Add(item);
    }
}







public class Item
{



    /// <summary>
    /// Предметы в инвентаре:
    /// Типы:
    /// Прописывается цифрой
    /// Тут просто прописаны типы шмоток
    /// -1 = Not_eq - не экипируется, потом нужно расширить до материалов или ещё чего-то
    /// 10 = Two_hand - любое двуручное оружие - посохи, мечи, луки, может быть просто два оружия в две руки главное в них прописать атаку.
    /// 0 = Right_hand - основное одноручное оружие, в основном оно будет иметь какую-то особенность, к примеру способность или повышенные статы
    /// 1 = Left_hand - вспомогательное оружие, к примеру щит или доп оружие в руку
    /// 2 = Helm - шлем 
    /// 3 = Chest - нагрудник 
    /// 4 = Pans - штаны
    /// 5 = Boots - ботинки
    /// 6 = Amulet - амулет 
    /// 7 = Ring - кольцо 
    /// 9 = Emblem - тринька тут супер важно что тринька
    /// CharacterEquipments[0] = Right_Hand;
    /// </summary>


    /// <summary>
    /// Сколько необходимо статов на экипировку оружия
    /// </summary>
    public int Strength_require; 
    public int Agility_require;
    public int Intelligence_require;



    /// <summary>
    /// Какой бонус даёт шмотка
    /// 
    /// </summary>
    public int Strength_bonus;
    public int Agility_bonus;
    public int Intelligence_bonus;


    /// <summary>
    /// Броня
    /// Урон 
    /// Резист соответственно
    /// </summary>
    public float Armor;
    public float Damage;
    public float Resist;


    public int Type;






    public int Id;
    //Id объекта, то как мы его идентифицируем, оно всегда должно быть просто номером объекта в листе 
    public Sprite Sprite;
    /// <summary>
    /// Этот спрайт для отражение в инвентаре, так как отражение в игре будет сделать пока довольно сложно
    /// </summary>
    public string Item_name;
    /// <summary>
    /// Название предмета
    /// </summary>
    public int Cost;
    /// <summary>
    /// Его стоимость при покупке, стоимость при продаже мы будем позже вычеслять
    /// </summary>
    public int MaxStack;
    public int NowInStack;
    /// <summary>
    /// По уммолчанию max MaxStack у equip items = 1
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item_name"></param>
    /// <param name="cost"></param>
    /// <param name="Type"></param>
    /// <param name="Sprite"></param>
    /// 
    public Sprite projectile;
    public Sprite Inventory_Sprite;
    public Skill Attack;
    //Not_eq
    public Item(int id, string item_name, int cost, int MaxStack, Sprite Sprite,Sprite Inventory_Sprite) 
    {
        this.Id = id;
        this.Item_name = item_name;
        this.Cost = cost;
        this.Type = -1;
        this.Sprite = Sprite;
        this.MaxStack = MaxStack;
        this.NowInStack = 1;
        this.Inventory_Sprite = Inventory_Sprite;
    }
    //Armor
    public Item(int id, string item_name, int cost, int Type, Sprite Sprite, Sprite Inventory_Sprite, float DMG, float Armor = 0, float Resist = 0, int Strength_bonus = 0, int Agility_bonus = 0, int Intelligence_bonus = 0, int Strength_require = 0, int Agility_require = 0, int Intelligence_require = 0) 
    {
        this.Id = id;
        this.Item_name = item_name;
        this.Cost = cost;
        this.Type = Type;
        this.Sprite = Sprite;
        this.Armor = Armor;
        this.Damage = DMG;
        this.Resist = Resist;
        this.Strength_bonus = Strength_bonus;
        this.Agility_bonus = Agility_bonus;
        this.Intelligence_bonus = Intelligence_bonus;
        this.Strength_require = Strength_require;
        this.Agility_require = Agility_require;
        this.Intelligence_require = Intelligence_require;
        this.MaxStack = 1;
        this.NowInStack = 1;
        this.Inventory_Sprite = Inventory_Sprite;
    }
    //Projectile weapon
    public Item(int id, string item_name, int cost, int Type, Sprite Sprite, Sprite Inventory_Sprite, float DMG, Sprite projectile, Skill Attack,Skill AttackEffect = null, float Armor = 0, float Resist = 0, int Strength_bonus = 0, int Agility_bonus = 0, int Intelligence_bonus = 0, int Strength_require = 0, int Agility_require = 0, int Intelligence_require = 0)
    {
        this.Id = id;
        this.Item_name = item_name;
        this.Cost = cost;
        this.Type = Type;
        this.Sprite = Sprite;
        this.Armor = Armor;
        this.Damage = DMG;
        this.Resist = Resist;
        this.Strength_bonus = Strength_bonus;
        this.Agility_bonus = Agility_bonus;
        this.Intelligence_bonus = Intelligence_bonus;
        this.Strength_require = Strength_require;
        this.Agility_require = Agility_require;
        this.Intelligence_require = Intelligence_require;
        this.MaxStack = 1;
        this.NowInStack = 1;
        this.projectile = projectile;
        this.Inventory_Sprite = Inventory_Sprite;
        this.Attack = Attack;
    }
    //Two_Hand weapon
    public Item(int id, string item_name, int cost, int Type, Sprite Sprite, Sprite Inventory_Sprite, float DMG, Skill Attack, float Armor = 0, float Resist = 0, int Strength_bonus = 0, int Agility_bonus = 0, int Intelligence_bonus = 0, int Strength_require = 0, int Agility_require = 0, int Intelligence_require = 0)
    {
        this.Id = id;
        this.Item_name = item_name;
        this.Cost = cost;
        this.Type = Type;
        this.Sprite = Sprite;
        this.Armor = Armor;
        this.Damage = DMG;
        this.Resist = Resist;
        this.Strength_bonus = Strength_bonus;
        this.Agility_bonus = Agility_bonus;
        this.Intelligence_bonus = Intelligence_bonus;
        this.Strength_require = Strength_require;
        this.Agility_require = Agility_require;
        this.Intelligence_require = Intelligence_require;
        this.MaxStack = 1;
        this.NowInStack = 1;
        this.projectile = null;
        this.Inventory_Sprite = Inventory_Sprite;
        this.Attack = Attack;
    }
}