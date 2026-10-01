#Requires -Version 5.1
<#
.SYNOPSIS
  Git clean-filter: абсолютный PublishDir/PublishUrl из UI VS → $(MpzfPublishRoot)<суффикс>\.
  Смена пути публикации в Visual Studio не даёт diff в git.
#>
param(
    [Parameter(Position = 0)]
    [string] $RelativePath = ''
)

$ErrorActionPreference = 'Stop'
$utf8 = New-Object System.Text.UTF8Encoding $false

$stdin = [Console]::OpenStandardInput()
$ms = New-Object System.IO.MemoryStream
$stdin.CopyTo($ms)
$content = $utf8.GetString($ms.ToArray())

$fileName = [System.IO.Path]::GetFileNameWithoutExtension($RelativePath)

$suffixByProfile = @{
    'win-x64'             = 'win-x64\'
    'win-x86'             = 'win-x86\'
    'Astra_Linux_1.6'     = 'Astra_Linux_CE_2.12_And_SE_1.6\linux-x64\'
    'Astra_Linux_1.7-1.8' = 'Astra_Linux_SE_1.7_And_1.8\linux-x64\'
}

$suffix = $suffixByProfile[$fileName]
if (-not $suffix) {
    $outBytes = $utf8.GetBytes($content)
    $stdout = [Console]::OpenStandardOutput()
    $stdout.Write($outBytes, 0, $outBytes.Length)
    exit 0
}

$canonical = '$(MpzfPublishRoot)' + $suffix

function Set-TagValue([string] $xml, [string] $name, [string] $value) {
    $start = "<$name>"
    $end = "</$name>"
    $i = $xml.IndexOf($start, [StringComparison]::Ordinal)
    if ($i -lt 0) { return $xml }
    $j = $xml.IndexOf($end, $i, [StringComparison]::Ordinal)
    if ($j -lt 0) { return $xml }
    return $xml.Substring(0, $i + $start.Length) + $value + $xml.Substring($j)
}

$content = Set-TagValue $content 'PublishDir' $canonical
if ($content.IndexOf('<PublishUrl>', [StringComparison]::Ordinal) -ge 0) {
    $content = Set-TagValue $content 'PublishUrl' $canonical
}

# Флаги Astra живут в csproj по имени профиля — убрать из .pubxml при коммите
foreach ($tag in @('AstraLegacy', 'MpzfInstallGuideFile')) {
    $content = [regex]::Replace($content, "\r?\n\s*<$tag>[\s\S]*?</$tag>", '')
}

$outBytes = $utf8.GetBytes($content)
$stdout = [Console]::OpenStandardOutput()
$stdout.Write($outBytes, 0, $outBytes.Length)
