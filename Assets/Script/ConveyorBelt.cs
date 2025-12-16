using UnityEngine;
using UnityEngine.UI;

public class ConveyorSlotsLoop : MonoBehaviour
{
    public RectTransform content;
    public RectTransform viewport;
    public float speed = 120f;

    float step; // slotWidth + spacing

    void Start()
    {
        // Lấy size slot
        RectTransform slot = content.GetChild(0) as RectTransform;
        float slotWidth = slot.rect.width;

        // Lấy spacing từ Layout Group
        HorizontalLayoutGroup layout = content.GetComponent<HorizontalLayoutGroup>();
        float spacing = layout != null ? layout.spacing : 0f;

        step = slotWidth + spacing;
    }

    void Update()
    {
        // Di chuyển băng chuyền
        content.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        RectTransform firstSlot = content.GetChild(0) as RectTransform;

        // Vị trí slot đầu trong world
        Vector3 slotWorld = firstSlot.TransformPoint(firstSlot.rect.center);

        // Mép trái viewport
        Vector3 viewportLeft = viewport.TransformPoint(
            new Vector3(-viewport.rect.width / 2, 0, 0)
        );

        // Nếu slot đầu ra khỏi viewport
        if (slotWorld.x < viewportLeft.x - step / 2)
        {
            // Đưa slot đầu xuống cuối
            firstSlot.SetAsLastSibling();

            // Bù lại vị trí để không bị giật
            content.anchoredPosition += Vector2.right * step;
        }
    }
}
