﻿<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ReportViewerWebForm.aspx.cs" Inherits="ReportViewerForMvc.ReportViewerWebForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        iframe {
  /*for the report viewer*/
  border: none;
  padding: 0;
  margin: 0;
  width: 100%;
  height: 100%;
}
    </style>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
          
        </div>  
<%--        <rsweb:ReportViewer ID="ReportViewer1" runat="server"  ></rsweb:ReportViewer>--%>

                <rsweb:ReportViewer  runat ="server"   Width="99.9%" Height="100%" AsyncRendering="true" ZoomMode="Percent" KeepSessionAlive="true" id="ReportViewer1" SizeToReportContent="true" >  
        </rsweb:ReportViewer>    
    </form>
</body>
</html>
