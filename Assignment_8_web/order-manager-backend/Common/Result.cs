namespace order_manager_backend.Common
{
    public class Result<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static Result<T> Success(T data, string message = "操作成功")
        {
            return new Result<T>
            {
                Code = 200,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Fail(string message = "操作失败", int code = 400)
        {
            return new Result<T>
            {
                Code = code,
                Message = message,
                Data = default
            };
        }
    }

    public class Result
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public static Result Success(string message = "操作成功")
        {
            return new Result
            {
                Code = 200,
                Message = message
            };
        }

        public static Result Fail(string message = "操作失败", int code = 400)
        {
            return new Result
            {
                Code = code,
                Message = message
            };
        }
    }
} 