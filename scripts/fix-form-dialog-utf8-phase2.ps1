$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent
$viewsRoot = Join-Path $repoRoot 'Client_App\Views'
$replacementChar = [char]0xFFFD
$utf8 = New-Object System.Text.UTF8Encoding $false

function Ensure-UsingResources([string]$text) {
    if ($text -match 'using Client_App\.Resources;') { return $text }
    if ($text -match 'using Client_App\.Interfaces\.Logger;') {
        return $text -replace '(using Client_App\.Interfaces\.Logger;)', "`$1`r`nusing Client_App.Resources;"
    }
    if ($text -match 'using Client_App\.Commands\.AsyncCommands\.Save;') {
        return $text -replace '(using Client_App\.Commands\.AsyncCommands\.Save;)', "`$1`r`nusing Client_App.Resources;"
    }
    return $text -replace '(namespace Client_App)', "using Client_App.Resources;`r`n`r`n`$1"
}

$files = Get-ChildItem -Path $viewsRoot -Recurse -Filter '*.cs'
$changed = 0

foreach ($file in $files) {
    $text = [System.IO.File]::ReadAllText($file.FullName, $utf8)
    if (-not $text.Contains($replacementChar)) { continue }

    $original = $text
    $text = Ensure-UsingResources $text

    $text = [regex]::Replace(
        $text,
        'ContentTitle = FormDialogTexts\.SaveChangesTitle,\s*\r?\n\s*ContentHeader = FormDialogTexts\.NotificationHeader,\s*\r?\n\s*ContentMessage\s*=\s*\r?\n\s*\$"[^"]*"\s*\+',
        "ContentTitle = FormDialogTexts.OrgTitleSaveErrorTitle,`r`n                    ContentHeader = FormDialogTexts.ErrorHeader,`r`n                    ContentMessage = FormDialogTexts.OrgDuplicateSaveErrorMessage +")

    $text = [regex]::Replace(
        $text,
        'ContentMessage\s*=\s*\r?\n\s*\$"[^"]*"\s*\+\s*\r?\n\s*\$"[^"]*"\s*\+\s*\r?\n\s*\$"[^"]*"',
        'ContentMessage = FormDialogTexts.OrgDuplicateSaveErrorMessage')

    $text = [regex]::Replace(
        $text,
        'ContentTitle = FormDialogTexts\.SaveChangesTitle,\s*\r?\n\s*ContentHeader = FormDialogTexts\.NotificationHeader,\s*\r?\n\s*ContentMessage = \$"[^"]*\{reps\.Master_DB\.RegNoRep\.Value\}_\{reps\.Master_DB\.OkpoRep\.Value\}[^"]*"\s*\+\s*\r?\n\s*\$"\{Environment\.NewLine\}[^"]*"\s*\+\s*\r?\n\s*\$"\{currentReport\.FormNum_DB\}[^"]*"\s*\+\s*\r?\n\s*\$"\{Environment\.NewLine\}[^"]*"\s*\+\s*\r?\n\s*\$"\{rep\.StartPeriod_DB\}-\{rep\.EndPeriod_DB\}\."',
        'ContentTitle = FormDialogTexts.NotificationHeader,`r`n                            ContentHeader = FormDialogTexts.NotificationHeader,`r`n                            ContentMessage = FormDialogTexts.PeriodIntersectionMessage($"{reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}", currentReport.FormNum_DB, currentReport.StartPeriod_DB, currentReport.EndPeriod_DB, rep.StartPeriod_DB, rep.EndPeriod_DB)')

    $text = [regex]::Replace(
        $text,
        'ContentMessage = \$"[^"]*\{vm\.FormType\}[^"]*"\s*\+\s*\r?\n\s*\$"\{Environment\.NewLine\}[^"]*"',
        'ContentMessage = FormDialogTexts.RemoveEmptyRowsMessage(vm.FormType)')

    $text = [regex]::Replace($text, 'if \(res is "[^"]*"\)', 'if (res is FormDialogTexts.Yes)')

    $text = [regex]::Replace(
        $text,
        'ButtonDefinitions\s*=\s*\[\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \},\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \},\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \}\s*\r?\n\s*\],',
        "ButtonDefinitions =`r`n                [`r`n                    FormDialogTexts.YesButton,`r`n                    FormDialogTexts.NoButton,`r`n                    FormDialogTexts.CancelButton`r`n                ],")

    $text = [regex]::Replace(
        $text,
        'ButtonDefinitions\s*=\s*\[\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \},\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \}\s*\r?\n\s*\],',
        "ButtonDefinitions =`r`n                [`r`n                    FormDialogTexts.YesButton,`r`n                    FormDialogTexts.NoButton`r`n                ],")

    if ($text -ne $original) {
        [System.IO.File]::WriteAllText($file.FullName, $text, $utf8)
        $changed++
    }
}

Write-Host "Phase2 updated $changed view files."
