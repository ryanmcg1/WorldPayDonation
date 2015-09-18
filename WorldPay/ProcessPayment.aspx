<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessPayment.aspx.cs" Inherits="CMSTemplates_Website_2014_Redesign_WorldPay_ProcessPayment" 
    MasterPageFile="~/CMSTemplates/Website 2014 Redesign/Masters/LowerPage.master" %>

<asp:Content ID="donations" ContentPlaceHolderID="cplhRightContent" runat="server" >
<div id="wrapper" class="donation-wrapper">    
    <asp:ValidationSummary ID="valSumm" CssClass="errorMessage" runat="server"  />
    <ul class="breadcrumb-donate">
	    <li><p class='active'">1. Donation Details</p></li>
	    <li><p>2. Payment Details</p></li>
	    <li><p>3. Donation Summary</p></li>
    </ul>
<asp:Panel ID="pnl1" runat="server">
    <div class="box">
      <h2>Donation Amount</h2>
      <div class="donation-left">
        <p>I would like to make a donation for the amount of:</p>
        <div class="no-checkedselector">
            <div class="toggle-container">
                <asp:RadioButton ID="toggle5" runat="server"  GroupName="donations" CssClass="toggle" />
                <asp:Label runat="server" AssociatedControlID="toggle5" CssClass="radio-label">£5</asp:Label>
                
                <asp:RadioButton ID="toggle10" runat="server"  GroupName="donations" CssClass="radio-toggle" />
                <asp:Label runat="server" AssociatedControlID="toggle10" CssClass="radio-label">£10</asp:Label>

                <asp:RadioButton ID="toggle50" runat="server"  GroupName="donations" CssClass="toggle" />
                <asp:Label ID="Label1" runat="server" AssociatedControlID="toggle50"  CssClass="radio-label">£50</asp:Label>

                <asp:RadioButton ID="toggle100" runat="server"  GroupName="donations" CssClass="toggle" />
                <asp:Label ID="Label2" runat="server" AssociatedControlID="toggle100" CssClass="radio-label">£100</asp:Label>
            </div>
        </div>
        
        
        <div class="or_vertical"><span>or</span></div>
        
        <label class="text-label">Enter amount in &pound;'s<small>eg. 15.00</small></label>
        <asp:TextBox ID="txtDonationAmount" runat="server" CssClass="donationAmount"></asp:TextBox>
          <asp:RequiredFieldValidator ID="reqTotalAmmount" runat="server"  ControlToValidate="txtDonationAmount" ErrorMessage="Please enter an amount">&nbsp;</asp:RequiredFieldValidator>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDonationAmount" ErrorMessage="Donation amount must be in currency format" ValidationExpression="^\d+\.?\d?\d$">*</asp:RegularExpressionValidator>

        <p></br/>
<p>
	TEST</p></p>
      </div>
      
      <div class="donation-right">
        <p>I would like to make this donation to:</p>

        <asp:DropDownList ID="drpDonations" runat="server" CssClass="donation_to">
        </asp:DropDownList>
        
        <asp:CheckBox id="corporate" runat="server" />
        <asp:Label runat="server" AssociatedControlID="corporate" class="css-label">Corporate Donation </asp:Label>
        <p class="checkbox">This donation is on behalf of a company.</p>

          <div class="giftAid">
        <asp:CheckBox id="gift_aid" runat="server" />
        <asp:Label runat="server"  AssociatedControlID="gift_aid">Gift Aid Donation</asp:Label>
        <p class="checkbox">Yes, I am a UK tax payer and I would like The PO Charity to treat all the donations I make from 1 April 2009, 
            until I notify you otherwise, as Gift Aid donations.
            I confirm I have paid or will pay an amount of Income Tax and/or Capital Gains Tax for the current tax year (6 April to 5 April) 
            that is at least equal to the amount of tax that all the charities and Community Amateur Sports Clubs (CASCs) that I donate to 
            will reclaim on my gifts for the current tax year.  I understand that other taxes such as VAT and Council Tax do not qualify.  
            I understand the OUR Charity will reclaim 25p of tax on every £1 that I have given. For more information on Gift Aid visit the 
            <a target="_blank" href="http://www.hmrc.gov.uk/individuals/giving/gift-aid.htm">Gift Aid page on HMRC website</a>
        </p>
    </div>
    </div>
