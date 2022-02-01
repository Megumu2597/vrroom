public class ConstellationLineData : CsvData
{
    public string Name { get; set; }    // 星座名
    public int StartHip { get; set; }   // 線分開始HIP番号
    public int EndHip { get; set; }     // 線分終了HIP番号

    public override void SetData(string[] data)
    {
        Name = data[0];
        StartHip = int.Parse(data[1]);
        EndHip = int.Parse(data[2]);
    }
}