
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    public Image _imgFood;
    private Color _normalColor = new Color(1f, 1f, 1f, 1f);
    private Color _fadeColor = new Color(1f, 1f, 1f, 0.5f);

    private void Awake()
    {
        _imgFood = this.transform.GetChild(0).GetComponent<Image>();
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
    public void OnFadeFood()
    {
        this.OnActiveFood(true);
        _imgFood.color = _fadeColor;    
    }
    public void OnHideFood()
    {
        this.OnActiveFood(true);
        _imgFood.color = _normalColor;
    }
    public void Clear()
    {
        _imgFood.gameObject.SetActive(false);
        _imgFood.sprite = null;
    }
    public bool HasFood => _imgFood.gameObject.activeInHierarchy;
    public Sprite GetSpriteFood => _imgFood.sprite;
}
