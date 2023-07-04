using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Model.View
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {
        
        public int Id { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表说明
        /// </summary>
        public string TableNameDesc { get; set; }

        /// <summary>
        /// 字段序号
        /// </summary>
        public int ColSort { get; set; }

        /// <summary>
        /// 自增标识
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 占用字节数
        /// </summary>
        public int ByteLen { get; set; }



        /// <summary>
        /// 长度
        /// </summary>
        public int Len { get; set; }

        /// <summary>
        /// 小数位
        /// </summary>
        public int Digtal { get; set; }

        /// <summary>
        /// 允许为空
        /// </summary>
        public string AllowNull { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 列描述
        /// </summary>
        public string ColumnDesc { get; set; }


        /// <summary>
        /// 数据库
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// 别名
        /// </summary>

        public string Alias { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }

}
