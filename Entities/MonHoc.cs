public class MonHoc
{
  public int Id { get; set; }
  public string? MaMon { get; set; }
  public string? MaLopHocPhan { get; set; }
  public string? TenMonHoc { get; set; }
  public string? STC { get; set; }
  public string? LoaiHocPhan { get; set; }
  public string? ThongTin { get; set; }
  public string? GiangVien { get; set; }
  public string? GioiHan { get; set; }
  public string? NgayBatDau { get; set; }
  public string? NgayKetThuc { get; set; }

  public MonHoc()
  {

  }

  public override string ToString()
  {
    return $"{MaMon} - {MaLopHocPhan} - {TenMonHoc} - {STC} - {LoaiHocPhan} - {ThongTin} - {GiangVien} - {GioiHan} - {NgayBatDau} - {NgayKetThuc}";
  }
}