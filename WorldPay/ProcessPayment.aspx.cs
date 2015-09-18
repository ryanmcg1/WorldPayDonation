using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Net;
using System.Collections.Specialized;
using CMS.UIControls;
using CMS.Ecommerce;
using CMS.EcommerceProvider;
using CMS.SiteProvider;
using CMS.SettingsProvider;
using CMS.GlobalHelper;
using CMS.DataEngine;
using CMS.CMSHelper;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Text;
using CMS.FormControls;
using CMS.FormEngine;
using CMS.DocumentEngine;
using CMS.MediaLibrary;
using System.Globalization;
using WorldPayDirectDotNet;
using System.Xml.XPath;

public partial class CMSTemplates_Website_2014_Redesign_WorldPay_ProcessPayment : TemplatePage
{
    OrderStatusInfo OSI;
    OrderInfo order = new OrderInfo();
    private ShoppingCartInfo mShoppingCart = null;
    private SKUInfo mSKU = null;
    private bool? mSKUHasOptions = null;
    int totalAmmount = 0;

    public ShoppingCartInfo ShoppingCart
    {
        get
        {
            if (mShoppingCart == null)
            {
                mShoppingCart = ECommerceContext.CurrentShoppingCart;
            }

            return mShoppingCart;
        }
        set
        {
            mShoppingCart = value;
        }
    }

    public int SKUID
    {
        get
        {
            return ValidationHelper.GetInteger(drpDonations.SelectedValue,0);// ValidationHelper.GetInteger(ViewState["SKUID"], 0);
        }
        set
        {
            ViewState["SKUID"] = value;

            // Invalidate SKU data
            mSKU = null;
            mSKUHasOptions = null;
        }
    }

