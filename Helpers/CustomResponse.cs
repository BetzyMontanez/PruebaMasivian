namespace RouletteApi.Helpers
{
    public class CustomResponse<T>
    {
        public int StatusCode { get; }
        public string StatusName { get; }
        public string DataName { get; }
        public T Data { get; }
        public CustomResponse(int statusCode, string statusName, string dataName, T data)
        {
            this.StatusCode = statusCode;
            this.StatusName = statusName;
            this.DataName = dataName;
            this.Data = data;
        }
    }
}
