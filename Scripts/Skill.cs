using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill
{
    public string Name { get; set; }
    public readonly float Cooldown = 0;
    
    public readonly float manacost = 0;
    public readonly float energycost = 0;
    public readonly float hpcost = 0;
    private float cooldown_remain = 0;
    public float Cooldown_remain
    {
        get { return cooldown_remain; }
        set { if (value < 0) value = 0; cooldown_remain = value; }
    }

    public virtual string Description { get { return "Description Error"; } }
    private Image icon;
    public Sprite Sprite { get; }
    protected GameObject Father;
    protected float animation_time = 0;
    protected float animation_time_spended = 0;
    public abstract IEnumerator Casting();
    public Image Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    protected Skill(GameObject creature, float hpcost = 0, float manacost = 0, float energycost =0,float cooldown = 0)
    {
        Father = creature;
        this.hpcost = hpcost;
        this.manacost = manacost;
        this.energycost = energycost;
        this.Cooldown = cooldown;
    }

    public bool CanCast()
    {
        var father = Father.GetComponent<Creature>();
        if (Cooldown_remain == 0)
        {

            if (father is Hero)
            {
                Hero hero = father as Hero;

                if (hero.Health > hpcost)
                {
                    if (hero.Mana > manacost)
                    {
                        if (hero.Energy > energycost)
                        {
                            return true;
                        }
                        else
                        {
                            Debug.Log("Not enough energy");
                        }
                    }
                    else
                    {
                        Debug.Log("Not enough mana");
                    }
                }
                else
                {

                    Debug.Log("NOT HP");
                }
            }
            else
            {
                return true;
            }
        }
        else 
        {
            Debug.Log("Cooldown: " + Cooldown_remain);
        }
        return false;
    }
    public void TakeResourses()
    {
        var father = Father.GetComponent<Creature>();
        if (father is Hero)
        {
            Hero hero = father as Hero;
            hero.Mana -= manacost;
            hero.Health -= hpcost;
            hero.Energy -= energycost;
        }
    }
    public void SetCooldown() 
    {
        Cooldown_remain = Cooldown;
    }
    public void SetCooldown(float a) 
    {
        Cooldown_remain = a;
    }

}



public class SkillHandler 
{
    readonly Creature Father;
    public List<Skill> Skills = new List<Skill>();
    public int SkillCount 
    { 
        get 
        {
            return Skills.Count;
        } 
    }
    public SkillHandler(Creature Father)
    {
        this.Father = Father;
    }
    public void AddSkill(Skill skill)
    {
        Skills.Add(skill);
        IfIsHero();

    }
    public void RemoveSkill(Skill skill)
    {
        Skills.Remove(skill);
        IfIsHero();
    }
    public void OnTik() 
    {
        for (int i = 0; i < Skills.Count; i++)
        {
            Skills[i].Cooldown_remain -= Time.deltaTime;
        }
    }
    void IfIsHero() 
    {
        if (Father is Hero)
        {
            (Father as Hero).SkillBookUpdate();
        }
    }
    
}


public class Charge : Skill
{
    readonly float speed = 30f;
    public Charge(GameObject creature, float hpcost = 0, float manacost = 0, float energycost = 0) : base(creature, hpcost, manacost, energycost)
    {
        animation_time = 0.2f;
    }
    
