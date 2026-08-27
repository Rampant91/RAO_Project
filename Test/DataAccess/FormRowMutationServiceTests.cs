using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Services.DataAccess;
using Models.Forms;
using Models.Forms.Form1;
using Xunit;

namespace Test.DataAccess;

public class FormRowMutationServiceTests
{
    [Fact]
    public void MergeSessionRows_KeepsModified_SkipsDeleted_AppendsAdded()
    {
        var snap1 = new Form11 { Id = 1, NumberInOrder_DB = 1 };
        var snap2 = new Form11 { Id = 2, NumberInOrder_DB = 2 };
        var snap3 = new Form11 { Id = 3, NumberInOrder_DB = 3 };
        var edited = new Form11 { Id = 2, NumberInOrder_DB = 2 };
        var added = new Form11 { Id = 0, NumberInOrder_DB = 4 };

        var merged = FormRowMutationService.MergeSessionRows(
            [snap1, snap2, snap3],
            [added],
            new Dictionary<int, Form> { [2] = edited },
            excludeIds: new HashSet<int> { 1 });

        Assert.Equal(3, merged.Count);
        Assert.Same(edited, merged[0]);
        Assert.Same(snap3, merged[1]);
        Assert.Same(added, merged[2]);
        Assert.DoesNotContain(merged, f => f.Id == 1);
    }

    [Fact]
    public void MergeSessionRows_EmptySnapshot_OnlyAdded()
    {
        var added = new Form11 { Id = 0, NumberInOrder_DB = 1 };
        var merged = FormRowMutationService.MergeSessionRows(
            [],
            [added],
            new Dictionary<int, Form>(),
            new HashSet<int>());

        Assert.Single(merged);
        Assert.Same(added, merged[0]);
    }

    [Fact]
    public void EnumerateForms_AcceptsNonGenericList()
    {
        var a = new Form11 { Id = 1 };
        var b = new Form11 { Id = 2 };
        var listed = new System.Collections.ArrayList { a, "skip", b };

        var result = FormRowMutationService.EnumerateForms(listed);

        Assert.Equal(2, result.Length);
        Assert.Same(a, result[0]);
        Assert.Same(b, result[1]);
    }

    [Fact]
    public void EnumerateForms_NullOrEmpty_ReturnsEmpty()
    {
        Assert.Empty(FormRowMutationService.EnumerateForms(null));
        Assert.Empty(FormRowMutationService.EnumerateForms(Array.Empty<Form11>()));
    }

    [Fact]
    public void FilterLiveForms_MapsStalePageInstance_DropsAlreadyRemoved()
    {
        var live2 = new Form11 { Id = 2, NumberInOrder_DB = 1 };
        var live3 = new Form11 { Id = 3, NumberInOrder_DB = 2 };
        var staleDeleted = new Form11 { Id = 1, NumberInOrder_DB = 1 };
        var staleSameId = new Form11 { Id = 2, NumberInOrder_DB = 2 };
        var unsaved = new Form11 { Id = 0, NumberInOrder_DB = 3 };

        var live = FormRowMutationService.FilterLiveForms(
            [staleDeleted, staleSameId, unsaved],
            [live2, live3, unsaved]);

        Assert.Equal(2, live.Count);
        Assert.Same(live2, live[0]);
        Assert.Same(unsaved, live[1]);
    }

    [Fact]
    public void BuildLiveSlots_DropsDeleted_InsertsAddedBeforeTarget()
    {
        var before = new Form11 { Id = 0, InsertBeforeId = 3 };
        var slots = FormRowMutationService.BuildLiveSlots(
            [1, 2, 3, 4],
            [before],
            excludeIds: new HashSet<int> { 2 });

        Assert.Equal(4, slots.Count);
        Assert.Equal(1, slots[0].Id);
        Assert.Same(before, slots[1].Added);
        Assert.Equal(3, slots[2].Id);
        Assert.Equal(4, slots[3].Id);
    }

    [Fact]
    public void BuildLiveSlots_InsertBeforeUnsaved_UsesReference()
    {
        var unsaved = new Form11 { Id = 0 };
        var beforeUnsaved = new Form11 { Id = 0, InsertBeforeForm = unsaved };
        var slots = FormRowMutationService.BuildLiveSlots(
            [10],
            [unsaved, beforeUnsaved],
            new HashSet<int>());

        Assert.Equal(3, slots.Count);
        Assert.Equal(10, slots[0].Id);
        Assert.Same(beforeUnsaved, slots[1].Added);
        Assert.Same(unsaved, slots[2].Added);
    }

    [Fact]
    public void GetPendingDelta_AddedMinusDeleted()
    {
        Assert.Equal(99, FormRowNumberCompact.LiveCount(dbCount: 100, addedCount: 2, deletedCount: 3));
    }
}
