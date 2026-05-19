$p = "c:\Projects\RAO_Project\Client_App\Views\Forms\Forms1\Form_11.axaml.cs"
$lines = [System.IO.File]::ReadAllLines($p, [System.Text.UTF8Encoding]::new($false))
$out = $lines[0..263] + "}"
[System.IO.File]::WriteAllLines($p, $out, [System.Text.UTF8Encoding]::new($false))
Write-Host "Truncated to $($out.Length) lines"
