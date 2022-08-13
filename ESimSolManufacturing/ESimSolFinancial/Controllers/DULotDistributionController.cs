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

namespace ESimSolFinancial.Controllers
{
    public class DULotDistributionController : Controller
    {
        #region Declaration
        DULotDistribution _oDULotDistribution = new DULotDistribution();
        List<DULotDistribution> _oDULotDistributions = new List<DULotDistribution>();
        List<DyeingOrderReport> _oDyeingOrderReports = new List<DyeingOrderReport>();
        string _sErrorMessage = "";
        #endregion

        #region DULotDistribution
        public ActionResult ViewDULotDistributions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrderReports = new List<DyeingOrderReport>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.BUID = buid;
            return View(_oDyeingOrderReports);
        }
        public ActionResult ViewDULotDistribution(int nDODID, int buid)
        {
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();

            oDyeingOrderDetail = oDyeingOrderDetail.Get(nDODID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDyeingOrder = DyeingOrder.Get(oDyeingOrderDetail.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);

            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DULotDistribution, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            string sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());
            if (string.IsNullOrEmpty(sWUIDs))
                sWUIDs = "0";
            oDULotDistributions = DULotDistribution.GetsByWU(oDyeingOrderDetail.DyeingOrderDetailID, sWUIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DULotDistributions = oDULotDistributions;
            ViewBag.WorkingUnits = oWorkingUnits; 
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.DyeingOrder = oDyeingOrder;
            ViewBag.BUID = buid;
            return View(oDyeingOrderDetail);
        }
        public ActionResult ViewDUDyeingOrderDetails(int nDOID, int buid)
        {
         
            _oDyeingOrderReports = new List<DyeingOrderReport>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and DyeingOrderID=" + nDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DULotDistribution, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.WorkingUnits = oWorkingUnits; 
            ViewBag.BUID = buid;
            return View(_oDyeingOrderReports);
        }

        [HttpPost]
        public JsonResult Save_Transfer(DULotDistribution oDULotDistribution)
        {
            _oDULotDistribution = new DULotDistribution();
            try
            {
                _oDULotDistribution = oDULotDistribution.Save_Transfer(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oDULotDistribution = new DULotDistribution();
                _oDULotDistribution.ErrorMessage = "Invalid entry Order No";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDULotDistribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Reduce(DULotDistribution oDULotDistribution)
        {
            _oDULotDistribution = new DULotDistribution();
            try
            {
                _oDULotDistribution = oDULotDistribution.Save_Reduce(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oDULotDistribution = new DULotDistribution();
                _oDULotDistribution.ErrorMessage = "Invalid entry Order No";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDULotDistribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         public JsonResult GetsLot(DULotDistribution oDULotDistribution)
         {
             string sSQL = "";
             _oDULotDistributions = new List<DULotDistribution>();
             _oDULotDistribution = new DULotDistribution();
             List<Lot> _oLots = new List<Lot>();
             try
             {
                 if (oDULotDistribution.IsRaw)
                 {
                     sSQL = "Select top(150)* from View_Lot Where balance>0.2 and isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ")";
                      if (!string.IsNullOrEmpty(oDULotDistribution.LotNo))
                          sSQL = sSQL + " And LotNo Like '%" + oDULotDistribution.LotNo + "%'";
                     if (oDULotDistribution.ProductID > 0)
                         sSQL = sSQL + " And ProductID=" + oDULotDistribution.ProductID;
                     if (oDULotDistribution.WorkingUnitID > 0)
                         sSQL = sSQL + " And WorkingUnitID=" + oDULotDistribution.WorkingUnitID;
                     if (oDULotDistribution.LotID > 0)
                         sSQL = sSQL + " And LotID=" + oDULotDistribution.LotID;
                     _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                     foreach(Lot oitem in _oLots)
                     {
                         _oDULotDistribution = new DULotDistribution();
                         _oDULotDistribution.LotID = oitem.LotID;
                         _oDULotDistribution.LotNo = oitem.LotNo;
                         _oDULotDistribution.ProductID = oitem.ProductID;
                         _oDULotDistribution.ProductName = oitem.ProductName;
                         _oDULotDistribution.Qty = oitem.Balance;
                         _oDULotDistribution.WorkingUnitID = oitem.WorkingUnitID;
                         _oDULotDistribution.LocationName = oitem.LocationName;
                         _oDULotDistribution.OperationUnitName = oitem.OperationUnitName;
                         _oDULotDistribution.DODID = oDULotDistribution.DODID;
                         _oDULotDistribution.DODID_Dest = oDULotDistribution.DODID_Dest;
                         _oDULotDistribution.IsRaw = oDULotDistribution.IsRaw;
                         _oDULotDistribution.IsFinish = oDULotDistribution.IsFinish;
                         _oDULotDistribution.MUName = oitem.MUName;
                         _oDULotDistributions.Add(_oDULotDistribution);
                     }

                 }
                 else
                 {
                     sSQL = "SELECT * FROM View_DULotDistribution";
                     string sReturn = "";
                     if (!String.IsNullOrEmpty(oDULotDistribution.LotNo))
                     {
                         oDULotDistribution.LotNo = oDULotDistribution.LotNo.Trim();
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "LotNo like '%" + oDULotDistribution.LotNo + "%'";
                     }
                     if (oDULotDistribution.WorkingUnitID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "WorkingUnitID=" + oDULotDistribution.WorkingUnitID;
                     }
                     if (oDULotDistribution.ProductID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "ProductID=" + oDULotDistribution.ProductID;
                     }
                     if (oDULotDistribution.DODID > 0)
                     {

                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "DODID=" + oDULotDistribution.DODID;
                     }
                     if (oDULotDistribution.LotID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "LotID=" + oDULotDistribution.LotID;
                     }
                     sSQL = sSQL + "" + sReturn;
                     _oDULotDistributions = DULotDistribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 }

             }
             catch (Exception ex)
             {
                 _oDULotDistributions = new List<DULotDistribution>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oDULotDistributions);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        #endregion
         #region Search
         [HttpPost]
         public JsonResult GetbyNo(DyeingOrder oDyeingOrder)
         {
             List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
             string sSQL = "";
         
             sSQL = "SELECT top(120)* FROM View_DyeingOrderReport";
             string sReturn = "";
             if (!String.IsNullOrEmpty(oDyeingOrder.OrderNo))
             {
                 oDyeingOrder.OrderNo = oDyeingOrder.OrderNo.Trim();
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + "OrderNo Like'%" + oDyeingOrder.OrderNo + "%'";
             }
             if (!String.IsNullOrEmpty(oDyeingOrder.ExportPINo))
             {
                 oDyeingOrder.ExportPINo = oDyeingOrder.ExportPINo.Trim();
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + "PINo Like'%" + oDyeingOrder.OrderNo + "%'";
             }
             if (!String.IsNullOrEmpty(oDyeingOrder.SampleInvocieNo))
             {
                 oDyeingOrder.SampleInvocieNo = oDyeingOrder.SampleInvocieNo.Trim();
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + "SampleInvoiceNo Like'%" + oDyeingOrder.SampleInvocieNo + "%'";
             }

             if (oDyeingOrder.ContractorID > 0)
             {
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + " ContractorID  in(" + oDyeingOrder.ContractorID + ")";
             }

             if (oDyeingOrder.DyeingOrderType > 0)
             {
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + " DyeingOrderType  in(" + oDyeingOrder.DyeingOrderType + ")";
             }
             //if (oDyeingOrder.LabDipDetailID > 0)
             //{
             //    Global.TagSQL(ref sReturn);
             //    sReturn = sReturn + "LabDipDetailID=" + oDyeingOrder.LabDipDetailID + "";
             //}
          
             Global.TagSQL(ref sReturn);
             sReturn = sReturn + "Status!="+(int)EnumDyeingOrderState.Cancelled;

             sSQL = sSQL + sReturn + " order by DyeingOrderID, ProductID";

             oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(oDyeingOrderReports);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
         [HttpPost]
         public JsonResult GetsDUDULotDistribution(DULotDistribution oDULotDistribution)
         {
             List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
             List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();

             oWorkingUnits = WorkingUnit.GetsPermittedStore(oDULotDistribution.BUID, EnumModuleName.DULotDistribution, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
             string sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());
             if (string.IsNullOrEmpty(sWUIDs))
                 sWUIDs = "0";
             oDULotDistributions = DULotDistribution.GetsByWU(oDULotDistribution.DODID, sWUIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);

             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(oDULotDistributions);
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
      
    }
}