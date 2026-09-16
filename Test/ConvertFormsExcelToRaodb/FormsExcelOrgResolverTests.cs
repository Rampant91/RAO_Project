using System;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelOrgResolverTests
{
    [Fact]
    public void ReportingKey_Uses_Row1_Okpo_When_Present()
    {
        var master = BuildMaster("10000", "AAAAAAAA", "10001", "BBBBBBBB");
        var (regNo, okpo) = FormsExcelOrgResolver.GetForm10ReportingRegNoOkpo(master);
        Assert.Equal("10001", regNo);
        Assert.Equal("BBBBBBBB", okpo);
    }

    [Fact]
    public void ReportingKey_Okpo_From_Row0_When_Row1_Okpo_Dash()
    {
        var master = BuildMaster("10000", "AAAAAAAA", "10001", "-");
        var (regNo, okpo) = FormsExcelOrgResolver.GetForm10ReportingRegNoOkpo(master);
        Assert.Equal("10001", regNo);
        Assert.Equal("AAAAAAAA", okpo);
    }

    [Fact]
    public void Lookup_Marks_Ambiguous_Duplicates()
    {
        var org1 = new Reports { Master = BuildMaster("10000", "AAAAAAAA", "", "") };
        var org2 = new Reports { Master = BuildMaster("10000", "AAAAAAAA", "", "") };

        var lookup = FormsExcelOrgResolver.BuildLookup([org1, org2]);
        var status = lookup.TryResolve("10000", "AAAAAAAA", out _);
        Assert.Equal(FormsExcelOrgResolveStatus.Ambiguous, status);
    }

    [Fact]
    public void ReportingKey_Uses_Canonical_Pair_When_Duplicate_Orders()
    {
        var master = new Report { FormNum_DB = "1.0" };
        master.Rows10.Add(CreateRow(order: 1, id: 200, regNo: "", okpo: "", jurLico: ""));
        master.Rows10.Add(CreateRow(order: 1, id: 100, regNo: "52084", okpo: "36747513", jurLico: "ООО"));
        master.Rows10.Add(CreateRow(order: 2, id: 101, regNo: "", okpo: "", jurLico: ""));
        master.Rows10.Add(CreateRow(order: 2, id: 201, regNo: "", okpo: "", jurLico: ""));

        var (regNo, okpo) = FormsExcelOrgResolver.GetForm10ReportingRegNoOkpo(master);
        Assert.Equal("52084", regNo);
        Assert.Equal("36747513", okpo);
    }

    private static Report BuildMaster(string reg0, string okpo0, string reg1, string okpo1)
    {
        var master = new Report { FormNum_DB = "1.0" };
        master.Rows10.Add(CreateRow(order: 1, id: 1, regNo: reg0, okpo: okpo0, jurLico: ""));
        master.Rows10.Add(CreateRow(order: 2, id: 2, regNo: reg1, okpo: okpo1, jurLico: ""));
        return master;
    }

    private static Form10 CreateRow(int order, int id, string regNo, string okpo, string jurLico)
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
