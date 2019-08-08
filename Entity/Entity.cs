using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motuiDotnetSdkDemo.Entity
{
    /// <summary>
    /// 继承接口,用于限制传入值
    /// </summary>
    public interface IRobotEntity { }

    /// <summary>
    /// 返回对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public Int32 Errcode { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public object Count { get; set; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNext { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public object P { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public object Prow { get; set; }
        /// <summary>
        /// 业务数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 时间戳*(接口请求id)
        /// </summary>
        public long RequestId { get; set; }
    }

    /// <summary>
    /// 获取开放账号信息
    /// </summary>
    public class OpenMember : IRobotEntity
    {
        /// <summary>
        /// 开放账号id
        /// </summary>
        public Int32 OpenMemberId { get; set; }
        /// <summary>
        /// 开放账号信息
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 开放账号头像
        /// </summary>
        public string Avatar { get; set; }
    }

    /// <summary>
    /// 获取登录信息
    /// </summary>
    public class LoginQrcode : IRobotEntity
    {
        public string QrCodeUrl { get; set; }
        public int RobotInfoId { get; set; }
    }

    /// <summary>
    /// 心跳信息
    /// </summary>
    public class LoginProgress : IRobotEntity
    {
        public string Status { get; set; }
        public string StatusLabel { get; set; }
        public Weixinrobot WeixinRobot { get; set; }
    }

    /// <summary>
    /// 微信信息
    /// </summary>
    public class Weixinrobot : IRobotEntity
    {
        public bool AllowAddApplication { get; set; }
        public bool AllowDeleted { get; set; }
        public bool AllowLogout { get; set; }
        public bool AllowReLogin { get; set; }
        public bool AllowSwitchWeixinRobot { get; set; }
        public string Avatar { get; set; }
        public int ExpireApplicationNum { get; set; }
        public bool NeedExpire { get; set; }
        public string Nickname { get; set; }
        public int Progress { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public int Weixinid { get; set; }
        public string Wxid { get; set; }
    }

    /// <summary>
    /// 同意某人的好友请求
    /// </summary>
    public class AgreeWxUser : IRobotEntity
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isAgreeWxUserRequest { get; set; }
    }

    /// <summary>
    /// 删除好友
    /// </summary>
    public class DelWxUser : IRobotEntity
    {
        public Boolean IsDelWxUser { get; set; }
    }

    /// <summary>
    /// 转发图片
    /// </summary>
    public class ForwardWxImageMessage : IRobotEntity
    {
        public bool IsForwardWxImageMessage { get; set; }
    }

    /// <summary>
    ///转发视频
    /// </summary>
    public class ForwardWxVideoMessage : IRobotEntity
    {
        public bool IsForwardWxVideoMessage { get; set; }
    }

    /// <summary>
    /// 用户通信录
    /// </summary>
    public class GetWxFriendList
    {
        public string Alias { get; set; }
        public string BigAvatar { get; set; }
        public string City { get; set; }
        public string DisplayName { get; set; }
        public string InviterUsername { get; set; }
        public string Nickname { get; set; }
        public string Province { get; set; }
        public string Remark { get; set; }
        public int Sex { get; set; }
        public string Signature { get; set; }
        public string SmallAvatar { get; set; }
        public string Ticket { get; set; }
        public string Wxid { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class SendWxsTxtMessageString
    {
        public int errCode { get; set; }
        public string msg { get; set; }
        public string sendMessage { get; set; }
        public string wxid { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }



}
