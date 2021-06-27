using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Creature
{
    public GameObject InventorySlot;
    public SkillHandler SkillHandler;
    public Text TextStat1;
    public Text TextStat2;
    public Text Hpnumber;
    public Image maxHp;
    public Image currentHp;
    public Image currentEnergy;
    public Image maxEnergy;
    public Image currentMana;
    public Image maxMana;
    public GameObject MainCamera;
    public GameObject SkillBook;
    public GameObject SkillObject;
    public GameObject BuffPanel;
    public GameObject BuffObj;
    /// <summary>
    /// 0 - правая рука
    /// 1 - левая рука
    /// 
    /// 2 - голова 
    /// 3 - туловище 
    /// 4 - ноги 
    /// 5 - сапоги 
    /// 6 - амулет 
    /// 7,8 - кольца
    /// 9 - триня, эмблема
    /// 
    /// </summary>
    // Позже нужно сделать их приватными и обращаться только через свойства
    public int BaseStrength;
    public int BaseAgility;
    public int BaseIntelligence;
    public int BonusStrength;
    public int BonusAgility;
    public int BonusIntelligence;
    public Item[] items;
    float energy = 0;
    public Inventory inventory;
    public int money;

    public float Energy
    {
        get
        {
            return energy;
        }
        set
        {
            if (value < 0)
                energy = 0;
            else
                energy = value;
            if (value > MaxEnergy)
                energy = 100;
        }
    }
    public float MaxEnergy = 100;
    public float EnergyPerSecond = 10;
    float mana = 0;
    public float Mana 
    {
        get 
        {
            return energy;
        }
        set 
        {
            if (value < 0)
                mana = 0;
            else
                mana = value;
            if (value > MaxMana)
                mana = MaxMana;
        }
    }
    public float MaxMana = 1000;
    public float ManaPerSecond = 10;
    float second = 0;
    GameObject Left_Hand;
    GameObject Right_Hand;
    GameObject TwoHand;
    private void Awake()
    {
        SkillHandler = new SkillHandler(this);
        PassiveSkillHandler = new PassiveSkillHandler(this);
        BuffHandler = new BuffHandler();
        PassiveSkillHandler.AddSkill(new pBleed(gameObject, 50, 1, 5, 1));
        SkillHandler.AddSkill(new Charge(gameObject));
        SkillHandler.AddSkill(new sHeal(gameObject, 50, 3, 9));
        SkillHandler.AddSkill(new sVortex(gameObject, 50, 2, energycost: 0f, cooldown: 5));
        SkillHandler.AddSkill(new sAdrenalin(gameObject, 50, hpcost: 50, cooldown: 3));
        inventory = new Inventory(54, InventorySlot); //создаем инвентарь с 54 слотами

        Health = 250;
        Energy = 0;
        money = 50;
        MaxEnergy = 100;
        EnergyPerSecond = 5;
        immortal = false;
    }
    void Start()
    {
        TwoHand = gameObject.transform.GetChild(0).gameObject;
        Right_Hand = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        Left_Hand = gameObject.transform.GetChild(0).GetChild(1).gameObject;//получаю спрайты для оружия
        


        

        
        StatsUpdate();
        

    }

    // Update is called once per frame
    void Update()
    {
        StatsUpdate();
        BuffsUpdate();
        PerSecond();
        BuffHandler.OnUpdate();
        OnMoveHandler();
        SkillHandler.OnTik();
        inventory.OnTik();
    }

    void PerSecond()
    {
        second += Time.deltaTime;
        if (second >= 1)
        {
            EnergyUpdate();
            ManaUpdate();
            second = 0;
        }
        
    }





    public void StatsUpdate()
    {
        BonusAgility = 0;
        BonusIntelligence = 0;
        BonusStrength = 0;
        Damage = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                Damage += items[i].Damage;
                BonusAgility += items[i].Agility_bonus;
                BonusStrength += items[i].Strength_bonus;
                BonusIntelligence += items[i].Intelligence_bonus;
            }
        }
        UpdateHPBar();

    }


    void BuffsUpdate() // Занимается обработкой баффов на каждом кадре
    {
        int count = BuffPanel.transform.childCount;
        if (BuffHandler.buffs.Count != count)
        {

            for (int i = 0; i < count; i++)
            {
                Destroy(BuffPanel.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < BuffHandler.buffs.Count; i++)
            {
                GameObject temp = Instantiate(BuffObj);
                temp.transform.SetParent(BuffPanel.transform);
                temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
        for (int i = 0; i < BuffHandler.buffs.Count; i++)
        {
            var temp = BuffPanel.transform.GetChild(i).gameObject;
            temp.transform.GetChild(1).gameObject.GetComponent<Text>().text = Mathf.RoundToInt(BuffHandler.buffs[i].Time_remain).ToString();
            temp.transform.GetChild(2).gameObject.GetComponent<Text>().text = BuffHandler.buffs[i].Stack.ToString();
        }

    }

    








    public override void TakeDamage(Attack atack)
    {
        if (atack.type == 1)
        {
            TakePureDamage(atack);
        }
        else
        {
            if (atack.type == 2)
            {
                TakePhysicalDamage(atack);
            }
            else
            {
                TakeMagicDamage(atack);
            }
        }
    }
    void EnergyUpdate()
    {
        Energy += EnergyPerSecond;
    }
    void ManaUpdate() 
    {
        Mana += ManaPerSecond;
    }



    protected override void Death()
    {
        MainCamera.GetComponent<cam_script>().enabled = false;
        gameObject.SetActive(false);
    }
    public void UpdateWeapon() //обновляем предметы, юзается при обновлении спрайта инвентаря
    {
        if (this.items[0] != null)
        {
            if (this.items[0].Type == 10)
            {
                TwoHand.GetComponent<SpriteRenderer>().sprite = this.items[0].Sprite;
                Left_Hand.GetComponent<SpriteRenderer>().sprite = null;
                Right_Hand.GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                TwoHand.GetComponent<SpriteRenderer>().sprite = null;
                Right_Hand.GetComponent<SpriteRenderer>().sprite = this.items[0].Sprite;
            }
        }
        else
        {
            Right_Hand.GetComponent<SpriteRenderer>().sprite = null;
            TwoHand.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (this.items[1] != null)
        {
            Left_Hand.GetComponent<SpriteRenderer>().sprite = this.items[1].Sprite;
        }
        else
        {
            Left_Hand.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    void UpdateHPBar() //обновляет интерфейс при изменении
    {
        TextStat1.text = " Damage: " + Damage.ToString() + "\r\n Health: " + Health.ToString() + "\r\n Speed: " + MoveSpeed.ToString();
        Hpnumber.text = Health.ToString() + "/" + MaxHP.ToString();

        var size = currentHp.GetComponent<RectTransform>().sizeDelta;
        var maxsize = maxHp.GetComponent<RectTransform>().sizeDelta;
        float percent = Health / MaxHP;
        size = new Vector2(maxsize.x * percent, size.y);
        currentHp.GetComponent<RectTransform>().sizeDelta = size;

        size = currentEnergy.GetComponent<RectTransform>().sizeDelta;
        maxsize = maxEnergy.GetComponent<RectTransform>().sizeDelta;
        percent = energy / MaxEnergy;
        size = new Vector2(maxsize.x * percent, size.y);
        currentEnergy.GetComponent<RectTransform>().sizeDelta = size;

        size = currentMana.GetComponent<RectTransform>().sizeDelta;
        maxsize = maxMana.GetComponent<RectTransform>().sizeDelta;
        percent = mana / MaxMana;
        size = new Vector2(maxsize.x * percent, size.y);
        currentMana.GetComponent<RectTransform>().sizeDelta = size;
    }
    public void SkillBookUpdate()
    {
        for (int i = 0; i < SkillBook.transform.childCount  ;i++) 
        {
            Destroy(SkillBook.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < PassiveSkillHandler.SkillCount; i++)
        {
            var temp = Instantiate(SkillObject);
            temp.transform.GetChild(2).gameObject.GetComponent<Text>().text = PassiveSkillHandler.Skills[i].Description;
            temp.transform.SetParent(SkillBook.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < SkillHandler.SkillCount; i++)
        {
            var temp = Instantiate(SkillObject);
            temp.transform.GetChild(2).gameObject.GetComponent<Text>().text = SkillHandler.Skills[i].Description;
            temp.transform.SetParent(SkillBook.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }

    }
}