</div>
    <div class="box">
      <h2>Personalise Donation</h2>
      
      <div class="donation-wide">
        <p>Please use the comment box below to tell us about your donation or fundraising activity.</p>
        
        <textarea id="userComment" name="comments" runat="server" onkeyup="textCounter(this,'counter',300);" maxlength="300"></textarea>
        <div id="characters-remaining">
            <div id="character-left">(Maximum characters: 300)</div>
            <div id="character-right">Remaining: <span class="remaining-characters"><input disabled  maxlength="3" size="3" value="300" id="counter"></span></div>
        </div>


        <asp:CheckBox id="use_comment" runat="server" />
        <asp:Label runat="server"  AssociatedControlID="use_comment">Allow use of Comments</asp:Label>
          
        <p class="checkbox">Yes, I am happy for my comments to be used for your publicity. I understand that comments and image will be moderated before appearing on the website. An extract of up to 140 characters will be used.</p>
    
        <asp:CheckBox id="use_name" runat="server" />
        <asp:Label runat="server"  AssociatedControlID="use_name">Allow use of name</asp:Label>
        <p class="checkbox">Yes, I am happy for my name to be published along with my comment.</p>

      </div>
      
      <div class="donation-narrow">
        <p>Upload an image to accompany your comment.</p>
        
        <div id="upload-avatar"><img src="~/app_themes/websitedefault/images/avatar_default.jpg" /></div>

        <div id="upload-button">
            <div class="fileUpload button button-upload">
                <span>Choose file</span>
                <asp:FileUpload ID="docUpload" runat="server" CssClass="upload" />
            </div> 
          <p>Upload your fundraising image or a picture of yourself (optional). The image must be no greater than 900 x 900, and less than 4MB in size. Alternatively you can email your image to: <a href="mailto:charitable.funds@company.com">charitable.funds@company.com</a></p>
        </div>
      </div>      
    </div>
    <!-- end of box2 -->

    <div>
        <p>All fields marked with an * are required.</p>
            
                <%--<asp:DropDownList ID="drpTitle" runat="server" CssClass="" >
                    <asp:ListItem Value="" Text="Select a title"></asp:ListItem>
                    <asp:ListItem Value="Mr" Text="Mr"></asp:ListItem>
                    <asp:ListItem Value="Mrs" Text="Mrs"></asp:ListItem>
                    <asp:ListItem Value="Ms" Text="Ms"></asp:ListItem>
                    <asp:ListItem Value="Dr" Text="Dr"></asp:ListItem>
                </asp:DropDownList> --%>
                
      
        
        <ul class="form-list">  
            <li>
                <asp:Label ID="lblTitle" CssClass="text-label" runat="server" EnableViewState="false">Title: <span class="blue">*</span></asp:Label>
               
                <asp:TextBox ID="txtTitle" runat="server" EnableViewState="false" placeholder="Mr, Mrs." Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" EnableClientScript="true" ErrorMessage="You must enter a title." ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                <br /><br />
            </li>    
            <li>
            <asp:Label ID="LblCustomerName" runat="server" ControlName="TxtCustomerName" CssClass="text-label">First Name:<span class="blue">*</span></asp:Label>
            <asp:TextBox ID="TxtCustomerName" CssClass="textbox" runat="server" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqName" runat="server"  ControlToValidate="TxtCustomerName" ErrorMessage="Please enter your first name">&nbsp;</asp:RequiredFieldValidator>
                <br /><br />
          </li>
          <li>
            <asp:Label ID="LblCustomerSurname" runat="server" ControlName="txtCustomerSurname" CssClass="text-label">Surname:<span class="blue">*</span></asp:Label>
            <asp:TextBox ID="txtCustomerSurname" CssClass="textbox" runat="server" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqCustomerSurname" runat="server" ControlToValidate="txtCustomerSurname" ErrorMessage="Please enter your surname">&nbsp;</asp:RequiredFieldValidator>
            <br /><br />
          </li>
           <li>
                <asp:CheckBox ID="correct_details" runat="server" />
                <asp:Label runat="server"  AssociatedControlID="correct_details" class="css-label">Confirm Details</asp:Label>
                <asp:CustomValidator runat="server" ID="CustomValidator3" OnServerValidate="validateCorrectDetails" 
                      CssClass="red" ErrorMessage="Please confirm correct details">Please check correct.</asp:CustomValidator>
                <p class="checkbox">Yes, I confirm these details are all correct and accurate.</p> 
            
                <asp:CheckBox ID="dataprotection" runat="server"/>
                <asp:Label runat="server"  CssClass="css-label" AssociatedControlID="dataprotection">Data Protection Signature</asp:Label>
                 <asp:CustomValidator runat="server" ID="CustomValidator1" OnServerValidate="validateDataprotection" 
                      CssClass="red" ErrorMessage="Please check Data Protection">Please check data.</asp:CustomValidator>
                <p class="checkbox">Yes, I agree to The Robert Jones and Agnes Hunt Hospital Charity keeping a record of my donation and personal information. <br />
                Your information will only be used to administer charitable donations, and our marketing/publicity if you have ticked the boxes above, 
                    and will not be passed on to any third party commercial organisation.  If you would like any further information about your rights under the Data Protection Act, 
                    please contact the Trust.</p> 
              </li>
        </ul>
    </div>
    <asp:Button runat="server" OnClick="ProcessTransaction_Click" CssClass="button button-submit" Text="GO TO PAYMENT" />


