public class ResultViewModel
{
    public ResultViewModel(bool isSuccess = true, string message = "")
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public static ResultViewModel Error(string message)
        => new ResultViewModel(false, message);

    public static ResultViewModel Success(string? message = "")
        => new ResultViewModel(true,message);
}

public class ResultViewModel<T> : ResultViewModel
{
    public ResultViewModel(T? data, bool isSuccess = true, string message = "")
        : base(isSuccess, message)
    {
        Data = data;
    }

    public T? Data { get; private set; }

    public static ResultViewModel<T> Success(T data, string? message = "")
        => new ResultViewModel<T>(data,true,message);

    public static ResultViewModel<T> Error(string message)
        => new ResultViewModel<T>(default, false, message);
}
