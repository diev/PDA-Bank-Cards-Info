<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Staff.aspx.cs"
    Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Банковские карты</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <asp:Panel ID="PagePanel" runat="server" HorizontalAlign="Left"
                Width="100%" Height="100%">
                <table width="100%" cellspacing="0" cellpadding="2">
                    <tr valign="top">
                        <td align="left">
                            <asp:Label ID="Logo" runat="server" Text="<%$ AppSettings:Logo %>"
                                SkinID="Logo" />
                            <asp:DropDownList ID="AdminUserList" runat="server" AutoPostBack="True"
                                Visible="false" OnSelectedIndexChanged="AdminUserList_SelectedIndexChanged" />
                            <asp:DropDownList ID="AdminActionList" runat="server" Visible="False">
                                <asp:ListItem Value="AddUser" Text="Добавить нового" Selected="True" />
                                <asp:ListItem Value="Comment" Text="Комментарий" />
                                <asp:ListItem Value="Email" Text="Email" />
                                <asp:ListItem Value="Unlock" Text="Разблокировать" />
                                <asp:ListItem Value="Password" Text="Сменить пароль" />
                                <asp:ListItem Value="ChEmail" Text="Сменить email" />
                                <asp:ListItem Value="Disable" Text="Запретить доступ" />
                                <asp:ListItem Value="Enable" Text="Разрешить доступ" />
                                <asp:ListItem Value="User" Text="Права клиента" />
                                <asp:ListItem Value="Operator" Text="Права оператора" />
                                <asp:ListItem Value="Admin" Text="Права админа" />
                                <asp:ListItem Value="Delete" Text="Удалить насовсем" />
                            </asp:DropDownList>
                            <asp:Button ID="ActionButton" runat="server" Text="OK" CommandName="Action"
                                OnCommand="AdminButton_Command" Visible="False" />
                            <asp:Button ID="NextUserButton" runat="server" Text="&gt;&gt;"
                                CommandName="NextUser" OnCommand="AdminButton_Command" Visible="False" />
                        </td>
                        <td align="right">
                            <asp:LoginStatus ID="UserStatus" runat="server" LogoutAction="Redirect"
                                LogoutPageUrl="~/Default.aspx" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="AddUserPanel" runat="server" Visible="False" SkinID="Admin"
                    DefaultButton="AddUserButton1">
                    <table width="100%">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="AddUserNameLabel" runat="server" Text="Логин" />
                                <asp:TextBox ID="AddUserName" runat="server" MaxLength="20" TabIndex="1" />
                                <asp:Label ID="AddUserPassLabel" runat="server" Text="Пароль" />
                                <asp:TextBox ID="AddUserPass" runat="server" MaxLength="20" TabIndex="2" />
                                <asp:Label ID="AddUserEmailLabel" runat="server" Text="Email" />
                                <asp:TextBox ID="AddUserEmail" runat="server" MaxLength="256"
                                    TabIndex="3" />
                            </td>
                            <td align="right">
                                <asp:Button ID="AddUserButton1" runat="server" Text="OK" CommandName="AddUserOK"
                                    OnCommand="AdminButton_Command" TabIndex="4" />
                                <asp:Button ID="AddUserButton2" runat="server" Text="Отменить"
                                    CommandName="AdminCancel" OnCommand="AdminButton_Command"
                                    TabIndex="5" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="BigTextPanel" runat="server" Visible="False" SkinID="Admin"
                    DefaultButton="BigTextButton1">
                    <table width="100%">
                        <tr valign="top">
                            <td>
                                <asp:TextBox ID="BigTextBox" runat="server" TextMode="MultiLine"
                                    Rows="5" MaxLength="3000" Width="98%" TabIndex="1" />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right">
                                <asp:Button ID="BigTextButton1" runat="server" Text="OK" CommandName="BigTextOK"
                                    OnCommand="AdminButton_Command" TabIndex="2" />
                                <asp:Button ID="BigTextButton2" runat="server" Text="Отменить"
                                    CommandName="AdminCancel" OnCommand="AdminButton_Command"
                                    TabIndex="3" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="ConfirmPanel" runat="server" Visible="False" SkinID="Admin"
                    DefaultButton="ConfirmButton1">
                    <table width="100%">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="ConfirmLabel" runat="server" />
                            </td>
                            <td align="right">
                                <asp:Button ID="ConfirmButton1" runat="server" Text="OK" CommandName="ConfirmOK"
                                    OnCommand="AdminButton_Command" TabIndex="1" />
                                <asp:Button ID="ConfirmButton2" runat="server" Text="Отменить"
                                    CommandName="AdminCancel" OnCommand="AdminButton_Command"
                                    TabIndex="2" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="MsgPanel" runat="server" Visible="false" SkinID="Message"
                    DefaultButton="MsgOK">
                    <table width="100%">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="Msg" runat="server" />
                            </td>
                            <td align="right">
                                <asp:Button ID="MsgOK" runat="server" Text="OK" CommandName="MessageOK"
                                    OnCommand="AdminButton_Command" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Content" runat="server">
                    <asp:GridView ID="GridCards" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="card_id,card_number,card_type_name" DataSourceID="SqlCards"
                        OnRowDataBound="GridCards_RowDataBound" AllowSorting="True"
                        EmptyDataText="Нет подключенных карт" 
                        onrowcommand="GridCards_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Карта" SortExpression="card_number">
                                <ItemTemplate>
                                    <asp:Label ID="CardNo" runat="server" />
                                    <asp:Label ID="CardType" runat="server" />
                                    <asp:Label ID="CardExp" runat="server" />
                                    <asp:Label ID="CardStatus" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Доступно" SortExpression="amount_available">
                                <ItemTemplate>
                                    <asp:Label ID="CardAmount" runat="server" />
                                    <asp:LinkButton ID="CardAmountQ" runat="server" Text="?"
                                        CommandName="select" CommandArgument="Trans" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Блокировано" SortExpression="amount_on_hold">
                                <ItemTemplate>
                                    <asp:Label ID="CardHold" runat="server" />
                                    <asp:LinkButton ID="CardHoldQ" runat="server" Text="?" Visible="false"
                                        CommandName="select" CommandArgument="Holds" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Задолженность" SortExpression="amount_debt">
                                <ItemTemplate>
                                    <asp:Label ID="CardDebt" runat="server" />
                                    <asp:LinkButton ID="CardDebtQ" runat="server" Text="?" Visible="false"
                                        CommandName="select" CommandArgument="Debts" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="GridTrans" runat="server" AutoGenerateColumns="False"
                        DataSourceID="SqlTrans" AllowPaging="True" AllowSorting="True"
                        OnRowDataBound="GridTrans_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Дата" SortExpression="operation_date">
                                <ItemTemplate>
                                    <asp:Label ID="TranDate" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Сумма" SortExpression="amount">
                                <ItemTemplate>
                                    <asp:Label ID="TranAmount" runat="server" Font-Bold="true" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Вал." SortExpression="operation_currency_iso">
                                <ItemTemplate>
                                    <asp:Label ID="TranCur" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Операция" SortExpression="comment">
                                <ItemTemplate>
                                    <asp:Label ID="TranComment" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="GridHolds" runat="server" 
                        AllowPaging="True" AllowSorting="True" 
                        AutoGenerateColumns="False" DataSourceID="SqlHolds" 
                        OnRowDataBound="GridHolds_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Дата" 
                                SortExpression="operation_date">
                                <ItemTemplate>
                                    <asp:Label ID="HoldDate" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Сумма" 
                                SortExpression="amount">
                                <ItemTemplate>
                                    <asp:Label ID="HoldAmount" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Блокировка" 
                                SortExpression="comment">
                                <ItemTemplate>
                                    <asp:Label ID="HoldComment" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="Excel" runat="server" onclick="Excel_Click" 
                        Text="Экспорт в Microsoft Excel" Visible="False" />
                    <asp:DetailsView ID="DetailsDebts" runat="server" AutoGenerateRows="False"
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
                    <asp:SqlDataSource ID="SqlCards" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
                        SelectCommand="wsp_dm_pc_get_cards_num" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="AdminUserList" Name="client_login"
                                PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlTrans" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
                        SelectCommand="wsp_dm_pc_get_operations" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="AdminUserList" Name="client_login"
                                PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="GridCards" Name="card_id" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlHolds" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
                        SelectCommand="wsp_dm_pc_get_on_hold" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="AdminUserList" Name="client_login"
                                PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="GridCards" Name="card_id" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDebts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionDataMart %>"
                        SelectCommand="wsp_dm_pc_get_debt" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="AdminUserList" Name="client_login"
                                PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="GridCards" Name="card_id" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
        </center>
    </div>
    </form>
</body>
</html>
