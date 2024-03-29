﻿/*
* ==============================================================================
*
* FileName: ApiResult.cs
* Created: 2020/3/26 13:52:51
* Author: Meiam
* Description: 
*
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Bc.Model
{
  /// <summary>
      /// 统一接口返回
      /// </summary>
      /// <typeparam name="T"></typeparam>
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
            public string timestamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

            /// <summary>
            /// 自定义属性
            /// </summary>
            public object attr { get; set; }

        }

        public class ApiResult<T> : ApiResult
        {
            /// <summary>
            /// 接口返回值
            /// </summary>
            public T Data;

        }


        public class TData
        {
            /// <summary>
            /// 操作结果，Tag为1代表成功，0代表失败，其他的验证返回结果，可根据需要设置
            /// </summary>
            public int tag { get; set; }

            /// <summary>
            /// 提示信息或异常信息
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// 扩展Message
            /// </summary>
            public string description { get; set; }
        }
        public class TData<T> : TData
        {
            /// <summary>
            /// 列表的记录数
            /// </summary>
            public int total { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public T data { get; set; }
        }
    /// <summary>
    /// 返回带分页的Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiPageResult<T> : ApiResult
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return Page > 0; }
        }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return Page + 1 < TotalPages; }
        }

        public List<T> Items { get; set; }

        public object TotalField { get; set; }
    }
}