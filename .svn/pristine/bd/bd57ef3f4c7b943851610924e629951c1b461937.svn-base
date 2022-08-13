using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class CurrencyController : Controller
    {
        #region Declaration
        Currency _oCurrency = new Currency();
        List<Currency> _oCurrencys = new List<Currency>();
        CurrencyConversion _oCurrencyConversion = new CurrencyConversion();
        Company _oCompany = new Company();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewCurrencys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oCurrencys = new List<Currency>();
            _oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BaseCurrencyID = _oCompany.BaseCurrencyID;
            return View(_oCurrencys);
        }

        public ActionResult ViewCurrency(int id)
        {
            _oCurrency = new Currency();
            if (id > 0)
            {
                _oCurrency = _oCurrency.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            
            return PartialView(_oCurrency);
        }

        [HttpPost]
        public JsonResult Save(Currency oCurrency)
        {
            _oCurrency = new Currency();
            try
            {
                _oCurrency = oCurrency;
                _oCurrency = _oCurrency.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCurrency = new Currency();
                _oCurrency.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCurrency);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                Currency oCurrency = new Currency();
                sFeedBackMessage = oCurrency.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCurrencyConversion(int id)
        {
            _oCurrency = new Currency();
            if (id > 0)
            {
                _oCurrency = _oCurrency.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCurrency.ConversionListForSelectedCurrncy = CurrencyConversion.GetsByFromCurrency(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCurrency.Banks = Bank.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCurrency.MyCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCurrency.ToCurrencys = Currency.GetsLeftSelectedCurrency(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oCurrency);
        }

        [HttpPost]
        public JsonResult SaveCurrencyConversion(CurrencyConversion oCurrencyConversion)
        {
            _oCurrencyConversion = new CurrencyConversion();
            try
            {
                _oCurrencyConversion = oCurrencyConversion.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCurrencyConversion.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCurrencyConversion);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteCurrencyConversion(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                CurrencyConversion oCurrencyConversion = new CurrencyConversion();
                sFeedBackMessage = oCurrencyConversion.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Old Tast Task

        #region Functions
        private bool ValidateInput(Currency oCurrency)
        {
            if (oCurrency.CurrencyName == null || oCurrency.CurrencyName == "")
            {
                _sErrorMessage = "Please enter Currency Name";
                return false;
            }
            if (oCurrency.IssueFigure == null || oCurrency.IssueFigure == "")
            {
                _sErrorMessage = "Please enter Issue Figure";
                return false;
            }
            if (oCurrency.Symbol == null || oCurrency.Symbol == "")
            {
                _sErrorMessage = "Please enter Currency Symbol";
                return false;
            }
            if (oCurrency.SmallerUnit == null || oCurrency.SmallerUnit == "")
            {
                _sErrorMessage = "Please enter Currency Smaller Unit";
                return false;
            }
            return true;
        }
        #endregion

        public ActionResult RefreshList(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oCurrencys = new List<Currency>();
            _oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oCurrencys);
        }

        public ActionResult ConversionRateActivation(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oCurrencys = new List<Currency>();
            _oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oCurrencys);
        }

        public ActionResult Add()
        {
            _oCurrency = new Currency();
            return PartialView(_oCurrency);
        }

        [HttpPost]
        public ActionResult Add(Currency oCurrency)
        {
            try
            {
                if (this.ValidateInput(oCurrency))
                {
                    oCurrency = oCurrency.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    return RedirectToAction("RefreshList");
                }
                TempData["message"] = _sErrorMessage;
                return View(oCurrency);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }

        public ActionResult Edit(int id)
        {
            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oCurrency);
        }

        [HttpPost]
        public ActionResult Edit(Currency oCurrency)
        {
            try
            {
                if (this.ValidateInput(oCurrency))
                {
                    oCurrency = oCurrency.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    return RedirectToAction("RefreshList");
                }
                TempData["message"] = _sErrorMessage;
                return View(oCurrency);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //public ActionResult Delete(int id)
        //{
        //    Currency oCurrency = new Currency();
        //    oCurrency = oCurrency.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    return View(oCurrency);
        //}

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Currency oCurrency = new Currency();
            oCurrency.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return RedirectToAction("RefreshList");
        }

        public ViewResult Details(int id)
        {
            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oCurrency);
        }
        #endregion


    }
}
