$root = "C:\Projects\RAO_Project\Client_App"
$csChanged = 0
$axChanged = 0

Get-ChildItem $root -Recurse -Filter *.cs | ForEach-Object {
    $text = [IO.File]::ReadAllText($_.FullName)
    $orig = $text
    $text = $text.Replace("using MessageBox.Avalonia.DTO;", "using MsBox.Avalonia.Dto;")
    $text = $text.Replace("using MessageBox.Avalonia.Enums;", "using MsBox.Avalonia.Enums;")
    $text = $text.Replace("using MessageBox.Avalonia.Models;", "using MsBox.Avalonia.Models;")
    $text = $text.Replace("MessageBox.Avalonia.MessageBoxManager", "MessageBoxManager")
    $text = $text.Replace("MessageBox.Avalonia.Enums.", "")
    $text = $text.Replace("using AvaloniaEdit.Utils;" + [Environment]::NewLine, "")
    if ($text.Contains("MessageBoxManager") -and -not $text.Contains("using MsBox.Avalonia;")) {
        $text = "using MsBox.Avalonia;" + [Environment]::NewLine + $text
    }
    if ($text -ne $orig) {
        [IO.File]::WriteAllText($_.FullName, $text)
        $script:csChanged++
    }
}

Get-ChildItem $root -Recurse -Filter *.axaml.cs | ForEach-Object {
    $lines = [IO.File]::ReadAllLines($_.FullName)
    $changed = $false
    for ($i = 0; $i -lt $lines.Length; $i++) {
        if ($lines[$i] -match '^\s*public (sealed )?class ' -and $lines[$i] -notmatch 'partial') {
            $lines[$i] = $lines[$i].Replace("public sealed class ", "public partial class ")
            $lines[$i] = $lines[$i].Replace("public class ", "public partial class ")
            $changed = $true
        }
    }
    if ($changed) {
        [IO.File]::WriteAllLines($_.FullName, $lines)
        $script:axChanged++
    }
}

Write-Output "Updated $csChanged cs and $axChanged axaml.cs files"
