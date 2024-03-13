
namespace Thunder.HttpClientWrap
{
    /// <summary>
    /// 客户端认证
    /// </summary>
    /// <typeparam name="T">登录请求模型</typeparam>
    /// <typeparam name="V">登录返回模型</typeparam>
    public interface IAuthenticationService<T, V>
    {
        /// <summary>
        /// 是否授权成功
        /// </summary>
        bool Authed { get; }
        Action Updated { get; set; }
        /// <summary>
        /// 登录检查
        /// </summary>
        /// <returns></returns>
        Task CheckAuth();
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录请求模型</param>
        /// <returns>登录返回模型</returns>
        Task<ServiceResponse<V>> Login(T user);
        Task Logout();
    }
}