</asp:Panel>
   
<asp:Panel ID="pnl2" runat="server" Visible="false">
    <div class="box">
      <h2>Payment Method</h2>
      <p>Please fill in the details below. All fields marked with an * are required</p>
      
      <ul class="form-list">
        <li>
          <div class="large-box">
        <asp:Label ID="LblCardType" runat="server" ControlName="DDLCardType" CssClass="text-label">Card Type:<span class="blue">*</span></asp:Label>
        <asp:DropDownList ID="DDLCardType" runat="server">
            <asp:ListItem Value="VISA">Visa</asp:ListItem>
            <asp:ListItem Value="ECMC">Mastercard</asp:ListItem>
            <asp:ListItem Value="MAESTRO">Maestro</asp:ListItem>
        </asp:DropDownList><br /><br />
          </div>
          
          <div class="medium-box">
            <div id="card-types">
              <p>Permitted card types below:</p>
                <img src="~/app_themes/websiteDefault/images/visa.gif" />
                <img src="~/app_themes/websiteDefault/images/mastercard.gif" />
                <img src="~/app_themes/websiteDefault/images/Maestro.jpg" height="40" width="60" />
                
            </div>
          </div>
        </li>
        
        <li>
          <div class="large-box">
            <asp:Label ID="LblCardName" runat="server" ControlName="TxtCardName" CssClass="text-label">Name as it appears on the card:<span class="blue">*</span></asp:Label>
            <asp:TextBox ID="TxtCardName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"  ControlToValidate="TxtCardName" ErrorMessage="Please enter Card Name" EnableClientScript="True">&nbsp;</asp:RequiredFieldValidator>
            <br /><br />
          </div>
        </li>
        <li>
          <div class="large-box">
            <asp:Label ID="LblCardNumber" runat="server" ControlName="TxtCardNumber" CssClass="text-label">Card Number:<span class="blue">*</span></asp:Label>
            <asp:TextBox ID="TxtCardNumber" runat="server" MaxLength="16"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"  ControlToValidate="TxtCardNumber" ErrorMessage="Please enter Card Number" EnableClientScript="True">&nbsp;</asp:RequiredFieldValidator>
              <%--<asp:RegularExpressionValidator ValidationExpression="[0-9]{15}" runat="server" ControlToValidate="TxtCardNumber" ErrorMessage="Please enter a 16 digit card number">&nbsp;</asp:RegularExpressionValidator>--%>
          </div>
          <div class="small-box-combined">
            <asp:Label ID="LblStartDate" runat="server" ControlName="DDLStartMonth" CssClass="text-label" Enabled="false">Valid From: (if available)</asp:Label>
            <asp:DropDownList ID="DDLStartMonth" runat="server" CssClass="expiration expiration-month">
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="02">02</asp:ListItem>
                <asp:ListItem Value="03">03</asp:ListItem>
                <asp:ListItem Value="04">04</asp:ListItem>
                <asp:ListItem Value="05">05</asp:ListItem>
                <asp:ListItem Value="06">06</asp:ListItem>
                <asp:ListItem Value="07">07</asp:ListItem>
                <asp:ListItem Value="08">08</asp:ListItem>
                <asp:ListItem Value="09">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="DDLStartYear" runat="server" CssClass="expiration">
                <asp:ListItem Value="2008">2008</asp:ListItem>
            </asp:DropDownList>
          </div>
          <div class="small-box">
            <asp:Label ID="LblIssueNumber" runat="server" ControlName="TxtIssueNumber" CssClass="text-label">Issue No:(Maestro only)</asp:Label>
            <asp:TextBox ID="TxtIssueNumber" runat="server" MaxLength="3"></asp:TextBox><br /><br />

          </div>
          
        </li>
        
        <li>
          <div class="large-box" style="padding-top:25px">
            <asp:CheckBox ID="terms" runat="server"/>
            <asp:Label runat="server"  CssClass="css-label" AssociatedControlID="terms">Terms &amp; Conditions <span class="blue">*</span></asp:Label>
              <asp:CustomValidator runat="server" ID="CustomValidator2" OnServerValidate="validateTerms" EnableClientScript="false"
                  CssClass="red" ErrorMessage="Please check Terms & Conditions">Please confirm.</asp:CustomValidator>
            <p class="checkbox">Yes, I confirm to the terms and conditions of this donation.</p>
          </div>
          
          <div class="small-box-combined">
            <asp:Label ID="LblExpiryDate" runat="server" ControlName="DDLExpiryMonth" CssClass="text-label">Card Expiry:<span class="blue">*</span></asp:Label>
            <asp:DropDownList ID="DDLExpiryMonth" runat="server" CssClass="expiration expiration-month">
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="02">02</asp:ListItem>
                <asp:ListItem Value="03">03</asp:ListItem>
                <asp:ListItem Value="04">04</asp:ListItem>
                <asp:ListItem Value="05">05</asp:ListItem>
                <asp:ListItem Value="06">06</asp:ListItem>
                <asp:ListItem Value="07">07</asp:ListItem>
                <asp:ListItem Value="08">08</asp:ListItem>
                <asp:ListItem Value="09">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
            </asp:DropDownList>
            
            
            <asp:DropDownList ID="DDLExpiryYear" runat="server" CssClass="expiration">
            </asp:DropDownList>
              <asp:CustomValidator runat="server" ID="expiryYear" ControlToValidate="DDLExpiryYear" OnServerValidate="expiryYear_ServerValidate"
                   ErrorMessage="Incorrect Expiry date" EnableClientScript="false">&nbsp;</asp:CustomValidator>
          </div>
          
          <div class="small-box">

            <asp:Label ID="LblCVC" runat="server" ControlName="TxtCVC" CssClass="text-label">Card Sec No:<span class="blue">*</span></asp:Label>
            <asp:TextBox ID="TxtCVC" TextMode="Password" runat="server" MaxLength="3"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCVC" EnableClientScript="false" ErrorMessage="Please enter a Secrity Number">&nbsp;</asp:RequiredFieldValidator>
            <br /><br />
          </div>
        </li>
      </ul>
      <a href="javascript:window.history.go(-1)" class="button button-back">Back</a>
      <asp:button ID="BtnProcessTransaction" runat="server" CssClass="button button-submit" Text="Process Payment" onclick="ProcessTransaction_Click" />
    </div>
