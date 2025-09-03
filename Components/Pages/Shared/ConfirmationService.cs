public class ConfirmationRequest
{
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public string ConfirmText { get; set; } = "Yes";
    public string CancelText { get; set; } = "Cancel";
    public ConfirmationType Type { get; set; } = ConfirmationType.Confirm;
    public TaskCompletionSource<bool> TaskCompletionSource { get; set; } = new();
}

public enum ConfirmationType
{
    Confirm,
    Info
}
public interface IConfirmationService
{
    Task<bool> ConfirmAsync(string title, string message, string confirmText = "Yes", string cancelText = "Cancel");
    Task<bool> ConfirmDeleteAsync(string itemName);
    Task ShowInfoAsync(string title, string message);
}

public class ConfirmationService : IConfirmationService
{
    public event Func<ConfirmationRequest, Task>? OnConfirmationRequested;

    public async Task<bool> ConfirmAsync(string title, string message, string confirmText = "Yes", string cancelText = "Cancel")
    {
        var request = new ConfirmationRequest
        {
            Title = title,
            Message = message,
            ConfirmText = confirmText,
            CancelText = cancelText,
            Type = ConfirmationType.Confirm
        };

        if (OnConfirmationRequested != null)
        {
            await OnConfirmationRequested.Invoke(request);
            return await request.TaskCompletionSource.Task;
        }

        return false;
    }

    public async Task<bool> ConfirmDeleteAsync(string itemName)
    {
        return await ConfirmAsync(
            "Confirm Delete", 
            $"Are you sure you want to delete '{itemName}'? This action cannot be undone.",
            "Delete", 
            "Cancel"
        );
    }

    public async Task ShowInfoAsync(string title, string message)
    {
        var request = new ConfirmationRequest
        {
            Title = title,
            Message = message,
            ConfirmText = "OK",
            Type = ConfirmationType.Info
        };

        if (OnConfirmationRequested != null)
        {
            await OnConfirmationRequested.Invoke(request);
            await request.TaskCompletionSource.Task;
        }
    }
}