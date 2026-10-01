using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spravochniki;
using Xunit;

namespace Test.Oksm;

/// <summary>
/// Единый ОКСМ: актуальные имена для UI, исторические — в проверках;
/// выпадающий список — срез на дату операции.
/// </summary>
public sealed class OksmCatalogTests
{
    public OksmCatalogTests()
    {
        Spravochniks.ResetOksmCatalogForTests();
    }

    [Fact]
    public void InMemoryCatalog_CurrentNames_ForUi_LegacyAcceptedInChecks()
    {
        var catalog = new OksmCatalog(
            [
                new OksmCountryEntry { Kod = "528", ShortName = "НИДЕРЛАНДЫ, КОРОЛЕВСТВО" },
                new OksmCountryEntry { Kod = "826", ShortName = "СОЕДИНЕННОЕ КОРОЛЕВСТВО ВЕЛИКОБРИТАНИИ И СЕВЕРНОЙ ИРЛАНДИИ" },
                new OksmCountryEntry { Kod = "862", ShortName = "ВЕНЕСУЭЛА, БОЛИВАРИАНСКАЯ РЕСПУБЛИКА" },
                new OksmCountryEntry { Kod = "248", ShortName = "АЛАНДСКИЕ ОСТРОВА" },
                new OksmCountryEntry { Kod = "643", ShortName = "РОССИЯ" }
            ],
            [
                "НИДЕРЛАНДЫ",
                "СОЕДИНЕННОЕ КОРОЛЕВСТВО",
                "ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)",
                "ЭЛАНДСКИЕ ОСТРОВА"
            ],
            [
                new OksmShortNamePeriod { Kod = "528", ShortName = "НИДЕРЛАНДЫ", ValidTo = new DateOnly(2023, 10, 1) },
                new OksmShortNamePeriod { Kod = "826", ShortName = "СОЕДИНЕННОЕ КОРОЛЕВСТВО", ValidTo = new DateOnly(2023, 5, 1) },
                new OksmShortNamePeriod { Kod = "862", ShortName = "ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)", ValidTo = new DateOnly(2024, 8, 1) },
                new OksmShortNamePeriod { Kod = "248", ShortName = "ЭЛАНДСКИЕ ОСТРОВА", ValidTo = new DateOnly(2023, 5, 1) }
            ]);

        Assert.True(catalog.IsCurrentShortName("НИДЕРЛАНДЫ, КОРОЛЕВСТВО"));
        Assert.False(catalog.IsCurrentShortName("НИДЕРЛАНДЫ"));

        Assert.True(catalog.IsAcceptedShortName("НИДЕРЛАНДЫ, КОРОЛЕВСТВО"));
        Assert.True(catalog.IsAcceptedShortName("нидерланды"));
        Assert.True(catalog.IsAcceptedShortName(" СОЕДИНЕННОЕ КОРОЛЕВСТВО "));
        Assert.True(catalog.IsAcceptedShortName("ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)"));
        Assert.True(catalog.IsAcceptedShortName("ЭЛАНДСКИЕ ОСТРОВА"));
        Assert.True(catalog.IsAcceptedShortName("АЛАНДСКИЕ ОСТРОВА"));

        Assert.False(catalog.IsAcceptedShortName(""));
        Assert.False(catalog.IsAcceptedShortName(null));
        Assert.False(catalog.IsAcceptedShortName("НЕСУЩЕСТВУЮЩАЯ СТРАНА"));

        Assert.Equal(5, catalog.CurrentCount);
        Assert.Equal(9, catalog.AcceptedShortNameCount);
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", catalog.CurrentKodToShortName["528"]);
    }

