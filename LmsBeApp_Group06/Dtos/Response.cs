namespace LmsBeApp_Group06.Dtos
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Messager { get; set; }
    }
}
