$ErrorActionPreference = 'Stop'
$root = Join-Path $PSScriptRoot '..\Models\Forms' | Resolve-Path

$equalityMap = @{
    'FormTextEquality'        = 'AddText'
    'FormDateEquality'        = 'AddDate'
    'FormExponentialEquality' = 'AddExponential'
    'FormRadionuclidsEquality'= 'AddRadionuclids'
    'FormDoubleEquality'      = 'AddDouble'
}

function Get-Fields([string]$body) {
    $pairs = New-Object System.Collections.Generic.List[object]
    foreach ($eq in $equalityMap.Keys) {
        $add = $equalityMap[$eq]
        $rx = [regex]::new("$eq\.Equals\((\w+_DB)\s*,\s*formToCompare\.\1\)")
        foreach ($m in $rx.Matches($body)) {
            $pairs.Add([pscustomobject]@{ Add = $add; Name = $m.Groups[1].Value })
        }
    }
    $rxRaw = [regex]::new('(\w+_DB)\s*==\s*formToCompare\.\1')
    foreach ($m in $rxRaw.Matches($body)) {
        $name = $m.Groups[1].Value
        if (-not ($pairs | Where-Object Name -eq $name)) {
            $pairs.Add([pscustomobject]@{ Add = 'AddRaw'; Name = $name })
        }
    }
    $ordered = New-Object System.Collections.Generic.List[object]
    $seen = @{}
    foreach ($m in [regex]::Matches($body, '(\w+_DB)')) {
        $name = $m.Groups[1].Value
        if ($seen.ContainsKey($name)) { continue }
        $hit = $pairs | Where-Object Name -eq $name | Select-Object -First 1
        if ($hit) {
            $ordered.Add($hit)
            $seen[$name] = $true
        }
    }
    return $ordered
}

function Make-Contribute($fields, [string]$indent) {
    $sb = New-Object System.Text.StringBuilder
    [void]$sb.AppendLine('')
    [void]$sb.AppendLine("${indent}/// <summary>")
    [void]$sb.AppendLine("${indent}/// Вклад полей содержимого в fingerprint (тот же набор, что в <see cref=`"IsContentEqual`"/>).")
    [void]$sb.AppendLine("${indent}/// </summary>")
    [void]$sb.AppendLine("${indent}public override void ContributeToContentFingerprint(Models.Comparers.FormContent.ContentFingerprintSink sink)")
    [void]$sb.AppendLine("${indent}{")
    foreach ($f in $fields) {
        [void]$sb.AppendLine("${indent}    sink.$($f.Add)(nameof($($f.Name)), $($f.Name));")
    }
    [void]$sb.AppendLine("${indent}}")
    return $sb.ToString()
}

$skip = @('Form.cs','Form1.cs','Form2.cs','FormCreator.cs','FormStaticData.cs')
$count = 0
Get-ChildItem -Path $root -Recurse -Filter 'Form*.cs' | ForEach-Object {
    if ($skip -contains $_.Name) { return }
    if ($_.Directory.Name -eq 'Form3') { return }
    $text = [IO.File]::ReadAllText($_.FullName)
    if ($text -match 'ContributeToContentFingerprint') { return }

    $rx8 = [regex]::new('(?s)(public override bool IsContentEqual\(Form otherForm\)\s*\{.*?\r?\n        \})')
    $rx4 = [regex]::new('(?s)(public override bool IsContentEqual\(Form otherForm\)\s*\{.*?\r?\n    \})')
    $m = $rx8.Match($text)
    $indent = '        '
    if (-not $m.Success) {
        $m = $rx4.Match($text)
        $indent = '    '
    }
    if (-not $m.Success) {
        Write-Host "SKIP $($_.FullName)"
        return
    }
    $fields = Get-Fields $m.Value
    if ($fields.Count -eq 0) {
        Write-Host "WARN no fields $($_.Name)"
        return
    }
    $contrib = Make-Contribute $fields $indent
    $newText = $text.Substring(0, $m.Index + $m.Length) + $contrib + $text.Substring($m.Index + $m.Length)
    [IO.File]::WriteAllText($_.FullName, $newText)
    Write-Host "OK $($_.Name) ($($fields.Count))"
    $count++
}
Write-Host "Updated $count files"
