namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Построение ключей сопоставления по полям, переносимым при переводе РВ → РАО (без пересчёта).
/// </summary>
public static class Operation41PairingKeyMapping
{
    /// <summary>
    /// Основные радионуклиды в 1.6 при переводе из 1.2 (константа в SourceTransmissionBaseAsyncCommand).
    /// </summary>
    public const string Form12To16MainRadionuclids =
        "уран-238; торий-234; протактиний-234м; уран-234";

    public static Operation41PairingKey FromForm11(
        string opCode,
        string opDate,
        string pasNum,
        string facNum,
        string type,
        string radionuclids,
        string creationDate,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packNumber,
        int? quantity) =>
        new(
            opCode,
            opDate,
            pasNum,
            facNum,
            type,
            radionuclids,
            creationDate,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            quantity);

    /// <summary>Ключ строки 1.2 для сопоставления с 1.6.</summary>
    public static Operation41PairingKey FromForm12(
        string opCode,
        string opDate,
        string passportNumber,
        string factoryNumber,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packName,
        string packType,
        string packNumber) =>
        new(
            opCode,
            opDate,
            passportNumber,
            factoryNumber,
            string.Empty,
            string.Empty,
            string.Empty,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            packName,
            packType,
            Form12To16MainRadionuclids,
            string.Empty,
            string.Empty,
            opDate,
            1);

    /// <summary>Ключ строки 1.6 при сопоставлении с 1.2.</summary>
    public static Operation41PairingKey FromForm16For12(
        string opCode,
        string opDate,
        string mainRadionuclids,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packName,
        string packType,
        string packNumber,
        string activityMeasurementDate) =>
        new(
            opCode,
            opDate,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            packName,
            packType,
            mainRadionuclids,
            string.Empty,
            string.Empty,
            activityMeasurementDate,
            1);

    /// <summary>Ключ строки 1.3 для сопоставления с 1.6.</summary>
    public static Operation41PairingKey FromForm13(
        string opCode,
        string opDate,
        string passportNumber,
        string factoryNumber,
        string type,
        string radionuclids,
        string creationDate,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packName,
        string packType,
        string packNumber) =>
        new(
            opCode,
            opDate,
            passportNumber,
            factoryNumber,
            type,
            radionuclids,
            creationDate,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            packName,
            packType,
            radionuclids,
            string.Empty,
            string.Empty,
            creationDate,
            1);

    /// <summary>Ключ строки 1.6 при сопоставлении с 1.3.</summary>
    public static Operation41PairingKey FromForm16For13(
        string opCode,
        string opDate,
        string mainRadionuclids,
        string activityMeasurementDate,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packName,
        string packType,
        string packNumber) =>
        new(
            opCode,
            opDate,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            packName,
            packType,
            mainRadionuclids,
            string.Empty,
            string.Empty,
            activityMeasurementDate,
            1);

    /// <summary>Ключ строки 1.4 для сопоставления с 1.6.</summary>
    public static Operation41PairingKey FromForm14(
        string opCode,
        string opDate,
        string passportNumber,
        string radionuclids,
        string activityMeasurementDate,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packName,
        string packType,
        string packNumber) =>
        new(
            opCode,
            opDate,
            passportNumber,
            string.Empty,
            string.Empty,
            radionuclids,
            string.Empty,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            packName,
            packType,
            radionuclids,
            string.Empty,
            string.Empty,
            activityMeasurementDate,
            1);

    /// <summary>Ключ строки 1.6 при сопоставлении с 1.4.</summary>
    public static Operation41PairingKey FromForm16For14(
        string opCode,
        string opDate,
        string mainRadionuclids,
        string activityMeasurementDate,
        byte? documentVid,
        string documentNumber,
        string documentDate,
        string packName,
        string packType,
        string packNumber) =>
        new(
            opCode,
            opDate,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            Operation41PairingKeyComparer.NormalizeDocumentVid(documentVid),
            documentNumber,
            documentDate,
            packNumber,
            packName,
            packType,
            mainRadionuclids,
            string.Empty,
            string.Empty,
            activityMeasurementDate,
            1);
}
