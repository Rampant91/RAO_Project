$ErrorActionPreference = 'Stop'
$utf8 = New-Object System.Text.UTF8Encoding $false

function Fix-FileLines([string]$path, [scriptblock]$transform) {
    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.AddRange([string[]][System.IO.File]::ReadAllLines($path, $utf8))
    & $transform $lines
    [System.IO.File]::WriteAllLines($path, $lines, $utf8)
}

$form10 = 'c:\Projects\RAO_Project\Client_App\Views\Forms\Forms1\Form_10.axaml.cs'
Fix-FileLines $form10 {
    param($lines)
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match 'ContentMessage = "') {
            if ($lines[$i] -notmatch 'FormDialogTexts') {
                $lines[$i] = '                    ContentMessage = FormDialogTexts.IncompleteJuridicalPersonFieldsCloseMessage,'
                while ($i + 1 -lt $lines.Count -and $lines[$i + 1] -match '^\s+\$"\{Environment\.NewLine\}') { $lines.RemoveAt($i + 1) }
                while ($i + 1 -lt $lines.Count -and $lines[$i + 1] -match '^\s+\$"') { $lines.RemoveAt($i + 1) }
            }
        }
        if ($lines[$i] -match 'if \(answer is not ') {
            $lines[$i] = '            if (answer is not FormDialogTexts.Yes)'
        }
    }
}

$form20 = 'c:\Projects\RAO_Project\Client_App\Views\Forms\Forms2\Form_20.axaml.cs'
Fix-FileLines $form20 {
    param($lines)
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match 'ContentMessage = FormDialogTexts\.OrgDuplicateSaveErrorMessage \+') {
            $lines[$i] = '                    ContentMessage = FormDialogTexts.OrgDuplicateSaveErrorMessage,'
            while ($i + 1 -lt $lines.Count -and $lines[$i + 1] -match '^\s+\$"') { $lines.RemoveAt($i + 1) }
        }
    }
}

Write-Host 'Patched Form_10 and Form_20 dialog strings.'
