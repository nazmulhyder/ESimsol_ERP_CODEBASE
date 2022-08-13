using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUReturnChallanController : Controller
    {
        #region Declaration
        DUReturnChallan _oDUReturnChallan = new DUReturnChallan();
        DUReturnChallanDetail _oDUReturnChallanDetail = new DUReturnChallanDetail();
        List<DUReturnChallan> _oDUReturnChallans = new List<DUReturnChallan>();
        List<DUReturnChallanDetail> _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
        string _sErrorMessage = "";
        #endregion

        #region DUReturnChallan
        public ActionResult ViewDUReturnChallans(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDUReturnChallans = new List<DUReturnChallan>();
            _oDUReturnChallans = DUReturnChallan.Gets("Select * from View_DUReturnChallan where isnull(ApprovedBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
          
            ViewBag.BUID = buid;
            return View(_oDUReturnChallans);
        }

        public ActionResult ViewDUReturnChallan(int nId, double ts)
        {
            _oDUReturnChallan = new DUReturnChallan();
          
            if (nId > 0)
            {
                _oDUReturnChallan = _oDUReturnChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUReturnChallan.DUReturnChallanDetails = DUReturnChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if( _oDUReturnChallan.DUReturnChallanDetails.Count>0)
                {
                    _oDUReturnChallan.OrderNo = string.Join(",", _oDUReturnChallan.DUReturnChallanDetails.Select(x => x.OrderNo).Distinct().ToList());
                    _oDUReturnChallan.PINo = string.Join(",", _oDUReturnChallan.DUReturnChallanDetails.Select(x => x.PINo).Distinct().ToList());
                }

            }
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.DUReturnChallan, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            return View(_oDUReturnChallan);
        }
      
        private bool ValidateInput(DUReturnChallan oDUReturnChallan)
        {
           if (oDUReturnChallan.DUDeliveryChallanID <=0)
             {
               _sErrorMessage = "Please pick Party";
               return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(DUReturnChallan oDUReturnChallan)
        {
           
            try
            {
                if (this.ValidateInput(oDUReturnChallan))
                {
                    _oDUReturnChallan = oDUReturnChallan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUReturnChallan.DUReturnChallanID > 0)
                    {
                        _oDUReturnChallan.DUReturnChallanDetails = DUReturnChallanDetail.Gets(_oDUReturnChallan.DUReturnChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _oDUReturnChallan.ErrorMessage=_sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDUReturnChallan = new DUReturnChallan();
                _oDUReturnChallan.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
     
        [HttpPost]
        public JsonResult Approve(DUReturnChallan oDUReturnChallan)
        {
            string sErrorMease = "";
            _oDUReturnChallan = oDUReturnChallan;
            try
            {
                _oDUReturnChallan = oDUReturnChallan.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(DUReturnChallan oDUReturnChallan)
        {
            try
            {
                if (oDUReturnChallan.DUReturnChallanID <= 0) { throw new Exception("Please select an valid item."); }
                oDUReturnChallan.ErrorMessage = oDUReturnChallan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUReturnChallan = new DUReturnChallan();
                oDUReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUReturnChallan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //  [HttpPost]
        //public JsonResult DeleteDetail(DUReturnChallanDetail oDUReturnChallanDetail)
        //{
        //    try
        //    {
        //        if (oDUReturnChallanDetail.DUReturnChallanDetailID > 0)
        //        {
        //            oDUReturnChallanDetail.ErrorMessage = oDUReturnChallanDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oDUReturnChallanDetail = new DUReturnChallanDetail();
        //        oDUReturnChallanDetail.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oDUReturnChallanDetail.ErrorMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

          [HttpPost]
        public JsonResult GetDCs(DUDeliveryChallan oDUDeliveryChallan)
          {
             
              List<DUDeliveryChallan> oDUDeliveryChallans = new List<DUDeliveryChallan>();
              List<DUReturnChallan> oDUReturnChallans = new List<DUReturnChallan>();
              DUReturnChallan oDUReturnChallan = new DUReturnChallan();
              try
              {
                  string sSQL = "SELECT * FROM View_DUDeliveryChallan";
                  string sReturn = "";
                  if (!String.IsNullOrEmpty(oDUDeliveryChallan.ChallanNo))
                  {
                      oDUDeliveryChallan.ChallanNo = oDUDeliveryChallan.ChallanNo.Trim();
                      Global.TagSQL(ref sReturn);
                      sReturn = sReturn + "ChallanNo like '%" + oDUDeliveryChallan.ChallanNo + "%'";
                  }
                 
                  Global.TagSQL(ref sReturn);
                  sReturn = sReturn + "isnull(IsDelivered,0)=1";
                 
                  sSQL = sSQL + "" + sReturn;
                  oDUDeliveryChallans = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                  foreach(DUDeliveryChallan oitem in oDUDeliveryChallans)
                  {
                      oDUReturnChallan = new DUReturnChallan();
                      oDUReturnChallan.DeliveryChallanNo = oitem.ChallanNo;
                      oDUReturnChallan.Qty = oitem.Qty;
                      oDUReturnChallan.DUDeliveryChallanID = oitem.DUDeliveryChallanID;
                      oDUReturnChallan.OrderType = oitem.OrderType;
                      oDUReturnChallan.ContractorName = oitem.ContractorName;
                      oDUReturnChallan.DONo = oitem.DONos;
                      oDUReturnChallans.Add(oDUReturnChallan);
                  }

              }
              catch (Exception ex)
              {
                  oDUReturnChallans = new List<DUReturnChallan>();
              }
              JavaScriptSerializer serializer = new JavaScriptSerializer();
              string sjson = serializer.Serialize(oDUReturnChallans);
              return Json(sjson, JsonRequestBehavior.AllowGet);
          }
         [HttpPost]
         public JsonResult GetDCDetails(DUDeliveryChallanDetail oDUDeliveryChallanDetail)
         {
             _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
             List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
             DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
             try
             {
                
                 oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(oDUDeliveryChallanDetail.DUDeliveryChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 foreach (DUDeliveryChallanDetail oitem in oDUDeliveryChallanDetails)
                 {
                     oDUReturnChallanDetail = new DUReturnChallanDetail();
                     oDUReturnChallanDetail.LotID = oitem.LotID;
                     oDUReturnChallanDetail.LotNo = oitem.LotNo;
                     oDUReturnChallanDetail.Qty = oitem.Qty;
                     oDUReturnChallanDetail.DUDeliveryChallanDetailID = oitem.DUDeliveryChallanDetailID;
                     oDUReturnChallanDetail.PTUID = oitem.PTUID;
                     oDUReturnChallanDetail.MUnit = oitem.MUnit;
                     oDUReturnChallanDetail.ProductCode = oitem.ProductCode;
                     oDUReturnChallanDetail.ProductName = oitem.ProductName;
                     oDUReturnChallanDetail.MUnit = oitem.MUnit;
                     oDUReturnChallanDetail.ProductID = oitem.ProductID;
                     oDUReturnChallanDetail.OrderNo = oitem.OrderNo;
                     oDUReturnChallanDetail.PINo = oitem.PI_SampleNo;
                     oDUReturnChallanDetail.OrderNo = oitem.OrderNo;
                     _oDUReturnChallanDetails.Add(oDUReturnChallanDetail);
                 }
             }
             catch (Exception ex)
             {
                 _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oDUReturnChallanDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public JsonResult GetsDCDetail(DUDeliveryChallanDetail oDUDeliveryChallanDetail)
         {
             _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
             List<DUDeliveryChallanRegister> oDUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
             DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
             try
             {

                 string sSQL = "Select top(200)* from View_DUDeliveryChallanRegister";
                 string sReturn = "";
                 if (!String.IsNullOrEmpty(oDUDeliveryChallanDetail.ChallanNo))
                 {
                     oDUDeliveryChallanDetail.ChallanNo = oDUDeliveryChallanDetail.ChallanNo.Trim();
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "ChallanNo like '%" + oDUDeliveryChallanDetail.ChallanNo + "%'";
                 }
                 if (!String.IsNullOrEmpty(oDUDeliveryChallanDetail.OrderNo))
                 {
                     oDUDeliveryChallanDetail.OrderNo = oDUDeliveryChallanDetail.OrderNo.Trim();
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "OrderNo like '%" + oDUDeliveryChallanDetail.OrderNo + "%'";
                 }
                 if (!String.IsNullOrEmpty(oDUDeliveryChallanDetail.LotNo))
                 {
                     oDUDeliveryChallanDetail.LotNo = oDUDeliveryChallanDetail.LotNo.Trim();
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "LotNo like '%" + oDUDeliveryChallanDetail.LotNo + "%'";
                 }
                 //if (!String.IsNullOrEmpty(oDUDeliveryChallanDetail.Co))
                 //{
                 //    oDUDeliveryChallanDetail.LotNo = oDUDeliveryChallanDetail.LotNo.Trim();
                 //    Global.TagSQL(ref sReturn);
                 //    sReturn = sReturn + "LotNo like '%" + oDUDeliveryChallanDetail.LotNo + "%'";
                 //}
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + "isnull(IsDelivered,0)=1 order by  ChallanDate Desc,LotNo ASC ";

                 sSQL = sSQL + "" + sReturn;
                 oDUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                 foreach (DUDeliveryChallanRegister oitem in oDUDeliveryChallanRegisters)
                 {
                     oDUReturnChallanDetail = new DUReturnChallanDetail();
                     oDUReturnChallanDetail.LotID = oitem.LotID;
                     oDUReturnChallanDetail.LotNo = oitem.LotNo;
                     oDUReturnChallanDetail.Qty = oitem.Qty;
                     oDUReturnChallanDetail.Qty_Order = oitem.Qty_Order;
                     oDUReturnChallanDetail.DUDeliveryChallanDetailID = oitem.DUDeliveryChallanDetailID;
                     oDUReturnChallanDetail.DyeingOrderDetailID = oitem.DyeingOrderDetailID;
                     oDUReturnChallanDetail.DUDeliveryChallanID = oitem.DUDeliveryChallanID;
                     oDUReturnChallanDetail.MUnit = oitem.MUnit;
                     oDUReturnChallanDetail.ProductCode = oitem.ProductCode;
                     oDUReturnChallanDetail.ProductName = oitem.ProductName;
                     oDUReturnChallanDetail.MUnit = oitem.MUnit;
                     oDUReturnChallanDetail.ProductID = oitem.ProductID;
                     //oDUReturnChallanDetail.OrderNo = oitem.OrderNo;
                    // oDUReturnChallanDetail.OrderNo = oitem.DyeingOrderID;
                     oDUReturnChallanDetail.PINo = oitem.PINo;
                     oDUReturnChallanDetail.OrderNo =oitem.NoCode+""+ oitem.OrderNo;
                     oDUReturnChallanDetail.RCNo = oitem.ChallanNo;
                     oDUReturnChallanDetail.RCDate = oitem.ChallanDateSt;
                     oDUReturnChallanDetail.ContractorName = oitem.ContractorName;
                     _oDUReturnChallanDetails.Add(oDUReturnChallanDetail);
                 }
             }
             catch (Exception ex)
             {
                 _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oDUReturnChallanDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }

        #endregion

        #region Search
         [HttpPost]
        public JsonResult AdvSearch(DUReturnChallan oDUReturnChallan)
        {
            _oDUReturnChallans = new List<DUReturnChallan>();
            try
            {
                string sSQL = MakeSQL(oDUReturnChallan);
                _oDUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUReturnChallans = new List<DUReturnChallan>();
                //_oDUReturnChallan.ErrorMessage = ex.Message;
                //_oDUReturnChallans.Add(_oDUReturnChallan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUReturnChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(DUReturnChallan oDUReturnChallan)
        {
            string sParams = oDUReturnChallan.Note;

          
            int nCboReturnDate = 0;
            DateTime dFromReturnDate = DateTime.Today;
            DateTime dToReturnDate = DateTime.Today;
          
            int nOrderType = 0;
            string sProductIDs = "";
         
            string sPINo = "";
            string sReturnChallanNo = "";
            string sChallanNo = "";
            int nBUID = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                _oDUReturnChallan.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                nCboReturnDate = Convert.ToInt32(sParams.Split('~')[1]);
                sTemp = Convert.ToString(sParams.Split('~')[2]);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromReturnDate = Convert.ToDateTime(sParams.Split('~')[2]);
                    dToReturnDate = Convert.ToDateTime(sParams.Split('~')[3]);
                }

                sReturnChallanNo = Convert.ToString(sParams.Split('~')[4]);

                //nOrderType = Convert.ToInt32(sParams.Split('~')[8]);
                //sPINo = Convert.ToString(sParams.Split('~')[9]);
                //sInvoiceNo = Convert.ToString(sParams.Split('~')[10]);
                //sPONo = Convert.ToString(sParams.Split('~')[11]);
              
                //nBUID = Convert.ToInt32(sParams.Split('~')[13]);
            }


            string sReturn1 = "SELECT * FROM View_DUReturnChallan AS DC ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oDUReturnChallan.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DC.ContractorID in(" + _oDUReturnChallan.ContractorName + ")";
            }
            #endregion

            #region Return Challan  No
            if (!string.IsNullOrEmpty(sReturnChallanNo))
            {
                sReturnChallanNo = sReturnChallanNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUReturnChallanNo Like  '%" + sReturnChallanNo + "%'";
            }
            #endregion
            #region DODate Date
            if (nCboReturnDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboReturnDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboReturnDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboReturnDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboReturnDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboReturnDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboReturnDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion
            
            #region nOrderType
            /// This Searching Critria for Different Window Bulk and Sample
            if (nOrderType > 0)
            {
                if (nOrderType != (int)EnumOrderType.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DC.OrderType="+nOrderType;
                }
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUReturnChallanID in (Select DOD.DUReturnChallanID from DUReturnChallanDetail as DOD where ProductID in (" + sProductIDs + "))";
            }
            #endregion

            #region P/I  No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderType=3 and  DC.DUReturnChallanID in ( Select DCD.DUReturnChallanID from DUReturnChallanDetail as DCD where OrderID in  (Select ExportSC.ExportSCID from ExportSC where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINO Like  '%" + sPINo + "%')))";
            }
            #endregion
         
           
            string sSQL = sReturn1 + " " + sReturn + "";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetbyNo(DUReturnChallan oDUReturnChallan)
        {
            _oDUReturnChallans = new List<DUReturnChallan>();

            try
            {
                string sSQL = "SELECT * FROM View_DUReturnChallan ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDUReturnChallan.DUReturnChallanNo))
                {
                    oDUReturnChallan.DeliveryChallanNo = oDUReturnChallan.DeliveryChallanNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ChallanNo like '%" + oDUReturnChallan.DeliveryChallanNo + "%'";
                }
              
                #region DO No
                //if (!String.IsNullOrEmpty(oDUReturnChallan.DONos))
                //{
                //    oDUReturnChallan.DONos = oDUReturnChallan.DONos.Trim();
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "DUReturnChallanID in ( Select DCD.DUReturnChallanID from view_DUReturnChallanDetail as DCD where DONo Like  '%" + oDUReturnChallan.DONos + "%')";
                //}
                #endregion
                //if (oDUReturnChallan.OrderType > 0)
                //{
                   
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "OrderType=" + oDUReturnChallan.OrderType;
                //}

                sSQL = sSQL + "" + sReturn;
                _oDUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUReturnChallans = new List<DUReturnChallan>();
                //_oDUReturnChallan.ErrorMessage = ex.Message;
                //_oDUReturnChallans.Add(_oDUReturnChallan);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUReturnChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetCompanyLogo
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
        #endregion

        #region Print
        public ActionResult PrintDUReturnChallan(int nId, double nts)
        {
            string sOrderNo = "";
            _oDUReturnChallan = new DUReturnChallan();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
        
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                if (nId > 0)
                {
                    _oDUReturnChallan = _oDUReturnChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUReturnChallan.DUReturnChallanID > 0)
                    {
                        _oDUReturnChallan.DUReturnChallanDetails = DUReturnChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    oDUOrderSetup = oDUOrderSetup.GetByType(_oDUReturnChallan.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.DUReturnChallan + "  AND BUID = " + oBusinessUnit.BusinessUnitID + " Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDUReturnChallan oReport = new rptDUReturnChallan();
            byte[] abytes = oReport.PrepareReport(_oDUReturnChallan, oCompany, oBusinessUnit,1,  oDUOrderSetup,  oApprovalHeads);
            return File(abytes, "application/pdf");

        }

        [HttpPost]
        public ActionResult SetDUReturnChallanData(DUReturnChallan oDUReturnChallan)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUReturnChallan);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDUReturnChallanList()
        {
            _oDUReturnChallan = new DUReturnChallan();
            try
            {
                _oDUReturnChallan = (DUReturnChallan)Session[SessionInfo.ParamObj];
                _oDUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUReturnChallanID IN (" + _oDUReturnChallan.ErrorMessage + ") Order By DUReturnChallanID", (int)Session[SessionInfo.currentUserID]);
                _oDUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + _oDUReturnChallan.ErrorMessage + ") Order By DUReturnChallanID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUReturnChallan = new DUReturnChallan();
                _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oDUReturnChallan.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oDUReturnChallan.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            rptDUReturnChallanDetails oReport = new rptDUReturnChallanDetails();
            byte[] abytes = oReport.PrepareReport(_oDUReturnChallans, _oDUReturnChallanDetails, oCompany, bIsRateView);
            return File(abytes, "application/pdf");
        }

        public void ExcelDUReturnChallanList()
        {
            _oDUReturnChallan = new DUReturnChallan();
            try
            {
                _oDUReturnChallan = (DUReturnChallan)Session[SessionInfo.ParamObj];
                _oDUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUReturnChallanID IN (" + _oDUReturnChallan.ErrorMessage + ") Order By DUReturnChallanID", (int)Session[SessionInfo.currentUserID]);
                _oDUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + _oDUReturnChallan.ErrorMessage + ") Order By DUReturnChallanID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUReturnChallan = new DUReturnChallan();
                _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            }

            if (_oDUReturnChallans.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUReturnChallan.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oDUReturnChallan.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                bool bIsRateView = false;
                List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
                oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

                oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
                if (oAuthorizationRoleMapping.Count > 0)
                {
                    bIsRateView = true;
                }

                int count = 0, nStartCol = 2, nTotalCol = 0;

                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Return Challan Details");
                    sheet.Name = "Return Challan Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 15; //challan no
                    sheet.Column(nStartCol++).Width = 12; //Date
                    sheet.Column(nStartCol++).Width = 25; //Buyer
                    sheet.Column(nStartCol++).Width = 15; //Order No
                    sheet.Column(nStartCol++).Width = 15; //PI No
                    sheet.Column(nStartCol++).Width = 40; //Product
                    sheet.Column(nStartCol++).Width = 18; //Color
                    sheet.Column(nStartCol++).Width = 15; //qty
                    if (bIsRateView)
                    {
                        sheet.Column(nStartCol++).Width = 12; //U. Price
                        sheet.Column(nStartCol++).Width = 20; //Note
                    }
                    

                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Return Challan Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Challan No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "PI No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    if (bIsRateView)
                    {
                        cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    }
                    
                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (DUReturnChallan oItem in _oDUReturnChallans)
                    {
                        List<DUReturnChallanDetail> oTempDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                        oTempDUReturnChallanDetails = _oDUReturnChallanDetails.Where(x => x.DUReturnChallanID == oItem.DUReturnChallanID).ToList();
                        int rowCount = (oTempDUReturnChallanDetails.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.DUReturnChallanNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ReturnDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion

                        #region Detail
                        if (oTempDUReturnChallanDetails.Count > 0)
                        {
                            foreach (DUReturnChallanDetail oItemDetail in oTempDUReturnChallanDetails)
                            {
                                nStartCol = 6;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.OrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.PINo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Qty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                if (bIsRateView)
                                {
                                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.UnitPrice; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = (oItemDetail.Qty * oItemDetail.UnitPrice); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                }
                                
                                rowIndex++;
                            }
                        }
                        else
                        {
                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            if (bIsRateView)
                            {
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            }
                            
                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = _oDUReturnChallanDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bIsRateView)
                    {
                        cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = _oDUReturnChallanDetails.Select(x => (x.Qty * x.UnitPrice)).Sum(); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Return_Challan_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
                #endregion

        }

        #endregion
    }
}