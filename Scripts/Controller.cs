using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D r_body;
    public float speed;
    float TempSpeed;
    float horizontal;
    float vertical;
    Hero hero;
    public Animator LegAnimator;
    public Animator BodyAnimator;
    public GameObject Legs;
    public GameObject Body;
    public GameObject BG_inventory;
    public bool enable = true;
    bool Move_Block;
    public GameObject Light;
    // Start is called before the first frame update
    void Start()
    {
        r_body = GetComponent<Rigidbody2D>();
        hero = GetComponent<Hero>();
        LegAnimator = Legs.GetComponent<Animator>();
        BodyAnimator = Body.GetComponent<Animator>();
        Move_Block = true;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (BG_inventory.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    BG_inventory.SetActive(false);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    BG_inventory.SetActive(true);
                }
            }
            if (enable)
            {
                enable = false;
            }
            else
            {
                enable = true;
            }
        }

        if (enable) 
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(hero.SkillHandler.Skills[0].Casting());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(hero.SkillHandler.Skills[1].Casting());

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(hero.SkillHandler.Skills[2].Casting());
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(hero.SkillHandler.Skills[3].Casting());
            }
            if (Input.GetButtonDown("Fire1"))
            {
                MainAtack();
            }
            if (Input.GetKeyDown(KeyCode.Z)) 
            {
                Light.SetActive(!Light.activeSelf);
            }
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (Move_Block) 
        {
            Move();
        }
        else 
        {
            LegAnimator.SetBool("Run", false);
            BodyAnimator.SetBool("Run", false);
        }

    }
    public void SetMoveBlock() // use only for charge 
    {
        Move_Block = false;
    }
    public void RemoveMoveBlock() // use only for charge
    {
        Move_Block = true;
    }
    void Move() 
    {
        speed = GetComponent<Hero>().MoveSpeed;
        TempSpeed = speed;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            LegAnimator.SetBool("Run", true);
            BodyAnimator.SetBool("Run", true);
        }
        else 
        {
            LegAnimator.SetBool("Run", false);
            BodyAnimator.SetBool("Run", false);

        }
       
        //if()
        
        var direction = Input.mousePosition - UnityEngine.Camera.main.WorldToScreenPoint(transform.position); // Нахождение катетов для расчёта тангенса, а в последствии и количества градусов угла. 
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Нахождение тангенса угла и перевод его в градусы.

        r_body.MoveRotation(angle + 90);
        Vector3 force = new Vector2(horizontal, vertical).normalized;

        var temp = Mathf.Acos(Vector3.Dot(force, direction) / (force.magnitude * direction.magnitude))*Mathf.Rad2Deg;
        if (!float.IsNaN(temp))
        {
            if (temp > 140)
            {
                TempSpeed /= 2;
            }
            else if (temp > 60)
            {
                TempSpeed /= 1.5f;
            }
            
        }
        r_body.velocity = new Vector2(horizontal, vertical).normalized * TempSpeed;
    

    }
    void MainAtack() //тут создаем прожектайлы, надо реализовать по или делегатом или интерфейсом
    {
        if (hero.items[0] != null && hero.items[0].Attack != null)
        {
            StartCoroutine(hero.items[0].Attack.Casting());
        }
    }
    public void OffLight() 
    {
        Light.SetActive(false);
    }
}
