using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace Models.DBRealization.SchemaAnalysis;

public static class EfModelTextColumnIndex
{
    public static IReadOnlyList<TextColumnDescriptor> Load(IModel model)
    {
        var result = new List<TextColumnDescriptor>();

        foreach (var entityType in model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName is null || !RaodbTextColumnTables.Set.Contains(tableName))
                continue;

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType != typeof(string))
                    continue;

                var columnName = property.GetColumnName();
                if (string.IsNullOrEmpty(columnName))
                    continue;

                result.Add(new TextColumnDescriptor
                {
                    Table = tableName,
                    Column = columnName,
                    EntityTypeName = entityType.ClrType.FullName,
                    PropertyName = property.Name
                });
            }
        }

        return result
            .OrderBy(x => x.Table, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.Column, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
