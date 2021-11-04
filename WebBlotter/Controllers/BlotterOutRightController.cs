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
    public class BlotterOutRightController : Controller
    {
        UtilityClass UC = new UtilityClass();

        public ActionResult BlotterOutRight(FormCollection form)
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
                DateVal = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.DateVal = DateVal;
            }
            #endregion

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOutRight/GetAllblotterOutRight?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBPBlotterOR_Result> blotterOR = response.Content.ReadAsAsync<List<Models.SP_GetSBPBlotterOR_Result>>().Result;
            if (blotterOR.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterOR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("BlotterOutRight", blotterOR);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterOutRight model = new SBP_BlotterOutRight();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveAction;
                Session["ActiveController"] = ActiveController;

                if (ModelState.IsValid)
                {
                    model.CreateDate = DateTime.Now.Date;
                    model.Date = DateTime.Now.Date;

                    //ViewBag.FRBanks = GetAllNostroBanks();
                }
            }
            catch (Exception ex) { 

           
            }
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

                return PartialView("_Create");
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }

       


        [HttpPost]
        public ActionResult _Create(FormCollection form)
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
                List<SBP_BlotterOutRight> BlotterOR = new List<SBP_BlotterOutRight>();
                for (int i = 0; i <= Request.Form.Count; i++)
                {
                    if (Request.Form["Broker[" + i + "]"]!=null)
                    {

                        var DataType_data = (Session["BR"].ToString() != "01") ? Request.Form["DataType[" + i + "]"] : "SBP";
                        var Bank_data = Request.Form["Bank[" + i + "]"];
                        var Rate_data = Convert.ToDouble(Request.Form["Rate[" + i + "]"]);
                        var Broker_data = Request.Form["Broker[" + i + "]"];
                        var Issue_Date_data = Request.Form["Issue_Date[" + i + "]"];
                        var IssueType_data = Request.Form["IssueType[" + i + "]"];
                        var InFlow_data = Convert.ToDecimal(Request.Form["InFlow[" + i + "]"]);
                        var OutFLow_data = UC.CheckNegativeValue(Convert.ToDecimal(Request.Form["OutFLow[" + i + "]"]));
                        var Note_data = Request.Form["Note[" + i + "]"];

                        BlotterOR.Add(new SBP_BlotterOutRight
                        {
                            DataType = DataType_data,
                            Bank = Bank_data,
                            Rate = Rate_data,
                            Broker = Broker_data,
                            Issue_Date = Convert.ToDateTime(Issue_Date_data.ToString()),
                            IssueType = IssueType_data,
                            InFlow = InFlow_data,
                            OutFLow = OutFLow_data,
                            Note = Note_data,
                            UserID = Convert.ToInt16(Session["UserID"].ToString()),
                            BID = Convert.ToInt16(Session["BranchID"].ToString()),
                            BR = Convert.ToInt16(Session["BR"].ToString()),
                            CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString()),
                            CreateDate = DateTime.Now,
                            Date = DateTime.Now,
                            Status = true
                        });
                    }
                }


                if (ModelState.IsValid)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterOutRight/InsertOutRight", BlotterOR);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterOutRight");
                }
            }
            catch (Exception ex)
            {

            }


            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult Delete(IEnumerable<int> employeeIdsToDelete)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterOutRight/DeleteOutRight", employeeIdsToDelete);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(employeeIdsToDelete), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterOutRight");
        }
    }
}

        

