using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour
{
    public string imageType;
    private void Awake()
    {
        if (string.IsNullOrEmpty(imageType))
        {
            Image img = GetComponent<Image>();
            if(img != null)
            {
                imageType = img.sprite.name;
            }
        }
    }
    public string GetItemType()
    {
        return imageType;
    }
}
