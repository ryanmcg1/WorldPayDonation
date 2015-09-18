using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.UIControls;
using CMS.Ecommerce;
using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.CMSHelper;
using CMS.SettingsProvider;
using System.Web.Configuration;
using CMS.EmailEngine;
using CMS.PortalEngine;


public partial class CMSTemplates_Website_2014_Redesign_WorldPay_processResults : TemplatePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        processResults();

        if(CMSContext.ViewMode == ViewModeEnum.Edit)
        {
            pnlResult.Visible = true;
            pnlResultError.Visible = true;
            pnlResultCancelled.Visible = true;
        }
    }

    public void SendEmail(MacroResolver mcr, string TemplateName, string ToEmail)
    {

        EmailMessage email = new EmailMessage();
        EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(TemplateName, CMSContext.CurrentSiteID);
        email.EmailFormat = EmailFormatEnum.Html;
        email.Recipients = ToEmail;
        EmailSender.SendEmailWithTemplateText(CMSContext.CurrentSiteName, email, eti, mcr, true);
    }

    public void processResults()
    {
        int orderKey = 0;
        string orderKeyReturn = QueryHelper.GetString("orderKey", "");
        string orderStatus = QueryHelper.GetString("paymentStatus", "");

        if (orderKeyReturn != "" && orderStatus != "")
        {
            orderKeyReturn = orderKeyReturn.Replace(WebConfigurationManager.AppSettings["MerchantCode"], "");
            orderKeyReturn = orderKeyReturn.Replace("", "");//ID
            orderKeyReturn = orderKeyReturn.Replace("^", "");
            orderKey = int.Parse(orderKeyReturn);

            //Can be uncommented for debugging purposes
            //Response.Write("oderkey: " + orderKey + "<br><br>");
            //Response.Write("status: " + orderStatus + "<br><br>");
            //Response.Write("orderID: " + orderKey + "<br><br>");

            PaymentResultInfo PRI;
            //Transaction Successful - Add your code to process a successful transaction here (before the break httpa.Response.Redirect).
            OrderInfo order = OrderInfoProvider.GetOrderInfo(orderKey);

            if (order != null)
            {
                OrderStatusInfo OSI;
                switch (orderStatus.ToUpper())
                {
                    case "AUTHORISED":
                        CMS.SettingsProvider.InfoDataSet<OrderItemInfo> oii = OrderItemInfoProvider.GetOrderItems(orderKey);
                        OSI = OrderStatusInfoProvider.GetOrderStatusInfo("Complete", CMSContext.CurrentSiteName);
                        PRI = new PaymentResultInfo();
                        PRI.PaymentDate = DateTime.Now;
                        PRI.PaymentIsCompleted = true;
                        PRI.PaymentStatusValue = "Order & Payment Complete.";
                        PRI.PaymentTransactionID = orderKey.ToString();
                        order.OrderStatusID = OSI.StatusID;
                        order.OrderPaymentResult = PRI;
                        order.OrderIsPaid = true;
                        order.Update();
                        pnlResult.Visible = true;
                        ltlDonationResultTitle.Text = oii.Items[0].OrderItemSKUName;

                        double giftAidTotal;
                        giftAidTotal = (order.OrderTotalPrice / 100) * 25;
                        if (ValidationHelper.GetBoolean(order.GetValue("IsGiftAid"), false))
                        {
                            giftAidTotal = Math.Truncate(giftAidTotal * 100) / 100;
                        }
                        else
                        {
                            giftAidTotal = 0;
                        }

                        double orderPriceTotal = Math.Truncate(order.OrderTotalPrice * 100) / 100;
                        double total = 0;

                        litDonationAmmountRestult.Text = order.OrderTotalPrice.ToString("0.00");
                        if (ValidationHelper.GetBoolean(order.GetValue("IsGiftAid"), false) == true && order.OrderTotalPrice != 0)
                        {
                            litGiftAidResult.Text = giftAidTotal.ToString("0.00");
                        }
                        else
                        {
                            litGiftAidResult.Text = "0.00";
                        }

                        //Show total price
                        total = giftAidTotal + orderPriceTotal;
                        litTotalResult.Text = total.ToString("0.00");

                        //Send finance email
                        MacroResolver mcr = MacroResolver.GetInstance();
                        mcr.AddDynamicParameter("OrderCode", order.OrderID.ToString());
                        mcr.AddDynamicParameter("DonationAmount", total.ToString("0.00"));
                        mcr.AddDynamicParameter("ImageChecked", order.GetStringValue("ImagePath", "") != "" ? true : false);
                        mcr.AddDynamicParameter("CommentChecked", order.GetBooleanValue("AllowUseOfComment", false));

                        SendEmail(mcr, "Donation-Finance", "email@address.com");
                        break;

             

                    case "CANCELLED":
                        OSI = OrderStatusInfoProvider.GetOrderStatusInfo("Failed", CMSContext.CurrentSiteName);
                        if (OSI != null)
                        {

                            PRI = new PaymentResultInfo();
                            PRI.PaymentDate = DateTime.Now;
                            PRI.PaymentIsCompleted = false;
                            PRI.PaymentStatusValue = "Cancelled";
                            PRI.PaymentTransactionID = orderKey.ToString();
                            order.OrderStatusID = OSI.StatusID;
                            order.OrderPaymentResult = PRI;
                            order.OrderIsPaid = false;
                            order.Update();

                            pnlResultCancelled.Visible = true;
                            litErrorCancelled.Text = "Your order was Cancelled.";
                        }
                        break;

                    //case "PENDING":
                    //    pnlResultError.Visible = true;
                    //    litReturnError.Text = "Your order is pending.";
                    //    break;

                    default:
                        OSI = OrderStatusInfoProvider.GetOrderStatusInfo("Failed", CMSContext.CurrentSiteName);
                        if (OSI != null)
                        {
                            PRI = new PaymentResultInfo();
                            PRI.PaymentDate = DateTime.Now;
                            PRI.PaymentIsCompleted = false;
                            PRI.PaymentStatusValue = "failed";
                            PRI.PaymentTransactionID = orderKey.ToString();
                            order.OrderStatusID = OSI.StatusID;
                            order.OrderPaymentResult = PRI;
                            order.OrderIsPaid = false;
                            order.Update();
                            pnlResultError.Visible = true;
                            litReturnError.Text = "Error detected.";
                        }
                        break;
                }
            }
            else
            {
                pnlResultError.Visible = true;
                litReturnError.Text = "Your order was not found. Please contact a member of support.";
            }
        }
    }
}