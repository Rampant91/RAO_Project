$ErrorActionPreference = 'Stop'
$formsRoot = 'c:\Projects\RAO_Project\Client_App\Views\Forms'
$utf8 = New-Object System.Text.UTF8Encoding $false
$changed = 0
Get-ChildItem -Path $formsRoot -Recurse -Filter '*.cs' | ForEach-Object {
    $text = [IO.File]::ReadAllText($_.FullName, $utf8)
    if ($text -notmatch 'case "[^"]*" or null:') { return }
    $updated = [regex]::Replace($text, 'case "[^"]*" or null:', 'case FormDialogTexts.Cancel or null:')
    if ($updated -ne $text) {
        [IO.File]::WriteAllText($_.FullName, $updated, $utf8)
        $changed++
    }
}
Write-Host "Fixed Cancel case in $changed form files."
