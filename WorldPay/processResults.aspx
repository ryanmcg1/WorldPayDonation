<%@ Page Language="C#" AutoEventWireup="true" CodeFile="processResults.aspx.cs" Inherits="CMSTemplates_Website_2014_Redesign_WorldPay_processResults"
    MasterPageFile="~/CMSTemplates/Website 2014 Redesign/Masters/LowerPage.master" %>

<asp:Content ID="donationReturns" ContentPlaceHolderID="cplhRightContent" runat="server" >

<div id="wrapper" class="donation-wrapper">    
    <asp:ValidationSummary ID="valSumm" CssClass="errorMessage" runat="server"  />
    <ul class="breadcrumb-donate">
	    <li><p>1. Donation Details</p></li>
	    <li><p>2. Payment Details</p></li>
	    <li><p class="active">3. Donation Summary</p></li>
    </ul>
</div>


<asp:Panel ID="pnlResult" runat="server" Visible="false">
    <div class="box">
      <h2>Thank you, your donation has been successfully completed!</h2>
      <table class="donations">
        <thead>
          <tr>
            <th>Donation</th>
            <th class="amount">Amount</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
              <h3><asp:Literal ID="ltlDonationResultTitle" runat="server" EnableViewState="false"></asp:Literal></h3>
              <p><asp:Literal ID="ltlDonationResultMessage" runat="server" EnableViewState="false"></asp:Literal></p>
            </td>
            <td class="amount">&pound;<asp:Literal ID="litDonationAmmountRestult" runat="server" ></asp:Literal></td>
          </tr>
        </tbody>
      </table>
      <table class="donations donations-total">
        <tr>
          <td class="title">Gift Aid to be claimed</td>
          <td class="amount">&pound;<asp:Literal ID="litGiftAidResult" runat="server" ></asp:Literal></td>
        </tr>
        <tr>
          <td class="title">Total value of donation (including Gift Aid)</td>
          <td class="amount amount-total">&pound;<asp:Literal ID="litTotalResult" runat="server" ></asp:Literal></td>
        </tr>
      </table>
            <p>
                <cms:CMSEditableRegion ID="erSummmary" runat="server" EnableViewState="false" RegionType="HtmlEditor" />
            </p>
      <a href="~/Donations" class="button">Return to Donations</a>
    </div>
    </asp:Panel>

    <%--//Failed--%>
    <asp:Panel ID="pnlResultError" runat="server" Visible="false">
           <div class="box">
          <h2>Sorry there was a problem with your donation.</h2> 
          <p><asp:Literal ID="litReturnError" runat="server"></asp:Literal></p>
          <cms:CMSEditableRegion ID="erFailSummary" runat="server" EnableViewState="false" RegionType="HtmlEditor" />
          <a href="~/Donations" class="button">Return to Donations</a>
        </div>
    </asp:Panel>

    <%--//Cancelled--%>
    <asp:Panel ID="pnlResultCancelled" runat="server" Visible="false">
           <div class="box">
          <h2>Sorry there was a problem with your donation.</h2> 
          <p><asp:Literal ID="litErrorCancelled" runat="server" Visible="false"></asp:Literal></p>
          <cms:CMSEditableRegion ID="erCancelled" runat="server" EnableViewState="false" RegionType="HtmlEditor" />
          <a href="~/Donations" class="button">Return to Donations</a>
        </div>
    </asp:Panel>

</asp:Content>