</asp:Panel>

<%--<asp:Panel ID="pnlResult" runat="server"  Visible="false">
    <div id="banner">
        <cms:CMSEditableImage ID="eiTopBar" runat="server" AlternateText="Top Banner Image" ImageTitle="Top Banner Image" />
        <div id="banner-logo"></div>
        <div id="banner-text"><cms:CMSEditableRegion ID="erTitle" runat="server" RegionType="TextBox" RegionTitle="Image Title" /></div>
    </div>   
    <div class="box">
      <h2>Thank you, your donation has been successfully completed!</h2>
      <p>An acknowledgement email will be sent to the address provided.</p>
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


  <asp:Panel ID="pnlResultError" runat="server" Visible="false">
           <div class="box">
          <h2>Sorry there was a problem with your order.</h2> 
          <p><asp:Literal ID="litReturnError" runat="server"></asp:Literal></p>
          <cms:CMSEditableRegion ID="erFailSummary" runat="server" EnableViewState="false" RegionType="HtmlEditor" />
          <a href="~/Donations" class="button">Return to Donations</a>
        </div>
    </asp:Panel>--%>
 </div>

     <div class="worldpayLogo">
        <img src="~/app_themes/websiteDefault/images/poweredByWorldPay.gif" />
    </div>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
<script type="text/javascript">

    $(document).ready(function () {
        CheckCorporateCheckBox();

        $('[id$=toggle5]').click(function () {
            $('[id*=txtDonationAmount]').val("5.00");
        });

        $('[id$=toggle10]').click(function () {
            $('[id*=txtDonationAmount]').val("10.00");
        });

        $('[id$=toggle50]').click(function () {
            $('[id*=txtDonationAmount]').val("50.00");
        });


        $('[id$=toggle100]').click(function () {
            $('[id*=txtDonationAmount]').val("100.00");
        });
    });

    $('[id*="corporate"]').click(function () {
        CheckCorporateCheckBox();

    });


    function CheckCorporateCheckBox() {
        if ($('[id*="corporate"]').attr("checked")) {
            $('.giftAid').hide();
        } else {
            $('.giftAid').show();
        }
    }


    function textCounter(field, field2, maxlimit) {
        var countfield = document.getElementById(field2);
        if (field.value.length > maxlimit) {
            field.value = field.value.substring(0, maxlimit);
            return false;
        } else {
            countfield.value = maxlimit - field.value.length;
        }
    }
</script>

</asp:Content>