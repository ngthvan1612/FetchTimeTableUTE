
Console.OutputEncoding = System.Text.Encoding.UTF8;
var fetchMonHoc = new FetchMonHoc();
var ds = fetchMonHoc.Run();

using (var dbs = new UTEDbContext())
{
  dbs.MonHocs.AddRange(ds);
  dbs.SaveChanges();
}
