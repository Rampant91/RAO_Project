# Сетевые обновления для отдела

## Каналы обновлений

| Режим | Источник | Когда недоступен |
|-------|----------|------------------|
| Обычный пользователь | Сайт norao.ru | Ручная проверка: сообщение об интернете |
| Отдел (`-n` / developer.mode) | Сетевая папка | Авто: тихо; ручная: «диск Y недоступен» |

Без доступного сетевого диска **Y** отдел **не может** проверить и установить сетевое обновление.

## Структура на шаре (как у вас сейчас)

Корень: `Y:\АЧ 2021\Программа\Исходные`

Путь до exe (Windows):

```
Y:\АЧ 2021\Программа\Исходные\{majorVersion}\{buildId}\win-x64\Client_App.exe
```

Пример:

```
Y:\АЧ 2021\Программа\Исходные\1.3.0\11_test5\win-x64\Client_App.exe
```

В UI версия показывается как **`1.3.0.11_test5`** (`majorVersion` + `.` + `releaseId`).

В корне `Исходные` один файл **`latest.json`**:

```json
{
  "majorVersion": "1.3.0",
  "releaseId": "11_test5",
  "displayName": "1.3.0.11_test5",
  "assemblyVersion": "1.3.0.11",
  "publishedAt": "2026-08-12T09:00:00",
  "notes": "Описание изменений",
  "required": false
}
```

## Запуск с сетевого диска

Если программа запущена **прямо из** `Исходные\…\win-x64\` (путь установки лежит под корнем обновлений):

- автообновление **не предлагается** (дистрибутив на шаре не трогаем);
- ручная проверка объясняет, что нужно скопировать папку локально;
- откат тоже недоступен.

Автообновление рассчитано только на **локальную** копию.

При обновлении / откате **не затираются**:
- `.mpzf-update\`
- `Logs\` / `logs\` рядом с exe
- существующий `Client_App.dll.config` / `Client_App.exe.config` (при обновлении оставляется локальный; при откате возвращается из `previous`)
- данные в `\RAO\` (логи, БД и т.д.)

`data\Updater` (MpzfUpdater) **обновляется**: копируется в staging и дополнительно синхронизируется с шары при проверке обновлений / перед запуском updater (файл не занят, пока работает только Client_App). Это чинит уже выложенные установки, где updater раньше не обновлялся. Локальные FDD-остатки (`dll` / `runtimeconfig`) при sync удаляются, чтобы single-file updater не требовал shared .NET.

## Локально у пользователя

```
data\
  Updater\          ← MpzfUpdater.exe
  …
.mpzf-update\
  state.json      ← какая версия установлена / previous
  prefs.json      ← LastUpdateCheck, LastNotifiedReleaseId, пропуски (на эту копию)
  previous\
  staging\
  updater.log     ← лог применения / ошибок MpzfUpdater
  last-error.txt  ← краткая ошибка прошлого прогона (показывается при старте)
```

Автодиалог при старте: новый `releaseId` в `latest.json` показывается сразу (даже если сегодня уже проверяли).
«Напомнить позже» откладывает **только тот же** `releaseId` примерно на сутки (throttle пишется **только** по этой кнопке, не при показе диалога).
«Пропустить версию» пишет `SkippedReleaseId` и больше не предлагает этот релиз.

Если метки в `state.json` совпали с `latest`, но `Client_App.dll` не совпадает с релизом на шаре — снова предлагается обновление (repair).

При первом запуске с пустым `state.json`: если `Client_App.dll` (размер и время записи) совпадает с релизом из `latest.json`, версия **фиксируется без диалога**. Иначе в UI: «локальная установка (версия ещё не зафиксирована)».

MpzfUpdater применяет релиз **чистой заменой** (не merge): при сбое после начала замены откатывает из `previous`. Read-only атрибуты снимаются перед перезаписью.

## Выкладка (Publish)

1. Publish **Client_App** через профиль VS (`win-x64` / `win-x86` / `Astra_Linux_1.7-1.8` / `Astra_Linux_1.6`).
2. `PublishDir` профиля указывает на **папку payload** (туда попадают exe и `data\`):

```
{корень_релиза}\
  win-x64\                          ← профиль win-x64
    Client_App.exe
    data\                           ← Spravochniki, Manual\, Changelog, REDDB\win-x64, Updater\
  win-x86\                          ← профиль win-x86
    …
    data\REDDB\win-x32\             ← важно: папка REDDB — win-x32
  Astra_Linux_SE_1.7_And_1.8\       ← профиль Astra_Linux_1.7-1.8
    Инструкция_по_установке_….txt
    linux-x64\
      Client_App
      data\
  Astra_Linux_CE_2.12_And_SE_1.6\   ← профиль Astra_Linux_1.6 (AstraLegacy=true)
    Инструкция_по_установке_….txt
    linux-x64\
      Client_App
      libtommath0.deb               ← рядом с исполняемым
      data\                         ← REDDB из linux-x64_astra_1.6 → data\REDDB\linux-x64
```

3. Автоматически копируются:
   - репозиторная `data\` (в т.ч. **`Manual\`** с руководством и инструкциями по установке, Spravochniki, Excel, Changelog) без чужих платформ REDDB, без `Updater` и без `AstraLegacy`;
   - `data\REDDB\{win-x64|win-x32|linux-x64}` под RID профиля  
     (для `Astra_Linux_1.6` источник — `linux-x64_astra_1.6`, в дистрибутиве всё равно `linux-x64`);
   - `data\Updater\` с актуальным self-contained `MpzfUpdater` (single-file, без shared .NET на ПК пользователя);
   - для Astra-профилей (`MpzfInstallGuideFile`): инструкция из `data\Manual\` → **родительская** папка относительно `linux-x64`;
   - для `Astra_Linux_1.6` (`AstraLegacy=true`): `data\AstraLegacy\libtommath0.deb` → корень payload (`linux-x64`, рядом с исполняемым).

Ручное копирование `data` после publish больше не нужно.

Профили `.pubxml` локальные (`*.pubxml` в `.gitignore`); при клонировании репозитория создайте их по образцу в `Client_App\Properties\PublishProfiles\` или скопируйте с рабочей машины.

## Откат

Сервис → «Откатиться на предыдущую версию» — только отдел, локальная установка, есть `previous`.
