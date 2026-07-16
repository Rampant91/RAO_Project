namespace Client_App.Behaviors;

/// <summary>
/// Совместимость для форм 2.x/4.x/5.x (кастомная шапка) до их миграции на TableHeader namespace в XAML.
/// </summary>
public class ColumnWidthSyncBehavior : TableHeader.TableHeaderColumnWidthSyncBehavior;
