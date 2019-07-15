using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Base.Api
{
    public class DefaultModule : Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {

            //控制器属性注入
            var controllersTypesInAssembly = typeof(Startup).Assembly.GetExportedTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
            builder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();

            ////注册Biz中的对象
            //builder.RegisterAssemblyTypes(GetAssemblyByName("Base.BusinessService")).Where(a => a.Name.EndsWith("Biz"))
            //    .PropertiesAutowired()
            //    .AsImplementedInterfaces();

        }
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(String AssemblyName)
        {
            return Assembly.Load(AssemblyName);
        }
    }
}
