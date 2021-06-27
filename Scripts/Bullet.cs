using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public Attack attack;
    float time = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 5) 
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Creature creature = collision.GetComponent<Creature>();
        if (creature != null) 
        {
            attack.Resipient = creature;
            creature.TakeDamage(attack);
        }
        transform.position += transform.right * 1; 
        rb.velocity = new Vector2(0,0);
        GetComponent<Collider2D>().enabled = false;
    }
}
