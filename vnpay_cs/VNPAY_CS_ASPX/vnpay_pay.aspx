<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vnpay_pay.aspx.cs" Inherits="VNPAY_CS_ASPX._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VNPAY DEMO</title>
    <link href="Styles/bootstrap.min.css" rel="stylesheet" />
</head>
<body>

 
    <div class="jumbotron">
    <h1>VNPAY DEMO</h1>
</div>
<div class="container">
    <div class="row">
        <div class="col-md-4 order-md-2 mb-4"
            <h4 class="d-flex justify-content-between align-items-center mb-3">
                <span class="text-muted">Đơn hàng</span>
                <span class="badge badge-secondary badge-pill">1</span>
            </h4>
            <ul class="list-group mb-3">
                <li class="list-group-item d-flex justify-content-between lh-condensed">
                    <div>
                        <h6 class="my-0">Tên sản phẩm</h6>
                        <small class="text-muted">Thanh toán hóa đơn</small>
                    </div>
                    <span class="text-muted">100.000₫</span>
                </li>
                <li class="list-group-item d-flex justify-content-between">
                    <span>Tổng tiền thanh toán</span>
                    <strong>100.000₫</strong>
                </li>
            </ul>
        </div>

        <form id="form2" runat="server">
            <div class="col-md-8 order-md-1">

                <h4>Chọn phương thức thanh toán:</h4>

                <div class="d-block my-3">
                     <h5 class="mb-3">Cách 1: Chuyển hướng sang VNPAY chọn phương thức thanh toán</h5>
                    <div class="custom-control custom-radio">
                      <asp:RadioButton id="bankcode_Default" Text="Cổng thanh toán VNPAYQR" Checked="True" GroupName="RadioGroup1" runat="server" /><br />
                    </div>

                    <h5 class="mb-3">Cách 2: Tách phương thức thanh toán tại site của Merchant</h5>
                    <div class="custom-control custom-radio">
                       <asp:RadioButton id="bankcode_Vnpayqr" Text="Thanh toán qua ứng dụng hỗ trợ VNPAYQR" GroupName="RadioGroup1" runat="server"/><br />
                       
                    </div>
                    <div class="custom-control custom-radio">
                      <asp:RadioButton id="bankcode_Vnbank" Text="ATM-Tài khoản ngân hàng nội địa" GroupName="RadioGroup1" runat="server"/><br />
                    </div>
                    <div class="custom-control custom-radio">
                       <asp:RadioButton id="bankcode_Intcard" Text="Thanh toán qua thẻ quốc tế" GroupName="RadioGroup1" runat="server"/><br />
                    </div>

                    <h4>Chọn ngôn ngữ thanh toán:</h4>
                     <div class="custom-control custom-radio">
                      <asp:RadioButton id="locale_Vn" Text="Tiếng việt" Checked="True" GroupName="RadioGroup2" runat="server" /><br />
                      <asp:RadioButton id="locale_En" Text="Tiếng anh" GroupName="RadioGroup2" runat="server" /><br />
                    </div>

                     <asp:Button ID="btnPay" runat="server" Text="Thanh toán (Redirect)" CssClass="btn btn-default" OnClick="btnPay_Click" />
                </div>

            </div>

            
     </form>

        </div>

        <p>
          &nbsp;
        </p>
       <footer class="footer">
          <p>&copy; VNPAY 2020</p>
       </footer>

    </div>

</body>
</html>