using Bc.Common;
using Bc.Common.Redis;
using Bc.Model;
using SqlSugar;
using System;
using System.Diagnostics;

namespace Bc.Dao
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DbContext
    {

        public SqlSugarClient Db;   //用来处理事务多表查询和复杂的操作

        public static SqlSugarClient Current
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = AppSettings.Configuration["DbConnection:ConnectionString"],
                    DbType = (DbType)Convert.ToInt32(AppSettings.Configuration["DbConnection:DbType"]),
                    IsAutoCloseConnection = false,
                    IsShardSameThread = true,
                    InitKeyType = InitKeyType.Attribute,
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        //   DataInfoCacheService = new RedisCache()
                    },
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true
                    }
                });
            }
        }
        public static SqlSugarClient DBHelper(string config)
        {

            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = config,
                DbType = (DbType)Convert.ToInt32(AppSettings.Configuration["DbConnection:DbType"]),
                IsAutoCloseConnection = false,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //   DataInfoCacheService = new RedisCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true
                }
            });

        }
        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = AppSettings.Configuration["DbConnection:ConnectionString"],
                DbType = (DbType)Convert.ToInt32(AppSettings.Configuration["DbConnection:DbType"]),
                IsAutoCloseConnection = true,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //   DataInfoCacheService = new RedisCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true
                }
            });
            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Debug.WriteLine(sql);
            };
        }

        public DbSet<T> DbTable<T>() where T : class, new()
        {
            return new DbSet<T>(Db);
        }

        public DbSet<sysdiagrams> sysdiagramsDb => new DbSet<sysdiagrams>(Db);


    }

    /// <summary>
    /// 扩展ORM
    /// </summary>
    public class DbSet<T> : SimpleClient<T> where T : class, new()
    {
        public DbSet(SqlSugarClient context) : base(context)
        {

        }
    }
}
