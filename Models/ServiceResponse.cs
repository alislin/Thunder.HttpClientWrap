using System.Text.Json.Serialization;

namespace Thunder.HttpClientWrap
{
    public enum ActionStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Successfully = 1,
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        /// <summary>
        /// 返回数据内容
        /// </summary>
        [JsonPropertyName("data")]
        [JsonPropertyOrder(2)]
        public T Data { get; set; }

        /// <summary>
        /// 构造成功返回结果
        /// </summary>
        /// <param name="data">成功时返回的数据</param>
        internal ServiceResponse(T data)
        {
            this.Status = ActionStatus.Successfully;
            this.Data = data;
        }

        /// <summary>
        /// 构造错误返回结果
        /// </summary>
        /// <param name="error">错误原因</param>
        internal ServiceResponse(string error)
        {
            this.Status = ActionStatus.Failure;
            this.Error = error;
        }
        internal ServiceResponse(string errorCode, string error)
        {
            this.Status = ActionStatus.Failure;
            this.Error = error;
            this.ErrorCode = errorCode;
        }

        public ServiceResponse()
        {
        }

        //internal ServiceResponse(ValidationResult vr)
        //{
        //    if (vr.IsValid)
        //        throw new ArgumentException("验证结果必须是失败的");

        //    this.Status = ActionStatus.Failure;
        //    this.Error = string.Join("\n", vr.Errors.Select(x => x.ToString()).ToList());
        //}
    }

    public partial class ServiceResponse
    {
        public readonly static string ResultCode_Data_Repeated = "Date_Repeated";
        public readonly static string ResultCode_Data_Not_Found = "Date_Not_Found";
        public readonly static string ResultCode_Partial_Success = "Partial_Success";


        #region " Properties "

        /// <summary>
        /// 返回状态
        /// </summary>
        [JsonPropertyName("status")]
        [JsonPropertyOrder(0)]
        public ActionStatus Status { get; set; }

        /// <summary>
        /// 错误信息，当成功时 Error 为 Null
        /// </summary>
        [JsonPropertyName("error")]
        [JsonPropertyOrder(1)]
        public string Error { get; set; }

        [JsonIgnore]
        public string ErrorCode { get; set; }

        /// <summary>
        /// FluentValidation 错误信息，目前仅为 ASP.NET MVC 验证服务
        /// </summary>
        //[JsonIgnore]
        //public List<ValidationFailure> FluentErrors { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        public bool Succeeded => this.Status == ActionStatus.Successfully;

        /// <summary>
        /// 是否失败
        /// </summary>
        [JsonIgnore]
        public bool Failed => !this.Succeeded;

        #endregion

        #region " Constructor "

        /// <summary>
        /// 构造错误返回结果
        /// </summary>
        /// <param name="error">错误原因</param>
        protected internal ServiceResponse(string error)
        {
            this.Status = ActionStatus.Failure;
            this.Error = error;
        }

        protected internal ServiceResponse(string errorCode, string errorMsg)
        {
            this.Status = ActionStatus.Failure;
            this.Error = errorMsg;
            this.ErrorCode = errorCode;
        }

        //protected internal ServiceResponse(ValidationResult vr)
        //{
        //    if (vr.IsValid)
        //        throw new ArgumentException("验证结果必须是失败的");

        //    this.Status = ActionStatus.Failure;
        //    this.FluentErrors = vr.Errors.ToList();

        //    var opt_error = vr.Errors.FirstOrDefault();
        //    if (opt_error == null)
        //        throw new ArgumentException("验证结果没有错误信息");

        //    this.Error = opt_error.ToString();
        //}

        /// <summary>
        /// 构造成功返回结果
        /// </summary>
        public ServiceResponse()
        {
            this.Status = ActionStatus.Successfully;
        }

        #endregion

        #region " Public Mehtods "

        /// <summary>
        /// 创建成功返回结果
        /// </summary>
        public static ServiceResponse Success() => new ServiceResponse();

        /// <summary>
        /// 创建成功返回结果
        /// </summary>
        /// <param name="data">返回数据</param>
        public static ServiceResponse<T> Success<T>(T data) => new ServiceResponse<T>(data);

        /// <summary>
        /// 创建成功返回结果
        /// </summary>
        public static ServiceResponse Success(Action action)
        {
            action?.Invoke();
            return Success();
        }

        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        /// <param name="error">失败原因</param>
        public static ServiceResponse Fail(string error) => new ServiceResponse(error);

        public static ServiceResponse Fail(string errorCode, string errorMsg) => new ServiceResponse(errorCode, errorMsg);

        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        public static ServiceResponse Fail(ServiceResponse response)
        {
            if (response.Succeeded)
                throw new NotImplementedException("必须是失败的返回结果");

            if (string.IsNullOrWhiteSpace(response.Error))
                throw new NotImplementedException("必须是失败的返回结果");

            return new ServiceResponse(response.Error);
        }

        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        /// <param name="format">复合格式字符串</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static ServiceResponse Fail(string format, params object[] args)
        {
            return new ServiceResponse
            {
                Status = ActionStatus.Failure,
                Error = string.Format(format, args)
            };
        }

        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static ServiceResponse<T> Fail<T>(string error)
        {
            return new ServiceResponse<T>(error);
        }

        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        /// <param name="format">复合格式字符串</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static ServiceResponse<T> Fail<T>(string format, params object[] args)
        {
            return new ServiceResponse<T>(string.Format(format, args));
        }
        public static ServiceResponse<T> Fail<T>(string errorCode, string errorMsg)
        {
            return new ServiceResponse<T>(errorCode, errorMsg);
        }
        #endregion

        #region " Public Methods For Validation Result "

        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        /// <param name="vr">FluentValidation 验证失败结果</param>
        //public static ServiceResponse Fail(ValidationResult vr) => new ServiceResponse(vr);


        /// <summary>
        /// 创建失败返回结果
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        //public static ServiceResponse<T> Fail<T>(ValidationResult error)
        //{
        //    return new ServiceResponse<T>(error);
        //}

        #endregion

        #region " After "

        public ServiceResponse Then(params Action[] actions)
        {
            if (actions == null)
                return this;

            foreach (var action in actions)
            {
                try
                {
                    action.Invoke();
                }
                catch
                {
                    break;
                }
            }

            return this;
        }

        #endregion
    }

}