    [Fact]
    public void AsOf_UsesHistoricalShortName_BeforeChangeEffectiveDate()
    {
        var catalog = new OksmCatalog(
            [
                new OksmCountryEntry { Kod = "528", ShortName = "НИДЕРЛАНДЫ, КОРОЛЕВСТВО" },
                new OksmCountryEntry { Kod = "826", ShortName = "СОЕДИНЕННОЕ КОРОЛЕВСТВО ВЕЛИКОБРИТАНИИ И СЕВЕРНОЙ ИРЛАНДИИ" },
                new OksmCountryEntry { Kod = "862", ShortName = "ВЕНЕСУЭЛА, БОЛИВАРИАНСКАЯ РЕСПУБЛИКА" },
                new OksmCountryEntry { Kod = "643", ShortName = "РОССИЯ" }
            ],
            legacyShortNames: null,
            periods:
            [
                new OksmShortNamePeriod { Kod = "528", ShortName = "НИДЕРЛАНДЫ", ValidTo = new DateOnly(2023, 10, 1) },
                new OksmShortNamePeriod { Kod = "826", ShortName = "СОЕДИНЕННОЕ КОРОЛЕВСТВО", ValidTo = new DateOnly(2023, 5, 1) },
                new OksmShortNamePeriod { Kod = "862", ShortName = "ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)", ValidTo = new DateOnly(2024, 8, 1) }
            ]);

        var beforeNl = catalog.GetKodToShortNameAsOf(new DateOnly(2023, 9, 30));
        Assert.Equal("НИДЕРЛАНДЫ", beforeNl["528"]);
        Assert.Equal("СОЕДИНЕННОЕ КОРОЛЕВСТВО ВЕЛИКОБРИТАНИИ И СЕВЕРНОЙ ИРЛАНДИИ", beforeNl["826"]);

        var onNlChange = catalog.GetKodToShortNameAsOf(new DateOnly(2023, 10, 1));
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", onNlChange["528"]);

        var beforeUk = catalog.GetKodToShortNameAsOf(new DateOnly(2023, 4, 30));
        Assert.Equal("СОЕДИНЕННОЕ КОРОЛЕВСТВО", beforeUk["826"]);

        var beforeVe = catalog.GetKodToShortNameAsOf(new DateOnly(2024, 7, 31));
        Assert.Equal("ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)", beforeVe["862"]);

        var afterVe = catalog.GetKodToShortNameAsOf(new DateOnly(2024, 8, 1));
        Assert.Equal("ВЕНЕСУЭЛА, БОЛИВАРИАНСКАЯ РЕСПУБЛИКА", afterVe["862"]);

        Assert.Same(catalog.CurrentKodToShortName, catalog.GetKodToShortNameAsOf(null));
    }

    [Fact]
    public void AsOf_HidesMembershipUntilValidFrom()
    {
        var catalog = new OksmCatalog(
            [
                new OksmCountryEntry { Kod = "643", ShortName = "РОССИЯ" },
                new OksmCountryEntry { Kod = "897", ShortName = "ДНР" },
                new OksmCountryEntry { Kod = "898", ShortName = "ЛНР" }
            ],
            periods:
            [
                new OksmShortNamePeriod { Kod = "897", ShortName = "ДНР", ValidFrom = new DateOnly(2022, 2, 25) },
                new OksmShortNamePeriod { Kod = "898", ShortName = "ЛНР", ValidFrom = new DateOnly(2022, 2, 25) }
            ]);

        var before = catalog.GetKodToShortNameAsOf(new DateOnly(2022, 2, 24));
        Assert.False(before.ContainsKey("897"));
        Assert.False(before.ContainsKey("898"));
        Assert.True(before.ContainsKey("643"));

        var onDate = catalog.GetKodToShortNameAsOf(new DateOnly(2022, 2, 25));
        Assert.Equal("ДНР", onDate["897"]);
        Assert.Equal("ЛНР", onDate["898"]);
    }

    [Fact]
    public void Spravochniks_GetOksmForOperationDate_ParsesStringOrFallsBackToCurrent()
    {
        var catalog = new OksmCatalog(
            [new OksmCountryEntry { Kod = "528", ShortName = "НИДЕРЛАНДЫ, КОРОЛЕВСТВО" }],
            periods: [new OksmShortNamePeriod { Kod = "528", ShortName = "НИДЕРЛАНДЫ", ValidTo = new DateOnly(2023, 10, 1) }]);

        Spravochniks.SetOksmCatalogForTests(catalog);

        Assert.Equal("НИДЕРЛАНДЫ", Spravochniks.GetOksmForOperationDate("15.09.2023")["528"]);
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", Spravochniks.GetOksmForOperationDate("01.10.2023")["528"]);
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", Spravochniks.GetOksmForOperationDate("")["528"]);
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", Spravochniks.GetOksmForOperationDate(null)["528"]);
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", Spravochniks.GetOksmForOperationDate("не дата")["528"]);
    }

    [Fact]
    public void Spravochniks_UsesInjectedCatalog_ForAcceptedAndCurrent()
    {
        var catalog = new OksmCatalog(
            [new OksmCountryEntry { Kod = "528", ShortName = "НИДЕРЛАНДЫ, КОРОЛЕВСТВО" }],
            ["НИДЕРЛАНДЫ"]);

        Spravochniks.SetOksmCatalogForTests(catalog);

        Assert.True(Spravochniks.IsCurrentOksmShortName("НИДЕРЛАНДЫ, КОРОЛЕВСТВО"));
        Assert.False(Spravochniks.IsCurrentOksmShortName("НИДЕРЛАНДЫ"));
        Assert.True(Spravochniks.IsAcceptedOksmShortName("НИДЕРЛАНДЫ"));
        Assert.True(Spravochniks.IsAcceptedOksmShortName("НИДЕРЛАНДЫ, КОРОЛЕВСТВО"));
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", Spravochniks.OKSM["528"]);
        Assert.DoesNotContain(Spravochniks.OKSM.Values, v => v == "НИДЕРЛАНДЫ");
    }

