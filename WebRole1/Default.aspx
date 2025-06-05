<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebRole1.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Azure Cloud Service Demo</title>
    <style>
        body {
            font-family: Segoe UI, Arial;
            margin: 20px;
            line-height: 1.6;
        }
        .container {
            max-width: 800px;
            margin: 0 auto;
        }
        .info-section {
            margin-bottom: 20px;
            padding: 15px;
            background-color: #f0f8ff;
            border-radius: 5px;
        }
        .button-container {
            margin: 20px 0;
        }
        .result-label {
            font-weight: bold;
            margin-right: 5px;
        }
        .result-value {
            color: #0066cc;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="info-section">
                <h2>Тестовий проєкт Windows Azure</h2>
                <p>
                    Цей проєкт демонструє особливості роботи Windows Azure. 
                    Фоновий сервіс отримує інформацію про сервер, на якому він знаходиться,
                    а веб-інтерфейс відображає цю інформацію.
                </p>
            </div>
            
            <div class="button-container">
                <asp:Button 
                    ID="GetInfo" 
                    runat="server" 
                    Text="Отримати інформацію" 
                    OnClick="GetInfo_Click" 
                    CssClass="btn-get-info" />
            </div>
            
            <div class="results">
                <p>
                    <asp:Label 
                        ID="Label1" 
                        runat="server" 
                        Text="IP адреса: " 
                        CssClass="result-label"></asp:Label>
                    <asp:Label 
                        ID="IP_Label" 
                        runat="server" 
                        CssClass="result-value"></asp:Label>
                </p>
                <p>
                    <asp:Label 
                        ID="Label2" 
                        runat="server" 
                        Text="DNS ім'я: " 
                        CssClass="result-label"></asp:Label>
                    <asp:Label 
                        ID="DNS_Label" 
                        runat="server" 
                        CssClass="result-value"></asp:Label>
                </p>
            </div>
        </div>
    </form>
</body>
</html>