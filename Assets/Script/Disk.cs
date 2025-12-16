using DG.Tweening;
using UnityEngine;

public class Disk : MonoBehaviour
{
    public float moveDistance = 300f;
    public float moveDuration = 5f;
    public float delayBeforeMove = 1f;   //  thời gian chờ

    void Start()
    {
        Sequence seq = DOTween.Sequence();

        //  chờ sau khi sinh ra
        seq.AppendInterval(delayBeforeMove);

        //  bắt đầu di chuyển
        seq.Append(
            transform.DOMoveX(transform.position.x + moveDistance, moveDuration)
                     .SetEase(Ease.OutCubic)
        );

        // ❌ huỷ object
        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
