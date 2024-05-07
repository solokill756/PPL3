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
    public partial class vnpay_refund : System.Web.UI.Page
    {
        private static readonly ILog Log =
       LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void btnRefund_Click(object sender, EventArgs e)
        {

            var vnp_Api = ConfigurationManager.AppSettings["vnp_Api"];
            var vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret KEy
            var vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; // Terminal Id

            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu hoàn tiền giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "refund";
            var vnp_TransactionType = RefundCategory.Text;
            var vnp_Amount = Convert.ToInt64(amount.Text) * 100;
            var vnp_TxnRef = OrderId.Text; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Hoan tien giao dich:" + OrderId.Text;
            var vnp_TransactionNo = ""; //Giả sử giá trị của vnp_TransactionNo không được ghi nhận tại hệ thống của merchant.
            var vnp_TransactionDate = payDate.Text;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_CreateBy = user.Text;
            var vnp_IpAddr = Utils.GetIpAddress();

            var signData = vnp_RequestId + "|" +vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TransactionType + "|" + vnp_TxnRef + "|" + vnp_Amount + "|" + vnp_TransactionNo + "|" + vnp_TransactionDate + "|" + vnp_CreateBy + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

            var rfData = new
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = vnp_TmnCode,
                vnp_TransactionType = vnp_TransactionType,
                vnp_TxnRef = vnp_TxnRef,
                vnp_Amount = vnp_Amount,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionNo = vnp_TransactionNo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateBy = vnp_CreateBy,
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash

            };
            var jsonData = new JavaScriptSerializer().Serialize(rfData);

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