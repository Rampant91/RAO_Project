using System;
using System.Linq;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelMasterCloneTests
{
    [Fact]
    public void Form10_Copies_Fields_Without_Sharing_Instance()
    {
        var source = BuildForm10Master("10000", "AAAAAAAA", "-", "-");
        source.Rows10.First().ShortJurLico_DB = "Тест";
        var clone = FormsExcelMasterClone.CloneForm10Master(source);
        Assert.Equal(2, clone.Rows10.Count);
        Assert.Equal("Тест", clone.Rows10.First().ShortJurLico_DB);
        clone.Rows10.First().ShortJurLico_DB = "Changed";
        Assert.Equal("Тест", source.Rows10.First().ShortJurLico_DB);
    }

    [Fact]
    public void Form10_Collapses_Duplicate_Orders_Keeping_Filled_Legal_In_Slot0()
    {
        var source = new Report { FormNum_DB = "1.0" };
        var emptyLegal = CreateForm10Row(order: 1, id: 200, regNo: "", okpo: "", jurLico: "");
        var filledLegal = CreateForm10Row(order: 1, id: 100, regNo: "52084", okpo: "36747513", jurLico: "ООО Тест");
        filledLegal.OrganUprav_DB = "Орган";
        source.Rows10.Add(emptyLegal);
        source.Rows10.Add(filledLegal);
        source.Rows10.Add(CreateForm10Row(order: 2, id: 101, regNo: "", okpo: "", jurLico: ""));
        source.Rows10.Add(CreateForm10Row(order: 2, id: 201, regNo: "", okpo: "", jurLico: ""));

        var clone = FormsExcelMasterClone.CloneForm10Master(source, out var warnings);

        Assert.Equal(2, clone.Rows10.Count);
        Assert.Equal("ООО Тест", clone.Rows10[0].JurLico_DB);
        Assert.Equal("52084", clone.Rows10[0].RegNo_DB);
        Assert.Empty(warnings);
    }

    [Fact]
    public void Form10_Keeps_Division_Data_In_Slot1_Independently()
    {
        var source = new Report { FormNum_DB = "1.0" };
        source.Rows10.Add(CreateForm10Row(order: 1, id: 1, regNo: "", okpo: "", jurLico: ""));
        source.Rows10.Add(CreateForm10Row(order: 2, id: 2, regNo: "10001", okpo: "BBBBBBBB", jurLico: "Филиал"));

        var clone = FormsExcelMasterClone.CloneForm10Master(source);

        Assert.Equal("Филиал", clone.Rows10[1].JurLico_DB);
        Assert.True(string.IsNullOrEmpty(clone.Rows10[0].JurLico_DB));
    }

    [Fact]
    public void Form10_Synthesizes_Missing_Order_Counterpart()
    {
        var source = new Report { FormNum_DB = "1.0" };
        source.Rows10.Add(CreateForm10Row(order: 1, id: 5, regNo: "10000", okpo: "AAAAAAAA", jurLico: "Юрлицо"));

        var clone = FormsExcelMasterClone.CloneForm10Master(source);

        Assert.Equal(2, clone.Rows10.Count);
        Assert.Equal("Юрлицо", clone.Rows10[0].JurLico_DB);
        Assert.Equal(2, clone.Rows10[1].NumberInOrder_DB);
    }

    [Fact]
    public void Form10_Warns_When_Two_Filled_Same_Order()
    {
        var source = new Report { FormNum_DB = "1.0" };
        var poorer = CreateForm10Row(order: 1, id: 10, regNo: "10000", okpo: "AAAAAAAA", jurLico: "А");
        var richer = CreateForm10Row(order: 1, id: 20, regNo: "10000", okpo: "AAAAAAAA", jurLico: "Б");
        richer.OrganUprav_DB = "Орган";
        source.Rows10.Add(poorer);
        source.Rows10.Add(richer);
        source.Rows10.Add(CreateForm10Row(order: 2, id: 11, regNo: "", okpo: "", jurLico: ""));

        var clone = FormsExcelMasterClone.CloneForm10Master(source, out var warnings);

        Assert.Equal("Б", clone.Rows10[0].JurLico_DB);
        Assert.Contains(warnings, w => w.Contains("NumberInOrder=1", StringComparison.Ordinal));
    }

    [Fact]
    public void Form20_Collapses_Duplicate_Orders()
    {
        var source = new Report { FormNum_DB = "2.0" };
        var empty = (Form20)FormCreator.Create("2.0");
        empty.Id = 200;
        empty.NumberInOrder_DB = 1;
        var filled = (Form20)FormCreator.Create("2.0");
        filled.Id = 100;
        filled.NumberInOrder_DB = 1;
        filled.RegNo_DB = "52084";
        filled.Okpo_DB = "36747513";
        filled.JurLico_DB = "ООО";
        var div = (Form20)FormCreator.Create("2.0");
        div.Id = 101;
        div.NumberInOrder_DB = 2;
        source.Rows20.Add(empty);
        source.Rows20.Add(filled);
        source.Rows20.Add(div);

        var clone = FormsExcelMasterClone.CloneForm20Master(source);
        Assert.Equal(2, clone.Rows20.Count);
        Assert.Equal("ООО", clone.Rows20[0].JurLico_DB);
        Assert.Equal(1, clone.Rows20[0].NumberInOrder_DB);
        Assert.Equal(2, clone.Rows20[1].NumberInOrder_DB);
    }

    private static Report BuildForm10Master(string reg0, string okpo0, string reg1, string okpo1)
    {
        var master = new Report { FormNum_DB = "1.0" };
        master.Rows10.Add(CreateForm10Row(order: 1, id: 1, regNo: reg0, okpo: okpo0, jurLico: ""));
        master.Rows10.Add(CreateForm10Row(order: 2, id: 2, regNo: reg1, okpo: okpo1, jurLico: ""));
        return master;
    }

    private static Form10 CreateForm10Row(int order, int id, string regNo, string okpo, string jurLico)
    {
        var row = (Form10)FormCreator.Create("1.0");
        row.Id = id;
        row.NumberInOrder_DB = order;
        row.RegNo_DB = regNo;
        row.Okpo_DB = okpo;
        row.JurLico_DB = jurLico;
        return row;
    }
}
