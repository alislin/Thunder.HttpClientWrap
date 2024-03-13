namespace Thunder.HttpClientWrap
{
    public class AuthenticationService : IAuthenticationService<LoginModel, UserModel>
    {
        private HttpWrap httpWrap;

        public bool Authed { get; private set; }
        public Action Updated { get; set; }

        public AuthenticationService(HttpWrap httpWrap)
        {
            this.httpWrap = httpWrap;
        }

        public async Task<ServiceResponse<UserModel>> Login(LoginModel user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return ServiceResponse.Fail<UserModel>("登录失败");
            }

            var resp = await httpWrap.Post<ServiceResponse<UserModel>>("/api/login", user);
            if (resp?.Failed ?? true)
            {
                return ServiceResponse.Fail<UserModel>("登录失败");
            }

            httpWrap.SetToken(resp.Data.Token);
            Authed = true;
            Updated?.Invoke();
            return resp;
        }

        public async Task Logout()
        {
            // 实现用户注销逻辑

        }

        public async Task CheckAuth()
        {
            var resp = await httpWrap.Get<ServiceResponse>("/api/login");
            Authed = resp?.Succeeded ?? false;
        }
    }
}