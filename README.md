# DDDBlogWithCQRS
Domain-driven design Blog With CQRS
### 1 迁移数据库
分别有三个数据库上下文，D3BlogDbContext，EventStoreSQLContext，AppIdentityDbContext。
分别在包管理控制台中切换到Infrastructure.Data下执行
```
update database -verbose -Context D3BlogDbContext
```
```
update database -verbose -Context EventStoreSQLContext
```

下边是identity的上下文。包管理器切换到Infrastructure.Identity下执行
```
update database -verbose -Context AppIdentityDbContext
```

然后运行一次MVC项目，对identit数据库进行初始化。默认会有两个用户，和角色。可参见AppIdentitySeedData.cs文件中


##### 因为数据库变更为mysql  使用serilog 时无法自定义列，所以将日志框架新增NLog 日志框架。原有serilog暂时不适用。如果使用sqlserver的话就使用serilog，比NLog方便些。


- 如果出现缺少XXX.XML文件的话  将api 和 application 右键->属性->生成 有个输出，将路径改到api的路径下就可以了。这是为了给api的swagger 添加说明文字使用。
比如 E:\项目\D3.Blog\D3.BlogApi\D3.Blog.Application.xml
