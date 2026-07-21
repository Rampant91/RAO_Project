from pathlib import Path

ROOT = Path(r"c:\Projects\RAO_Project\Client_App\Views\Forms\Forms2")
FILES = [ROOT / f"Form_{n}.axaml" for n in
         ["21", "22", "23", "24", "25", "26", "27", "28", "29", "210", "211", "212"]]

REPLACEMENTS = [
    (
        '    xmlns:local="using:Client_App.Behaviors"',
        '    xmlns:headerBehaviors="using:Client_App.Behaviors.TableHeader"\n'
        '    xmlns:dgBehaviors="using:Client_App.Behaviors.DataGridBehaviors"',
    ),
    (
        '\t\t<ScrollViewer Classes="tableScrollViewer" Grid.Row="1">',
        '\t\t<ScrollViewer Classes="tableScrollViewer" Grid.Row="1" HorizontalScrollBarVisibility="Disabled">',
    ),
    (
        '            <Grid x:Name="Table" RowDefinitions="Auto,*" ColumnDefinitions="Auto">',
        '            <Grid x:Name="Table" RowDefinitions="Auto,*" ColumnDefinitions="*">',
    ),
    (
        '                    Width="{Binding ElementName=dataGrid.Width}"',
        '                    Width="{Binding Bounds.Width, ElementName=dataGrid}"',
    ),
    (
        """                    <interactivity:Interaction.Behaviors>
                        <local:ColumnWidthSyncBehavior SourceDataGrid="{Binding ElementName=dataGrid}" />
                    </interactivity:Interaction.Behaviors>""",
        """                    <interactivity:Interaction.Behaviors>
                        <headerBehaviors:TableHeaderColumnWidthSyncBehavior SourceDataGrid="{Binding ElementName=dataGrid}" />
                        <headerBehaviors:TableHeaderColumnResizeBehavior SourceDataGrid="{Binding ElementName=dataGrid}"
                                                                       IncludeTrailingBoundary="True" />
                    </interactivity:Interaction.Behaviors>""",
    ),
    (
        '\t\t\t\t\tSelectedItem="{Binding SelectedForm}"\n\t\t\t\t\tColumnWidth="125">',
        '\t\t\t\t\tSelectedItem="{Binding SelectedForm}"\n                    Margin="8,-5,0,0"\n\t\t\t\t\tColumnWidth="125">',
    ),
    (
        """                        <local:DataGridColumnWidthLoadBehavior FormNum="notes" />
                            <local:DataGridPointerBehavior />
                            <local:DataGridSelectedItemsBehavior SelectedItems="{Binding SelectedNotes, Mode=TwoWay}" />""",
        """                        <dgBehaviors:DataGridColumnWidthLoadBehavior FormNum="notes" />
                            <dgBehaviors:DataGridPointerBehavior />
                            <dgBehaviors:DataGridSelectedItemsBehavior SelectedItems="{Binding SelectedNotes, Mode=TwoWay}" />""",
    ),
]

MAIN_BEHAVIOR_OLD = """                        <local:DataGridColumnWidthLoadBehavior FormNum="{form}" />
                        <local:DataGridPointerBehavior />
                        <local:DataGridSelectedItemsBehavior SelectedItems="{{Binding SelectedForms, Mode=TwoWay}}" />
                        <local:DeselectDataGridOnClickOutsideBehavior />
                        <local:DataGridEditingStateBehavior IsEditing="{{Binding DataGridIsEditing, Mode=TwoWay}}" />
                        <local:DataGridAlternateArrowsKeyBehavior IsEditing="{{Binding DataGridIsEditing, Mode=OneWay}}" />"""

MAIN_BEHAVIOR_NEW = """                        <dgBehaviors:DataGridColumnWidthLoadBehavior FormNum="{form}" />
                        <dgBehaviors:DataGridPointerBehavior />
                        <dgBehaviors:DataGridSelectedItemsBehavior SelectedItems="{{Binding SelectedForms, Mode=TwoWay}}" />
                        <dgBehaviors:DataGridClearSelectionOnEmptyAreaClickBehavior />
                        <dgBehaviors:DataGridEditingStateBehavior IsEditing="{{Binding DataGridIsEditing, Mode=TwoWay}}" />
                        <dgBehaviors:DataGridAlternateArrowsKeyBehavior IsEditing="{{Binding DataGridIsEditing, Mode=OneWay}}" />"""

FORM_NUMS = {
    "Form_21.axaml": "2.1",
    "Form_22.axaml": "2.2",
    "Form_23.axaml": "2.3",
    "Form_24.axaml": "2.4",
    "Form_25.axaml": "2.5",
    "Form_26.axaml": "2.6",
    "Form_27.axaml": "2.7",
    "Form_28.axaml": "2.8",
    "Form_29.axaml": "2.9",
    "Form_210.axaml": "2.10",
    "Form_211.axaml": "2.11",
    "Form_212.axaml": "2.12",
}

for path in FILES:
    text = path.read_text(encoding="utf-8")
    for old, new in REPLACEMENTS:
        if old not in text:
            print(f"WARN missing pattern in {path.name}: {old[:40]}...")
        text = text.replace(old, new)

    form_num = FORM_NUMS[path.name]
    old_main = MAIN_BEHAVIOR_OLD.format(form=form_num)
    new_main = MAIN_BEHAVIOR_NEW.format(form=form_num)
    if old_main not in text:
        print(f"WARN main behaviors missing in {path.name}")
    text = text.replace(old_main, new_main)

    path.write_text(text, encoding="utf-8")
    print(f"Updated {path.name}")
