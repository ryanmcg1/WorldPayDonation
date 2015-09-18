using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CMS.UIControls;
using CMS.FormEngine;
using CMS.CMSHelper;
using CMS.SettingsProvider;
using CMS.DataEngine;
using CMS.GlobalHelper;
using CMS.UIControls;
using CMS.Ecommerce;

public partial class CMSTemplates_Website_2014_Redesign_Donation : TemplatePage
{
    /// <summary>
    /// Values for comments
    /// </summary>
    public class DonationValues
    {
        public string Comments { get; set; }
        public string DonerName { get; set; }
        public DateTime DonationDate { get; set; }
        public double DonationAmount { get; set; }
        public double GiftAidAmount { get; set; }
        public string DonerImage { get; set; }

        public DonationValues(string comments, string donerName, DateTime donationDate, double donationAmount, double giftAidAmount, string donerImage)
        {
            Comments = comments;
            DonerName = donerName;
            DonationDate = donationDate;
            DonationAmount = donationAmount;
            GiftAidAmount = giftAidAmount;
            DonerImage = donerImage;
        }
    }

    /// <summary>
    /// Total raised
    /// </summary>
    double totalRaised = 0;

    /// <summary>
    /// Offline Total Raised
    /// </summary>
    double offlineTotal = 0;
    double onlineTotal = 0;

    /// <summary>
    /// Gift Aid Total Raised
    /// </summary>
    double giftTotal = 0;

    /// <summary>
    /// Target to be raised
    /// </summary>
    double target = 0;

    /// <summary>
    /// Count of donations
    /// </summary>
    int donationCount = 0;

    /// <summary>
    /// Pagination page count
    /// </summary>
    int pageNum = 0;


