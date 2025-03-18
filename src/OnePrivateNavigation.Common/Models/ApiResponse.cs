namespace OnePrivateNavigation.Common.Models
{
    /// <summary>
    /// 通用API响应封装
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 创建成功的响应
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        /// <returns>API响应</returns>
        public static ApiResponse<T> Ok(T data, string message = "操作成功")
        {
            return new ApiResponse<T>
            {
                Code = 200,
                Message = message,
                Data = data,
                Success = true
            };
        }

        /// <summary>
        /// 创建失败的响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误码</param>
        /// <returns>API响应</returns>
        public static ApiResponse<T> Error(string message, int code = 400)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Message = message,
                Data = default,
                Success = false
            };
        }
    }
}