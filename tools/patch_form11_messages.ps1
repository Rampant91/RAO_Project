$path = "c:\Projects\RAO_Project\Client_App\Views\Forms\Forms1\Form_11.axaml.cs"
$lines = [System.Collections.Generic.List[string]]::new()
$lines.AddRange([System.IO.File]::ReadAllLines($path, [System.Text.UTF8Encoding]::new($false)))

function Replace-Region($startPattern, $replacement) {
    $start = -1
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match $startPattern) { $start = $i; break }
    }
    if ($start -lt 0) { throw "Start not found: $startPattern" }
    $end = $start
    while ($end -lt $lines.Count -and $lines[$end] -notmatch '^\s+#endregion') { $end++ }
    $lines.RemoveRange($start, $end - $start + 1)
    $lines.InsertRange($start, [string[]]$replacement)
}

Replace-Region '#region MessageSaveChanges' @(
    '        #region MessageSaveChanges',
    '',
    '        var res = await ShowSaveChangesDialogAsync(vm);',
    '',
    '        #endregion'
)

for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '^\s+case .+:\s*$' -and $lines[$i+1] -match '_isCloseConfirmed = true') {
        if ($lines[$i+3] -match 'RemoveEmptyForms') {
            $lines[$i] = '            case Form_11UiText.Yes:'
        }
        elseif ($lines[$i+2] -match 'dbm\.Restore') {
            $lines[$i] = '            case Form_11UiText.No:'
        }
    }
    if ($lines[$i] -match 'case .+ or null:') {
        $lines[$i] = '            case Form_11UiText.Cancel or null:'
    }
    if ($lines[$i] -match 'if \(res is ') {
        $lines[$i] = '            if (res is Form_11UiText.Yes)'
    }
}

Replace-Region '#region MessageFindIntersection' @(
    '                    #region MessageFindIntersection',
    '',
    '                    await ShowIntersectionDialogAsync(',
    '                        reps.Master_DB.RegNoRep.Value,',
    '                        reps.Master_DB.OkpoRep.Value,',
    '                        currentReport.FormNum_DB,',
    '                        currentReport.StartPeriod_DB,',
    '                        currentReport.EndPeriod_DB,',
    '                        rep.StartPeriod_DB,',
    '                        rep.EndPeriod_DB);',
    '',
    '                    #endregion'
)

Replace-Region '#region MessageRemoveEmptyForms' @(
    '            #region MessageRemoveEmptyForms',
    '',
    '            var res = await ShowRemoveEmptyRowsDialogAsync(vm);',
    '',
    '            #endregion'
)

[System.IO.File]::WriteAllLines($path, $lines, [System.Text.UTF8Encoding]::new($false))
Write-Host "Patched $path"
