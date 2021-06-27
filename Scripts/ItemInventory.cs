using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemInventory : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool enable = false;
    GameObject swap;
    public GameObject Hero;
    public Item item;
    public GameObject Info_Panel;
    public Inventory inventory;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>().inventory;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //swap.GetComponent<RectTransform>().transform.position = eventData.pointerCurrentRaycast.screenPosition;
        if(swap!=null)
        swap.GetComponent<RectTransform>().transform.position = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x-20, eventData.pointerCurrentRaycast.screenPosition.y-20) ;
    }
    public void OnBeginDrag(PointerEventData data)
    {
        if (!inventory.IsEmpity(gameObject.name))
        {
            CreateSwapObj();
        }
    }
    void CreateSwapObj() 
    {
        this.swap = new GameObject();
        swap.name = "SwapItem";
        swap.AddComponent<CanvasRenderer>();
        swap.AddComponent<RectTransform>();
        swap.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        swap.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        Image mImage = swap.AddComponent<Image>();
        mImage.sprite = GetComponent<Image>().sprite;
        swap.transform.position = new Vector3(0, 0, 0);
        swap.transform.SetParent(transform.parent.transform.parent.transform.parent.transform);
        swap.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        swap.AddComponent<CanvasScaler>();
    }
    public void SetHero(GameObject hero) 
    {
        Hero = hero;
        
    }

    public void OnEndDrag(PointerEventData eventData) //В этом слоте посылаем себя как то что должно быть перемещено в свап обджект
    {
        if (swap != null)
        {
            Destroy(swap);
            inventory.MoveToCurrentSlot(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //Я и есть свап обджект и я не тот что в OnEndDrag
    {
        inventory.SetSwapObject(gameObject);
        Info_Panel.SetActive(PanelUpdate());
        Info_Panel.transform.position = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x + 70, eventData.pointerCurrentRaycast.screenPosition.y - 40);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventory.SetSwapObject(null);
        Info_Panel.SetActive(false);
    }
    bool PanelUpdate() 
    {
        int i = inventory.FindSlotForName(name);
        if (inventory.items[i] != null && inventory.items[i].Item_name != null)
        {
            Item current_item = inventory.items[i];
            Info_Panel.transform.GetChild(0).GetComponent<Text>().text = current_item.Item_name;
            Info_Panel.transform.GetChild(1).GetComponent<Image>().sprite = current_item.Sprite;
            return true;
        }
        else 
        {
            return false;
        }
    }
}
