
using System;

namespace Lumos.DbRelay
{

    /// <summary>
    /// 系统的枚举
    /// </summary>
    public partial class Enumeration
    {
        public enum SmsSendResult
        {
            Unknow = 0,
            Success = 1,
            Failure = 2,
            Exception = 3
        }

        public enum UserStatus
        {
            Unknow = 0,
            Normal = 1,
            Disable = 2
        }

        public enum BelongSite
        {
            Unknow = 0,
            Admin = 1,
            Merchant = 2,
            Client = 3,
        }

        public enum LoginType
        {
            Unknow = 0,
            Website = 1,
            AndroidApp = 2,
            IosApp = 3,
            Wechat = 4
        }

        public enum LoginResult
        {

            Unknow = 0,
            Success = 1,
            Failure = 2
        }

        public enum LoginResultTip
        {
            Unknow = 0,
            VerifyPass = 1,
            UserNotExist = 2,
            UserPasswordIncorrect = 3,
            UserDisabled = 4,
            UserDeleted = 5,
            UserAccessFailedMaxCount = 6
        }

        public enum OperateType
        {
            Unknow = 0,
            New = 1,
            Update = 2,
            Delete = 3,
            Save = 4,
            Submit = 5,
            Pass = 6,
            Reject = 7,
            Refuse = 8,
            Cancle = 9,
            Serach = 101,
            ExportExcel = 102
        }

        public enum SysOrganizationStatus
        {

            Unknow = 0,
            Valid = 1,
            Invalid = 2
        }

        //1开头的为管理端系统职位，2开头为商户端系统职位
        public enum SysPositionId
        {

            Unknow = 0,
            AdminAdministrator = 100,
            AdminJuniorOperators = 101,
            MerchantAdministrator = 200,
            MerchantOM = 201,
            MerchantAM = 202,
            MerchantRS = 203
        }


        public enum WxAutoReplyType
        {
            Unknow = 0,
            Subscribe = 1,
            Keyword = 2,
            MenuClick = 3,
            NotKeyword = 4
        }
    }
}
