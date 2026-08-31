using System;
using System.Collections.Generic;

namespace Models.DBRealization.SchemaAnalysis;

/// <summary>
/// Таблицы RAODB, в которых ищем текстовые колонки для sizing BLOB→VARCHAR.
/// При добавлении нового DbSet со строковыми полями — дописать сюда.
/// </summary>
public static class RaodbTextColumnTables
{
    public static readonly IReadOnlyList<string> All =
    [
        "ReportCollection_DbSet",
        "notes",
        "form_10",
        "form_11",
        "form_12",
        "form_13",
        "form_14",
        "form_15",
        "form_16",
        "form_17",
        "form_18",
        "form_19",
        "form_20",
        "form_21",
        "form_22",
        "form_23",
        "form_24",
        "form_25",
        "form_26",
        "form_27",
        "form_28",
        "form_29",
        "form_210",
        "form_211",
        "form_212",
        "form_40",
        "form_41",
        "form_50",
        "form_51",
        "form_52",
        "form_53",
        "form_54",
        "form_55",
        "form_56",
        "form_57",
        "package_passport",
        "characteristic_package",
        "radionuclid",
        "storage_point",
        "license_info"
    ];

    public static readonly HashSet<string> Set =
        new(All, StringComparer.OrdinalIgnoreCase);
}
