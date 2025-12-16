using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grill : MonoBehaviour
{
    public int requiredCount = 5;
    public float mergeDuration = 0.4f;
    public float grillDisappearDuration = 0.6f;
    public GameObject diskPrefabs;
    public Transform diskPosition;

    private FoodSlot[] slots;
    private RectTransform grillRect;

    public GrillManager grillManager;
    RectTransform rect;

    private void Awake()
    {

        rect = GetComponent<RectTransform>();
        slots = GetComponentsInChildren<FoodSlot>();
        grillRect = transform as RectTransform;
        grillManager = GetComponentInParent<GrillManager>();
    }
    public void CheckAndClear()
    {
        Dictionary<Sprite, List<FoodSlot>> map = new();

        foreach (var slot in slots)
        {
            if (!slot.HasFood) continue;

            Sprite spr = slot.GetSpriteFood;
            if (spr == null) continue;

            if (!map.ContainsKey(spr))
                map[spr] = new List<FoodSlot>();

            map[spr].Add(slot);
        }

        foreach (var pair in map)
        {
            if (pair.Value.Count >= requiredCount)
            {
                MergeItemsToCenter(pair.Value.GetRange(0, requiredCount));
                break;
            }
        }

    }

    // ================== MERGE + DISAPPEAR ==================
    void MergeItemsToCenter(List<FoodSlot> items)
    {
        Sequence seq = DOTween.Sequence();

        int count = items.Count;
        int centerIndex = count / 2;

        RectTransform centerItem = items[centerIndex].ItemRect;
        Vector2 centerPos = centerItem.anchoredPosition;

        float baseOffset = 20f;
        float spacing = 15f;
        float mergeTime = 0.45f;

        // ================== 1️⃣ CHỤM 5 ITEM ==================
        for (int i = 0; i < count; i++)
        {
            if (i == centerIndex) continue;

            RectTransform item = items[i].ItemRect;
            PrepareItem(item);

            int distance = Mathf.Abs(i - centerIndex);
            int direction = i < centerIndex ? -1 : 1;

            Vector2 targetPos =
                centerPos + new Vector2(
                    direction * (baseOffset + (distance - 1) * spacing),
                    0
                );

            seq.Join(
                item.DOAnchorPos(targetPos, mergeTime)
                    .SetEase(Ease.OutCubic)
            );
        }

        // ⏸️ đợi chụm xong
        seq.AppendInterval(0.1f);

        // ================== 2️⃣ SPAWN DISK ==================
        GameObject disk = null;

        seq.AppendCallback(() =>
        {
            disk = Instantiate(diskPrefabs, DiskPosition.instance.gameObject.transform);
        });

        // ================== 3️⃣ 5 ITEM BAY VÀO DISK ==================
        int arrivedCount = 0;
        float flyToDiskTime = 0.4f;

        foreach (var slot in items)
        {
            RectTransform item = slot.ItemRect;
            seq.Join(
                item.DOMove(DiskPosition.instance.gameObject.transform.position, flyToDiskTime)
                    .SetEase(Ease.InCubic)
                    .OnComplete(() =>
                    {
                        // làm con của disk
                        item.SetParent(disk.transform, true);
                        arrivedCount++;
                    })
            );
        }

        // ================== 4️⃣ DISK DI CHUYỂN SAU KHI NHẬN ĐỦ 5 ==================
        seq.AppendInterval(flyToDiskTime + 0.05f);

        // ================== 5️⃣ GRILL NHỎ LẠI + CLEAR ==================
        seq.Append(
            grillRect.DOScale(Vector3.zero, grillDisappearDuration)
                     .SetEase(Ease.InQuad)
        );

        seq.AppendCallback(() =>
        {
            foreach (var slot in items)
                slot.DetachItem();

            grillRect.localScale = Vector3.one; // reset cho lần sau
            //gameObject.SetActive(false);
            grillManager.RemoveAndSpawn(rect);
        });


        // ================== 5️⃣ CLEAR ==================
        seq.AppendInterval(1f);

        seq.AppendCallback(() =>
        {
            foreach (var slot in items)
                slot.DetachItem();

            //gameObject.SetActive(false);
            grillManager.RemoveAndSpawn(rect);
        });
    }
    void PrepareItem(RectTransform item)
    {
        // lưu vị trí world trước khi đổi parent
        Vector3 worldPos = item.position;

        LayoutElement le = item.GetComponent<LayoutElement>();
        if (le == null) le = item.gameObject.AddComponent<LayoutElement>();
        le.ignoreLayout = true;

        item.SetParent(grillRect, false);
        item.anchorMin = item.anchorMax = new Vector2(0.5f, 0.5f);
        item.pivot = new Vector2(0.5f, 0.5f);

        // khôi phục vị trí cũ (QUAN TRỌNG)
        item.position = worldPos;
    }


}
