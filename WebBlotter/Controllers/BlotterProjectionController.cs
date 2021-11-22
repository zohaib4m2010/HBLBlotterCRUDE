using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterProjectionController : Controller
    {
        // GET: BlotterProjection



        public ActionResult BlotterProjection(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterProjection/GetAllblotterProjection?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            List<Models.SP_Get_BlotterProjection_Result> blotterProj = response.Content.ReadAsAsync<List<Models.SP_Get_BlotterProjection_Result>>().Result;
            if (blotterProj.Count < 1)
                ViewData["DataStatus"] = "Data Not Available";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterProj), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterProjection", blotterProj);
        }

        [HttpPost]
        public ActionResult Update(string sno, string Date, string Proj_Inflow, string Proj_OutFlow,string note)
        {
            SBP_BlotterProjection BlotterProj = new SBP_BlotterProjection();
            BlotterProj.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterProj.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterProj.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterProj.SNO = Convert.ToInt32(sno);
            BlotterProj.Date = Convert.ToDateTime(Date);
            BlotterProj.Proj_InFlow = Convert.ToDecimal(Proj_Inflow.ToString());
            BlotterProj.Proj_OutFlow = Convert.ToDecimal(Proj_OutFlow.ToString());
            BlotterProj.Note = note;
            BlotterProj.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterProjection/UpdateProjection", BlotterProj);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterProj), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterProjection");
        }
    }
}