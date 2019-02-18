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

