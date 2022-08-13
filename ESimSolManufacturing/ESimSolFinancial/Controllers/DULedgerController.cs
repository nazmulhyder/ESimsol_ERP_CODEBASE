using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
    public class DULedgerController : Controller
    {
        #region Declaration
        DULedger _oDULedger = new DULedger();
        List<DULedger> _oDULedgers = new List<DULedger>();
        string _sErrorMessage = "";
        #endregion

        #region DULEDGER
        public ActionResult ViewDULedgers(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            DULedger oDULedger = new DULedger();
            List<DULedger> _oDULedgers = new List<DULedger>();
            List<DULedger> _oDULedgerCollections = new List<DULedger>();
            oDULedger.EndDate = DateTime.Now;
            oDULedger.StartDate = DateTime.Now.AddMonths(-4);
            oDULedger.ViewType = oDULedger.Layout = 1;
            oDULedger.MKT_CPID = oDULedger.CurrencyID = 0;
            _oDULedgers = DULedger.Gets(oDULedger, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oDULedgerCollections = this.GetsOrderIssue(_oDULedgers);

            #region Delivery
            //oDULedgerTemp = new DULedger();
            //oDULedgerTemp.OrderTypeSt = "Delivery";
            //oDULedgerTemp.PaymentTypeSet = "";
            //oDULedgerTemp.DyeingOrderType = 0;
            //oDULedgerTemp.OrderCount = _oDULedgers.Where(x => x.Qty_DC > 0).ToList().Sum(x => x.OrderCount);
            //oDULedgerTemp.Qty = _oDULedgers.Where(x => x.Qty_DC > 0).Sum(x => x.Qty_DC);
            //oDULedgerTemp.Amount = _oDULedgers.Where(x => x.Qty_DC > 0).Sum(x => x.Amount_DC);
            //_oDULedgerCollections.Add(oDULedgerTemp);

            //foreach (var oItem in _oDULedgers.Where(x => x.Qty_DC > 0))
            //{
            //    oDULedgerTemp = new DULedger();
            //    oDULedgerTemp.OrderTypeSt = oItem.DyeingOrderTypeSt;
            //    oDULedgerTemp.PaymentTypeSet = oItem.PaymentTypeSt;
            //    oDULedgerTemp.DyeingOrderType = oItem.DyeingOrderType;
            //    oDULedgerTemp.OrderCount = oItem.OrderCount;
            //    oDULedgerTemp.Qty = oItem.Qty_DC;
            //    oDULedgerTemp.Amount = oItem.Amount_DC;
            //    _oDULedgerCollections.Add(oDULedgerTemp);
            //}
            #endregion

            #region Payment
            //oDULedgerTemp = new DULedger();
            //oDULedgerTemp.OrderTypeSt = "Payment";
            //oDULedgerTemp.PaymentTypeSet = "";
            //oDULedgerTemp.DyeingOrderType = 0;
            //oDULedgerTemp.OrderCount = _oDULedgers.Where(x => x.Qty_Paid > 0).ToList().Sum(x => x.OrderCount);
            //oDULedgerTemp.Qty = _oDULedgers.Where(x => x.Qty_Paid > 0).Sum(x => x.Qty_Paid);
            //oDULedgerTemp.Amount = _oDULedgers.Where(x => x.Qty_Paid > 0).Sum(x => x.Amount_Paid);
            //_oDULedgerCollections.Add(oDULedgerTemp);

            //foreach (var oItem in _oDULedgers.Where(x => x.Qty_Paid > 0))
            //{
            //    oDULedgerTemp = new DULedger();
            //    oDULedgerTemp.OrderTypeSt = oItem.DyeingOrderTypeSt;
            //    oDULedgerTemp.PaymentTypeSet = oItem.PaymentTypeSt;
            //    oDULedgerTemp.DyeingOrderType = oItem.DyeingOrderType;
            //    oDULedgerTemp.OrderCount = oItem.OrderCount;
            //    oDULedgerTemp.Qty = oItem.Qty_Paid;
            //    oDULedgerTemp.Amount = oItem.Amount_Paid;
            //    _oDULedgerCollections.Add(oDULedgerTemp);
            //}
            #endregion

            #region Pending
            //oDULedgerTemp = new DULedger();
            //oDULedgerTemp.OrderTypeSt = "Pending Delivery";
            //oDULedgerTemp.PaymentTypeSet = "";
            //oDULedgerTemp.DyeingOrderType = 0;
            //oDULedgerTemp.OrderCount = _oDULedgers.Where(x => x.Qty_YetTo > 0).ToList().Sum(x => x.OrderCount); ;
            //oDULedgerTemp.Qty = _oDULedgers.Where(x => x.Qty_YetTo > 0).Sum(x => x.Qty_Paid);
            //oDULedgerTemp.Amount = _oDULedgers.Where(x => x.Qty_YetTo > 0).Sum(x => x.Amount_Paid);
            //_oDULedgerCollections.Add(oDULedgerTemp);

            //foreach (var oItem in _oDULedgers.Where(x => x.Qty_YetTo > 0))
            //{
            //    oDULedgerTemp = new DULedger();
            //    oDULedgerTemp.OrderTypeSt = oItem.DyeingOrderTypeSt;
            //    oDULedgerTemp.PaymentTypeSet = oItem.PaymentTypeSt;
            //    oDULedgerTemp.DyeingOrderType = oItem.DyeingOrderType;
            //    oDULedgerTemp.OrderCount = oItem.OrderCount;
            //    oDULedgerTemp.Qty = oItem.Qty_Paid;
            //    oDULedgerTemp.Amount = oItem.Amount_Paid;
            //    _oDULedgerCollections.Add(oDULedgerTemp);
            //}
            #endregion

            ViewBag.DULedger = oDULedger;
            ViewBag.BUID = buid;
            return View(_oDULedgerCollections);
        }
        public ActionResult ViewDULedgersDetails(string sParam, int buid)
        {
            _oDULedger = new DULedger();
            List<DULedger> _oDULedgers = new List<DULedger>();
            if (!string.IsNullOrEmpty(sParam))
            {
                _oDULedgers = this.Gets(sParam);
                ViewBag.DULedger = _oDULedger;
            }
            ViewBag.BUID = buid;
            return View(_oDULedgers);
        }
        public ActionResult ViewDULedgersOrderDetails(string sParam, int buid)
        {
            _oDULedger = new DULedger();
            List<DULedger> _oDULedgers = new List<DULedger>();
            if (!string.IsNullOrEmpty(sParam))
            {
                _oDULedgers = this.Gets(sParam);
                ViewBag.DULedger = _oDULedger;
            }
            ViewBag.BUID = buid;
            return View(_oDULedgers);
        }
        #endregion

        #region Report
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public ActionResult PrintDULedger(String sParam, int nBUID)
        {
            List<DULedger> oDULedgers = new List<DULedger>();
            List<DULedger> oDULedgers_ExportBill = new List<DULedger>();

            if (string.IsNullOrEmpty(sParam))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                oDULedgers = this.Gets(sParam);

                if (_oDULedger.Layout == 1)
                    oDULedgers = this.GetsOrderIssue(oDULedgers);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDULedger oReport = new rptDULedger();
            byte[] abytes = oReport.PrepareReport(_oDULedger, oDULedgers, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Funtions
        [HttpPost]
        public JsonResult GetsDULedger(DULedger oDULedger)
        {
            List<DULedger> oDULedgers = new List<DULedger>();
            oDULedgers = new List<DULedger>();
            try
            {
                if (!string.IsNullOrEmpty(oDULedger.Params))
                    oDULedgers = this.Gets(oDULedger.Params);

                if (_oDULedger.Layout==1)
                    oDULedgers = this.GetsOrderIssue(oDULedgers);
            }
            catch (Exception ex)
            {
                oDULedgers = new List<DULedger>();
                oDULedger.ErrorMessage = ex.Message;
                oDULedgers.Add(oDULedger);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDULedgers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public List<DULedger> GetsOrderIssue(List<DULedger> oDULedgers) 
        {
            List<DULedger> _oDULedgerCollections = new List<DULedger>();
            
            #region Order Issue CurrencyWise
            oDULedgers = oDULedgers.OrderBy(x => x.CurrencyID).ThenBy(x => x.DyeingOrderType).ToList();
            int nCurrencyID = 9999, nType=0;
            foreach(var oItem in oDULedgers)
            {
                if (nCurrencyID != oItem.CurrencyID) 
                {
                    DULedger oDULedgerTemp = new DULedger();
                    oDULedgerTemp.OrderTypeSt = "Order Issue" + (string.IsNullOrEmpty(oItem.CurrencyName)?"": " ("+oItem.CurrencyName+")");
                    oDULedgerTemp.DyeingOrderType = 0;
                    oDULedgerTemp = MakeTypeWiseOrder(oDULedgerTemp, oDULedgers.Where(x=>x.CurrencyID==oItem.CurrencyID).ToList());
                    _oDULedgerCollections.Add(oDULedgerTemp);

                    nType=0;
                    #region Order Issue DyeingOrderType Wise
                    foreach (var oDU in oDULedgers.Where(x => x.CurrencyID == oItem.CurrencyID))
                    {
                        if (nType!=oDU.DyeingOrderType)
                        {
                            oDULedgerTemp = new DULedger();
                            oDULedgerTemp.OrderTypeSt = "";
                            oDULedgerTemp.PaymentTypeSet = oDU.DyeingOrderTypeSt;
                            oDULedgerTemp.DyeingOrderType = oDU.DyeingOrderType;

                            if (oDULedgers.Where(x => x.CurrencyID == oDU.CurrencyID & x.DyeingOrderType == oDU.DyeingOrderType).ToList().Count > 1)
                            {
                                oDULedgerTemp = MakeTypeWiseOrder(oDULedgerTemp, oDULedgers.Where(x => x.CurrencyID == oDU.CurrencyID & x.DyeingOrderType == oDU.DyeingOrderType).ToList());
                                _oDULedgerCollections.Add(oDULedgerTemp);
                            }

                            MakeDetail(ref _oDULedgerCollections, oDULedgers.Where(x => x.CurrencyID == oDU.CurrencyID & x.DyeingOrderType == oDU.DyeingOrderType).ToList());
                        }
                        nType=oDU.DyeingOrderType;
                    }
                    #endregion
                }
                nCurrencyID = oItem.CurrencyID;
            }
            #endregion

            return _oDULedgerCollections;
        }
        public DULedger MakeTypeWiseOrder(DULedger oDULedgerTemp, List<DULedger> oDULedgers) 
        {
            oDULedgerTemp.CurrencyID = oDULedgers.Select(x => x.CurrencyID).FirstOrDefault();
            oDULedgerTemp.OrderCount = oDULedgers.Sum(x => x.OrderCount);
            oDULedgerTemp.QtySt = ( oDULedgers.Sum(x => x.Qty)!= 0 ? (Global.MillionFormat_Round(oDULedgers.Sum(x => x.Qty))+oDULedgers.Select(x=>x.MUName).FirstOrDefault()): "-" );
            oDULedgerTemp.AmountSt =(oDULedgers.Sum(x => x.Amount)!=0? (oDULedgers.Select(x => x.CurrencySymbol).FirstOrDefault() + Global.MillionFormat_Round(oDULedgers.Sum(x => x.Amount))):"-");

            oDULedgerTemp.Qty_DCSt =oDULedgers.Where(x => x.Qty_DC > 0).Sum(x => x.Qty_DC)!=0?Global.MillionFormat_Round( oDULedgers.Where(x => x.Qty_DC > 0).Sum(x => x.Qty_DC)) + oDULedgers.Select(x => x.MUName).FirstOrDefault() :"-";
            oDULedgerTemp.Amount_DCSt =oDULedgers.Where(x => x.Qty_DC > 0).Sum(x => x.Amount_DC)!=0?oDULedgers.Select(x => x.CurrencySymbol).FirstOrDefault()+ Global.MillionFormat_Round(oDULedgers.Where(x => x.Qty_DC > 0).Sum(x => x.Amount_DC)):"-";
            oDULedgerTemp.Qty_Paid = oDULedgers.Where(x => x.Qty_Paid > 0).Sum(x => x.Qty_Paid);
            oDULedgerTemp.Amount_PaidSt =oDULedgers.Where(x => x.Qty_Paid > 0).Sum(x => x.Amount_Paid)!=0? oDULedgers.Select(x => x.CurrencySymbol).FirstOrDefault() + Global.MillionFormat_Round(oDULedgers.Where(x => x.Qty_Paid > 0).Sum(x => x.Amount_Paid)):"-";
            oDULedgerTemp.Qty_YetToSt =(oDULedgerTemp.Qty - oDULedgerTemp.Qty_DC)!=0 ? ( Global.MillionFormat_Round( (oDULedgerTemp.Qty - oDULedgerTemp.Qty_DC))+oDULedgers.Select(x=>x.MUName).FirstOrDefault()):"-";
            oDULedgerTemp.Amount_YetToSt =((oDULedgerTemp.Amount - oDULedgerTemp.Amount_Paid)!=0? oDULedgers.Select(x => x.CurrencySymbol).FirstOrDefault() + Global.MillionFormat_Round((oDULedgerTemp.Amount - oDULedgerTemp.Amount_Paid)):"-");
            return oDULedgerTemp;
        }
        public List<DULedger> MakeDetail(ref List<DULedger> oDULedgerCollections, List<DULedger> oDULedgers) 
        {
            foreach (var oItem in oDULedgers)
            {
                DULedger oDULedgerTemp = new DULedger();
                oDULedgerTemp = oItem;
                oDULedgerTemp.CurrencyID = oItem.CurrencyID;
                oDULedgerTemp.QtySt = Global.MillionFormat_Round(oItem.Qty) + oItem.MUName;
                oDULedgerTemp.AmountSt = "" + Global.MillionFormat_Round(oItem.Amount);
                oDULedgerTemp.OrderTypeSt = oItem.DyeingOrderTypeSt;
                oDULedgerTemp.PaymentTypeSet = oItem.PaymentTypeSt;
                oDULedgerTemp.Qty_YetToSt = Global.MillionFormat_Round(oItem.Qty - oItem.Qty_DC) + "";
                oDULedgerTemp.Amount_YetToSt = "" + Global.MillionFormat_Round(oItem.Amount - oItem.Amount_Paid);
                oDULedgerTemp.Amount_PaidSt = "" + Global.MillionFormat_Round(oItem.Amount_Paid);
                oDULedgerTemp.Qty_DCSt = "" + Global.MillionFormat_Round(oItem.Qty_DC);
                oDULedgerTemp.Amount_DCSt = "" + Global.MillionFormat_Round(oItem.Amount_DC);
                oDULedgerCollections.Add(oDULedgerTemp);
            }
            return oDULedgerCollections;
        }
        public List<DULedger> Gets(string sParam) 
        {
            _oDULedger.StartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            _oDULedger.EndDate = Convert.ToDateTime(sParam.Split('~')[1]);
            _oDULedger.ViewType = Convert.ToInt32(sParam.Split('~')[2]);
            _oDULedger.Layout = Convert.ToInt32(sParam.Split('~')[3]);
            _oDULedger.DyeingOrderType = Convert.ToInt32(sParam.Split('~')[4]);
            _oDULedger.PaymentType = Convert.ToInt32(sParam.Split('~')[5]);
            _oDULedger.MKT_CPID = Convert.ToInt32(sParam.Split('~')[6]);
            _oDULedger.CurrencyID = Convert.ToInt32(sParam.Split('~')[7]);

            _oDULedgers = DULedger.Gets(_oDULedger, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return _oDULedgers;
        }
        #endregion

    }

}

