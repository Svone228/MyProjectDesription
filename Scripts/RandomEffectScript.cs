using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var temp = transform.GetChild(i).gameObject.GetComponent<Animator>();
            if (temp != null) 
            {
                temp.SetFloat("Random", Random.Range(0.3f, 1));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
