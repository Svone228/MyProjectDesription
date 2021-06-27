using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHole : MonoBehaviour
{
    public float force;
    public float radius;
    int LayerMask = 1 << 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D[] result = Physics2D.CircleCastAll(gameObject.transform.position, radius, Vector2.zero, Mathf.Infinity, LayerMask);
        for (int i = 0; i < result.Length; i++)
        {
            Rigidbody2D temp = result[i].collider.gameObject.GetComponent<Rigidbody2D>();
            temp.AddForce((new Vector2(gameObject.transform.position.x,gameObject.transform.position.y) - new Vector2(result[i].collider.gameObject.transform.position.x, result[i].collider.gameObject.transform.position.y)).normalized * temp.mass * force);


        }
    }
}
