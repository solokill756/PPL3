using System;
using System.Web;
using System.IO;
using System.Net;
using System.Configuration;
using VNPAY_CS_ASPX.Models;
using log4net;
using System.Web.Script.Serialization;

namespace VNPAY_CS_ASPX
{
    public partial class vnpay_querydr : System.Web.UI.Page
    {
        private static readonly ILog Log =
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void btnQuery_Click(object sender, EventArgs e)
        {

            var vnp_Api = ConfigurationManager.AppSettings["vnp_Api"];
            var vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret KEy
            var vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; // Terminal Id

            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "querydr";
            var vnp_TxnRef = orderId.Text; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Truy van giao dich:" + orderId.Text;
            var vnp_TransactionDate = payDate.Text;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_IpAddr = Utils.GetIpAddress();

            var signData = vnp_RequestId + "|" +vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TxnRef + "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

            var qdrData = new
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = vnp_TmnCode,
                vnp_TxnRef = vnp_TxnRef,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash

            };
            var jsonData = new JavaScriptSerializer().Serialize(qdrData);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(vnp_Api);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var strData = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                strData = streamReader.ReadToEnd();
            }
            display.InnerHtml = "<b>VNPAY RESPONSE:</b> " + strData;
            
        }
    }
}