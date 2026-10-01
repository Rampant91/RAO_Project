#Requires -Version 5.1
<#
.SYNOPSIS
  Регистрирует локальный git clean-filter mpzf-pubxml для PublishProfiles.
  Запускать один раз после клона (из корня репозитория).
#>
$ErrorActionPreference = 'Stop'
$repoRoot = git rev-parse --show-toplevel 2>$null
if (-not $repoRoot) {
    Write-Error "Запустите из git-репозитория RAO_Project."
}
Set-Location $repoRoot

$cleanScript = Join-Path $repoRoot 'tools\git\mpzf-pubxml-clean.ps1'
if (-not (Test-Path -LiteralPath $cleanScript)) {
    Write-Error "Не найден $cleanScript"
}

# Относительный путь со слэшами / — иначе git/sh съедает backslash в Windows-пути
$cleanRel = 'tools/git/mpzf-pubxml-clean.ps1'
$cleanCmd = "powershell.exe -NoProfile -ExecutionPolicy Bypass -File $cleanRel %f"
git config filter.mpzf-pubxml.clean $cleanCmd
# smudge не задаём — identity (в рабочей копии после Browse остаётся абсолютный путь из VS)
git config --unset filter.mpzf-pubxml.smudge 2>$null
git config --unset filter.mpzf-pubxml.required 2>$null

Write-Host "OK: filter.mpzf-pubxml.clean зарегистрирован (local git config)."
Write-Host "  $cleanCmd"
Write-Host "Профили: Client_App/Properties/PublishProfiles/*.pubxml"
Write-Host "Путь публикации можно менять в UI VS — в git status это не попадёт (после setup)."
