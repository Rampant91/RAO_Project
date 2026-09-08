#!/usr/bin/env python3
"""Generate ContributeToContentFingerprint next to IsContentEqual in Form*.cs files."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "Models" / "Forms"

EQUALITY_TO_ADD = {
    "FormTextEquality": "AddText",
    "FormDateEquality": "AddDate",
    "FormExponentialEquality": "AddExponential",
    "FormRadionuclidsEquality": "AddRadionuclids",
    "FormDoubleEquality": "AddDouble",
}

METHOD_RE = re.compile(
    r"(public override bool IsContentEqual\(Form otherForm\)\s*\{.*?\n    \})",
    re.DOTALL,
)


def extract_fields(body: str) -> list[tuple[str, str]]:
    """Return list of (sink_method, field_name)."""
    fields: list[tuple[str, str]] = []

    for eq, add in EQUALITY_TO_ADD.items():
        for m in re.finditer(
            rf"{eq}\.Equals\((\w+_DB)\s*,\s*formToCompare\.\1\)",
            body,
        ):
            fields.append((add, m.group(1)))

    # raw equality: Field_DB == formToCompare.Field_DB
    for m in re.finditer(r"(\w+_DB)\s*==\s*formToCompare\.\1", body):
        name = m.group(1)
        # skip if already captured via Equality.Equals
        if any(f == name for _, f in fields):
            continue
        fields.append(("AddRaw", name))

    # preserve order of appearance in body
    ordered: list[tuple[str, str]] = []
    seen = set()
    for m in re.finditer(r"(\w+_DB)", body):
        name = m.group(1)
        if name in seen:
            continue
        for add, f in fields:
            if f == name:
                ordered.append((add, f))
                seen.add(name)
                break
    return ordered


def make_contribute(fields: list[tuple[str, str]], indent: str = "    ") -> str:
    lines = [
        "",
        f"{indent}/// <summary>",
        f"{indent}/// Вклад полей содержимого в fingerprint (тот же набор, что в <see cref=\"IsContentEqual\"/>).",
        f"{indent}/// </summary>",
        f"{indent}public override void ContributeToContentFingerprint(Models.Comparers.FormContent.ContentFingerprintSink sink)",
        f"{indent}{{",
    ]
    for add, name in fields:
        lines.append(f"{indent}    sink.{add}(nameof({name}), {name});")
    lines.append(f"{indent}}}")
    return "\n".join(lines)


def process_file(path: Path) -> bool:
    text = path.read_text(encoding="utf-8")
    if "ContributeToContentFingerprint" in text:
        return False

    # support both 4-space and 8-space class indent (nested Form5)
    patterns = [
        (re.compile(
            r"(public override bool IsContentEqual\(Form otherForm\)\s*\{.*?\n(        )}\)",
            re.DOTALL,
        ), "        "),
        (re.compile(
            r"(public override bool IsContentEqual\(Form otherForm\)\s*\{.*?\n(    )}\)",
            re.DOTALL,
        ), "    "),
    ]

    for rx, indent in patterns:
        m = rx.search(text)
        if not m:
            continue
        body = m.group(0)
        fields = extract_fields(body)
        if not fields:
            print(f"WARN no fields: {path}")
            return False
        contrib = make_contribute(fields, indent)
        new_text = text[: m.end()] + contrib + text[m.end() :]
        # ensure using if needed - fully qualified name used
        path.write_text(new_text, encoding="utf-8")
        print(f"OK {path.relative_to(ROOT.parent.parent)} ({len(fields)} fields)")
        return True

    print(f"SKIP no IsContentEqual: {path}")
    return False


def main() -> None:
    count = 0
    for path in sorted(ROOT.rglob("Form*.cs")):
        if path.name in {"Form.cs", "Form1.cs", "Form2.cs", "FormCreator.cs", "FormStaticData.cs"}:
            continue
        if path.parts[-2] == "Form3":
            continue
        if process_file(path):
            count += 1
    print(f"Updated {count} files")


if __name__ == "__main__":
    main()
