using Models.DBRealization.SchemaAnalysis;
using Xunit;

namespace Test.SchemaAnalysis;

public class VarcharSizeProposalTests
{
    [Theory]
    [InlineData(0, 16)]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 8)]
    [InlineData(14, 14)]
    [InlineData(15, 16)]
    [InlineData(100, 128)]
    [InlineData(8192, 8192)]
    [InlineData(9000, 9000)]
    public void RoundUp_uses_steps(int max, int expected) =>
        Assert.Equal(expected, VarcharSizeProposal.RoundUp(max));

    [Fact]
    public void ApplyDomainFloor_okpo_and_formnum()
    {
        Assert.Equal(14, VarcharSizeProposal.ApplyDomainFloor("ProviderOrRecieverOKPO_DB", 8));
        Assert.Equal(8, VarcharSizeProposal.ApplyDomainFloor("FormNum_DB", 4));
        Assert.Equal(2, VarcharSizeProposal.ApplyDomainFloor("OperationCode_DB", 1));
    }

    [Fact]
    public void ShouldKeepBlob_only_when_exceeds_threshold()
    {
        Assert.False(VarcharSizeProposal.ShouldKeepBlob(4894, 8192));
        Assert.True(VarcharSizeProposal.ShouldKeepBlob(9000, 9000));
        Assert.False(VarcharSizeProposal.ShouldKeepBlob(32, 64));
    }

    [Fact]
    public void Apply_comment_and_note_columns_get_varchar_proposal()
    {
        var comment = new TextColumnDescriptor
        {
            Table = "notes",
            Column = "Comment_DB",
            StorageKind = TextColumnStorageKind.BlobText,
            MaxCharLength = 4894
        };
        var note = new TextColumnDescriptor
        {
            Table = "form_57",
            Column = "Note_DB",
            StorageKind = TextColumnStorageKind.BlobText,
            MaxCharLength = 70
        };

        VarcharSizeProposal.Apply([comment, note]);

        Assert.Equal(8192, comment.ProposedVarchar);
        Assert.False(comment.KeepBlob);
        Assert.Equal(128, note.ProposedVarchar);
        Assert.False(note.KeepBlob);
    }

    [Fact]
    public void Apply_marks_varchar_columns_without_keep_blob()
    {
        var column = new TextColumnDescriptor
        {
            Table = "form_50",
            Column = "Name_DB",
            StorageKind = TextColumnStorageKind.Varchar,
            DeclaredLength = 256,
            MaxCharLength = 120
        };

        VarcharSizeProposal.Apply([column]);

        Assert.False(column.KeepBlob);
        Assert.Equal(256, column.ProposedVarchar);
    }

    [Fact]
    public void Apply_blob_column_gets_proposal()
    {
        var column = new TextColumnDescriptor
        {
            Table = "form_10",
            Column = "RegNo_DB",
            StorageKind = TextColumnStorageKind.BlobText,
            MaxCharLength = 12
        };

        VarcharSizeProposal.Apply([column]);

        Assert.Equal(14, column.ProposedVarchar);
        Assert.False(column.KeepBlob);
        Assert.Equal(2, column.SuggestedWave);
    }

    [Fact]
    public void Merge_combines_database_and_model_rows()
    {
        var db = new TextColumnDescriptor
        {
            Table = "notes",
            Column = "Comment_DB",
            StorageKind = TextColumnStorageKind.BlobText
        };
        var model = new TextColumnDescriptor
        {
            Table = "notes",
            Column = "Comment_DB",
            EntityTypeName = "Models.Forms.Note",
            PropertyName = "Comment_DB"
        };

        var merged = TextColumnDescriptor.Merge([db], [model]);

        Assert.Single(merged);
        Assert.Equal("Models.Forms.Note", merged[0].EntityTypeName);
        Assert.Equal(TextColumnStorageKind.BlobText, merged[0].StorageKind);
    }
}
