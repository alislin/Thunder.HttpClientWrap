# Thunder.HttpClientWrap
封装 HttpClient

服务注册，`program.cs`

```cs
using Thunder.HttpClientWrap;

// 默认服务注册
builder.Services.AddNoty()
                .AddHttpWrap()
                .AddAuthentication();

// 自定义登录认证服务注册（如果另外实现登录，可用这种方式认证）
// LoginHandle:IAuthenticationService<LoginModel, UserModel>
// builder.Services.AddScoped<LoginHandle>();
```