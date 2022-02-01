using System.Linq;
using UnityEngine;

public class DrawConstellation : MonoBehaviour
{
    static float SpaceSize = 1500.0f;    　　　　// 星座球の半径
    static float StarBaseSize = 8.0f;           // 星の大きさの基準

    [SerializeField]
    GameObject starPrefab;                      // 星のプレハブ
    [SerializeField]
    GameObject linePrefab;                      // 星座線のプレハブ
    [SerializeField]
    GameObject namePrefab;                      // 星座名のプレハブ

    public ConstellationData ConstellationData { get; set; }    // 描画する星座データ

    GameObject linesParent;                     // ラインをまとめるゲームオブジェクト

    // ラインをまとめるのゲームオブジェクトのプロパティ
    public GameObject LinesParent { get { return linesParent; } }

    void Start()
    {
        // GameObject の名前を星座名に変更
        if (ConstellationData.Name != null)
        {
            gameObject.name = ConstellationData.Name.Name;
        }

        // データから星座を作成
        CreateConstellation();
    }

    // 星座の作成
    void CreateConstellation()
    {
        // リストから星を作成
        foreach (var star in ConstellationData.Stars)
        {
            // 星の作成
            var starObject = CreateStar(star);
            // 自分の子供に接続
            starObject.transform.SetParent(transform, false);
        }

        if (ConstellationData.Lines != null)
        {
            // 星座線の親を作成
            linesParent = new GameObject("Lines");
            // 自分の子供に接続
            linesParent.transform.SetParent(transform, false);
            var parent = linesParent.transform;

            // リストから星座線を作成
            foreach (var line in ConstellationData.Lines)
            {
                // 星座線の作成
                var lineObject = CreateLine(line);
                // 星座線の親の子供に接続
                lineObject.transform.SetParent(parent, false);
            }
        }

        if (ConstellationData.Name != null)
        {
            // 星座名を作成
            var nameObject = CreateName(ConstellationData.Name, ConstellationData.Position);
            // 自分の子供に接続
            nameObject.transform.SetParent(transform, false);
        }
    }

    // 星の作成
    GameObject CreateStar(StarData starData)
    {
        // 星のプレハブからインスタンス作成
        var star = Instantiate(starPrefab);
        var starTrans = star.transform;

        // 星の見える方向へ回転させる
        starTrans.localRotation = Quaternion.Euler(starData.Declination, starData.RightAscension, 0.0f);
        // 星の名前をHIP番号にする
        star.name = string.Format("{0}", starData.Hip);

        var child = starTrans.GetChild(0);
        // 子供の球の位置を天球の位置へ移動させる
        child.transform.localPosition = new Vector3(0.0f, 0.0f, SpaceSize);

        // 視等級を星のサイズにする
        var size = StarBaseSize - starData.ApparentMagnitude;
        child.transform.localScale = new Vector3(size, size, size);

        // Rendererの取得
        var meshRanderer = child.GetComponent<Renderer>();
        var color = Color.white;

        // 星のカラータイプにより色を設定する
        switch (starData.ColorType)
        {
            case "O":   // 青
                color = Color.blue;
                break;
            case "B":   // 青白
                color = Color.Lerp(Color.blue, Color.white, 0.5f);
                break;
            default:
            case "A":   // 白
                color = Color.white;
                break;
            case "F":   // 黄白
                color = Color.Lerp(Color.white, Color.yellow, 0.5f);
                break;
            case "G":   // 黄
                color = Color.yellow;
                break;
            case "K":   // 橙
                color = new Color(243.0f / 255.0f, 152.0f / 255.0f, 0.0f);
                break;
            case "M":   // 赤
                color = new Color(200.0f / 255.0f, 10.0f / 255.0f, 0.0f);
                break;
        }

        // マテリアルに色を設定する
        meshRanderer.material.SetColor("_Color", color);

        return star;
    }

    // 星座線の作成
    GameObject CreateLine(ConstellationLineData lineData)
    {
        // 始点の星の情報を取得
        var start = GetStar(lineData.StartHip);
        // 終点の星の情報を取得
        var end = GetStar(lineData.EndHip);
        // 星座線のプレハブからインスタンス作成
        var line = Instantiate(linePrefab);
        // LineRendererの取得
        var lineRenderer = line.GetComponent<LineRenderer>();

        // LineRendererの始点と終点の位置を登録（星の見える方向へ回転させた後、天球の位置まで移動をさせる）
        lineRenderer.SetPosition(0, Quaternion.Euler(start.Declination, start.RightAscension, 0.0f) * new Vector3(0.0f, 0.0f, SpaceSize));
        lineRenderer.SetPosition(1, Quaternion.Euler(end.Declination, end.RightAscension, 0.0f) * new Vector3(0.0f, 0.0f, SpaceSize));

        return line;
    }

    // StarDataのデータ検索
    StarData GetStar(int hip)
    {
        // 同じHIP番号を検索
        return ConstellationData.Stars.FirstOrDefault(s => hip == s.Hip);
    }

    // 星座名の作成
    GameObject CreateName(ConstellationNameData nameData, ConstellationPositionData positionData)
    {
        // 星座名のプレハブからインスタンス作成
        var text = Instantiate(namePrefab);
        var textTrans = text.transform;

        // 星の見える方向へ回転させる
        textTrans.localRotation = Quaternion.Euler(positionData.Declination, positionData.RightAscension, 0.0f);
        text.name = nameData.Name;

        // 子供の3D Textの位置を天球の位置へ移動させる
        var child = textTrans.GetChild(0);
        child.transform.localPosition = new Vector3(0.0f, 0.0f, SpaceSize);

        // TextMeshを取得して、星座の名前に変更する
        var textMesh = child.GetComponent<TextMesh>();
        textMesh.text = string.Format("{0}座", nameData.JapaneseName);

        return text;
    }
}