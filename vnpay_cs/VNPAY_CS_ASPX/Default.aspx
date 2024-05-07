<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VNPAY_CS_ASPX._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VNPAY DEMO</title>
    <link href="Styles/bootstrap.min.css" rel="stylesheet" />
</head>
<body>

<div class="container">
    <div class="header clearfix">
        <h3 class="text-muted">VNPAY DEMO</h3>
    </div>
      <div class="form-group">
           <button onclick="pay()">Giao dịch thanh toán</button><br>
      </div>
      <div class="form-group">
           <button onclick="querydr()">API truy vấn kết quả thanh toán</button><br>
       </div>
       <div class="form-group">
           <button onclick="refund()">API hoàn tiền giao dịch</button><br>
       </div>

       <p>
         &nbsp;
       </p>
      <footer class="footer">
         <p>&copy; VNPAY 2020</p>
      </footer>
</div>
    <script>
             function pay() {
              window.location.href = "/vnpay_pay.aspx";
            }
            function querydr() {
              window.location.href = "/vnpay_querydr.aspx";
            }
             function refund() {
              window.location.href = "/vnpay_refund.aspx";
            }
        </script>
</body>
</html>