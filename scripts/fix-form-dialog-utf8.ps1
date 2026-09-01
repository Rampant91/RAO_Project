$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent
$formsRoot = Join-Path $repoRoot 'Client_App\Views\Forms'
$replacementChar = [char]0xFFFD
$utf8 = New-Object System.Text.UTF8Encoding $false

function Ensure-UsingResources([string]$text) {
    if ($text -match 'using Client_App\.Resources;') { return $text }
    if ($text -match 'using Client_App\.Interfaces\.Logger;') {
        return $text -replace '(using Client_App\.Interfaces\.Logger;)', "`$1`r`nusing Client_App.Resources;"
    }
    return $text -replace '(namespace Client_App)', "using Client_App.Resources;`r`n`r`n`$1"
}

$files = Get-ChildItem -Path $formsRoot -Recurse -Filter '*.cs'
$changed = 0

foreach ($file in $files) {
    $text = [System.IO.File]::ReadAllText($file.FullName, $utf8)
    if (-not $text.Contains($replacementChar)) { continue }

    $original = $text
    $text = Ensure-UsingResources $text

    $text = [regex]::Replace(
        $text,
        'ButtonDefinitions\s*=\s*\[\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \},\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \},\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \}\s*\r?\n\s*\],',
        "ButtonDefinitions =`r`n                [`r`n                    FormDialogTexts.YesButton,`r`n                    FormDialogTexts.NoButton,`r`n                    FormDialogTexts.CancelButton`r`n                ],")

    $text = [regex]::Replace(
        $text,
        'ButtonDefinitions\s*=\s*\[\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \},\s*\r?\n\s*new ButtonDefinition \{ Name = "[^"]*" \}\s*\r?\n\s*\],',
        "ButtonDefinitions =`r`n                [`r`n                    FormDialogTexts.YesButton,`r`n                    FormDialogTexts.NoButton`r`n                ],")

    if ($text -match 'ContentTitle = "[^"]*' + [regex]::Escape($replacementChar)) {
        $text = [regex]::Replace($text, 'ContentTitle = "([^"]*)"', 'ContentTitle = FormDialogTexts.SaveChangesTitle', 1)
    }

    if ($text -match 'ContentHeader = "[^"]*' + [regex]::Escape($replacementChar)) {
        $text = [regex]::Replace($text, 'ContentHeader = "([^"]*)"', 'ContentHeader = FormDialogTexts.NotificationHeader')
    }

    $text = [regex]::Replace(
        $text,
        'ContentMessage = \$"[^"]*\{vm\.FormType\}\?"',
        'ContentMessage = FormDialogTexts.SaveFormMessage(vm.FormType)')

    $lines = $text -split "\r?\n", -1
    $caseIndex = 0
    for ($i = 0; $i -lt $lines.Length; $i++) {
        $line = $lines[$i]
        if ($line -match '^\s*switch\s*\(') { $caseIndex = 0 }
        if ($line -notmatch '^\s*case\s+"[^"]*":') { continue }
        if (-not $line.Contains($replacementChar)) { continue }

        $caseIndex++
        $indent = ($line -replace 'case.*', '')
        $lines[$i] = switch ($caseIndex) {
            1 { "${indent}case FormDialogTexts.Yes:" }
            2 { "${indent}case FormDialogTexts.No:" }
            default { "${indent}case FormDialogTexts.Cancel:" }
        }
    }

    $text = ($lines -join "`r`n")
  if ($text -ne $original) {
        [System.IO.File]::WriteAllText($file.FullName, $text, $utf8)
        $changed++
    }
}

Write-Host "Updated $changed form files."
