<%@ Page Title="Карты" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Cards.aspx.cs" Inherits="Cards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <asp:DataList ID="CardsList" runat="server" DataSourceID="SqlCards"
        DataKeyField="card_id" OnSelectedIndexChanged="CardsList_SelectedIndexChanged"
        OnPreRender="CardsList_PreRender" ShowFooter="False">
        <HeaderTemplate>
            Подключенные карты
        </HeaderTemplate>
        <ItemTemplate>
            <b>
                <%# Eval("card_number") %></b>
            <%# Eval("card_type_name") %>
            <%# Eval("exp_date", "{0:MM'/'yy}") %><br />
            <small><center><font color=gray>(Статус: <%# Eval("card_status") %>)</font></center></small>
            <table width="100%">
                <tr>
                    <td>
                        Доступно:
                    </td>
                    <td align="right">
                        <b><%# Eval("amount_available", "{0:F2}") %></b>
                        <%# Eval("card_currency") %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Блокир.:
                    </td>
                    <td align="right">
                        <b><%# Eval("amount_on_hold", "{0:F2}") %></b>
                        <%# Eval("card_currency") %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Задолж.:
                    </td>
                    <td align="right">
                        <b><%# Eval("amount_debt", "{0:F2}") %></b>
                        <%# Eval("card_currency") %>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="MoreButton" runat="server" Text="Подробнее" CommandName="select" /></div>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <FooterTemplate>
            Вы подключились к системе, но, как видно, у Вас нет на данный
            момент подключенных карт, которые были бы сейчас действительны.
            <br />
            Пожалуйста, позвоните в Банк и уточните этот вопрос.
        </FooterTemplate>
    </asp:DataList>
    <asp:DataList ID="TransList" runat="server" DataSourceID="SqlTrans"
        Width="100%" >
        <HeaderTemplate>
            Операции за
            <%# RepDate.ToString("MMMM yy") %>
            <asp:Button ID="FilterButton1" runat="server" Text="<<" CommandName="prev"
                CommandArgument="-1" OnCommand="FilterButton_Command" />
            <asp:Button ID="FilterButton2" runat="server" Text=">>" CommandName="next"
                CommandArgument="1" OnCommand="FilterButton_Command" />
            <asp:Button ID="FilterButton3" runat="server" Text=">|" CommandName="today"
                CommandArgument="0" OnCommand="FilterButton_Command" />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Label ID="Date" runat="server" Text='<%# Eval("transaction_date", "{0:dd.MM}") %>' />
            <asp:Label ID="Sum" runat="server" Text='<%# Eval("amount", "{0:F2}") %>' Font-Bold="true"
                /><asp:Label ID="Cur" runat="server" Text='<%# Eval("operation_currency_iso") %>' />
            <table>
                <tr>
                    <td>
                        <%# Eval("comment") %>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <FooterTemplate>
            Итого операций:
            <%# TransList.Items.Count.ToString() %>
        </FooterTemplate>
    </asp:DataList>
    
    <asp:DataList ID="HoldsList" runat="server" DataSourceID="SqlHolds"
        OnPreRender="HoldsList_PreRender" ShowHeader="False" Width="100%">
        <HeaderTemplate>
            Блокировки по карте
        </HeaderTemplate>
        <ItemTemplate>
            <%# Eval("operation_date", "{0:dd.MM}")%>
            <b><%# Eval("amount", "{0:F2}") %></b>
            <%# Eval("operation_currency_iso") %>
            <table>
                <tr>
                    <td>
                        <%# Eval("comment") %>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
    <asp:DetailsView ID="DetailDebts" runat="server" AutoGenerateRows="False"
        DataSourceID="SqlDebts">
        <HeaderTemplate>
            Задолженность по валютам
        </HeaderTemplate>
        <Fields>
            <asp:BoundField DataField="amount_810" DataFormatString="{0:F2}RUR"
                HeaderText="Рубли РФ:" SortExpression="amount_810">
                <ItemStyle HorizontalAlign="Right" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="amount_840" DataFormatString="{0:F2}USD"
                HeaderText="Доллары США:" SortExpression="amount_840">
                <ItemStyle HorizontalAlign="Right" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="amount_978" DataFormatString="{0:F2}EUR"
                HeaderText="Евро:" SortExpression="amount_978">
                <ItemStyle HorizontalAlign="Right" Wrap="False" />
            </asp:BoundField>
        </Fields>
    </asp:DetailsView>
    <br />
    <asp:Button ID="BackButton" runat="server" Text="Карты" OnClick="BackButton_Click"
        Visible="False" />
    <asp:SqlDataSource ID="SqlCards" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
        SelectCommand="wsp_dm_pc_get_cards_num" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="client_login" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlTrans" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
        SelectCommand="wsp_dm_pc_get_operations" SelectCommandType="StoredProcedure"
        FilterExpression="transaction_date&gt;='{0}' and transaction_date&lt;'{1}'">
        <SelectParameters>
            <asp:Parameter Name="client_login" Type="String" />
            <asp:ControlParameter ControlID="CardsList" Name="card_id" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlHolds" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
        SelectCommand="wsp_dm_pc_get_on_hold" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="client_login" Type="String" />
            <asp:ControlParameter ControlID="CardsList" Name="card_id" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDebts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
        SelectCommand="wsp_dm_pc_get_debt" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="client_login" Type="String" />
            <asp:ControlParameter ControlID="CardsList" Name="card_id" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
