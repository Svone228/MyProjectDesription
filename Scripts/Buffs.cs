using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Buff
{
    public string name;// имя баффа 
    protected Creature sender;
    protected Image image;
    protected BuffHandler FatherHandler;
    protected Creature Father;//тот на ком висит бафф
    protected float time;//Продолжительность баффа
    protected float spendtime = 0;//Пройденое время с начала баффа
    public int max_stack = 1;//максимальое количество стаков, по умолчанию 1і
    private int stack = 1; // Количество стаков, по умолчанию 1
    protected float interval; //Интервал между тиками
    public readonly int debufftype; // тип дебаффа (юзается для диспела в будущем)
    public int Stack
    {
        get
        {
            return stack;
        }
        set 
        {
            if (value > max_stack) 
            {
                stack = max_stack;
            }
            else 
            {
                stack = value;
            }

        }
    }
    public float Time_remain 
    {
        get 
        {
            return time - spendtime;
        }
    }
    public Buff(string name, Creature sender, Creature Father, float time)
    {
        this.name = name;
        this.sender = sender;
        this.Father = Father;
        this.time = time;
        this.FatherHandler = Father.BuffHandler;
        max_stack = 1;
        stack = 1;
        debufftype = 0;
    }
    public virtual void AddStack() // Стак мы поменялся в бафф хенделере, здесь мы ток обновляем таймеры. Но я оставил этот метод если что-то необычное произойдет
    {
        OnStart();
    }
    public abstract void OnStart();//Вызывается при старте, должна обновлять 
    public abstract void OnTiсk();//Вызывается на каждый фрейм
    public abstract void OnEnd();//Вызывается в конце баффа
    public void RemoveSelf()//Вызывать когда нужно задиспелить этот дафф
    {
        Father.BuffHandler.RemoveBuff(this);
    }
    public abstract void Subscribe();//Получить того на ком висит бафф + подписаться на ивенты
}
public class BuffHandler // Обработчик бафов
{
    public List<Buff> buffs = new List<Buff>();
    public List<Buff> deletelist = new List<Buff>();
    public BuffHandler()
    {
    }
    public void OnUpdate()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].OnTiсk();
        }
        if (deletelist.Count != 0)
        {
            for (int i = 0; i < deletelist.Count; i++) 
            {
                buffs.Remove(deletelist[0]);
            }
        }
    }
    public void AddBuff(Buff buff)
    {
        int i;
        for (i = 0; i < buffs.Count; i++)
        {
            if (buff.GetType() == buffs[i].GetType()) //сравниваем типы
            {
                break;
            }
        }
        if (i == buffs.Count)//Если не нашли нужный тип то добавляем его в список
        {
            buffs.Add(buff);
            buff.OnStart();
            buff.Subscribe();
        }
        else //если нашли нужный тип то добавляем на него стак (если стак всего 1 то нужно просто обновить бафф)
        {
            buff.Stack = buffs[i].Stack;
            buff.Stack += 1;
            buffs[i] = buff;
            buffs[i].AddStack();
        }
    }
    public void RemoveBuff(Buff buff)
    {
        deletelist.Add(buff);
    }
}








public class Bleed : Buff
{
    private readonly int typedamage;
    //Типы нужны чтобы их можно было снимать - типо противоядия водицой или кровотечения бинтами
    //0 - положительный бафф
    //
    private readonly float damage; // положительная величина - дамаг, отрицательная - хилл
    private float spendinterval;

    public Bleed(string name, float damage, float time, float interval, int typedamage, Creature sender, Creature Father) : base(name, sender, Father, time)
    {
        this.damage = damage;
        this.interval = interval;
        this.typedamage = typedamage;
        this.time = time;
    }
    public override void OnStart()
    {
        spendtime = 0;
        spendinterval = 0;
    }
    public override void OnTiсk()// add to update
    {
        spendtime += Time.deltaTime;
        spendinterval += Time.deltaTime;
        if (spendinterval >= interval)
        {
            spendinterval -= interval;
            Father.TakeDamage(new Attack(null, Father, typedamage, damage));
        }
        if (spendtime >= time)
        {
            OnEnd();
        }
    }
    public override void OnEnd()
    {
        RemoveSelf();

    }
    public override void Subscribe()
    {

    }

}

public class HealOnTime : Buff
{
    private readonly float healCount;
    private float spendinterval = 0;
    public HealOnTime(string name, Creature sender, float healDuration, float tickInterval, float healCount, Creature Father) : base(name, sender, Father, healDuration)
    {
        this.interval = tickInterval;
        this.healCount = healCount;
        max_stack = 2;
    }


    public override void OnEnd()
    {
        RemoveSelf();
    }

    public override void OnStart()
    {
        spendtime = 0;
        spendinterval = 0;
    }

    public override void OnTiсk()
    {
        spendtime += Time.deltaTime;
        spendinterval += Time.deltaTime;

        if (spendinterval >= interval)
        {
            spendinterval = 0;
            Father.TakeHeal(new Heal(Father, Father, healCount*Stack));
        }
        if (spendtime >= time)
        {
            OnEnd();
        }
    }

    public override void Subscribe()
    {

    }
}