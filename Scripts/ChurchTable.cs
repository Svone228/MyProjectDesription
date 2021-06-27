using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchTable : InteractionObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {
        var Dialogue = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Dialogue>();
        var a = Dialogue.ConfirmUse("Вы хотите восстановить здоровье?");
        a.onClick.AddListener(Heal);
    }
    public void Heal() 
    {
        
        var Hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
        Debug.Log(Hero.money);
        if (Hero.money > 10)
        {
            if (Hero.Health != Hero.MaxHP) 
            {
                Hero.money -= 10;
                Hero.FullHP();
            }
        }
        else 
        {
            //когда-то допишу
        }
        Debug.Log(Hero.money);
    }
}
