namespace CoffeeShop.Helper
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResponse<T> Success(T data, int statusCode = 200)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ServiceResponse<T> Failure(string error, int statusCode)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = false,
                ErrorMessage = error,
                StatusCode = statusCode
            };
        }
    }
}