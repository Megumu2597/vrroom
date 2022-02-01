using UnityEngine;

public class VisibleConstellationLine : MonoBehaviour
{
    GameObject lines;   // Linesゲームオブジェクト

    void Start()
    {
        // 親からLinesを検索する
        var constellation = transform.GetComponentInParent<DrawConstellation>();
        lines = constellation.LinesParent;
        // 星座線を非表示にする
        lines.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // レイヤーがViewHitかどうか
        if (other.gameObject.layer == LayerMask.NameToLayer("ViewHit"))
        {
            // コライダーに当たったら表示する
            lines.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // レイヤーがViewHitかどうか
        if (other.gameObject.layer == LayerMask.NameToLayer("ViewHit"))
        {
            // コライダーに当たらなくなったら非表示にする
            lines.SetActive(false);
        }
    }
}