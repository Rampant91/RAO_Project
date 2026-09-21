using System.Linq;
using Client_App.Helpers.MasterTitleRows;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Xunit;

namespace Test.DatabaseCleanup;

public sealed class MasterTitleRowsNormalizerTests
{
    [Fact]
    public void Form10_Collapses_Four_Rows_To_Canonical_Index_Order()
    {
        var master = new Report { FormNum_DB = "1.0" };
        // Сначала пустой order=1, потом заполненный — до normalize индексы «ломают» UI.
        master.Rows10.Add(CreateForm10(id: 2, order: 1, jurLico: ""));
        master.Rows10.Add(CreateForm10(id: 1, order: 1, jurLico: "ООО Тест", organ: "Орган"));
        master.Rows10.Add(CreateForm10(id: 4, order: 2, jurLico: ""));
        master.Rows10.Add(CreateForm10(id: 3, order: 2, jurLico: ""));

        MasterTitleRowsNormalizer.NormalizeForm10InPlace(master, out var warnings);

        Assert.Equal(2, master.Rows10.Count);
        Assert.Equal(1, master.Rows10[0].NumberInOrder_DB);
        Assert.Equal(2, master.Rows10[1].NumberInOrder_DB);
        Assert.Equal("ООО Тест", master.Rows10[0].JurLico_DB);
        Assert.Empty(warnings);
    }

    [Fact]
    public void Form10_When_Only_Division_Present_Synthesizes_Legal_At_Index0()
    {
        var master = new Report { FormNum_DB = "1.0" };
        master.Rows10.Add(CreateForm10(id: 5, order: 2, jurLico: "Филиал", okpo: "12345678"));

        MasterTitleRowsNormalizer.NormalizeForm10InPlace(master, out _);

        Assert.Equal(2, master.Rows10.Count);
        Assert.Equal(1, master.Rows10[0].NumberInOrder_DB);
        Assert.Equal(2, master.Rows10[1].NumberInOrder_DB);
        Assert.Equal("Филиал", master.Rows10[1].JurLico_DB);
        Assert.True(string.IsNullOrEmpty(master.Rows10[0].JurLico_DB)
                    || master.Rows10[0].JurLico_DB == "-");
    }

    [Fact]
    public void Form10_Two_Filled_Same_Order_Keeps_Richer_And_Warns()
    {
        var master = new Report { FormNum_DB = "1.0" };
        master.Rows10.Add(CreateForm10(id: 10, order: 1, jurLico: "А"));
        var richer = CreateForm10(id: 20, order: 1, jurLico: "Б", organ: "Орган");
        master.Rows10.Add(richer);
        master.Rows10.Add(CreateForm10(id: 11, order: 2, jurLico: ""));

        MasterTitleRowsNormalizer.NormalizeForm10InPlace(master, out var warnings);

        Assert.Equal(2, master.Rows10.Count);
        Assert.Equal("Б", master.Rows10[0].JurLico_DB);
        Assert.Contains(warnings, w => w.Contains("NumberInOrder=1"));
    }

    private static Form10 CreateForm10(int id, int order, string jurLico, string organ = "", string okpo = "")
    {
        var row = (Form10)FormCreator.Create("1.0");
        row.Id = id;
        row.NumberInOrder_DB = order;
        row.JurLico_DB = jurLico;
        row.OrganUprav_DB = organ;
        row.Okpo_DB = okpo;
        return row;
    }
}
