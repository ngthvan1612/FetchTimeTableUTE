using Flurl.Http;
using HtmlAgilityPack;
using System.Text.Encodings.Web;

public class FetchMonHoc
{
  private static string URL_TRA_CUU_HOC_PHAN_UTE = "https://dkmh.hcmute.edu.vn/TraCuuHocPhan/Index";

  public FetchMonHoc()
  {

  }

  private string fixHtmlText(string htmlText)
  {
    return System.Net.WebUtility.HtmlDecode(htmlText);
  }

  private async Task<IEnumerable<MonHoc>> fetchWithPrefix(string prefix)
  {
    List<MonHoc> dsMonHoc = new List<MonHoc>();
    string html = await URL_TRA_CUU_HOC_PHAN_UTE
      .PostUrlEncodedAsync(new
      {
        ddlMonHoc = 0,
        txtSearch = prefix,
        btntim = "Tìm kiếm"
      })
      .ReceiveString();
    var doc = new HtmlDocument();
    doc.LoadHtml(html);
    var selectedTables = doc.DocumentNode.SelectNodes("//table[@style='border-collapse: collapse; font-size: 12px']");
    if (selectedTables is null)
      throw new NullReferenceException();
    var mainTable = selectedTables[0];
    var rows = mainTable.SelectNodes("tr").Skip(1);
    foreach (var row in rows)
    {
      var cells = row.SelectNodes("td");
      MonHoc mh = new MonHoc()
      {
        MaMon = fixHtmlText(cells[0].InnerText).Trim(),
        MaLopHocPhan  = fixHtmlText(cells[1].InnerText).Trim(),
        TenMonHoc = fixHtmlText(cells[2].InnerText).Trim(),
        STC = fixHtmlText(cells[3].InnerText).Trim(),
        LoaiHocPhan = fixHtmlText(cells[4].InnerText).Trim(),
        ThongTin = fixHtmlText(cells[5].InnerText).Trim(),
        GiangVien = fixHtmlText(cells[6].InnerHtml).Trim(),
        GioiHan = fixHtmlText(cells[7].InnerText).Trim(),
        NgayBatDau = fixHtmlText(cells[7].InnerText).Trim(),
        NgayKetThuc = fixHtmlText(cells[8].InnerText).Trim()
      };
      //Console.WriteLine(mh);
      dsMonHoc.Add(mh);
    }
    return dsMonHoc;
  }

  private List<MonHoc> fetchAll()
  {
    List<MonHoc> result = new List<MonHoc>();
    Queue<string> queue = new Queue<string>();
    queue.Enqueue("");
    string edges = "";
    for (char c = 'A'; c <= 'Z'; ++c)
      edges += c;
    for (char c = 'a'; c <= 'z'; ++c)
      edges += c;
    for (char c = '0'; c <= '9'; ++c)
      edges += c; 
    while (queue.Count > 0)
    {
      string u = queue.Dequeue();
      List<Task<IEnumerable<MonHoc>>> current = new List<Task<IEnumerable<MonHoc>>>();
      for (int i = 0; i < edges.Length; ++i)
      {
        Console.WriteLine("FETCH: " + u + edges[i]);
        current.Add(this.fetchWithPrefix(u + edges[i]));
      }
      Task.WaitAll(current.ToArray());
      for (int i = 0; i < edges.Length; ++i)
      {
        string v = u + edges[i];
        IEnumerable<MonHoc> res = current[i].Result;
        result.AddRange(res);
        if (res.Count() >= 100)
        {
          queue.Enqueue(v);
        }
      }
    }
    return result;
  }

  private List<MonHoc> uniqueListMonHoc(List<MonHoc> src)
  {
    List<MonHoc> res = new List<MonHoc>();
    SortedSet<string> memo = new SortedSet<string>();
    foreach (MonHoc m in src)
    {
      if (m.MaLopHocPhan == null || m.MaLopHocPhan.Length == 0)
        throw new NullReferenceException();
      if (memo.Contains(m.MaLopHocPhan))
        continue;
      res.Add(m);
      memo.Add(m.MaLopHocPhan);
    }
    return res;
  }

  public List<MonHoc> Run()
  {
    var raw = this.fetchAll();
    var result = this.uniqueListMonHoc(raw);
    Console.WriteLine("Tổng cộng có " + result.Count + " môn!");
    return result;
  }
}
