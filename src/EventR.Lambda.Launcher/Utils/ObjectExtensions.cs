namespace EventR.Lambda.Launcher.Utils;

public static class ObjectExtensions
{
    public static async Task<object?> AsTask(this object? obj)
    {
        if (obj is not Task task)
        {
            return obj;
        }

        await task.ConfigureAwait(false);

        var taskType = task.GetType();
        if (taskType.IsGenericType)
        {
            var result = taskType.GetProperty("Result")?.GetValue(task);
            return result;
        }

        return null;
    }
}