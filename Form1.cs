using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace motuiDotnetSdkDemo
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        public static Form1 MainForm = null;
        public Form1()
        {
            MainForm = this;
            //心跳设置30秒一次
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;


            InitializeComponent();
            textLogs.ReadOnly = true;
            this.button1.Enabled = false;
            this.Text = "欢迎使用魔力转圈圈";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 需要修改
            GetListenerOpen("http://192.168.0.196:10002/");
        }
        /// <summary>
        ///  记录日志
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="source"></param>
        public void OnLog(string logs, string source = "系统")
        {
            textLogs.AppendText(source + ":" + logs + "\r\n");
        }
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
                var result = Service.LoginProgress();
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"当前状态:{result.Data.StatusLabel}");
                    if (result.Errcode == 10000)
                    {
                        timer.Stop();
                        pictureBox1.Image = Service.GetImage(result.Data.WeixinRobot.Avatar);
                        this.Text = $"{result.Data.WeixinRobot.Nickname},欢迎登录";
                    }
                    else if(result.Errcode == 1003)
                    {
                        timer.Stop();
                        this.Text = "登录失败,请重新获取二维码";
                    }
                    else if(result.Errcode == -1001)
                    {
                        timer.Stop();
                        this.Text = "登录失败,请重新获取二维码";
                    }
                });
            });
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            var result = Service.LoginProgress();
            OnLog($"当前状态:{result.Data.StatusLabel}");
        }

            /// <summary>
            /// 创建账号按钮被点击
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Button2_Click(object sender, EventArgs e)
        {
            // 创建账号
            this.Text = "正在获取账号,请稍等";
            var result = Service.CreateOpenMember(this.openId.Text, this.openKey.Text, this.openMemberid.Text);
            OnLog($"{result.Msg},账号id:{result.Data.OpenMemberId}");
            this.Text = "创建账号成功,请点击登录按钮";
            this.button1.Enabled = true;
            this.button2.Enabled = false;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                this.Invoke((EventHandler)delegate
                {
                    pictureBox1.Image = Service.GetImage("https://img-my.csdn.net/uploads/201107/29/0_131194772196y8.gif");
                    this.Text = "正在获取二维码";
                });
                var result = Service.LoginQrcode();
                this.Invoke((EventHandler)delegate
                {
                    timer.Start();
                    this.Text = "二维码获取成功,请扫码!";
                    OnLog($"{result.Msg},机器人id:{result.Data?.RobotInfoId}");
                    pictureBox1.Image = Service.GetImage(result.Data.QrCodeUrl);
                });
            });
        }

        /// <summary>
        /// 同意某人的好友请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click_1(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var result = Service.AgreeWxUser(this.agreeWxUserFromwxid.Text, this.agreeWxUserTicket.Text);
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"操作状态:{result.Data.isAgreeWxUserRequest}");
                });
            });
        }
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var result = Service.DelUser(delWxUserFromWxid.Text);
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"操作状态:{result.Data.IsDelWxUser}");
                });
            });
        }

        /// <summary>
        /// 转发图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var result = Service.ForwardWxImage(this.forwardWxImageMessageFromwxid.Text,this.forwardWxImageMessageXmlContent.Text);
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"操作状态:{result.Data.IsForwardWxImageMessage}");
                });
            });
        }

        /// <summary>
        /// 转发图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button6_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var result = Service.ForwardWxVideo(this.forwardWxVideoMessageFromWxid.Text, this.forwardWxVideoMessageXmlContent.Text);
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"操作状态:{result.Data.IsForwardWxVideoMessage}");
                });
            });
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var result = Service.GetWxFriendList();
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"操作状态:{result.Msg},{string.Join(",",result.Data.ToArray().Select(t=>t.ToString()).ToArray())}");
                });
            });
        }



        /// <summary>
        /// 发送批量文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button8_Click(object sender, EventArgs e)
        {
            var wxids = this.sendWxsTxtMessageWxids.Text;
            var message = this.sendWxsTxtMessageMessages.Text;
            Task.Run(() =>
            {
                var result = Service.SendWxsTextMessage(wxids, message);
                this.Invoke((EventHandler)delegate
                {
                    OnLog($"操作状态:{result.Errcode},{string.Join(",", result.Data.ToArray().Select(t => t.ToString()).ToArray())}");
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button10_Click(object sender, EventArgs e)
        {

        }

        #region http服务器
        /// <summary>
        /// 开启http服务器
        /// </summary>
        /// <param name="listenerUrl">必须以/结尾</param>
        internal static void GetListenerOpen(string listenerUrl)
        {
            Service.sSocket = new HttpListener();
            Service.sSocket.Prefixes.Add(listenerUrl);
            Service.sSocket.Start();
            Service.sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), Service.sSocket);
            Console.WriteLine($"已启动监听，访问{listenerUrl}");
        }

        static internal void GetContextCallBack(IAsyncResult ar)
        {
            try
            {
                Service.sSocket = ar.AsyncState as HttpListener;
                HttpListenerContext context = Service.sSocket.EndGetContext(ar);
                //再次监听请求
                Service.sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), Service.sSocket);
                //处理请求
                string a = Request(context.Request);
                //输出请求
                Service.Response(context.Response, a);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        /// <summary>
        /// 处理输入参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        static string Request(HttpListenerRequest request)
        {
            string temp = "welcome!";
            if (request.HttpMethod.ToLower().Equals("get"))
            {
                //GET请求处理
                temp = "暂无get请求的方法";
            }
            else if (request.HttpMethod.ToLower().Equals("post"))
            {
                //POST请求处理
                NameValueCollection postData = new NameValueCollection();
                if (request.HasEntityBody)
                {
                    var body = request.InputStream;
                    var encoding = Encoding.UTF8;
                    var reader = new StreamReader(body, encoding);
                    string s = reader.ReadToEnd();
                    body.Close();
                    reader.Close();
                    postData = System.Web.HttpUtility.ParseQueryString(s);
                    // 获取openMemberid
                    var openMemberid = postData.Get("openMemberid");
                    // 获取robotWeixinid,需要确认这个信息是否正确
                    var robotWeixinid = postData.Get("robotWeixinid");
                    // 获取eventName
                    var eventName = postData.Get("eventName");
                    // 获取base64Body
                    var base64Body = Encoding.UTF8.GetString(Convert.FromBase64String(postData.Get("base64Body")));
                    Form1.MainForm.Invoke((EventHandler)delegate
                    {
                        Form1.MainForm.OnLog($"机器人收到消息:{openMemberid}的微信{robotWeixinid},收到了{eventName},内容{base64Body}", "通知");
                    });
                    temp = "OK";
                }
            }
            return temp;
        }

        private static byte[] ReadLineAsBytes(Stream SourceStream)
        {
            var resultStream = new MemoryStream();
            while (true)
            {
                int data = SourceStream.ReadByte();
                resultStream.WriteByte((byte)data);
                if (data <= 10)
                    break;
            }
            resultStream.Position = 0;
            byte[] dataBytes = new byte[resultStream.Length];
            resultStream.Read(dataBytes, 0, dataBytes.Length);
            return dataBytes;
        }


        #endregion

        
    }
}
