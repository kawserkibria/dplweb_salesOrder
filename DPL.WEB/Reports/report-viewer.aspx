<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report-viewer.aspx.cs" Inherits="DPL.WEB.Reports.report_viewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server"  >
<%--<div  style="height:100%; width:100%;  overflow:auto">
     
                    <CR:CrystalReportViewer ID="ctrlReportViewer" 
          runat="server" AutoDataBind="true"  ToolPanelView="None"  
          style="height:100%; width:100%;"   />
</div>--%>

        <div style="width:1200px; margin-left: auto; margin-right: auto;">
        <CR:CrystalReportViewer ID="ctrlReportViewer" runat="server" 
        ToolPanelView="None" BestFitPage="False"  style="height:100%; width:100%;" />
</div>
</form>
</body>
</html>
