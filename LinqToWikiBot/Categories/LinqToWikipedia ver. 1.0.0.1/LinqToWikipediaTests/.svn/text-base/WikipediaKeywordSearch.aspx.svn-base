<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WikipediaKeywordSearch.aspx.cs" Inherits="LinqToWikipediaTests.WikipediaKeywordSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        input {font-size:8pt}
    </style>
</head>
<body style="font-family:Arial;font-size:10pt">
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

        <p style="text-align:center"><b>LinqToWikipedia</b> Keyword Search Live Demo</p>

        <p style="text-align:center;font-size:8pt">
        View the <a href="WikipediaOpenSearch.aspx">OpenSearch</a> Demo</p>
        
        <table border="0" align="center" width="700">
        <tr>
            <td>
                Select number of results to display - <i>Linq -&gt; Take()</i>: -&gt; &nbsp;
                
                <asp:DropDownList ID="ddl_results" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UpdateDisplayNumber" /> <br /><br />
                
                Select starting record number - <i>Linq -&gt; Skip()</i>: -&gt; &nbsp;
                
                <asp:DropDownList ID="ddl_skip" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UpdateSkipNumber" /> <br /><br />
                
                Enter multiple keywords:<br />
                
                <asp:TextBox ID="tb_keyword1" Width="50" runat="server" /> 
                <asp:TextBox ID="tb_keyword2" Width="50" runat="server" /> 
                <asp:TextBox ID="tb_keyword3" Width="50" runat="server" /> 
                <asp:TextBox ID="tb_keyword4" Width="50" runat="server" /> 
                <asp:TextBox ID="tb_keyword5" Width="50" runat="server" /> 
                
                <asp:Button ID="btn_keywordsearch" runat="server" Text="Query" />
                <br /><br />
                
                <asp:Panel ID="pnl_totalrecords" runat="server">
                
                    Total Records: <asp:Label ID="lbl_totalrecords" runat="server" Text="0" /><br /><br />
                
                </asp:Panel>
                   
                <asp:DataList ID="dl_results" runat="server">
                    <ItemTemplate>
                        <table border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <b><a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" target="_blank">
                                    <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                    </a></b>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size:8pt">(<%# DataBinder.Eval(Container.DataItem, "Description")%>)</td>
                            </tr>
                            <tr>
                                <td style="font-size:8pt">
                                    Word count: <%# DataBinder.Eval(Container.DataItem, "WordCount")%> 
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size:8pt">
                                    <i>Last updated: <%# DataBinder.Eval(Container.DataItem, "TimeStamp")%></i>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </ItemTemplate>
                </asp:DataList>
   
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
