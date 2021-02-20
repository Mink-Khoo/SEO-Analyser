<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SEO_Analyser.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SEO Analyser</title>
    <script>
        function checkInput() {
            var input = document.getElementById("tbInput");
            var regex = new RegExp('<\/?[a-z|!][\s\S]*');
            if (regex.test(input.value)) {
                alert('** Input value cannot be HTML text.');
                input.value = "";
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1 style="font-family: Calibri; text-align: center">SEO ANALYSER</h1>
        </div>
        <div>
            <table id="InputTable" style="font-family: Calibri; width: 50%; margin-left: 25%;">

                <tr>
                    <td>
                        <hr style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rbTextMode" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbTextMode_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Text">English Text</asp:ListItem>
                            <asp:ListItem>URL</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="tbInput" runat="server" Height="200px" Width="100%" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return checkInput()" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cbIsCalculateOccuranceInText" runat="server" Text="Calculates number of occurrences of each word in Text" Checked="True" /><br />
                        <asp:CheckBox ID="cbIsCalculateOccuranceInMetaTag" runat="server" Text="Calculates number of occurrences of each word listed in meta tags" Checked="True" /><br />
                        <asp:CheckBox ID="cbIsCalculateExternalLink" runat="server" Text="Calculates number of external links" Checked="True" /><br />
                        <asp:CheckBox ID="cbIsFilterStopWords" runat="server" Text="Filter out stop-words as below:" Checked="True" />
                        <!--Reference website: https://www.likemind.media/seo-stop-words-guide-2020-including-list/ -->
                        <asp:TextBox ID="tbStopWords" runat="server" TextMode="MultiLine" Height="130px" Width="100%">a about above after again against all am an and any are aren&#39;t as at be because been before being below between both but by can&#39;t cannot could couldn&#39;t did didn&#39;t do does doesn&#39;t doing don&#39;t down during each few for from further had hadn&#39;t has hasn&#39;t have haven&#39;t having he he&#39;d he&#39;ll he&#39;s her here here&#39;s hers herself him himself his how how&#39;s i i&#39;d i&#39;ll i&#39;m i&#39;ve if in into is isn&#39;t it it&#39;s its itself let&#39;s me more most mustn&#39;t my myself no nor not of off on once only or other ought our ours ourselves out over own same shan&#39;t she she&#39;d she&#39;ll she&#39;s should shouldn&#39;t so some such than that that&#39;s the their theirs them themselves then there there&#39;s these they they&#39;d they&#39;ll they&#39;re they&#39;ve this those through to too under until up very was wasn&#39;t we we&#39;d we&#39;ll we&#39;re we&#39;ve were weren&#39;t what what&#39;s when when&#39;s where where&#39;s which while who who&#39;s whom why why&#39;s with won&#39;t would wouldn&#39;t you you&#39;d you&#39;ll you&#39;re you&#39;ve your yours yourself yourselves</asp:TextBox><br />
                        <asp:Label ID="lblStopWordsWarning" runat="server" ForeColor="Red" Text="** Add stop-word in above field.<br/>** All stop-words must be seperated with whitespace."></asp:Label>

                    </td>
                </tr>
            </table>
            <hr style="width: 100%;" />
            <div style="width: 30%; float: left; margin-left: 5%;">
                <h4 style="text-align: center">
                    <asp:Label ID="lblWordResult" runat="server" Text=""></asp:Label></h4>
                <asp:GridView ID="gvOccuranceInText" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" OnRowDataBound="gvOccuranceInText_RowDataBound" OnSorting="gvOccuranceInText_Sorting">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>
            <div style="width: 30%; float: left">
                <h4 style="text-align: center">
                    <asp:Label ID="lblTagResult" runat="server" Text=""></asp:Label></h4>
                <asp:GridView ID="gvOccuranceInMetaTag" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" OnRowDataBound="gvOccuranceInMetaTag_RowDataBound" OnSorting="gvOccuranceInMetaTag_Sorting">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
            </div>
            <div style="width: 30%; float: left">
                <h4 style="text-align: center">
                    <asp:Label ID="lblExternalLinkCount" runat="server" Text=""></asp:Label></h4>

                <asp:GridView ID="gvExternalLink" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" OnRowDataBound="gvExternalLink_RowDataBound" OnSorting="gvExternalLink_Sorting">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
