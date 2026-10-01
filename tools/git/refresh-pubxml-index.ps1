#Requires -Version 5.1
<#
.SYNOPSIS
  После Browse в UI VS git status может показывать .pubxml как modified при пустом diff
  (размер файла ≠ записи в индексе). Эта команда обновляет stat через clean-filter;
  в индекс по-прежнему попадает только $(MpzfPublishRoot)…, не абсолютный путь.
#>
$ErrorActionPreference = 'Stop'
$repoRoot = git rev-parse --show-toplevel 2>$null
if (-not $repoRoot) { Write-Error "Запустите из git-репозитория." }
Set-Location $repoRoot

$filter = git config --get filter.mpzf-pubxml.clean
if (-not $filter) {
    Write-Host "Фильтр не настроен — запускаю setup..."
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "tools/git/setup-mpzf-pubxml-filter.ps1"
}

git add -- "Client_App/Properties/PublishProfiles/*.pubxml"
Write-Host "OK: stat профилей обновлён. Проверьте: git status -- Client_App/Properties/PublishProfiles/"
git status --short -- "Client_App/Properties/PublishProfiles/"
