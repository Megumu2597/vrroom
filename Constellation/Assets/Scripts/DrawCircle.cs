using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    [SerializeField]
    int lineCount = 100;        // ラインを描く数
    [SerializeField]
    Color color = Color.white;  // ラインの色
    [SerializeField]
    float radius = 1500.0f;     // 円の半径

    Material lineMaterial;      // ラインのマテリアル

    // ラインのマテリアルの作成
    void CreateLineMaterial()
    {
        // 一度だけ作成します
        if (lineMaterial == null)
        {
            // Unityの標準シェーダを取得
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            // マテリアルを作成してシェーダーを設定
            lineMaterial = new Material(shader);
            // このマテリアルをヒエラルキーに表示しない・シーンに保存しない
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    // 全てのカメラのシーンの描画後に呼び出される描画関数
    void OnRenderObject()
    {
        // マテリアルの作成
        CreateLineMaterial();

        // マテリアルを設定
        lineMaterial.SetPass(0);

        // 現在のマトリックス情報を保存
        GL.PushMatrix();

        // 現在のマトリックス情報をこのゲームオブジェクトのマトリックス情報へ更新
        GL.MultMatrix(transform.localToWorldMatrix);

        // ラインの描画を開始する
        GL.Begin(GL.LINES);

        // カラーの設定
        GL.Color(color);

        // XZ平面に円を描く
        {
            // 最初の頂点の位置
            var startPoint = new Vector3(Mathf.Cos(0.0f) * radius, 0.0f, Mathf.Sin(0.0f) * radius);
            // 1つ前の頂点の位置
            var oldPoint = startPoint;
            for (var Li = 0; Li < lineCount; ++Li)
            {
                // 今回の角度
                var angleRadian = (float)Li / (float)lineCount * (Mathf.PI * 2.0f);
                // 今回の頂点位置
                var newPoint = new Vector3(Mathf.Cos(angleRadian) * radius, 0.0f, Mathf.Sin(angleRadian) * radius);
                // 前の頂点位置から今回の位置へラインを引く
                GL.Vertex(oldPoint);
                GL.Vertex(newPoint);

                // 今回の位置を保存
                oldPoint = newPoint;
            }
            // 最後の頂点から最初の頂点へラインを引く
            GL.Vertex(oldPoint);
            GL.Vertex(startPoint);
        }
        // ラインの描画を終了する
        GL.End();

        // 保存したマトリックス情報へ戻す
        GL.PopMatrix();
    }
}