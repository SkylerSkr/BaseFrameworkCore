# BaseFrameworkCore
#项目介绍<br> 
这是一个基于.Net Core设计的基础框架,只是后端框架<br> 
使用了AutoFac做依赖注入,接口化编程更易于扩展<br> 
用Dapper做orm框架,很轻很快速,提供了简单的扩展方法,可以在biz层快速的处理单表的crud<br> 
请求和响应标准化,对前端保证一致性<br> 
使用过滤器做了日志记录和Token认证<br> 
使用管道重写了异常响应<br> 
使用拦截器做了全局事务<br> 

## 分层
### 1-SDK 请求层
Request文件夹：存放了请求基类和自定义请求,请求抽象类使用了逆变和协变,这样设计是为了后来做单元测试的时候更加方便。<br> 
Response文件夹：存放了响应通用响应。<br> 
Model文件夹：存放业务模型。<br>

## 2-Biz  业务层
存放Service类和接口，需要命名规范，因使用autofac的反射来映射。<br>
通过RepoBase，可以快速处理单表的CRUD

## 3-DataAccess 数据访问层
### Base-Domain 实体层
Entitys文件夹:实体,需继承EntityBase基类。<br>
Enum文件夹:数据库枚举。<br>
### Base.Repository 数据访问层
Repo文件夹：存放Repo，需继承RepositoryBase基类才可使用分页。<br>
DBProxy:管理数据库连接对象。
RepoBase:一些扩展方法，使用此扩展方法，可以在Biz层快速处理单表的crud。<br>

##PS
此项目大家提交的时候不可提交到master分支，请自己创建分支提交，谢谢大家。<br>
为了.Net社区，大家一起努力吧！<br>
