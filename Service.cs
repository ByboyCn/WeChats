using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using motuiDotnetSdkDemo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Drawing;
using motuiDotnetSdkDemo.exception;
using System.Threading;
using System.IO;
using System.Collections.Specialized;

namespace motuiDotnetSdkDemo
{
    public class Service
    {
        /// <summary>
        /// 服务器url
        /// </summary>
        private static readonly string SERVICE_HOST_URL= "http://byboy.cn1.utools.club";

        /// <summary>
        /// 开放平台id
        /// </summary>
        private static string Openid { get; set; }

        /// <summary>
        /// 开放平台key
        /// </summary>
        private static string OpenKey { get; set; }

        /// <summary>
        /// 开放平台商户id
        /// </summary>
        private static string OpenMerchantId { get; set; }

        /// <summary>
        /// 开放平台用户id
        /// </summary>
        private static string OpenMemberid { get; set; }

        /// <summary>
        /// 登录生成的机器人序号
        /// </summary>
        private static string RobotInfoId { get; set; }
        /// <summary>
        /// 生机器人数据库的序号
        /// </summary>
        private static string WeixinId { get; set; }
        /// <summary>
        /// 申请账号
        /// </summary>
        /// <param name="openid">开放平台openid</param>
        /// <param name="openkey">开放平台openkey</param>
        /// <param name="openMerchantId">开放平台openMerchantId</param>
        /// <returns></returns>
        internal static Response<OpenMember> CreateOpenMember(string openid ,string openkey,string openMerchantId)
        {
            Openid = openid;
            OpenKey = openkey;
            OpenMerchantId = openMerchantId;
            // postData内容是账号昵称和头像,可以不填写
            var html = PostHtml("/api/wxrobot/auth/createOpenMember", "{\"avatar\":\"\",\"nickname\": \"\"}");
            var jsonOpenMember = JsonConvert.DeserializeObject<Response<OpenMember>>(html);
            // 判断返回数据的错误代码是否等于0
            if (jsonOpenMember.Errcode == 0)
            {
                OpenMemberid = jsonOpenMember.Data.OpenMemberId.ToString();
            }
            return jsonOpenMember;
        }
        /// <summary>
        /// 获取机器人二维码
        /// </summary>
        /// <returns></returns>
        internal static Response<LoginQrcode> LoginQrcode()
        {
            var html = GetHtml("/api/wxrobot/auth/loginQrcode?type=createWeixinRobot");
            var jsonLoginQrcode = JsonConvert.DeserializeObject<Response<LoginQrcode>>(html);
            if(jsonLoginQrcode.Errcode == 0)
            {
                RobotInfoId = jsonLoginQrcode.Data.RobotInfoId.ToString();
            }
            return jsonLoginQrcode;
        }
        /// <summary>
        /// 心跳包
        /// </summary>
        /// <returns></returns>
        internal static Response<LoginProgress> LoginProgress()
        {
            var html = GetHtml($"/api/wxrobot/auth/getLoginProgress?robotInfoId={RobotInfoId}");
            var jsonLoginProgress = JsonConvert.DeserializeObject<Response<LoginProgress>>(html);
            WeixinId = jsonLoginProgress.Data?.WeixinRobot?.Weixinid.ToString();
            return jsonLoginProgress;

        }
        #region 好友操作
        /// <summary>
        /// 同意某人的好友请求
        /// </summary>
        /// <param name="fromwxid"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        internal static Response<AgreeWxUser> AgreeWxUser(string fromwxid, string ticket)
        {
            var json = new
            {
                fromwxid,
                ticket
            };
            var html = PostHtml($"/api/wxrobot/auth/getLoginProgress", JsonConvert.SerializeObject(json));
            var jsonAgreeWxUser = JsonConvert.DeserializeObject<Response<AgreeWxUser>>(html);
            return jsonAgreeWxUser;

        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="fromwxid"></param>
        /// <returns></returns>
        internal static Response<DelWxUser> DelUser(string fromwxid)
        {
            var json = new
            {
                fromwxid,
            };
            var html = PostHtml($"/api/wxrobot/wx/delWxUser", JsonConvert.SerializeObject(json));
            var jsonDelWxUser = JsonConvert.DeserializeObject<Response<DelWxUser>>(html);
            return jsonDelWxUser;
        }

        /// <summary>
        /// 转发图片
        /// </summary>
        /// <param name="fromwxid"></param>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        internal static Response<ForwardWxImageMessage> ForwardWxImage(string fromwxid, string xmlContent)
        {
            var json = new
            {
                fromwxid,
                xmlContent
            };
            var html = PostHtml($"/api/wxrobot/wx/forwardWxImageMessage", JsonConvert.SerializeObject(json));
            var jsonForwardWxImageMessage = JsonConvert.DeserializeObject<Response<ForwardWxImageMessage>>(html);
            return jsonForwardWxImageMessage;
        }
        
        /// <summary>
        /// 转发图片
        /// </summary>
        /// <param name="fromwxid"></param>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        internal static Response<ForwardWxVideoMessage> ForwardWxVideo(string fromwxid, string xmlContent)
        {
            var json = new
            {
                fromwxid,
                xmlContent
            };
            var html = PostHtml($"/api/wxrobot/wx/forwardWxVideoMessage", JsonConvert.SerializeObject(json));
            var jsonForwardWxImageMessage = JsonConvert.DeserializeObject<Response<ForwardWxVideoMessage>>(html);
            return jsonForwardWxImageMessage;
        }
        
        /// <summary>
         /// 转发图片
         /// </summary>
         /// <param name="fromwxid"></param>
         /// <param name="xmlContent"></param>
         /// <returns></returns>
        internal static Response<List<GetWxFriendList>> GetWxFriendList()
        {

            var html = GetHtml($"/api/wxrobot/wx/getWxFriendList");
            var jsonForwardWxImageMessage = JsonConvert.DeserializeObject<Response<List<GetWxFriendList>>>(html);
            return jsonForwardWxImageMessage;
        }

        internal static Response<List<SendWxsTxtMessageString>> SendWxsTextMessage(string wxids, string message)
        {
            var json = new
            {
                wxids,
                message
            };
            var html = PostHtml($"/api/wxrobot/wx/sendWxsTxtMessage", JsonConvert.SerializeObject(json));
            var jsonForwardWxImageMessage = JsonConvert.DeserializeObject<Response<List<SendWxsTxtMessageString>>>(html);
            return jsonForwardWxImageMessage;
        }
        #endregion




        #region webHttpServer
        internal static HttpListener sSocket;
        /// <summary>
        /// 输出方法
        /// </summary>
        /// <param name="response">response对象</param>
        /// <param name="responseString">输出值</param>
        /// <param name="contenttype">输出类型默认为json</param>
        internal static void Response(HttpListenerResponse response, string responsestring, string contenttype = "application/json")
        {
            response.StatusCode = 200;
            response.ContentType = contenttype;
            response.ContentEncoding = Encoding.UTF8;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsestring);
            //对客户端输出相应信息.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            //关闭输出流，释放相应资源
            output.Close();
        }

       
        #endregion
        #region web请求
        public static Image GetImage(string url)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml,application/json, */*",//    可选项有默认值  
                ContentType = "application/json;charset=UTF-8",//返回类型    可选项有默认值  
                ResultType = ResultType.Byte,//返回数据类型，是Byte还是String  
            };
            return http.GetImage(item);
        }
        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <returns></returns>
        private static string PostHtml(string url, string postData)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = SERVICE_HOST_URL + url,//URL     必需项  
                Method = "POST",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml,application/json, */*",//    可选项有默认值  
                ContentType = "application/json;",//返回类型    可选项有默认值  
                Postdata = postData,//Post数据     可选项GET时不需要写  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            item.Header.Add("openid", Openid);
            item.Header.Add("openKey", OpenKey);
            item.Header.Add("openMerchantId", OpenMerchantId);
            if (!string.IsNullOrEmpty(OpenMemberid))
            {
                item.Header.Add("openMemberid", OpenMemberid);
            }
            if (!string.IsNullOrEmpty(WeixinId))
            {
                item.Header.Add("robotWeixinid", WeixinId);
            }
            HttpResult result = http.GetHtml(item);
            if(result.StatusCode != HttpStatusCode.OK)
            {
                throw new NoResponseDataException("无内容");
            }
            return result.Html;
        }

        

        /// <summary>
        /// 发送get请求
        /// </summary>
        /// <returns></returns>
        private static string GetHtml(string url)
        {

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = SERVICE_HOST_URL + url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = "",//字符串Cookie     可选项  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Allowautoredirect = false,//是否根据３０１跳转     可选项  
                AutoRedirectCookie = false,//是否自动处理Cookie     可选项  
                                           //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                           //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            item.Header.Add("openid", Openid);
            item.Header.Add("openKey", OpenKey);
            item.Header.Add("openMerchantId", OpenMerchantId);
            if (!string.IsNullOrEmpty(OpenMemberid))
            {
                item.Header.Add("openMemberid", OpenMemberid);
            }
            if (!string.IsNullOrEmpty(WeixinId))
            {
                item.Header.Add("robotWeixinid", WeixinId);
            }
            HttpResult result = http.GetHtml(item);
            if (result.Html.Equals(""))
            {
                throw new NoResponseDataException("无内容");
            }
            return result.Html;
        }
        #endregion
    }
}
