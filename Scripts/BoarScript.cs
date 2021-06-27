using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarScript : BaseNPCScript
{
    // Start is called before the first frame update
    public int MoveDelation;
    public int delationnow;
    public float DelayRotation;
    public float DelayRotationNow;
    public GameObject Area;
    float tempspeed;
    bool antibugAgro;
    public GameObject target;
    public float AttackRange;
    bool Attacking;
    float AttackTime = 121;
    float AttackTimeNow;
    void Start()
    {
        tempspeed = 1;
        delationnow = 0;
        DelayRotation = 20;
        DelayRotationNow = 0;
        antibugAgro = false;
        Attacking = false;
        rb.angularDrag = 500;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CheckRange() || antibugAgro)
        {
            Attacking = false;
            target = Area;
        }

        else
        {
            target = CheckEnemy();
            if (target != null)
            {
                tempspeed = speed * 1.5f;
                if ((target.transform.position - transform.position).magnitude < AttackRange || AttackTimeNow > 0)
                {
                    Attack();
                }
                else 
                {
                    Attacking = false;
                }
            }
            else 
            {
                Attacking = false;
                target = CheckEnemy();
                tempspeed = speed;
                if (delationnow < MoveDelation)
                {
                    delationnow++;
                    if (DelayRotation > DelayRotationNow)
                    {
                        DelayRotationNow++;
                    }
                    else
                    {
                        if (DrawRays())
                        {
                            Rotate(true);
                        }
                        DelayRotationNow = 0;
                    }
                }
                else
                {
                    Rotate(false);
                    delationnow = 0;
                }
            }
        }
        if (!Attacking)
            MoveTo(target);
        else AttackTimeNow += 1;

    }
    public override void Move()
    {
        rb.velocity = transform.up * tempspeed;
        GetComponent<Animator>().SetBool("Run", true);
        GetComponent<Animator>().SetBool("Attack", false);

    }
    public new bool DrawRays() 
    {
        int degrees = Start_degrees;
        for (int i = 0; i < Raycount; i++)
        {
            Maslo = Physics2D.RaycastAll(transform.position, Quaternion.AngleAxis(degrees, Vector3.forward) * (transform.right * Ray_Length));
            for (int j = 1; j < Maslo.Length; j++)
            {


               
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(degrees, Vector3.forward) * (transform.right * Ray_Length), Color.green);
                if (Maslo[j].collider.isTrigger == false && (Maslo[j].transform.position - transform.position).magnitude <Ray_Length) 
                {
                    //Debug.Log("Я Попав " + i + " " + Maslo[j].collider.gameObject.name);
                    return true;
                }
            }
            degrees += Step;
        }
        return false;
    }
    bool CheckRange()
    {
        if (antibugAgro)
        {
            if ((transform.position - Area.transform.position).magnitude < Area.GetComponent<BoarArea>().BoarRange / 2) 
            {
                antibugAgro = false;
            }
        }
        if (Area != null)
        {
            if ((transform.position - Area.transform.position).magnitude > Area.GetComponent<BoarArea>().BoarRange)
            {
                antibugAgro = true;
                return true;
            }
        }
        return false;
    }

    void Attack() 
    {
        LookTo(target);
        rb.velocity = Vector2.zero;
        Attacking = true;
        GetComponent<Animator>().SetBool("Run", false);
        GetComponent<Animator>().SetBool("Attack", true);
        if (AttackTime < AttackTimeNow)
        {
            target.GetComponent<Creature>().TakeDamage(new Attack(GetComponent<Creature>(), target.GetComponent<Creature>(), 1, GetComponent<Creature>().Damage));
            AttackTimeNow = 0;
        }
    }
    GameObject CheckEnemy()
    {
        var LayerMask = 1 << 10;
        RaycastHit2D[] result = Physics2D.CircleCastAll(transform.position, 5, Vector2.zero, Mathf.Infinity, LayerMask);
        for (int i = 0; i < result.Length;)
        {
           return result[0].collider.gameObject;
        }
        return null;
    }
    void MoveTo(GameObject target) 
    {
        if (target != null) 
        {
            LookTo(target);
            Move();
        }
        else 
        {
            Move();
        }


    }
    void LookTo(GameObject taget) 
    {
        Vector3 diff = target.transform.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
