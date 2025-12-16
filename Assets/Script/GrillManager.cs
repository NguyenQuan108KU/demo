using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrillManager : MonoBehaviour
{
    public RectTransform grillPrefab;
    public float grillHeight = 180f;
    public float moveDuration = 0.35f;

    List<RectTransform> grills = new List<RectTransform>();

    void Awake()
    {
        grills.Clear();
        foreach (Transform t in transform)
            grills.Add(t.GetComponent<RectTransform>());
    }

    public void RemoveAndSpawn(RectTransform removed)
    {
        int index = grills.IndexOf(removed);
        if (index < 0) return;

        // 1️⃣ Dồn các grill trên xuống
        for (int i = 0; i < index; i++)
        {
            grills[i].DOAnchorPosY(
                grills[i].anchoredPosition.y - grillHeight,
                moveDuration
            );
        }

        // 2️⃣ Xóa grill
        removed.gameObject.SetActive(false);
        grills.RemoveAt(index);

        // 3️⃣ Spawn grill mới ở trên
        SpawnNewGrill();
    }

    void SpawnNewGrill()
    {
        // vị trí của grill đầu tiên cũ
        float targetY = grills[0].anchoredPosition.y;

        // spawn grill mới
        RectTransform newGrill =
            Instantiate(grillPrefab, transform);

        newGrill.SetAsFirstSibling();

        // spawn ở trên
        float spawnY = targetY + grillHeight;
        newGrill.anchoredPosition =
            new Vector2(grills[0].anchoredPosition.x, spawnY);

        // animate rơi xuống đúng vị trí grill đầu tiên
        newGrill.DOAnchorPosY(targetY, moveDuration);

        grills.Insert(0, newGrill);
    }

}
