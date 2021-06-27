using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightEnabler : MonoBehaviour
{
    public Sprite slEnabled;
    public Sprite slDisabled;
    SpriteRenderer currentSprite;
    GameObject[] pointLights;
    int delayInterval=0;
    int delay = 0;
    
    bool state;
    float[] ranges; //even - start range, odd end range
    void SetState(bool state)
    {
        for (int i = 0; i < pointLights.Length; i++)
        {
            pointLights[i].SetActive(state);
        }
    }
    public void LightEnable()
    {
        if(slEnabled != null)
        currentSprite.sprite = slEnabled;
        SetState(true);
        state = true;
    }
    public void LightDisable()
    {
        if (slDisabled != null)
            currentSprite.sprite = slDisabled;
        SetState(false);
        
        state = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        if (slDisabled != null)
            currentSprite.sprite = slEnabled;
        pointLights = new GameObject[transform.childCount];
        ranges = new float[transform.childCount * 2];
        for (int i = 0; i < pointLights.Length; i++)
        {
            pointLights[i] = transform.GetChild(i).gameObject;
            ranges[i * 2] = pointLights[i].GetComponent<Light2D>().pointLightOuterRadius;

            ranges[i * 2 + 1] = ranges[i * 2];//top limit

            ranges[i * 2 + 1] += ranges[i * 2]*0.05f; //top limit

            ranges[i * 2] -= ranges[i * 2]* 0.05f;//bottom limit
        }
        
    }


            // Update is called once per frame
    void FixedUpdate()
    {
        if (state)
        {
            delay += 1;
            if (delay > delayInterval)
            {
                for(int i = 0; i < pointLights.Length;i++)
                {
                    pointLights[i].GetComponent<Light2D>().pointLightOuterRadius = Random.Range(ranges[i*2], ranges[i*2+1]);
                }
                delayInterval = Random.Range(4, 7);
                delay = 0;
            }
        }            
    }
}