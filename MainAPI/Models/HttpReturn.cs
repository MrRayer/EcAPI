namespace MainAPI.Models
{
    public class HttpReturn
    {
        public int HttpCode { get; set; }
        public string? Message { get; set; }
        public HttpReturn(int code,string error = "")
        {
            HttpCode = code;
            if (error != "") Message = error;
        }
    }
}