    public override IEnumerator Casting()
    {
        Father.GetComponent<Rigidbody2D>().angularVelocity = 0;
        animation_time_spended = 0;
        Father.GetComponent<Controller>().SetMoveBlock();
        while (true)
        {
            animation_time_spended += Time.deltaTime;
            Father.GetComponent<Rigidbody2D>().velocity = Father.transform.up * speed * -1;
            if (animation_time_spended > animation_time)
            {
                Father.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                Father.GetComponent<Controller>().RemoveMoveBlock();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}


public class sHeal : Skill
{
    readonly float healAmount = 0;
    private readonly float healInterval = 0;
    private readonly float healDuration = 0;
    
    public sHeal(GameObject creature, float healAmount, float healInterval, float healDuration, float cooldown = 0, float hpcost = 0, float manacost = 0, float energycost = 0) : base(creature, hpcost, manacost, energycost, cooldown)
    {
        this.healAmount = healAmount;
        this.healInterval = healInterval;
        this.healDuration = healDuration;
        animation_time = 0.2f;
    }

    public override IEnumerator Casting()
    {
        animation_time_spended = 0;

        if (CanCast())
        {
            TakeResourses();
            Father.GetComponent<Creature>().BuffHandler.AddBuff(new HealOnTime("Heal sashi", Father.GetComponent<Creature>(), healDuration, healInterval, healAmount, Father.GetComponent<Creature>()));
            SetCooldown();
            yield break;

        }
    }
}

public class sVortex : Skill
{
    private readonly float damage = 0;
    private readonly float radius = 0;
    readonly int LayerMask = 1 << 9;
    public sVortex(GameObject creature, float damage, float radius, float cooldown = 0, float hpcost = 0, float manacost = 0, float energycost = 0) : base(creature, hpcost, manacost, energycost,cooldown)
    {
        this.damage = damage;
        this.radius = radius;
    }
    
    public override IEnumerator Casting()
    {
        if (CanCast())
        {
            TakeResourses();
            RaycastHit2D[] result = Physics2D.CircleCastAll(Father.transform.position, radius, Vector2.zero, Mathf.Infinity, LayerMask);
            for (int i = 0; i < result.Length; i++)
            {
                Creature temp = result[i].collider.gameObject.GetComponent<Creature>();
                temp.TakeDamage(new Attack(Father.GetComponent<Creature>(), temp, 2, damage));
                
            }
            SetCooldown();
            yield break;
        }
    }

    

}

public class meleeAtack : Skill
{
    public float RayLength = 0, damage = 0;
    RaycastHit2D[] RayResult = null;
    public int StartDegrees = 0, Raycount = 0, Step = 0, damageType = 0;


    public meleeAtack(GameObject creature, float RayLength, float damage, float cooldown = 0,float hpcost= 0 ,float manacost = 0, float energycost = 0, int damageType = 2, int StartDegrees = 45, int Raycount = 21, int Step = 5) : base(creature, hpcost, manacost, energycost, cooldown)
    {
        this.RayLength = RayLength;
        this.damage = damage;
        this.StartDegrees = StartDegrees;
        this.Raycount = Raycount;
        this.Step = Step;
        this.damageType = damageType;
    }

    public override IEnumerator Casting()
    {
        if (CanCast())
        {
            TakeResourses();
            int degrees = StartDegrees;
            List<GameObject> results = new List<GameObject>();
            for (int i = 0; i < Raycount; i++)
            {
                RayResult = Physics2D.RaycastAll(Father.transform.position, Quaternion.AngleAxis(degrees, Vector3.forward) * Father.transform.right * -1, RayLength);
                for (int j = 1; j < RayResult.Length; j++)
                {
                    if (!results.Contains(RayResult[j].collider.gameObject))
                    {
                        results.Add(RayResult[j].collider.gameObject);
                    }
                }

                Debug.DrawRay(Father.transform.position, Quaternion.AngleAxis(degrees, Vector3.forward) * (Father.transform.right * -1 * RayLength), Color.green);

                degrees += Step;
            }
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].GetComponent<Creature>() == null) 
                {
                    continue;
                }
                var attack = new Attack(Father.GetComponent<Creature>(), results[i].GetComponent<Creature>(), damageType, damage);
                results[i].GetComponent<Creature>().TakeDamage(attack);
                Debug.Log(results[i]);
                attack.Sender.AttackInvoke(attack);
            }
            SetCooldown();
            yield break;
        }
    }

}

public class sAdrenalin : Skill
{
    int energy_regen = 0;
    public sAdrenalin(GameObject creature, int energy_regen,float hpcost = 0, float manacost = 0, float energycost = 0, float cooldown = 0) : base(creature, hpcost, manacost, energycost, cooldown)
    {
        this.energy_regen = energy_regen;
    }

    public override IEnumerator Casting()
    {
        if (CanCast()) 
        {
            TakeResourses();
            (Father.GetComponent<Creature>() as Hero).Energy += energy_regen;
            
        }
        yield break;
    }
}