using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstellationViewer : MonoBehaviour
{
    // 星座CSVデータ
    [SerializeField]
    TextAsset starDataCSV;
    [SerializeField]
    TextAsset starMajorDataCSV;
    [SerializeField]
    TextAsset constellationNameDataCSV;
    [SerializeField]
    TextAsset constellationPositionDataCSV;
    [SerializeField]
    TextAsset constellationLineDataCSV;

    [SerializeField]
    GameObject constellationPrefab;         // 星座のプレハブ

    // 星座データ
    List<StarData> starData;
    List<StarMajorData> starMajorData;
    List<ConstellationNameData> constellationNameData;
    List<ConstellationPositionData> constellationPositionData;
    List<ConstellationLineData> constellationLineData;

    // 整理を行った星座のデータ
    List<ConstellationData> constellationData;

    void Start()
    {
        // CSV データの読み込み
        LoadCSV();

        // 星座データの整理
        ArrangementData();

        // 星座の作成
        CreateConstellation();
    }

    // CSV データの読み込み
    void LoadCSV()
    {
        starData = CsvLoader<StarData>.LoadData(starDataCSV);
        starMajorData = CsvLoader<StarMajorData>.LoadData(starMajorDataCSV);
        constellationNameData = CsvLoader<ConstellationNameData>.LoadData(constellationNameDataCSV);
        constellationPositionData = CsvLoader<ConstellationPositionData>.LoadData(constellationPositionDataCSV);
        constellationLineData = CsvLoader<ConstellationLineData>.LoadData(constellationLineDataCSV);
    }

    // 星座データの整理
    void ArrangementData()
    {
        // 星データを統合
        MergeStarData();

        constellationData = new List<ConstellationData>();

        // 星座名から星座に必要なデータを収集
        foreach (var name in constellationNameData)
        {
            constellationData.Add(CollectConstellationData(name));
        }

        // 星座に使われていない星の収集
        var data = new ConstellationData();
        data.Stars = starData.Where(s => s.UseConstellation == false).ToList();
        constellationData.Add(data);
    }

    // 星データを統合
    void MergeStarData()
    {
        // 今回使用する必要な星を判別する
        foreach (var star in starMajorData)
        {
            // 同じデータがあるか？
            var data = starData.FirstOrDefault(s => star.Hip == s.Hip);
            if (data != null)
            {
                // 同じデータがあった場合、位置データを更新する
                data.RightAscension = star.RightAscension;
                data.Declination = star.Declination;
            }
            else
            {
                // 同じデータがない場合、5等星より明るいのであれば、リストに追加する
                if (star.ApparentMagnitude <= 5.0f)
                {
                    starData.Add(star);
                }
            }
        }
    }

    // 星座データの収集
    ConstellationData CollectConstellationData(ConstellationNameData name)
    {
        var data = new ConstellationData();

        // 星座の名前登録
        data.Name = name;

        // 星座IDが同じものを登録
        data.Position = constellationPositionData.FirstOrDefault(s => name.Id == s.Id);

        // 星座の略称が同じものを登録
        data.Lines = constellationLineData.Where(s => name.Summary == s.Name).ToList();

        // 星座線が使用している星を登録
        data.Stars = new List<StarData>();
        foreach (var line in data.Lines)
        {
            var start = starData.FirstOrDefault(s => s.Hip == line.StartHip);
            data.Stars.Add(start);
            var end = starData.FirstOrDefault(s => s.Hip == line.EndHip);
            data.Stars.Add(end);

            // 星座で使用される星
            start.UseConstellation = end.UseConstellation = true;
        }

        return data;
    }

    // 星座の作成
    void CreateConstellation()
    {
        // 各星座を作成
        foreach (var data in constellationData)
        {
            var constellation = Instantiate(constellationPrefab);
            var drawConstellation = constellation.GetComponent<DrawConstellation>();

            drawConstellation.ConstellationData = data;

            // 自分の子供にする
            constellation.transform.SetParent(transform, false);
        }
    }
}