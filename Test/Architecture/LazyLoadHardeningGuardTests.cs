using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Регрессии lazy-load: импорт и паспорта не должны опираться на paged Report_Collection.
/// </summary>
public class LazyLoadHardeningGuardTests
{
    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "RAO_Project.sln"))
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return dir!.FullName;
        }
    }

    private static string ReadClient(params string[] relative)
    {
        var path = Path.Combine(new[] { RepoRoot, "Client_App" }.Concat(relative).ToArray());
        Assert.True(File.Exists(path), $"Missing {path}");
        return File.ReadAllText(path);
    }

    [Fact]
    public void ImportBase_LoadsOrgShellsBeforeMerge()
    {
        var src = ReadClient("Commands", "AsyncCommands", "Import", "ImportBaseAsyncCommand.cs");
        Assert.Contains("GetBaseOrgReportShellsAsync", src);
        Assert.Contains("LoadOrgWithReportShellsAsync", src);
        Assert.Contains("InvalidateMainWindowCachesAfterImport", src);
    }

    [Fact]
    public void PassportFill_EnsuresAllRowsBeforeFill()
    {
        var src = ReadClient("Commands", "AsyncCommands", "PassportFill", "PassportFillBaseCommand.cs");
        Assert.Contains("EnsureRowsAndTitleAsync", src);
        Assert.Contains("EnsureAllRowsLoadedAsync", src);
    }

    [Fact]
    public void SyncFormMutations_DoNotLoadFullSetOrDeadlockOverlay()
    {
        var sort = ReadClient("Commands", "SyncCommands", "NewSortFormSyncCommand.cs");
        var setOrder = ReadClient("Commands", "SyncCommands", "NewSetNumberOrderSyncCommand.cs");
        var baseForm = ReadClient("ViewModels", "Forms", "BaseFormVM.cs");

        Assert.DoesNotContain("EnsureAllRowsForMutationBlocking", sort);
        Assert.DoesNotContain("EnsureAllRowsForMutationBlocking", setOrder);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync().GetAwaiter().GetResult()", sort);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync().GetAwaiter().GetResult()", setOrder);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", sort);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", setOrder);
        Assert.Contains("RefreshVisibleRowsAsync", baseForm);
        Assert.Contains("SetDisplayOrder", baseForm);
    }

    [Fact]
    public void DeleteAddPaste_DoNotLoadFullSet_KeepFormListInPlace()
    {
        var del = ReadClient("Commands", "AsyncCommands", "Delete", "NewDeleteRowsAsyncCommand.cs");
        var mutation = ReadClient("Services", "DataAccess", "FormRowMutationService.cs");
        var baseForm = ReadClient("ViewModels", "Forms", "BaseFormVM.cs");
        var add = ReadClient("Commands", "AsyncCommands", "Add", "NewAddRowAsyncCommand.cs");
        var insert = ReadClient("Commands", "AsyncCommands", "Add", "NewAddRowsInAsyncCommand.cs");
        var paste = ReadClient("Commands", "AsyncCommands", "NewPasteRowsAsyncCommand.cs");
        var src = ReadClient("Commands", "AsyncCommands", "SourceTransmission", "SourceTransmissionAsyncCommand.cs");
        var srcAll = ReadClient("Commands", "AsyncCommands", "SourceTransmission", "SourceTransmissionAllAsyncCommand.cs");
        var save = ReadClient("Commands", "AsyncCommands", "Save", "SaveReportAsyncCommand.cs");
        var changeOrCreate = ReadClient("ViewModels", "ChangeOrCreateVM.cs");

        Assert.DoesNotContain("EnsureAllRowsForMutationAsync", del);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", del);
        Assert.Contains("RemoveFormRowById", del);
        Assert.Contains("FilterLiveForms", del);
        Assert.Contains("ClearFormRowSelection", del);
        Assert.DoesNotContain("RenumberFormRowsFromOne", del);
        Assert.DoesNotContain("SortForm.Execute", del);
        Assert.Contains("BuildLiveSlots", mutation);
        Assert.Contains("MergeSessionRows", mutation);
        Assert.DoesNotContain("EnsureAllRowsLoadedAsync", baseForm);
        Assert.Contains("_formList.Clear()", baseForm);
        Assert.DoesNotContain("FormList = new ObservableCollection<Form>", baseForm);
        Assert.Contains("DiscardUnsavedChangesAsync", baseForm);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync", add);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", add);
        Assert.DoesNotContain("SaveReportAsyncCommand", add);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync", insert);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", insert);
        Assert.Contains("InsertBeforeId", insert);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync", paste);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", src);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync", src);
        Assert.DoesNotContain("EnsureAllRowsForMutationAsync", srcAll);
        Assert.DoesNotContain("LoadFullSetIntoReportAsync", srcAll);
        Assert.Contains("CompactForm1xNumbersAsync", save);
        Assert.Contains("BeginTransactionAsync", save);
        Assert.DoesNotContain("CompactForm1xNumbersAsync", changeOrCreate);
        Assert.DoesNotContain("FormRowNumberCompact", changeOrCreate);
    }

    [Fact]
    public void SaveReport_RefreshesRowCountWithoutFullOrgReload()
    {
        var save = ReadClient("Commands", "AsyncCommands", "Save", "SaveReportAsyncCommand.cs");
        Assert.Contains("RefreshAfterFormReportSaved", save);
        Assert.DoesNotContain("UpdateReportsCollectionWithoutReCreation()", save);
    }

    [Fact]
    public void Forms4And5_UsePersistentWarmCacheCollections()
    {
        var forms4 = ReadClient("ViewModels", "MainWindowTabs", "Forms4TabControlVM.cs");
        var forms5 = ReadClient("ViewModels", "MainWindowTabs", "Forms5TabControlVM.cs");
        var tabBase = ReadClient("ViewModels", "MainWindowTabs", "FormsTabControlBaseVM.cs");
        Assert.Contains("Forms1WarmCache", tabBase);
        Assert.Contains("UpdateReportCollection", forms4);
        Assert.Contains("UpdateReportCollection", forms5);
        Assert.Contains("_cache", forms4);
        Assert.Contains("_cache", forms5);
        Assert.Contains("WarmSelectedOrgAndPrefetchReports", tabBase);
        Assert.Contains("RefreshOrgListAfterTitleChange", tabBase);
        Assert.DoesNotContain("HasFormNumAsync", forms4);
        Assert.DoesNotContain("HasFormNumAsync", forms5);
        Assert.DoesNotContain(".GetResult()", forms4);
        Assert.DoesNotContain(".GetResult()", forms5);
    }

    [Fact]
    public void Initialization_PrefetchesTabsAndSkipsTitlePreload()
    {
        var src = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("PrefetchTabs", src);
        Assert.DoesNotContain("await dbm.form_20.LoadAsync()", src);
        Assert.DoesNotContain("await dbm.form_40.LoadAsync()", src);
        Assert.DoesNotContain("await dbm.form_50.LoadAsync()", src);
        Assert.Contains("masterIdsWithForm20", src);
        Assert.Contains("zeroOrderNotes", src);
    }

    [Fact]
    public void Initialization_ActivatesFirstTabAfterPrefetch()
    {
        var src = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("Forms1TabControlVM.ActivateTab()", src);
        Assert.Contains("MainWindowPagingDefaults", src);
        Assert.DoesNotContain("ScheduleWarmInactiveTabs", src);
    }

    [Fact]
    public void CleanUpMasterRep_LoadsFormTablesDirectly()
    {
        var src = ReadClient("Services", "DataAccess", "TitleRowSanitizer.cs");
        Assert.Contains("db.form_10", src);
        Assert.Contains("db.form_20", src);
        Assert.Contains("AsNoTracking()", src);
        Assert.Contains("FormNum_DB == \"1.0\"", src);
        Assert.Contains("FormNum_DB == \"2.0\"", src);
        Assert.Contains("AttachForm10Updates", src);
    }

    [Fact]
    public void Initialization_SchedulesBackgroundTitleCleanup()
    {
        var src = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("ScheduleBackgroundTitleCleanup", src);
        Assert.Contains("TitleRowSanitizer.CleanUpAsync", src);
        Assert.Contains("RefreshAllTabsAfterTitleSanitizer", src);
        Assert.Contains("ResortLocalReportsCollection", src);
        Assert.DoesNotContain("await CleanUpMasterRep", src);
        Assert.DoesNotContain("LoadStatus = \"Очистка\"", src);
    }

    [Fact]
    public void Initialization_StartupSafeOptimizations()
    {
        var init = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("EnsureDatabaseBackupScheduleInitialized", init);
        Assert.Contains("ShowDatabaseBackupPromptIfDueAsync", init);
        Assert.DoesNotContain("await ProcessDataBaseBackup", init);
        Assert.Contains(".Include(r => r.Master_DB)", init);
        Assert.Contains("ChangeTracker.HasChanges()", init);
        Assert.Contains("LoadExistingTitleMasterIds", init);

        var startVm = ReadClient("ViewModels", "OnStartProgressBarVM.cs");
        Assert.Contains("ShowDatabaseBackupPromptIfDueAsync", startVm);
    }

    [Fact]
    public void StartupKostylSteps_LogErrorsAndContinue()
    {
        var src = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("LogStartupKostylError(\"Сортировка организаций\"", src);
        Assert.Contains("LogStartupKostylError(\"Сортировка примечаний\"", src);
        Assert.Contains("ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase)", src);
    }

    [Fact]
    public void ImportBase_DoesNotDoubleInvalidateOrgKeys()
    {
        var src = ReadClient("Commands", "AsyncCommands", "Import", "ImportBaseAsyncCommand.cs");
        Assert.Contains("InvalidateMainWindowCachesAfterImport", src);
        Assert.DoesNotContain("InvalidateAllOrgKeysCaches()", src);
        Assert.DoesNotContain("InvalidateOrgKeysCachesForm12()", src);
    }

    [Fact]
    public void Initialization_ResortUsesDbTitleKeysNotRegNoRep()
    {
        var init = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("GetForm20DisplayKeys", init);
        Assert.Contains("form20Keys", init);
        Assert.Contains("formNum == \"2.0\" && form20Keys.TryGetValue", init);
    }

    [Fact]
    public void MainWindowListQuery_ExposesForm20DisplayKeys()
    {
        var src = ReadClient("Services", "DataAccess", "MainWindowListQuery.cs");
        Assert.Contains("GetForm20DisplayKeys", src);
    }

    [Fact]
    public void TabSwitch_UsesActivateTabNotSyncOrgsReload()
    {
        var src = ReadClient("ViewModels", "MainWindowVM.cs");
        Assert.Contains("ActivateTab()", src);
        Assert.Contains("ScheduleWarmInactiveTabs", src);

        var setterStart = src.IndexOf("public byte SelectedReportType", StringComparison.Ordinal);
        Assert.True(setterStart >= 0);
        var nextRegion = src.IndexOf("#endregion", setterStart, StringComparison.Ordinal);
        Assert.True(nextRegion > setterStart);
        var setterBlock = src.Substring(setterStart, nextRegion - setterStart);
        Assert.DoesNotContain("UpdateOrgsPageInfo()", setterBlock);
        Assert.DoesNotContain("UpdateReportsCollection()", setterBlock);
    }
}
