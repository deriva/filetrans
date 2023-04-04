
using System;

namespace Bc.PublishWF.Common
{
    /// <summary>
    /// 统一接口返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ApiResult
    {
        public ApiResult()
        {
            code = 0;
        }

        /// <summary>
        /// 请求状态
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 返回时间戳
        /// </summary>
        public string TimeStamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        /// <summary>
        /// 自定义属性
        /// </summary>
        public object attr { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public class ResultHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ApiResult ToResponse(int result, string message = "", object t = null)
        {
            var response = new ApiResult
            {
                code = (int)(result < 1 ? 101 : 100),
                message = (result < 1 ? "失败:" : "成功:") + message,
                attr = t
            };
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ApiResult ToResult(int code, string message = "", object t = null)
        {
            var response = new ApiResult
            {
                code = code,
                message =  message,
                attr = t
            };
            return response;
        }
        /// <summary>
        /// ToSuccess
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ApiResult ToSuccess(object t = null)
        {
            var response = new ApiResult
            {
                code = 100,
                message = "成功",
                attr = t
            };
            return response;
        }

        /// <summary>
        /// ToSuccess
        /// </summary>
        /// <param name="message"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ApiResult ToSuccess(string message = "", object t = null)
        {
            var response = new ApiResult
            {
                code = 100,
                message = message,
                attr = t
            };
            return response;
        }

        /// <summary>
        /// ToFail
        /// </summary>
        /// <param name="message"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ApiResult ToFail(string message = "", object t = null)
        {
            var response = new ApiResult
            {
                code = 101,
                message = $"失败:{message}",
                attr = t
            };
            return response;
        }
    }
}
