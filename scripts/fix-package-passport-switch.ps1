$ErrorActionPreference = 'Stop'
$path = 'c:\Projects\RAO_Project\Client_App\Views\Passports\PackagePassportWindow.axaml.cs'
$utf8 = New-Object System.Text.UTF8Encoding $false
$lines = [System.Collections.Generic.List[string]]::new()
$lines.AddRange([string[]][System.IO.File]::ReadAllLines($path, $utf8))

$start = -1
$end = -1
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match 'switch \(res\)') { $start = $i }
    if ($start -ge 0 -and $lines[$i] -match '^\s+\}$' -and $i -gt $start + 10) {
        $end = $i
        break
    }
}

if ($start -lt 0 -or $end -lt 0) { throw 'switch block not found' }

$replacement = @(
'        switch (res)',
'        {',
'            case FormDialogTexts.Yes:',
'                {',
'                    _isCloseConfirmed = true;',
'',
'                    try',
'                    {',
'                        await dbm.SaveChangesAsync();',
'                    }',
'                    catch (Exception ex)',
'                    {',
'                        Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager',
'                        .GetMessageBoxCustom(new MessageBoxCustomParams',
'                        {',
'                            ButtonDefinitions =',
'                            [',
'                                FormDialogTexts.OkButton,',
'                            ],',
'                            ContentTitle = FormDialogTexts.SaveChangesTitle,',
'                            ContentHeader = FormDialogTexts.ErrorHeader,',
'                            ContentMessage = FormDialogTexts.SaveErrorMessage(ex.Message),',
'                            MinWidth = 400,',
'                            WindowStartupLocation = WindowStartupLocation.CenterOwner',
'                        }).ShowWindowDialogAsync(this));',
'                    }',
'',
'                    if (desktop.Windows.Count == 1)',
'                    {',
'                        desktop.MainWindow.WindowState = OwnerPrevState;',
'',
'                        break;',
'                    }',
'',
'                    args.Cancel = false;',
'',
'                    break;',
'                }',
'            case FormDialogTexts.No:',
'                {',
'                    _isCloseConfirmed = true;',
'                    dbm.Restore();',
'                    await dbm.SaveChangesAsync();',
'',
'                    break;',
'                }',
'            case FormDialogTexts.Cancel or null:',
'                {',
'                    _isCloseConfirmed = false;',
'                    return;',
'                }',
'        }'
)

for ($i = $end; $i -ge $start; $i--) { $lines.RemoveAt($i) }
for ($i = $replacement.Count - 1; $i -ge 0; $i--) { $lines.Insert($start, $replacement[$i]) }

[System.IO.File]::WriteAllLines($path, $lines, $utf8)
Write-Host "Replaced switch block in PackagePassportWindow."
