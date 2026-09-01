$ErrorActionPreference = 'Stop'
$path = 'c:\Projects\RAO_Project\Client_App\Views\Passports\PackagePassportWindow.axaml.cs'
$utf8 = New-Object System.Text.UTF8Encoding $false
$lines = [System.Collections.Generic.List[string]]::new()
$lines.AddRange([string[]][System.IO.File]::ReadAllLines($path, $utf8))

for ($i = 0; $i -lt $lines.Count; $i++) {
    $line = $lines[$i]
    if ($line -match 'ContentTitle = "' -and $line -notmatch 'FormDialogTexts') {
        $lines[$i] = '                ContentTitle = FormDialogTexts.SaveChangesTitle,'
    }
    if ($line -match 'ContentHeader = "' -and $line -notmatch 'FormDialogTexts') {
        $lines[$i] = '                ContentHeader = FormDialogTexts.NotificationHeader,'
    }
    if ($line -match 'ContentMessage = \$"' -and $line -notmatch 'FormDialogTexts') {
        $lines[$i] = '                ContentMessage = FormDialogTexts.SavePackagePassportMessage,'
    }
    if ($line -match 'new ButtonDefinition \{ Name = "' -and $line -notmatch 'FormDialogTexts') {
        $lines[$i] = '                                FormDialogTexts.OkButton,'
    }
    if ($line -match 'ContentHeader = "' -and $lines[$i-1] -match 'SaveChangesTitle' -and $line -notmatch 'NotificationHeader') {
        # error dialog header after save error title in catch block
    }
    if ($line -match 'case "' -and $line -notmatch 'FormDialogTexts') {
        if ($line -match 'or null') { $lines[$i] = '            case FormDialogTexts.Cancel or null:' }
        elseif ($line -match '^\s+case "[^"]{1,3}":\s*$' -and $line -notmatch 'case "') { }
    }
}

# second pass for switch cases and error header
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '^\s+case "') {
        if ($i -lt $lines.Count - 5 -and $lines[$i+2] -match 'SaveChangesAsync') {
            $lines[$i] = '            case FormDialogTexts.Yes:'
        }
        elseif ($i -lt $lines.Count - 5 -and $lines[$i+2] -match 'Restore\(\)') {
            $lines[$i] = '            case FormDialogTexts.No:'
        }
        elseif ($lines[$i] -match 'or null') {
            $lines[$i] = '            case FormDialogTexts.Cancel or null:'
        }
    }
    if ($lines[$i] -match 'ContentHeader = "' -and $i -gt 0 -and $lines[$i-1] -match 'SaveChangesTitle' -and $lines[$i] -notmatch 'NotificationHeader') {
        $lines[$i] = '                            ContentHeader = FormDialogTexts.ErrorHeader,'
    }
    if ($lines[$i] -match 'ContentMessage = \$"' -and $lines[$i] -match 'ex\.Message') {
        $lines[$i] = '                            ContentMessage = FormDialogTexts.SaveErrorMessage(ex.Message),'
        if ($i + 1 -lt $lines.Count -and $lines[$i+1] -match 'ex\.Message') { $lines.RemoveAt($i+1) }
    }
}

[System.IO.File]::WriteAllLines($path, $lines, $utf8)
Write-Host 'Patched PackagePassportWindow dialog strings.'
