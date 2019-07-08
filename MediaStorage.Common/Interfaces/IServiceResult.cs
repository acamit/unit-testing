namespace MediaStorage.Common
{
    public interface IServiceResult
    {
        string Action { get; set; }
        int Id { get; set; }
        bool IsConfirm { get; set; }
        bool IsSuccessful { get; set; }
        string Message { get; set; }

        void SetFailure(string message);
        void SetSuccess(string message);
    }
}