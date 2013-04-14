<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WikipediaOpenSearch.aspx.cs" Inherits="LinqToWikipediaTests.WikipediaOpenSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
        <style type="text/css">
        input {font-size:8pt}
    </style>
    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= tb_OpenSearch.ClientID %>', '');
        }; 
    
    </script>
</head>
<body style="font-family:Arial;font-size:10pt" onload="document.getElementById('<%= tb_OpenSearch.ClientID %>').focus()">
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

        <p style="text-align:center"><b>LinqToWikipedia</b> Open Search Live Demo</p>
        
        <p style="text-align:center;font-size:8pt">
        View the <a href="WikipediaKeywordSearch.aspx">Keyword Search</a> Demo</p>

        <table border="0" align="center" width="700">
        <tr>
            <td>
                Select number of results to display - <i>Linq -&gt; Take()</i>: -&gt; &nbsp;
                
                <asp:DropDownList ID="ddl_results" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UpdateDisplayNumber" /> <br /><br />
                
                Begin typing in the query box to view results: -&gt; &nbsp;
                
                <asp:TextBox ID="tb_OpenSearch" EnableViewState="false" runat="server" onkeyup="RefreshUpdatePanel();" AutoPostBack="true" OnTextChanged="OpenSearch_TextChanged"></asp:TextBox> 
                <br /><br />
                
                <asp:UpdatePanel ID="Update" runat="server"> 
                
                    <ContentTemplate>  
                        <asp:DataList ID="dl_results" runat="server">
                            <ItemTemplate>
                                <table border="0" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <%# CheckImage((Uri)DataBinder.Eval(Container.DataItem, "ImageUrl"), (Uri)DataBinder.Eval(Container.DataItem, "Url"))%>
                                        <td>
                                            <b><a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" target="_blank">
                                            <%# DataBinder.Eval(Container.DataItem, "Text")%>
                                            </a></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>(<span style="font-size:8pt"><%# DataBinder.Eval(Container.DataItem, "Description")%></span>)</td>
                                    </tr>
                                </table>
                                <br />
                            </ItemTemplate>
                        </asp:DataList>
                    </ContentTemplate> 
                    
                    <Triggers> 
                        <asp:AsyncPostBackTrigger ControlID="tb_OpenSearch" /> 
                        <asp:AsyncPostBackTrigger ControlID="ddl_results" />
                    </Triggers> 
                
                </asp:UpdatePanel> 
            </td>
        </tr>
        </table>
        
        <p style="text-align:center;font-size:8pt">
        Go to the <a href="http://linqtowikipedia.codeplex.com/">LinqToWikipedia Codeplex</a> project site<br />
        For more information visit the <a href="">LinqToWikipedia Blog</a><br /><br />
        <i><a href="http://www.geekswithblogs.net/ballhaus" target="_blank">Michael Ballhaus</a></i></p>
        
    </div>
    </form>
</body>
</html>
