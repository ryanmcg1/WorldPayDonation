<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Donation.aspx.cs" Inherits="CMSTemplates_Website_2014_Redesign_Donation" 
    MasterPageFile="~/CMSTemplates/Website 2014 Redesign/Masters/LowerPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cplhRightContent" runat="Server">
    <div id="donation-wrapper">
    
        <div id="banner">
            <cms:CMSEditableImage ID="eiTopBar" runat="server" AlternateText="Top Banner Image" ImageTitle="Top Banner Image" />
           <%-- <div id="banner-logo"></div>
            <div id="banner-text"><cms:CMSEditableRegion ID="erTitle" runat="server" RegionType="TextBox" RegionTitle="Image Title" /></div>--%>
        </div>
    
        <div id="intro-wrap">
            <div class="main-left">

            <h1><cms:CMSDocumentValue ID="title" runat="server" AttributeName="DocumentName" /></h1>
            <p><cms:CMSEditableRegion ID="erInfo" runat="server" EnableViewState="false" RegionType="HtmlEditor" DialogHeight="1000" RegionTitle="Main Content" /></p>
            </div>

            <div class="main-right">
            <div id="donate-info">
                <div id="pnlTotalRaised" runat="server" class="donation-intro" visible="false">
                    <h2>&pound;<cms:LocalizedLiteral ID="lblTotalRaised" runat="server" /></h2>

                    <asp:PlaceHolder ID="plcTargetRaised" runat="server" Visible="false">
                        <p>of &pound;<cms:LocalizedLiteral ID="DonationTarget" runat="server" EnableViewState="false"></cms:LocalizedLiteral> target raised</p>
                    </asp:PlaceHolder>

                </div>
                <br />
                
                <div id="pnlDonationsMafe" runat="server" class="donation-intro" visible="false">

                    <h2><cms:LocalizedLiteral ID="DonationsMade" runat="server" EnableViewState="false"></cms:LocalizedLiteral></h2>
                    <p>donations made</p>
                </div>
          
                <asp:PlaceHolder ID="plcAcceptedDate" runat="server" Visible="false">
                    <div class="donation-intro no-margin">
                        <p>Donations are accepted until: <br/><span><cms:LocalizedLiteral ID="lblTargetDate" runat="server" /></span></p>
                    </div>
                </asp:PlaceHolder>
                <a href="~/donation.aspx?t=<cms:CMSDocumentValue ID="title2" runat="server" AttributeName="DocumentName" />" class="button donateButton">Donate Now</a>
                
          
         
            </div>
            <asp:PlaceHolder ID="plcTarget" runat="server" Visible="false">
                <div id="donate-meter">
                    <div class="meter">
                        <div class="progress"></div>
                        <div class="counter"><span class="counter_number"><cms:LocalizedLiteral ID="lblPercentage" runat="server"></cms:LocalizedLiteral>%</span></div>
                    </div>
                </div>
            </asp:PlaceHolder>
            <!-- end of donate info -->
        <div class="contentBelowMeter">
            <cms:CMSEditableRegion ID="erRightBar" runat="server" RegionType="HtmlEditor" DialogHeight="500" RegionTitle="Right Bar" />
            </div>
            </div>
            <!-- end of main right -->
        </div>
        <!-- end of intro-wrap -->
    
        <cms:CMSUpdatePanel ID="upComments" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:HiddenField ID="hdnPageNumber" runat="server" Visible="false"/>
                        <div class="main-left">
        <h2>Donations</h2>
      
        <asp:Repeater ID="rptComments" runat="server">
            <HeaderTemplate>
                <ul class="donations">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <div class="donation-image"><img src="<%# Eval("DonerImage") %>"  /></div>
                    <div class="donation-content">
                        <div class="donation-comment">
                            <%# Eval("Comments") %>
                        </div>
                        <div class="donation-by"><%# Eval("DonerName") %><br />on <%# Eval("DonationDate") %></div>
                        <div class="donation-amount"><span><%# Eval("DonationAmount","{0:c}") %></span><%# Eval("GiftAidAmount","{0:c}") %> gift aid</div>
                    </div>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
      
        <ul class="pagination">
            <%--<asp:Literal ID="ltPagination" runat="server">1 of 2</asp:Literal>--%>
            <li><asp:LinkButton ID="lbBack" runat="server" OnClick="lbBack_Click" CssClass="page-arrows" /></li>
            <li><asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click" CssClass="page-arrows page-right" /></li>
        </ul>
    </div>
            </ContentTemplate>
        </cms:CMSUpdatePanel>

    
    <div class="main-right">
        <h2>Gallery</h2>
        <p class="helper">View larger images by clicking on the image below</p>
        <ul class="gallery">
            <asp:Repeater ID="rptImages" runat="server" >
                <ItemTemplate>
                    <li>
                        <a href="<%# Eval("DonerImage") %>" data-lightbox="gallery" data-title="<%# Eval("DonerName") %>">
                        <img class="item" src="<%# Eval("DonerImage") %>" />
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
      <asp:Panel ID="pnlSummary" runat="server" EnableViewState="false" Visible="false">
        <h2>Donation Summary</h2>
      
        <table class="donation-summary">
        <tr>
            <td class="type">Online donations:</td>
            <td class="amount">&pound;<cms:LocalizedLiteral ID="ltlTotalOnlineDontaions" runat="server" /></td>
        </tr>
        <tr>
            <td class="type">Gift Aid Claimed</td>
            <td class="amount">&pound;<cms:LocalizedLiteral ID="ltlTotalGiftCardDontaions" runat="server" /></td>
        </tr>
        <tr>
            <td class="type">Offline donations</td>
            <td class="amount">&pound;<cms:LocalizedLiteral ID="ltlTotalOfflineDontaions" runat="server" /></td>
        </tr>
        <tr>
            <td class="type"><strong>Total Donations</strong></td>
            <td class="amount">&pound;<cms:LocalizedLiteral ID="ltlTotalOverallDonations" runat="server" /></td>
        </tr>
        </table>
        </asp:Panel>
        <a href="~/donation.aspx?t=<cms:CMSDocumentValue ID="title3" runat="server" AttributeName="DocumentName" />" class="button button-full donateButton">Donate Now</a>
    </div>
        <div class="">
            <img src="~/app_themes/websiteDefault/images/poweredByWorldPay.gif" />
        </div>
    </div>

    <!-- enf of wrapper -->
    
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="~/js/lightbox.js"></script>
    
    <script>
        $(document).ready(function() {
      
            $(".progress").animate({height: $('.counter_number').text() }, 1500);
            $(".counter").animate({bottom: $('.counter_number').text() }, 1500);
      
            jQuery({ Counter: 0 }).animate({ Counter: $('.counter_number').text() }, {
            duration: 1500,
            step: function () {
                $('.counter_number').text(Math.ceil(this.Counter) + '%');
            }
            });
      
            $('ul.gallery li:nth-child(4n+4)').addClass('last');
      
        });
    </script>

</asp:Content>
