<%@ Page Title="Экспорт в Excel" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Excel.aspx.cs" Inherits="Excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <asp:GridView ID="TransGrid" runat="server" 
        Caption="Операции по карте" 
        DataSourceID="SqlTrans" >
    </asp:GridView>
    <asp:GridView ID="CardsGrid" runat="server" 
        Caption="Карточки" DataSourceID="SqlCards" 
        onselectedindexchanged="CardsGrid_SelectedIndexChanged" 
        DataKeyNames="card_id">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
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
            <asp:ControlParameter ControlID="CardsGrid" Name="card_id" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