    public SKUInfo SKU
    {
        get
        {
            if (mSKU == null)
            {
                mSKU = SKUInfoProvider.GetSKUInfo(SKUID);
            }

            return mSKU;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(CMSContext.ViewMode.ToString().ToLower() == "edit")
        {
            //pnlResult.Visible=true;
            //pnlResultError.Visible = true;
        }
        DateTime maxyear = DateTime.Now.AddYears(10);
        DateTime minyear = DateTime.Now.AddYears(-10);
        PopulateDonationItemList();
        for (int intAge = DateTime.Now.Year; intAge <= maxyear.Year; intAge += 1)
        {
            DDLExpiryYear.Items.Add(intAge.ToString());
        }
    }

    protected CustomerInfo createCustomer()
    {
        //Customer
        CustomerInfo customer = new CustomerInfo();
  
        
            customer.CustomerFirstName = TxtCustomerName.Text.ToString();
            customer.CustomerLastName = txtCustomerSurname.Text.ToString();
            customer.CustomerCompany = "NA";
            customer.SetValue("CustomerTitle",ValidationHelper.GetString(txtTitle.Text,""));

        try
        {
            customer.CustomerEmail = "Donator@email.com";
            customer.CustomerEnabled = true;
            customer.CustomerSiteID = CMSContext.CurrentSiteID;
            customer.Insert();
        }
        catch (Exception ex)
        {
            throw;
        }

        return customer;
    }


    protected void AddOrder()
    {
        CustomerInfo customer = createCustomer();
        CountryInfo ci = CountryInfoProvider.GetCountryInfo(ECommerceSettings.DefaultCountryName(CMSContext.CurrentSiteName));
        AddressInfo billing = new AddressInfo();
        PaymentOptionInfo pi = PaymentOptionInfoProvider.GetPaymentOptionInfo("WorldPay", CMSContext.CurrentSiteName);
        OSI = OrderStatusInfoProvider.GetOrderStatusInfo("OrderCreated", CMSContext.CurrentSiteName);

            
        billing.AddressName = customer.CustomerFirstName + " " + customer.CustomerLastName;
        billing.AddressPersonalName = "Home";
        billing.AddressCity = "";// TxtTown.Text.ToString();
        billing.AddressIsBilling = true;
        billing.AddressIsShipping = true;
        billing.AddressLine1 = "";// TxtAddress1.Text.ToString();
        billing.AddressLine2 = "";// TxtAddress2.Text.ToString();
        billing.AddressZip = "";// TxtPostcode.Text.ToString();
        billing.AddressCustomerID = customer.CustomerID;
        billing.AddressCountryID = ci.CountryID;
        billing.AddressEnabled = true;
        billing.AddressPersonalName = customer.CustomerLastName;
        billing.Insert();
        ShoppingCart.ShoppingCartBillingAddressID = billing.AddressID;
            
        //Order
        // Set order culture
        ShoppingCart.ShoppingCartCulture = CMSContext.PreferredCultureCode;
        // Update customer preferences
        CustomerInfoProvider.SetCustomerPreferredSettings(ShoppingCart);
        // Create order
        ShoppingCartInfoProvider.SetOrder(ShoppingCart);

        order.SetValue("CustomerTitle", txtTitle.Text);
        order.OrderCustomerID = customer.CustomerID;
        order.OrderBillingAddressID = billing.AddressID;
        order.OrderShippingAddressID = billing.AddressID;
        order.OrderTotalTax = ShoppingCart.TotalTax;
        order.OrderTotalPrice = ShoppingCart.TotalItemsPrice;
        order.OrderTotalPriceInMainCurrency = ShoppingCart.TotalItemsPrice;
        order.OrderSiteID = CMSContext.CurrentSiteID;
        order.OrderStatusID = OSI.StatusID;
        order.OrderIsPaid = false;
        order.OrderCurrencyID = CMSContext.CurrentSite.SiteDefaultCurrencyID;
        order.OrderPaymentOptionID = pi.PaymentOptionID;
        order.SetValue("ImagePath", ViewState["PhotoPath"]);
        if (!corporate.Checked )
	    {
            order.SetValue("IsGiftAid", gift_aid.Checked);
	    }
        else 
        {
            gift_aid.Checked = false;
        }
            
            
        order.SetValue("AllowUseOfName", use_name.Checked);            
        order.SetValue("AllowUseOfComment", use_comment.Checked);
        order.SetValue("Comment", userComment.InnerText);
        order.SetValue("CorporateDonation", corporate.Checked);
        OrderInfoProvider.SetOrderInfo(order);

        OrderItemInfo oii = OrderItemInfoProvider.GetOrderItemInfo(ShoppingCart.GetShoppingCartItem(ShoppingCart.CartItems[0].CartItemGUID));
        oii.OrderItemOrderID = order.OrderID;
        OrderItemInfoProvider.SetOrderItemInfo(oii);
    }

    /// <summary>
    /// Adds product to the shopping cart.
    /// </summary>
    private void AddProductToShoppingCart()
    {
        // Get cart item parameters
        ShoppingCartItemParameters cartItemParams = GetShoppingCartItemParameters();
        string error = null;

        if (!string.IsNullOrEmpty(error))
        {
            // Show error message and cancel adding the product to shopping cart
            ScriptHelper.RegisterStartupScript(Page, typeof(string), "ShoppingCartAddItemErrorAlert", ScriptHelper.GetAlertScript(error));
            return;
        }

        // Get cart item parameters in case something changed
        cartItemParams = GetShoppingCartItemParameters();

        // Log activity
        //LogProductAddedToSCActivity(SKUID, SKU.SKUName, Quantity);

        if (ShoppingCart != null)
        {
            bool updateCart = false;

            // Assign current shopping cart to current user
            CurrentUserInfo ui = CMSContext.CurrentUser;
            if (!ui.IsPublic())
            {
                ShoppingCart.User = ui;
                updateCart = true;
            }

            // Shopping cart is not saved yet
            if (ShoppingCart.ShoppingCartID == 0)
            {
                updateCart = true;
            }

            // Update shopping cart when required
            if (updateCart)
            {
                ShoppingCartInfoProvider.SetShoppingCartInfo(ShoppingCart);
            }

            // Add item to shopping cart
            ShoppingCartItemInfo addedItem = ShoppingCart.SetShoppingCartItem(cartItemParams);
        }
    }

    public ShoppingCartItemParameters GetShoppingCartItemParameters()
    {
        int Quantity = 1;
        //int SKUID  -> got from drop down
        double price = ValidationHelper.GetDouble(txtDonationAmount.Text, 0.00);

        List<ShoppingCartItemParameters> options = new List<ShoppingCartItemParameters>();

        // Create params
        ShoppingCartItemParameters cartItemParams = new ShoppingCartItemParameters(SKUID, Quantity, options);

        // Ensure minimum allowed number of items is met
        if (SKU.SKUMinItemsInOrder > Quantity)
        {
            cartItemParams.Quantity = SKU.SKUMinItemsInOrder;
        }

           
        //    // Get exchange rate from cart currency to site main currency         
        double rate = (SKU.IsGlobal) ? ShoppingCart.ExchangeRateForGlobalItems : ShoppingCart.ExchangeRate;

        //    // Set donation specific shopping cart item parameters
        //    cartItemParams.IsPrivate = donationProperties.DonationIsPrivate;

        //    // Get donation amount in site main currency
        cartItemParams.Price = ExchangeRateInfoProvider.ApplyExchangeRate(price, 1 / rate);
        return cartItemParams;
    }

    protected void validateDataprotection(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = false;
        if (dataprotection.Checked)
            e.IsValid = dataprotection.Checked;
    }

    protected void validateCorrectDetails(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = false;
        if (correct_details.Checked)
            e.IsValid = correct_details.Checked;
    }

    protected void validateTerms(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = false;
        if (terms.Checked)
            e.IsValid = terms.Checked;
    }
            
    protected bool ValidateAmmount()
    {
        if (txtDonationAmount.Text != "" || toggle5.Checked || toggle10.Checked || toggle50.Checked || toggle100.Checked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        
  
    protected void ProcessTransaction_Click(object sender, EventArgs e)
    {
        Page.Validate();

        if (Page.IsValid && ValidateAmmount())
        {
            ShoppingCart.CartItems.Clear();//.ClearData();
            AddProductToShoppingCart();
            GetImagePath();
            AddOrder();
            

            //Retrieve the InstallationID, MerchantCode and XMLPassword values from the web.config
            WorldPayDirectDotNet.PaymentRequest PaymentRequest = new PaymentRequest();
            PaymentRequest.MerchantCode = WebConfigurationManager.AppSettings["MerchantCode"];
            PaymentRequest.XMLPassword = WebConfigurationManager.AppSettings["XMLPassword"];

            //OrderCode - If your system has created a unique order/cart ID, enter it here.
            PaymentRequest.OrderCode = order.OrderID.ToString();

            //amount - A decimal number giving the cost of the purchase in terms of the minor currency unit e.g. £12.56 = 1256
            PaymentRequest.amount = (int)(double.Parse(txtDonationAmount.Text) * 100);

            //currencyCode - 3 letter ISO code for the currency of this payment.
            PaymentRequest.currencyCode = "GBP";

            //testMode - set to 0 for Live Mode, set to 100 for Test Mode.
            string URL="";
            if(ValidationHelper.GetInteger(WebConfigurationManager.AppSettings["testMode"], 0) == 0)
            {
                 URL= "https://secure.worldpay.com/jsp/merchant/xml/paymentService.jsp";
            }
            else
            {
                URL="https://secure-test.worldpay.com/jsp/merchant/xml/paymentService.jsp";
            }


            PaymentRequest.orderContent = "";

            //WorldPayDirectDotNet.PaymentRequest.PaymentResult PaymentResult = PaymentRequest.SubmitTransaction(PaymentRequest);
            StringBuilder builder = SubmitTransaction(PaymentRequest);

            string returnURL = string.Format("https://{0}/DonationReturn.aspx", CMSContext.CurrentSite.DomainName);
   

            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(PaymentRequest.MerchantCode + ":" + PaymentRequest.XMLPassword));
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(URL);
            wReq.ContentType = "text/xml";
            wReq.UserAgent = "Worldpay Payments";
            wReq.Timeout = 45 * 1000; // milliseconds
            wReq.AllowAutoRedirect = true;
            wReq.Headers.Add("Authorization", "Basic " + credentials);
            wReq.ContentLength = builder.ToString().Length;
            wReq.Method = "POST";

            StreamWriter sReq = new StreamWriter(wReq.GetRequestStream());
            sReq.Write(builder.ToString());
            sReq.Flush();
            sReq.Close();

            HttpWebResponse wResp = (HttpWebResponse)wReq.GetResponse();
            StreamReader sResp = new StreamReader(wResp.GetResponseStream());

            String responseXML = sResp.ReadToEnd();
            XmlDocument derp = new XmlDocument();
            derp.LoadXml(responseXML);
            XmlNode response = derp.DocumentElement.SelectSingleNode("reply");

            string URLExts = string.Format("&successURL={0}&failureURL={1}&pendingURL={2}&cancelURL={3}", returnURL, returnURL, returnURL, returnURL + "?paymentStatus=CANCELLED");
            
            if (response.InnerText != null)
            {
                Response.Redirect(response.InnerText + URLExts);
            }
        }
    }


    public StringBuilder SubmitTransaction(PaymentRequest PaymentRequest)
    {

        OrderItemInfo oii = OrderItemInfoProvider.GetOrderItemInfo(ShoppingCart.GetShoppingCartItem(ShoppingCart.CartItems[0].CartItemGUID));
        
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
        builder.AppendLine("<!DOCTYPE paymentService PUBLIC '-//WorldPay/DTD WorldPay PaymentService v1//EN' 'http://dtd.worldpay.com/paymentService_v1.dtd'>");
        builder.AppendLine("<paymentService version='1.4' merchantCode='" + PaymentRequest.MerchantCode + "'>");
        builder.AppendLine("<submit>");
        builder.AppendLine(string.Concat(new object[] { "<order orderCode='", PaymentRequest.OrderCode, "'>" }));
        builder.AppendLine("<description>" + txtTitle.Text + " " + TxtCustomerName.Text + " " + txtCustomerSurname.Text + ", " + oii.OrderItemSKUName + ", Gift Aid=" + gift_aid.Checked.ToString() + "</description>");
        builder.AppendLine(string.Concat(new object[] { "<amount value='", PaymentRequest.amount, "' currencyCode='", PaymentRequest.currencyCode, "' exponent='2'/>" }));
        builder.AppendLine("<orderContent>");
        builder.AppendLine("<![CDATA[ordercontent]]>");
        builder.AppendLine("</orderContent>");
        builder.AppendLine("<paymentMethodMask>");
        builder.AppendLine("<include code=\"ALL\"/>");
        builder.AppendLine("</paymentMethodMask>");
        builder.AppendLine("<shopper>");
        builder.AppendLine("<shopperEmailAddress>" + PaymentRequest.shopperEmailAddress + "</shopperEmailAddress>");
        builder.AppendLine("</shopper>");
        //builder.AppendLine("<shippingAddress><address><firstName></firstName><lastName></lastName><address1></address1><address2></address2><address3></address3><postalCode></postalCode><city></city>test<state></state><countryCode>UK</countryCode><telephoneNumber></telephoneNumber></address></shippingAddress>");
        builder.AppendLine("</order>");
        builder.AppendLine("</submit>");
        builder.AppendLine("</paymentService>");

        return builder;

    }


    protected void PopulateDonationItemList()
    {
        string donation = QueryHelper.GetString("t", "");
        string donationID ="";
        DataSet ds = SKUInfoProvider.GetSKUList("SKUEnabled = 1", "SKUCreated asc", "", -1);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                if (donation == dr[2].ToString())
                {
                    donationID = dr[0].ToString();
                }
                ListItem list = new ListItem();
                list.Value=dr[0].ToString();
                list.Text = dr[2].ToString();
                drpDonations.Items.Add(list);
            }

        if(donationID !="")
        { 
            drpDonations.SelectedValue = donationID;
        }

    }

    //protected void printResults(WorldPayDirectDotNet.PaymentRequest.PaymentResult PaymentResult)
    //{
    //    if(PaymentResult.PaymentSuccessful)
    //    {

    //        double giftAidTotal;
    //        giftAidTotal = (order.OrderTotalPrice / 100) * 25;
    //        if (ValidationHelper.GetBoolean(order.GetValue("IsGiftAid"),false))
    //        { 
    //            giftAidTotal = Math.Truncate(giftAidTotal * 100) / 100;
    //        }
    //        else
    //        {
    //            giftAidTotal = 0;

    //        }

    //        double orderPriceTotal = Math.Truncate(order.OrderTotalPrice*100) /100;
    //        double total=0;
    //        pnlResult.Visible = true;

    //        litDonationAmmountRestult.Text = order.OrderTotalPrice.ToString("0.00");
    //        if (gift_aid.Checked && order.OrderTotalPrice !=0)
    //        {
    //            litGiftAidResult.Text = giftAidTotal.ToString("0.00");
    //        }
    //        else
    //        {
    //            litGiftAidResult.Text="0.00"; 
    //        }

    //        //Show total price
    //        total = giftAidTotal + orderPriceTotal;
    //        litTotalResult.Text = total.ToString("0.00");
    //    }else
    //    {

    //        pnlResultError.Visible = true;
    //        litReturnError.Text=PaymentResult.errorMessage.ToString();
    //    }
    //}

    protected void GetImagePath()
    {
        string filePath = "";
        Guid filenameGuid = Guid.NewGuid();
        if (docUpload.HasFile)
        {
            string fileName = Server.MapPath(docUpload.FileName);

            MediaLibraryInfo libInfo = MediaLibraryInfoProvider.GetMediaLibraryInfo("COMPANYNAME", CMSContext.CurrentSiteName);
            filePath = Server.MapPath(string.Format("//_MediaLibraries///DonationUploads//{0}{1}", filenameGuid, System.IO.Path.GetExtension(docUpload.FileName)));
            File.WriteAllBytes(filePath, docUpload.FileBytes);
            if (File.Exists(filePath))
            {
                string path = MediaLibraryHelper.EnsurePath(filePath);
                MediaFileInfo fileInfo = new MediaFileInfo(filePath, libInfo.LibraryID, "");
                fileInfo.FileSiteID = CMSContext.CurrentSiteID;
                MediaFileInfoProvider.ImportMediaFileInfo(fileInfo);
            }
            ViewState["PhotoPath"]=  "/_MediaLibraries/DonationUploads/" + filenameGuid + System.IO.Path.GetExtension(docUpload.FileName);
            return;
        }
        ViewState["PhotoPath"]="";
        return;
    }

    protected void expiryYear_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (ValidationHelper.GetInteger(DDLExpiryYear.SelectedValue, 0) > DateTime.Now.Year)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = ((ValidationHelper.GetInteger(DDLExpiryYear.SelectedValue, 0) >= DateTime.Now.Year) && ValidationHelper.GetInteger(DDLExpiryMonth.SelectedValue, 0) > DateTime.Now.Month);
        }
    }
}