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
    public class BlotterRTGSController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterRTGS
        private List<Models.SP_GETAllTransactionTitles_Result> GetAllRTGSTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRTGS/GetAllRTGSTransactionTitles");
                response.EnsureSuccessStatusCode();
                List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;

                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult BlotterRTGS()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRTGS/GetAllBlotterRTGS?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAll_SBPBlotterRTGS_Result> blotterRTGS = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterRTGS_Result>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return View(blotterRTGS);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            SBP_BlotterRTGS model = new SBP_BlotterRTGS();
            try
            {
                if (ModelState.IsValid)
                {
                    model.RTGS_Date = DateTime.Now.Date;
                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
                else
                {

                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_BlotterRTGS BlotterRTGS)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterRTGS.RTGS_OutFLow = UC.CheckNegativeValue(BlotterRTGS.RTGS_OutFLow);
                    BlotterRTGS.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterRTGS.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterRTGS.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterRTGS.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterRTGS.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterRTGS/InsertRTGS", BlotterRTGS);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterRTGS");
                }
                else
                {

                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return View(BlotterRTGS);

        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRTGS/GetBlotterRTGS?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterRTGS BlotterRTGS = response.Content.ReadAsAsync<Models.SBP_BlotterRTGS>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return View(BlotterRTGS);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterRTGS BlotterRTGS)
        {
            BlotterRTGS.RTGS_OutFLow = UC.CheckNegativeValue(BlotterRTGS.RTGS_OutFLow);
            BlotterRTGS.UpdateDate = DateTime.Now;
            if (BlotterRTGS.RTGS_Date == null)
                BlotterRTGS.RTGS_Date = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterRTGS/UpdateRTGS", BlotterRTGS);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRTGS");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterRTGS/DeleteRTGS?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRTGS");
        }
    }
}