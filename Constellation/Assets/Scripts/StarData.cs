public class StarData : CsvData
{
    public int Hip { get; set; }                    // HIP番号
    public float RightAscension { get; set; }       // 赤経
    public float Declination { get; set; }          // 赤緯
    public float ApparentMagnitude { get; set; }    // 視等級
    public string ColorType;                        // 色
    public bool UseConstellation;                   // 星座で使用される星か

    public override void SetData(string[] data)
    {
        Hip = int.Parse(data[0]);
        RightAscension = RightAscensionToDegree(int.Parse(data[1]), int.Parse(data[2]), float.Parse(data[3]));
        Declination = DeclinationToDegree(int.Parse(data[4]), int.Parse(data[5]), float.Parse(data[6]));
        ApparentMagnitude = float.Parse(data[7]);
        ColorType = data[13].Substring(0, 1);
    }
}