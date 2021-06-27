using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestRandomizerScript : MonoBehaviour
{
    public float minscale;
    public float maxscale;
    public bool StartRandom;
    bool DoRandom;
    // Start is called before the first frame update
    void Start()
    {
        minscale = 0.75f;
        maxscale = 1.75f;
        StartRandom = false;
        DoRandom = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoRandom != StartRandom) 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var temp = transform.GetChild(i);
                float scale = Random.Range(minscale, maxscale);
                temp.transform.localScale = new Vector3(scale, scale, 1);
                var temp1 = temp.GetChild(0).transform;
                //Vector3(0, 0, Random.Range(0, 180));
                //temp1.transform.localRotation = new Quaternion(0, 0, Random.rotation.z, 0);
            }
        }
    }
}
