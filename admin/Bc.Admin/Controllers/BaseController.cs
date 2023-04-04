using Bc.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bc.Admin.Controllers
{

    public class BaseController : Controller
    {
        #region 统一返回封装

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static JsonResult toResponse(StatusCodeType statusCode)
        {
            ApiResult response = new ApiResult();
            response.code = (int)statusCode;
            response.message = statusCode.GetEnumText();
            return new JsonResult(response);
        }



        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toResponse(StatusCodeType statusCode, string retMessage)
        {
            ApiResult response = new ApiResult();
            response.code = (int)statusCode;
            response.message = retMessage;

            return new JsonResult(response);
        }




        /// <summary>
        /// 返回结果封装
        /// </summary>
        /// <param name="result"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toResponse(int result, string retMessage, object t = null)
        {
            var statusCode = StatusCodeType.ApiSuccess;
            var msg = "成功:" + retMessage;
            if (result < 1)
            {
                msg = "失败:" + retMessage;
                statusCode = StatusCodeType.Fail;
            }
            ApiResult response = new ApiResult();
            response.code = (int)statusCode;
            response.message = msg;
            response.attr = t;
            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toResponse(StatusCodeType statusCode, string retMessage, object data)
        {
            ApiResult response = new ApiResult();
            response.code = (int)statusCode;
            response.message = retMessage;
            response.attr = data;
            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult toResponse<T>(T data)
        {
            ApiResult<T> response = new ApiResult<T>();
            response.code = (int)StatusCodeType.ApiSuccess;
            response.message = StatusCodeType.ApiSuccess.GetEnumText();
            response.Data = data;
            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toSuccess(object data)
        {
            ApiResult response = new ApiResult();
            response.code = (int)StatusCodeType.ApiSuccess;
            response.message = "";
            response.attr = data;
            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toFail(string msg, object data = null)
        {
            ApiResult response = new ApiResult();
            response.code = (int)StatusCodeType.Fail;
            response.message = msg;
            response.attr = data;
            return new JsonResult(response);
        }
        #endregion

        public ActionResult Vn(string id)
        {
            return View(id);
        }

    }
}
