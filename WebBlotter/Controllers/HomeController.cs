using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AuthAccess]
    public class HomeController : Controller
    {

        public ActionResult index(FormCollection form)
        {
            //var currentDT = GetCurrentDT();
            //ViewData["SysCurrentDt"] = currentDT.ToString("dd-MMM-yyyy"); 


            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;

            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);


            //string StartDate = string.Empty;
            //string EndDate = string.Empty;
            //// Date Parameters
            //if (form["startdate"] != null)
            //{
            //    StartDate = form["startdate"].ToString();
            //    ViewBag.SetStartDate = StartDate;
            //}
            //else
            //{
                ViewBag.SetStartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            //}
            //if (form["enddate"] != null)
            //{
            //    EndDate = form["enddate"].ToString();
            //    ViewBag.SetEndDate = EndDate;
            //}
            //else
            //{
                ViewBag.SetEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            //}
            #endregion


            //UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            //ServiceRepository serviceObj = new ServiceRepository();
            //HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterDTLReportDayWise?&BR=" + Session["BR"].ToString() + "&StartDate=" + DateTime.Now.ToString("yyyy-MM-dd") + "&EndDate=" + DateTime.Now.ToString("yyyy-MM-dd"));
            //response.EnsureSuccessStatusCode();
            //List<Models.SBP_BlotterCRRReportDaysWiseBal> BlotterCRRReportsDayWiseBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterCRRReportDaysWiseBal>>().Result;

            //ViewBag.SBP_BlotterCRRReportDaysWiseBal = BlotterCRRReportsDayWiseBal;

            ////HttpResponseMessage response1 = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterDTLReportForToday?&BR=" + Session["BR"].ToString());
            ////response.EnsureSuccessStatusCode();
            ////Models.SBP_BlotterCRRReportDaysWiseBal BlotterCRRReportForTodayBal = response.Content.ReadAsAsync<Models.SBP_BlotterCRRReportDaysWiseBal>().Result;

            ////ViewBag.SBP_BlotterCRRReportForTodayBal = BlotterCRRReportForTodayBal;

            //HttpResponseMessage response2 = serviceObj.GetResponse("/api/Blotter/GetLatestOpeningBalaceForToday?&BR=" + Session["BR"].ToString() + "&Date=" + DateTime.Now.ToString("yyyy-MM-dd"));
            //response2.EnsureSuccessStatusCode();
            //Models.SBP_BlotterOpeningBalance BlotterOpeningBalaceForToday = response2.Content.ReadAsAsync<Models.SBP_BlotterOpeningBalance>().Result;

            //ViewBag.SBP_BlotterOpeningBalaceForToday = BlotterOpeningBalaceForToday;


            return View();
        }

        public ActionResult Default() {
            return View();
        }

        public ActionResult FillSBPBlotterCRRReport(string StartDate,string EndDate)
        {

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterDTLReportDayWise?&BR=" + Session["BR"].ToString() + "&StartDate=" + StartDate + "&EndDate=" + EndDate);
            response.EnsureSuccessStatusCode();
            List<Models.SBP_BlotterCRRReportDaysWiseBal> BlotterCRRReportsDayWiseBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterCRRReportDaysWiseBal>>().Result;

            ViewBag.SBP_BlotterCRRReportDaysWiseBal = BlotterCRRReportsDayWiseBal;


            return PartialView("_SBPBlotterCRRReportDaysWiseBal");

        }

        public ActionResult FillBlotterManualData(string Date, string flag, int Recon)
        {
            Models.GetBlotterMnualDataParam BMDP = new Models.GetBlotterMnualDataParam();
            BMDP.Recon = (Recon == 1) ? true : false;
            BMDP.DateFor = (Recon == 1) ? DateTime.Now :Convert.ToDateTime(Date);
            BMDP.flag = flag;
            BMDP.BR = Convert.ToInt32(Session["BR"].ToString());
            BMDP.CurId = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("/api/Blotter/GetOPICSManualData",BMDP);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetOPICSManualData_Result> OPICSManualData = response.Content.ReadAsAsync<List<Models.SP_GetOPICSManualData_Result>>().Result;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(OPICSManualData), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.FillBlotterManualData = OPICSManualData;


            return PartialView("_FillManualData");

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public DateTime GetCurrentDT()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT?brcode=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_SBPOpicsSystemDate_Result> blotterDT = response.Content.ReadAsAsync<List<Models.SP_SBPOpicsSystemDate_Result>>().Result;
               
                return Convert.ToDateTime(blotterDT[0].OpicsCurrentDate);
             

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SessionIsAlive()
        {

            if (Session["UserID"] == null)
            {
                Response.Redirect(new Uri(Request.Url, Url.Action("Login")).ToString(), false);
            }
        }

    }
}