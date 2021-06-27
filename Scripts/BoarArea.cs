using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarArea : MonoBehaviour
{
    public GameObject Mob;
    public int MobMax;
    public int SpawnTime;
    public int STimeRemain;
    public float SpawnRange;
    public float BoarRange;
    private void Start()
    {

    }
    private void Update()
    {
        if (STimeRemain > SpawnTime)
        {
            if (transform.childCount < MobMax)
            {
                BoarSpawn();
            }
            STimeRemain = 0;
        }
        else
        {
            STimeRemain++;
        }

    }
    Vector2 RandomSpawnPoint()
    {
        if (SpawnRange < 1)
        {
            SpawnRange = 2;
        }
        var result = Random.insideUnitCircle * SpawnRange;
        return result;
    }
    void BoarSpawn() 
    {
        var TempBoar = Instantiate(Mob, gameObject.transform);
        TempBoar.transform.localScale = new Vector3(0.5f, 0.4f, 1);
        TempBoar.transform.localPosition = RandomSpawnPoint();
        var pos = TempBoar.transform.position;
        pos.z = 0;
        TempBoar.transform.position = pos;
        TempBoar.GetComponent<BoarScript>().Area = gameObject;
    }
    
}
