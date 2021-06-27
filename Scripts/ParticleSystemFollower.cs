using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemFollower : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Hero;
    void Start()
    {
        Hero = GameObject.Find("Hero");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Hero.transform.position;
    }
}
