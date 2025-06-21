namespace FrmMain.Dto.Response
{
    public class ApiErrorResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
    }
    public class ResponseStatus
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
