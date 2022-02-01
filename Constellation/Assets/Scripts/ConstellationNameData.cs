public class ConstellationNameData : CsvData
{
    public int Id { get; set; }             // 星座ID
    public string Summary { get; set; }     // 略称
    public string Name { get; set; }        // 英語名
    public string JapaneseName { get; set; }// 日本語名

    public override void SetData(string[] data)
    {
        Id = int.Parse(data[0]);
        Summary = data[1];
        Name = data[2];
        JapaneseName = data[3];
    }
}