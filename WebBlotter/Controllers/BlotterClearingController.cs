﻿using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Classes;
using Newtonsoft.Json;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterClearingController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterClearing
        private List<Models.SP_GETAllTransactionTitles_Result> GetAllClearingTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterClearing/GetAllClearingTransactionTitles");
                response.EnsureSuccessStatusCode();
                List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;

                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult BlotterClearing(FormCollection form)
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

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterClearing/GetAllBlotterClearing?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAll_SBPBlotterClearing_Result> blotterClearing = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterClearing_Result>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterClearing), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_BlotterClearing", blotterClearing);
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
            SBP_BlotterClearing model = new SBP_BlotterClearing();
            try
            {
                if (ModelState.IsValid)
                {
                    model.Clearing_Date = DateTime.Now.Date;
                    ViewBag.ClearingTransactionTitles = GetAllClearingTransactionTitles();
                }
                else
                {

                    ViewBag.ClearingTransactionTitles = GetAllClearingTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", model);
        }
        public ActionResult Create(FormCollection form)
        {
            #region Added by shakir (Currency parameter)

            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
            UtilityClass.GetSelectedCurrecy(selectCurrency);

            #endregion

            SBP_BlotterClearing model = new SBP_BlotterClearing();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                if (ModelState.IsValid)
                {
                    model.Clearing_Date = DateTime.Now.Date;
                    ViewBag.ClearingTransactionTitles = GetAllClearingTransactionTitles();
                }
                else
                {
                    ViewBag.ClearingTransactionTitles = GetAllClearingTransactionTitles();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterClearing BlotterClearing, FormCollection form)
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
                    BlotterClearing.Clearing_OutFLow = UC.CheckNegativeValue(BlotterClearing.Clearing_OutFLow);
                    BlotterClearing.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterClearing.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterClearing.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterClearing.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterClearing.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterClearing/InsertClearing", BlotterClearing);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterClearing), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterClearing");
                }
                else
                {

                    ViewBag.ClearingTransactionTitles = GetAllClearingTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterClearing);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterClearing/GetBlotterClearing?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterClearing BlotterClearing = response.Content.ReadAsAsync<Models.SBP_BlotterClearing>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterClearing), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.ClearingTransactionTitles = GetAllClearingTransactionTitles();
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return PartialView("_Edit", BlotterClearing);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterClearing BlotterClearing)
        {
            BlotterClearing.Clearing_OutFLow = UC.CheckNegativeValue(BlotterClearing.Clearing_OutFLow);
            BlotterClearing.UpdateDate = DateTime.Now;
            if (BlotterClearing.Clearing_Date == null)
                BlotterClearing.Clearing_Date = DateTime.Now;
            BlotterClearing.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterClearing/UpdateClearing", BlotterClearing);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterClearing), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterClearing");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterClearing/DeleteClearing?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterClearing");
        }
    }
}