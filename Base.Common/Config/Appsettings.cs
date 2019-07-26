﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Base.Common.Config
{
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }

        //static Appsettings()
        //{
        //    //ReloadOnChange = true 当appsettings.json被修改时重新加载
        //    Configuration = new ConfigurationBuilder()
        //    .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })//请注意要把当前appsetting.json 文件->右键->属性->复制到输出目录->始终复制
        //    .Build();
        //}

        static Appsettings()
        {
            string Path = "appsettings.json";
            {
                //如果你把配置文件 是 根据环境变量来分开了，可以这样写
                //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
            }

            //Configuration = new ConfigurationBuilder()
            //.Add(new JsonConfigurationSource { Path = Path, ReloadOnChange = true })//请注意要把当前appsetting.json 文件->右键->属性->复制到输出目录->始终复制
            //.Build();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
                .Build();


        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }

                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
