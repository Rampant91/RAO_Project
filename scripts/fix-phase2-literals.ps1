$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent
$formsRoot = Join-Path $repoRoot 'Client_App\Views\Forms'
$utf8 = New-Object System.Text.UTF8Encoding $false
$broken = ',`r`n                            ContentHeader = FormDialogTexts.NotificationHeader,`r`n                            ContentMessage'
$fixed = ",`r`n                            ContentHeader = FormDialogTexts.NotificationHeader,`r`n                            ContentMessage"
$changed = 0

Get-ChildItem -Path $formsRoot -Recurse -Filter '*.cs' | ForEach-Object {
    $text = [System.IO.File]::ReadAllText($_.FullName, $utf8)
    if (-not $text.Contains($broken)) { return }
    $updated = $text.Replace($broken, $fixed)
    [System.IO.File]::WriteAllText($_.FullName, $updated, $utf8)
    $changed++
}

Write-Host "Fixed literal backtick-r-n in $changed files."
