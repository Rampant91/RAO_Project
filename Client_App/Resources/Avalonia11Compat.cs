using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input.Platform;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using System.Threading.Tasks;

namespace Client_App.Resources;

public sealed class MsBoxInputResult
{
    public string? Button { get; init; }
    public string? InputValue { get; init; }
    public string Message => InputValue ?? string.Empty;
}

public static class Avalonia11Compat
{
    public static IClipboard? MainClipboard =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow?.Clipboard;

    public static void BindHorizontalScroll(ScrollViewer viewer, AvaloniaObject source, AvaloniaProperty property)
    {
        source.GetObservable(property).Subscribe(value =>
        {
            var x = value switch
            {
                null => 0d,
                IConvertible convertible => convertible.ToDouble(CultureInfo.InvariantCulture),
                _ => double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
                    ? parsed
                    : 0d
            };

            if (!double.IsFinite(x))
            {
                x = 0d;
            }

            if (Math.Abs(viewer.Offset.X - x) > 0.001)
            {
                viewer.Offset = viewer.Offset.WithX(x);
            }
        });
    }

    public static void BindVerticalScrollBarSync(ScrollViewer viewer, ScrollBar bar)
    {
        var syncing = false;

        void SyncFromViewer()
        {
            if (syncing)
            {
                return;
            }

            syncing = true;
            try
            {
                var max = viewer.Extent.Height - viewer.Viewport.Height;
                if (!double.IsFinite(max) || max < 0)
                {
                    max = 0;
                }

                var y = viewer.Offset.Y;
                if (!double.IsFinite(y) || y < 0)
                {
                    y = 0;
                }

                if (Math.Abs(bar.Maximum - max) > 0.001)
                {
                    bar.Maximum = max;
                }

                var clamped = Math.Clamp(y, bar.Minimum, bar.Maximum);
                if (Math.Abs(bar.Value - clamped) > 0.001)
                {
                    bar.Value = clamped;
                }
            }
            finally
            {
                syncing = false;
            }
        }

        viewer.GetObservable(ScrollViewer.OffsetProperty).Subscribe(_ => SyncFromViewer());
        viewer.GetObservable(ScrollViewer.ExtentProperty).Subscribe(_ => SyncFromViewer());
        viewer.GetObservable(ScrollViewer.ViewportProperty).Subscribe(_ => SyncFromViewer());

        bar.GetObservable(RangeBase.ValueProperty).Subscribe(value =>
        {
            if (syncing)
            {
                return;
            }

            if (!double.IsFinite(value))
            {
                return;
            }

            syncing = true;
            try
            {
                var clamped = Math.Clamp(value, bar.Minimum, bar.Maximum);
                if (Math.Abs(viewer.Offset.Y - clamped) > 0.001)
                {
                    viewer.Offset = viewer.Offset.WithY(clamped);
                }
            }
            finally
            {
                syncing = false;
            }
        });

        SyncFromViewer();
    }

    public static async Task<MsBoxInputResult> ShowInputDialogAsync(MessageBoxCustomParams parameters, Window owner)
    {
        parameters.InputParams ??= new InputParams();
        var box = MessageBoxManager.GetMessageBoxCustom(parameters);
        var button = await box.ShowWindowDialogAsync(owner);
        return new MsBoxInputResult
        {
            Button = button,
            InputValue = box.InputValue
        };
    }
}
