# -*- coding: utf-8 -*-
import re
from pathlib import Path

path = Path(r"c:\Projects\RAO_Project\Client_App\Views\Forms\Forms1\Form_11.axaml.cs")
c = path.read_text(encoding="utf-8", errors="replace")

c = re.sub(
    r"#region MessageSaveChanges.*?#endregion",
    """#region MessageSaveChanges

        var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" },
                    new ButtonDefinition { Name = "Отмена" }
                ],
                ContentTitle = "Сохранение изменений",
                ContentHeader = "Уведомление",
                ContentMessage = $"Сохранить форму {vm.FormType}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(this));

        #endregion""",
    c,
    count=1,
    flags=re.DOTALL,
)

c = re.sub(
    r'case "[^"]*":\s*\{\s*_isCloseConfirmed = true;\s*\n\s*//[^\n]*\n\s*try\s*\{\s*await RemoveEmptyForms',
    'case "Да":\n            {\n                _isCloseConfirmed = true;\n\n                try\n                {\n                    await RemoveEmptyForms',
    c,
    count=1,
    flags=re.DOTALL,
)

c = re.sub(
    r'case "[^"]*":\s*\{\s*_isCloseConfirmed = true;\s*dbm\.Restore\(\);',
    'case "Нет":\n            {\n                _isCloseConfirmed = true;\n                dbm.Restore();',
    c,
    count=1,
)

c = re.sub(r'case "[^"]*" or null:', 'case "Отмена" or null:', c, count=1)

c = re.sub(
    r"#region MessageFindIntersection.*?#endregion",
    """#region MessageFindIntersection

                    await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Пересечение",
                            ContentHeader = "Уведомление",
                            ContentMessage = $"У организации {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value} " +
                                             $"{Environment.NewLine}присутствует отчёт по форме " +
                                             $"{currentReport.FormNum_DB} {currentReport.StartPeriod_DB}-{currentReport.EndPeriod_DB}" +
                                             $"{Environment.NewLine}пересекающийся с введённым периодом " +
                                             $"{rep.StartPeriod_DB}-{rep.EndPeriod_DB}.",
                            MinWidth = 450,
                            MinHeight = 170,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(this));

                    #endregion""",
    c,
    count=1,
    flags=re.DOTALL,
)

c = re.sub(
    r"#region MessageRemoveEmptyForms.*?#endregion",
    """#region MessageRemoveEmptyForms

            var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Нет" }
                    ],
                    ContentTitle = "Сохранение изменений",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"В форме {vm.FormType} присутствуют пустые строчки." +
                                     $"{Environment.NewLine}Вы хотите их удалить?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(this));

            #endregion""",
    c,
    count=1,
    flags=re.DOTALL,
)

c = re.sub(r'if \(res is "[^"]*"\)', 'if (res is "Да")', c, count=1)

path.write_text(c, encoding="utf-8", newline="\r\n")
print("Fixed", path)
