namespace Lab4.Helpers;

public class ResponseOrError<T>
{
    public T Response { get; set; } = default!;
    public string? Error { get; set; } = "";
}
