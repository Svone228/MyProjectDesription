using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarCreature : Creature
{
    void Awake() 
    {
        Health = MaxHP;
    }
    public override void TakeDamage(Attack attack)
    {
        base.TakeDamage(attack);
        Debug.Log("Damage!");
    }
}
