
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    private Image _imgFood;
    private GameObject currentItem;
    private Grill grill;
    private void Awake()
    {
        _imgFood = this.transform.GetChild(0).GetComponent<Image>();
        grill = GetComponentInParent<Grill>();
    }
    public void NotifyChanged()
    {
        grill.CheckAndClear();
    }
    public bool HasItem()
    {
        return currentItem != null;
    }
    public string GetItemType()
    {
        if (currentItem == null) return null;

        FoodItem item = currentItem.GetComponent<FoodItem>();
        if (item == null) return null;

        return item.GetItemType();
    }
    public void SetItem(GameObject item)
    {
        currentItem = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
    }

    public void ClearSlot()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }
    public void OnSetSlot(Sprite spr)
    {
        _imgFood.gameObject.SetActive(true);
        _imgFood.sprite = spr;
        //_imgFood.SetNativeSize();
    } 
    public void OnActiveFood(bool active)
    {
        _imgFood?.gameObject.SetActive(active);
    }
    public void Clear()
    {
        _imgFood.gameObject.SetActive(false);
        _imgFood.sprite = null;
    }
    public bool HasFood => _imgFood.gameObject.activeInHierarchy;
    public Sprite GetSpriteFood => _imgFood.sprite;
    public RectTransform ItemRect => _imgFood.rectTransform;
    public void DetachItem()
    {
        currentItem = null;
        //_imgFood.gameObject.SetActive(false);
        //_imgFood.sprite = null;
    }
}
