using System;
using System.Threading.Tasks;

namespace Client_App.Interfaces;

/// <summary>
/// Окно формы с overlay «Загрузка…» (<see cref="Views.Controls.FormContentLoadingOverlay"/>).
/// </summary>
public interface IFormContentLoadingHost
{
    bool IsContentLoading { get; }

    string ContentLoadingMessage { get; }

    Task WithContentLoadingAsync(Func<Task> work, bool clearVisibleRows = true, string? message = null);
}