    [Fact]
    public void LoadFromRepoFile_ContainsCurrentLegacyAndDatedPeriods()
    {
        var path = OksmCatalog.ResolveDefaultFilePath();
        Assert.True(File.Exists(path), "Ожидался data/Spravochniki/oksm.xlsx в репозитории");

        var catalog = OksmCatalog.LoadFromFile(path!);

        Assert.True(catalog.CurrentCount >= 250);
        Assert.True(catalog.IsCurrentShortName("НИДЕРЛАНДЫ, КОРОЛЕВСТВО"));
        Assert.True(catalog.IsCurrentShortName("СОЕДИНЕННОЕ КОРОЛЕВСТВО ВЕЛИКОБРИТАНИИ И СЕВЕРНОЙ ИРЛАНДИИ"));
        Assert.True(catalog.IsCurrentShortName("ВЕНЕСУЭЛА, БОЛИВАРИАНСКАЯ РЕСПУБЛИКА"));
        Assert.True(catalog.IsCurrentShortName("АЛАНДСКИЕ ОСТРОВА"));
        Assert.True(catalog.IsCurrentShortName("ДНР"));
        Assert.True(catalog.IsCurrentShortName("ЛНР"));

        Assert.False(catalog.IsCurrentShortName("НИДЕРЛАНДЫ"));
        Assert.True(catalog.IsAcceptedShortName("НИДЕРЛАНДЫ"));
        Assert.True(catalog.IsAcceptedShortName("СОЕДИНЕННОЕ КОРОЛЕВСТВО"));
        Assert.True(catalog.IsAcceptedShortName("ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)"));
        Assert.True(catalog.IsAcceptedShortName("ЭЛАНДСКИЕ ОСТРОВА"));

        Assert.True(catalog.AcceptedShortNameCount > catalog.CurrentCount);
        Assert.True(catalog.Periods.Count >= 4);

        var mid2022 = catalog.GetKodToShortNameAsOf(new DateOnly(2022, 6, 1));
        Assert.Equal("НИДЕРЛАНДЫ", mid2022["528"]);
        Assert.Equal("СОЕДИНЕННОЕ КОРОЛЕВСТВО", mid2022["826"]);
        Assert.Equal("ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)", mid2022["862"]);
        Assert.Equal("ЭЛАНДСКИЕ ОСТРОВА", mid2022["248"]);
        Assert.Equal("ДНР", mid2022["897"]);

        var early2022 = catalog.GetKodToShortNameAsOf(new DateOnly(2022, 2, 1));
        Assert.False(early2022.ContainsKey("897"));
        Assert.False(early2022.ContainsKey("898"));

        var now = catalog.GetKodToShortNameAsOf(new DateOnly(2025, 1, 1));
        Assert.Equal("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", now["528"]);
        Assert.Equal("ВЕНЕСУЭЛА, БОЛИВАРИАНСКАЯ РЕСПУБЛИКА", now["862"]);
    }

    [Fact]
    public void ResolveDefaultFilePath_PrefersRepoCopyOverBuildOutput()
    {
        var path = OksmCatalog.ResolveDefaultFilePath();
        Assert.NotNull(path);
        Assert.True(File.Exists(path));

        var normalized = path!.Replace('/', '\\');
        Assert.False(
            normalized.Contains("\\bin\\", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("\\obj\\", StringComparison.OrdinalIgnoreCase),
            $"Ожидался oksm.xlsx из репозитория, а не из bin/obj: {path}");

        var catalog = OksmCatalog.LoadFromFile(path);
        Assert.Equal(
            "НИДЕРЛАНДЫ, КОРОЛЕВСТВО",
            catalog.GetKodToShortNameAsOf(new DateOnly(2026, 1, 1))["528"]);
    }

    [Fact]
    public void DropdownSnapshot_WithoutDate_DoesNotExposeLegacyOnlyNames()
    {
        var path = OksmCatalog.ResolveDefaultFilePath();
        Assert.NotNull(path);
        Spravochniks.ResetOksmCatalogForTests();
        Spravochniks.SetOksmCatalogForTests(OksmCatalog.LoadFromFile(path!));

        var dropdownNames = Spravochniks.OKSM.Values.ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Contains("НИДЕРЛАНДЫ, КОРОЛЕВСТВО", dropdownNames);
        Assert.DoesNotContain("НИДЕРЛАНДЫ", dropdownNames);
        Assert.DoesNotContain("СОЕДИНЕННОЕ КОРОЛЕВСТВО", dropdownNames);
        Assert.DoesNotContain("ЭЛАНДСКИЕ ОСТРОВА", dropdownNames);
        Assert.DoesNotContain("ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)", dropdownNames);
    }
}
