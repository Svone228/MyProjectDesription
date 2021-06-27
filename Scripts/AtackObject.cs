using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack
{

    public Creature Sender;
    public Creature Resipient;
    public GameObject bullet;
    public int type; //1 - pure/2 - physical/3 - magic
    public float Damage { get; set; }//урон который наносит атака
     
    public Attack(Creature Sender, Creature Resipient, int type, float damage) 
    {
        this.Sender = Sender;
        this.Resipient = Resipient;
        this.type = type;
        this.Damage = damage;
    }   
}
public class Heal
{

    public Creature Sender;
    public Creature Resipient;
    public GameObject bullet;    
    public float heal { get; set; }//колличество восстановленого хп

    public Heal(Creature Sender, Creature Resipient, float heal)
    {
        this.Sender = Sender;
        this.Resipient = Resipient;
        this.heal = heal;
    }
}

