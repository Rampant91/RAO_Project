#!/usr/bin/env python3
"""One-off migration helpers for Avalonia 11."""
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "Client_App"

def migrate_cs(path: Path) -> bool:
    text = path.read_text(encoding="utf-8")
    original = text
    text = text.replace("using MessageBox.Avalonia.DTO;", "using MsBox.Avalonia.Dto;")
    text = text.replace("using MessageBox.Avalonia.Enums;", "using MsBox.Avalonia.Enums;")
    text = text.replace("using MessageBox.Avalonia.Models;", "using MsBox.Avalonia.Models;")
    text = text.replace("MessageBox.Avalonia.MessageBoxManager", "MessageBoxManager")
    text = text.replace("MessageBox.Avalonia.Enums.", "")
    text = text.replace("using AvaloniaEdit.Utils;\n", "")
    text = text.replace("using AvaloniaEdit.Utils;\r\n", "")
    if "MessageBoxManager" in text and "using MsBox.Avalonia;" not in text:
        text = "using MsBox.Avalonia;\n" + text
    if text != original:
        path.write_text(text, encoding="utf-8")
        return True
    return False

def migrate_axaml_cs(path: Path) -> bool:
    lines = path.read_text(encoding="utf-8").splitlines(keepends=True)
    changed = False
    for i, line in enumerate(lines):
        stripped = line.lstrip()
        if stripped.startswith("public class ") or stripped.startswith("public sealed class "):
            if "partial" not in line:
                lines[i] = line.replace("public sealed class ", "public partial class ", 1)
                lines[i] = lines[i].replace("public class ", "public partial class ", 1)
                changed = True
    if changed:
        path.write_text("".join(lines), encoding="utf-8")
    return changed

def main() -> None:
    cs_changed = 0
    axaml_changed = 0
    for path in ROOT.rglob("*.cs"):
        if migrate_cs(path):
            cs_changed += 1
    for path in ROOT.rglob("*.axaml.cs"):
        if migrate_axaml_cs(path):
            axaml_changed += 1
    print(f"Updated {cs_changed} .cs files, {axaml_changed} .axaml.cs files")

if __name__ == "__main__":
    main()
