using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterDMMOController : Controller
    {

        public ActionResult BlotterDMMO(FormCollection form)
        {
            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;

            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);

            var DateVal = (dynamic)null;
            if (form["SearchByDate"] != null)
            {
                DateVal = form["SearchByDate"].ToString();
                ViewBag.DateVal = DateVal;
            }
            else
            {
                ViewBag.DateVal = DateTime.Now.ToString("yyyy-MM-dd");
            }
            #endregion

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDMMO/GetAllblotterDMMO?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBP_DMMO_Result> blotterDMMO = response.Content.ReadAsAsync<List<Models.SP_GetSBP_DMMO_Result>>().Result;
            if (blotterDMMO.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterDMMO), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterDMMO", blotterDMMO);
        }

        [HttpPost]
        public ActionResult Update(string sno, string Date, string PakistanBalance, string SBPBalanace, string BalanceDifference)
        {
            SBP_BlotterDMMO BlotterDMMO = new SBP_BlotterDMMO();
            BlotterDMMO.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterDMMO.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterDMMO.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterDMMO.SNo = Convert.ToInt32(sno);
            BlotterDMMO.Date = Convert.ToDateTime(Date);
            BlotterDMMO.PakistanBalance = Convert.ToDecimal(PakistanBalance.ToString());
            BlotterDMMO.SBPBalanace = Convert.ToDecimal(SBPBalanace.ToString());
            BlotterDMMO.BalanceDifference = BalanceDifference == null ? 0 : Convert.ToDecimal(BalanceDifference.ToString());
            BlotterDMMO.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterDMMO/UpdateDMMO", BlotterDMMO);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterDMMO), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterDMMO");
        }
    }       
}