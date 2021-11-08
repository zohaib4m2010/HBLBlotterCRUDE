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
    public class BlotterReconBreakupsController : Controller
    {
        // GET: BlotterReconBreakups

        UtilityClass UC = new UtilityClass();
        // GET: BlotterRTGS
        private List<Models.SP_GETAllTransactionTitles_Result> GetAllRECONBrekupsTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterReconBreakups/GetAllRECONBreakupsTransactionTitles");
                response.EnsureSuccessStatusCode();
                List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;

                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Models.Branches> GetAllBranches()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/Branches/GetAllBranches");
                response.EnsureSuccessStatusCode();
                List<Models.Branches> GABR = response.Content.ReadAsAsync<List<Models.Branches>>().Result;

                return GABR;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult BlotterReconBreakups(FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)
                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

                UtilityClass.GetSelectedCurrecy(selectCurrency);
                var DateVal = "";
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterReconBreakups/GetAllBlotterBreakups?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAll_SBPBlotterReconBreakups_Results> blotterBreakup = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterReconBreakups_Results>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterBreakup), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_BlotterReconBreakups", blotterBreakup);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {

            var ActiveAction = RouteData.Values["action"].ToString();
            var ActiveController = RouteData.Values["controller"].ToString();
            Session["ActiveAction"] = ActiveController;
            Session["ActiveController"] = ActiveAction;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            SBP_BlotterReconBreakups model = new SBP_BlotterReconBreakups();
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ReconBreakupsTransactionTitles = GetAllRECONBrekupsTransactionTitles();
                }
                else
                {

                    ViewBag.ReconBreakupsTransactionTitles = GetAllRECONBrekupsTransactionTitles();
                }
            }
            catch (Exception ex) { }
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ReconBreakupsBranches = GetAllBranches();
                }
                else
                {

                    ViewBag.ReconBreakupsBranches = GetAllBranches();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", model);
        }

        public ActionResult Create(FormCollection form)
        {
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                #endregion

                if (ModelState.IsValid)
                {
                    ViewBag.ReconBreakupsTransactionTitles = GetAllRECONBrekupsTransactionTitles();
                }
                else
                {
                    ViewBag.ReconBreakupsTransactionTitles = GetAllRECONBrekupsTransactionTitles();
                }
            }
            catch (Exception ex) { }
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ReconBreakupsBranches = GetAllBranches();
                }
                else
                {

                    ViewBag.ReconBreakupsBranches = GetAllBranches();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterReconBreakups BlotterReconBreakup, FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)
                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

                UtilityClass.GetSelectedCurrecy(selectCurrency);
                #endregion
                if (ModelState.IsValid)
                {
                    BlotterReconBreakup.RECON_OutFLow = UC.CheckNegativeValue(BlotterReconBreakup.RECON_OutFLow);
                    BlotterReconBreakup.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterReconBreakup.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterReconBreakup.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterReconBreakup.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterReconBreakups/InsertReconBreakups", BlotterReconBreakup);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterReconBreakup), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterReconBreakups");
                }
                else
                {

                    ViewBag.RTGSTransactionTitles = GetAllRECONBrekupsTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterReconBreakup);

        }

        public ActionResult Edit(int id, FormCollection form)
        {
            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);
            #endregion


            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterReconBreakups/GetBlotterReconBreakupsById?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterReconBreakups BlotterReconBreakup = response.Content.ReadAsAsync<Models.SBP_BlotterReconBreakups>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterReconBreakup), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.ReconBreakupsBranches = GetAllBranches();
            ViewBag.RTGSTransactionTitles = GetAllRECONBrekupsTransactionTitles();
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return PartialView("_Edit", BlotterReconBreakup);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterReconBreakups BlotterReconBreakups)
        {
            BlotterReconBreakups.RECON_OutFLow = UC.CheckNegativeValue(BlotterReconBreakups.RECON_OutFLow);
            BlotterReconBreakups.UpdateDate = DateTime.Now;
            if (BlotterReconBreakups.RECON_Date == null)
                BlotterReconBreakups.RECON_Date = DateTime.Now;
            BlotterReconBreakups.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterReconBreakups/UpdateReconBreakups", BlotterReconBreakups);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterReconBreakups), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterReconBreakups");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterReconBreakups/DeleteReconBreakups?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterReconBreakups");
        }
    }
}