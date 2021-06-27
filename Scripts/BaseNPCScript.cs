using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNPCScript : MonoBehaviour
{
    public float Ray_Length;
    public RaycastHit2D[] Maslo;
    public int Start_degrees = 80;
    public int Raycount=20;
    public int Step=4;

    protected Rigidbody2D rb;
    public float speed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void DrawRays()
    {
        int degrees = Start_degrees;
        for (int i = 0; i < Raycount; i++)
        {
            Maslo = Physics2D.RaycastAll(transform.position, Quaternion.AngleAxis(degrees, Vector3.forward) * (transform.right * Ray_Length));
            for (int j = 1; j < Maslo.Length; j++)
            {


                Debug.Log("Я Попав " + i + " " + Maslo[j].collider.gameObject.name);
            }
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(degrees, Vector3.forward) * (transform.right * Ray_Length), Color.green);
            degrees += Step;
        }
    }
    public virtual void Move() 
    {
        rb.velocity = transform.up * speed;
    }
    public virtual void Rotate(bool collision) 
    {
        if (collision)
        {
            var temp = transform.rotation;
            var tempeu = temp.eulerAngles;
            tempeu.x = 0;
            tempeu.y = 0;
            tempeu.z +=Random.Range(80, 160) * Random.Range(0, 2) * 2 - 1;
            temp.eulerAngles = tempeu;
            transform.rotation = temp;
        }
        else 
        {
            
            var temp = transform.rotation;
            var tempeu = temp.eulerAngles;
            tempeu.x = 0;
            tempeu.y = 0;
            tempeu.z += Random.Range(0, 40)* Random.Range(0, 2) * 2 - 1;


            //Debug.Log(tempeu);
            temp.eulerAngles = tempeu;
            transform.rotation = temp;
        }
    }
}
