using UnityEngine;

public class Creature : MonoBehaviour
{
    public float Name;//Имя персонажа
    private float health = 1;
    public bool immortal = false;
    public float Health 
    {
        get 
        {
            return health;
        }
        set
        {
            if (value >= MaxHP) 
            {
                health = MaxHP;
                
            }
            else
            {
                health = value;
                if (health <= 0)
                {
                    health = 0;
                    if(!immortal)
                        
                    Death();
                }
            }
        }
    }//текущее хп 
    public float MaxHP;//Максимальное хп
    public float AbsoluteResist;// абсолютная защита от любого урона
    public float Resist;// В процентах
    public float Armor;// Тоже в процентах
    public float Damage;//
    public float MoveSpeed;//
    public BuffHandler BuffHandler;
    public delegate void onTakeDamage(Attack attack);
    public event onTakeDamage OnTakeDamage;
    public delegate void onAttack(Attack attack);
    public event onTakeDamage OnAttack;
    public delegate void onMove(float delta);
    public event onMove OnMove;
    public PassiveSkillHandler PassiveSkillHandler;
    Vector3 FatherPos;
    //Методы со временем нужно расширить чтобы получать больше сведений от того кто наносит урон и урон какого типа.
    private void Start()
    {
        BuffHandler = new BuffHandler();
        Health = MaxHP;
        immortal = false;
        FatherPos = transform.position;
        
        PassiveSkillHandler = new PassiveSkillHandler(this);
    }
    private void FixedUpdate()
    {
        OnMoveHandler();
        BuffHandler.OnUpdate();
    }
    public virtual void TakeDamage(Attack attack) // Определяям какой урон 
    {
        if (attack.type == 1)
        {
            TakePureDamage(attack);
        }
        else 
        {
            if (attack.type == 2) 
            {
                TakePhysicalDamage(attack);
            }
            else 
            {
                TakeMagicDamage(attack);
            }
        }
        OnTakeDamage?.Invoke(attack);
    }
    public virtual void TakeHeal(Heal heal) // Восстановление здоровья
    {
        Health += heal.heal;      
    }
    protected void TakeMagicDamage(Attack atack) // магический урон
    {
        Health -= atack.Damage * ((100-Resist)/100);
        if (Health <= 0)
        {
            Health = 0;
        }
    }
    protected void TakePhysicalDamage(Attack atack) // Физический урон 
    {
        Health -= atack.Damage * ((100 - Armor) / 100);
        if (Health <= 0)
        {
            Health = 0;
        }
    }
    protected void TakePureDamage(Attack atack) // Чистый урон урон 
    {
        Health -= atack.Damage;
        if (Health <= 0)
        {
            Health = 0;
        }
    }
    protected virtual void Death()
    {
        Destroy(gameObject);
        
    }
    public void AttackInvoke(Attack attack)
    {   
        OnAttack?.Invoke(attack);
    }
    protected void OnMoveHandler()
    {
        
        if (FatherPos != transform.position)
        {
            float delta = Mathf.Abs(FatherPos.magnitude - transform.position.magnitude);
            OnMove?.Invoke(delta);
            FatherPos = transform.position;
        }
    }
    public void FullHP() 
    {
        Health = MaxHP;
    }
}