    /// <summary>
    /// Page load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        loadComments();
    }

    private void loadComments()
    {
        int currentItemId = 1;
        if (CMSContext.CurrentDocument != null) currentItemId = CMSContext.CurrentDocument.GetIntegerValue("SKUID", 0);
        InfoDataSet<OrderItemInfo> orderItems = null;
        PagedDataSource comments = new PagedDataSource();
        DataTable images = new DataTable();
        images.Columns.Add(new DataColumn("DonerImage", Type.GetType("System.String")));
        images.Columns.Add(new DataColumn("DonerName", Type.GetType("System.String")));



        if (currentItemId != 0)
        {
            //get sku values needed
            GetSkuValues(currentItemId);

            //get all orders with current item
            orderItems = OrderItemInfoProvider.GetOrderItems("OrderItemSKUID = " + currentItemId, "OrderItemLastModified desc");
            //get all comments from orders
            comments = GetDonationValues(orderItems);

            //Images
            foreach (OrderItemInfo ori in orderItems)
            {
                OrderInfo order = OrderInfoProvider.GetOrderInfo(ori.OrderItemOrderID);
                bool allowImages = ValidationHelper.GetBoolean(order.GetValue("AllowUseOfImage"), false);
                bool allowUseOfName = ValidationHelper.GetBoolean(order.GetValue("AllowUseOfName"), false); 

                OrderStatusInfo OS = OrderStatusInfoProvider.GetOrderStatusInfo("Failed", CMSContext.CurrentSiteName);
                if (order.OrderStatusID == OS.StatusID) continue;

                if (ValidationHelper.GetString(order.GetValue("ImagePath"), "") != string.Empty && images.Rows.Count <= 50 && allowImages == true)
                {
                    DataRow dr = images.NewRow();
                    CustomerInfo customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
                    dr["DonerImage"] = order.GetValue("ImagePath");

                    if (allowUseOfName)
                    {
                        dr["DonerName"] = order.GetValue("CustomerTitle") + " " + customer.CustomerFirstName + " " + customer.CustomerLastName;
                    }
                    else
                    {
                        dr["DonerName"] = "Anonymous";
                    }
                    

                    images.Rows.Add(dr);
                }
            }

            //Comments
            if (comments != null)
            {
                //Get page number for pagination of repeater
                pageNum = ValidationHelper.GetInteger(hdnPageNumber.Value, 0); //QueryHelper.GetInteger("Page", 0);
                comments.CurrentPageIndex = pageNum;
                if (comments.PageCount == 1)
                {
                    lbBack.Visible = false;
                    lbNext.Visible = false;
                }
                else
                {
                    if (pageNum == 0) lbBack.Visible = false;
                    if (pageNum == comments.PageCount - 1) lbNext.Visible = false;
                }
                //bind comments to repeater
                rptComments.DataSource = comments;
                rptComments.DataBind();

                //bind images to repeater
                comments.AllowPaging = false;
                rptImages.DataSource = images;
                rptImages.DataBind();
            }


            //add offline and gif donations to total
            totalRaised = giftTotal + onlineTotal + offlineTotal;
            double totalSum = Math.Truncate(ValidationHelper.GetDouble(totalRaised, 0.00) * 100) / 100;

            double x = (Math.Truncate(ValidationHelper.GetDouble(totalRaised, 0.00) * 100) / 100) - (offlineTotal) - (giftTotal);



            lblTotalRaised.Text = string.Format("{0:N2}", totalSum);

            //Set fields
            ltlTotalOnlineDontaions.Text = string.Format("{0:N2}", onlineTotal);
            ltlTotalOfflineDontaions.Text = string.Format("{0:N2}", offlineTotal);
            ltlTotalGiftCardDontaions.Text = string.Format("{0:N2}", giftTotal);
            ltlTotalOverallDonations.Text = string.Format("{0:N2}", totalSum);


            //get percentage to rounded to nearest percent
            double percent = (totalRaised / target) * 100;
            var rounded = Math.Round(percent, 0);
            lblPercentage.Text = ValidationHelper.GetString(rounded, "0");

            //get total number of donations
            DonationsMade.Text = ValidationHelper.GetString(donationCount, "0");
        }
    }


    /// <summary>
    /// Gets and sets the product values
    /// </summary>
    /// <param name="currentItemId">SKU ID of Item</param>
    protected void GetSkuValues(int currentItemId)
    {
        DateTime targetDate = DateTime.MinValue;

        SKUInfo sku = SKUInfoProvider.GetSKUInfo(currentItemId);
        if (sku != null)
        {
            //get values
            target = sku.GetDoubleValue("DonationTarget", 0);
            targetDate = ValidationHelper.GetDateTime(sku.GetValue("DonationEndDate"), DateTime.MinValue);

            //set values
            DonationTarget.Text = ValidationHelper.GetString(target, "0");
            lblTargetDate.Text = targetDate.ToString("dd MMMM yyyy");

            if (target != 0)
            {
                plcTarget.Visible = true;
                plcTargetRaised.Visible = true;
                pnlSummary.Visible = true;
                pnlDonationsMafe.Visible = true;
                pnlTotalRaised.Visible = true;
            }
            if (targetDate != new DateTime(1900, 01, 01, 00, 00, 00)) plcAcceptedDate.Visible = true;

        }
    }


    /// <summary>
    /// Gets and sets the donation values
    /// </summary>
    /// <param name="orderItems">All order items</param>
    /// <returns>List of all donations items</returns>
    protected PagedDataSource GetDonationValues(InfoDataSet<OrderItemInfo> orderItems)
    {
        List<DonationValues> lstValues = new List<DonationValues>();
        double totalRaisedRunning = 0;
        double totalOfflineRunning = 0;
        double totalOnlineRunning = 0;
        OrderStatusInfo OS = OrderStatusInfoProvider.GetOrderStatusInfo("Failed", CMSContext.CurrentSiteName);
        OrderStatusInfo OS2 = OrderStatusInfoProvider.GetOrderStatusInfo("OrderCreated", CMSContext.CurrentSiteName);
        foreach (OrderItemInfo item in orderItems)
        {
            OrderInfo order = OrderInfoProvider.GetOrderInfo(item.OrderItemOrderID);
            CustomerInfo customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);


            if (order.OrderStatusID == OS.StatusID) continue;
            if (order.OrderStatusID == OS2.StatusID) continue;

            string name = string.Empty;
            string comments = string.Empty;

            double giftAidAmount = 0;

            DateTime date = order.OrderDate;
            double amount = 0;

            PaymentOptionInfo poi = PaymentOptionInfoProvider.GetPaymentOptionInfo(order.OrderPaymentOptionID);

  

            if (poi.PaymentOptionDisplayName != "WorldPay")
            {
                totalOfflineRunning += order.OrderTotalPrice;
                amount = order.OrderTotalPrice;
            }
            else
            {
                totalOnlineRunning += order.OrderTotalPrice;
                amount = order.OrderTotalPrice;
            }

            string imagePath = ValidationHelper.GetString(order.GetValue("ImagePath"), "");
            if (imagePath == "") imagePath = "~/app_themes/websitedefault/images/avatar_default.jpg";


            //Comment
            bool allowComments = ValidationHelper.GetBoolean(order.GetValue("AllowUseOfComment"), false);
            bool allowCommentsAdmin = ValidationHelper.GetBoolean(order.GetValue("UseCommentOnSite"), false);
            if (allowComments && allowCommentsAdmin) comments = ValidationHelper.GetString(order.GetValue("CustomisedCommentUsedOnLive"), "");
            
            //Allow Name
            bool allowName = ValidationHelper.GetBoolean(order.GetValue("AllowUseOfName"), false);
            if (allowName)
            {
                name = "Donation by <strong>" + customer.GetValue("CusomterTitle") + order.GetValue("CustomerTitle") + " " + customer.CustomerFirstName + " " + customer.CustomerLastName + "</strong>";
            }
            else
            {
                name = "Donation by <strong> Anonymous </strong>";
            }

            //Allow Image
            bool allowImage = ValidationHelper.GetBoolean(order.GetValue("AllowUseOfImage"), false);
            if (!allowImage)
            {
                imagePath = "/App_themes/websitedefault/images/avatar_default.jpg";
            }
         

            //Giftaid
            bool giftAid = ValidationHelper.GetBoolean(order.GetValue("IsGiftAid"), false);
            if (giftAid) giftAidAmount = Math.Round((amount / 100) * 25, 2);


            DonationValues donation = new DonationValues(comments, name, date, amount, giftAidAmount, imagePath);

            lstValues.Add(donation);
            giftTotal += giftAidAmount;
            double total = amount;
            totalRaisedRunning += total;
        }
        onlineTotal = totalOnlineRunning;
        offlineTotal = totalOfflineRunning;
        totalRaised = totalRaisedRunning + totalOfflineRunning;
        donationCount = lstValues.Count;

        //convert to paged data sauce
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = lstValues;
        pds.AllowPaging = true;
        pds.PageSize = 5;

        //return lstValues;
        return pds;

    }


    /// <summary>
    /// Previous page of pagination on click action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbBack_Click(object sender, EventArgs e)
    {

        int temp;
        temp = ValidationHelper.GetInteger(hdnPageNumber.Value, 0);
        temp = temp - 1;
        hdnPageNumber.Value = temp.ToString();// temp;
        loadComments();
        hdnPageNumber.Value = temp.ToString();
        if (temp > 0)
        {
            lbBack.Visible = true;
        }
        else
        {
            lbNext.Visible = true;
        }
        //Response.Redirect(CMSContext.CurrentDocument.NodeAliasPath + ".aspx?Page=" + Convert.ToString(pageNum - 1));
    }


    /// <summary>
    /// Next page of pagination on click action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbNext_Click(object sender, EventArgs e)
    {
        int temp;
        temp = ValidationHelper.GetInteger(hdnPageNumber.Value, 0);
        temp = temp + 1;
        hdnPageNumber.Value = temp.ToString();// temp;
        loadComments();
        hdnPageNumber.Value = temp.ToString();
        if (temp > 0)
        {
            lbBack.Visible = true;
        }
        if (temp <= 0)
        {
            lbBack.Visible = false;
        }

        //Response.Redirect(CMSContext.CurrentDocument.NodeAliasPath + ".aspx?Page=" + Convert.ToString(pageNum + 1));
    }
}
