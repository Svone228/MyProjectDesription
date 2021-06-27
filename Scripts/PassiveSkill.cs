using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill
{
    protected PassiveSkill(GameObject creature)
    {
        Father = creature;
    }
    public Sprite Sprite { get; }
    public virtual string Description { get { return "Description Error"; } }
    protected GameObject Father;
    public abstract void Unsubscribe();
    public abstract void Subscribe();
}
public class PassiveSkillHandler 
{
    Creature Father;
    public List<PassiveSkill> Skills = new List<PassiveSkill>();
    public int SkillCount
    {
        get
        {
            return Skills.Count;
        }
    }
    public PassiveSkillHandler(Creature Father) 
    {
        this.Father = Father;
    }
    public void AddSkill(PassiveSkill skill) 
    {
        Skills.Add(skill);
        skill.Subscribe();

    }
    public void RemoveSkill(PassiveSkill skill)
    {
        Skills.Remove(skill);
        skill.Unsubscribe();
    }
}
public class pBleed : PassiveSkill
{
    private readonly float damage;
    private readonly int damagetype;
    private readonly float duration;
    private readonly float tikintervals;
    public pBleed(GameObject creature, float damage, int damagetype, float duration, float tikintervals) : base(creature)
    {
        this.damage = damage;
        this.damagetype = damagetype;
        this.duration = duration;
        this.tikintervals = tikintervals;
    }
    public void AttackModifie(Attack attack)
    {
        Bleed DOT = new Bleed("DOT", damage, duration, tikintervals, damagetype, attack.Sender, attack.Resipient);
        attack.Resipient.BuffHandler.AddBuff(DOT);
    }
    public override void Subscribe()
    {
        Father.GetComponent<Creature>().OnAttack += AttackModifie;
    }

    public override void Unsubscribe()
    {
        throw new System.NotImplementedException();
    }
}