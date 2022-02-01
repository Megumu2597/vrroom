using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvLoader<TCsvData> where TCsvData : CsvData, new()
{
    // TextAssetデータの読み込み
    public static List<TCsvData> LoadData(TextAsset csvText)
    {
        var data = new List<TCsvData>();                // リストの作成
        var reader = new StringReader(csvText.text);    // 文字列読み込み

        // 1行ずつデータの最後まで処理を行う
        while (reader.Peek() > -1)
        {
            // 1行読み込み
            var line = reader.ReadLine();
            // データ作成
            var csvData = new TCsvData();
            // ,で区切ったデータの配列を作成してデータを登録する
            csvData.SetData(line.Split(','));
            // リストに登録
            data.Add(csvData);
        }
        return data;
    }
}