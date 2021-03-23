using System;
using Models.Client_Model;
using Models.Attributes;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage
{
    public class Excel : SavingEngine
    {
        public async Task Save(LocalDictionary dictionary, string Path)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "RAO_APP";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                foreach (var lst in dictionary.Forms)
                {
                    if (lst.Value.Storage.Count > 0)
                    {
                        var type = lst.Value.Storage[0].GetType();
                        var props = type.GetProperties();
                        if (props.Length > 0)
                        {
                            var attrs = props.OrderBy(x => x.Name);
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Форма:" + type.Name.Replace("Form", ""));

                            int cnt1 = 1;
                            foreach (var attr in attrs)
                            {

                                var val = attr.GetCustomAttributes(false);
                                if (val.Length > 0)
                                {
                                    var tr = ((FormVisualAttribute)val.First()).Name;
                                    worksheet.Cells[1, cnt1].Value = tr;
                                    cnt1++;
                                }
                            }

                            int ro = 2;
                            foreach (var item in lst.Value.Storage)
                            {
                                int cl = 1;
                                foreach (var attr in attrs)
                                {
                                    var va = attr.GetCustomAttributes(false);
                                    if (va.Length > 0)
                                    {
                                        var val = attr.GetValue(item);
                                        if (val.GetType() == typeof(string))
                                        {
                                            worksheet.Cells[ro, cl].Value = val;
                                            cl++;
                                        }
                                        else
                                        {
                                            var t = val.ToString();
                                            worksheet.Cells[ro, cl].Value = t;
                                            cl++;
                                        }
                                    }
                                }
                                ro++;
                            }
                        }
                    }
                }
                if (excelPackage.Workbook.Worksheets.Count > 0)
                {
                    //Save your file
                    FileInfo fi = new FileInfo(Path);
                    excelPackage.SaveAs(fi);
                }
            }
        }
        public async Task<LocalDictionary> Load(string Path)
        {
            return new LocalDictionary();
        }
    }
}
