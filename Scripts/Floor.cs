using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public GameObject floor;
    public List<GameObject> AnotherFloor;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        floor.SetActive(true);
        foreach (var item in AnotherFloor)
        {
            item.SetActive(false);
        }
    }
}
