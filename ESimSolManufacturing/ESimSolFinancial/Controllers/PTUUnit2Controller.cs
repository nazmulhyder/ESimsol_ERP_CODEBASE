 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class PTUUnit2Controller: Controller
    {
        #region Declaration
        ProductionOrder _oProductionOrder = new ProductionOrder();
        List<ProductionOrder> _oProductionOrders = new List<ProductionOrder>();
        ProductionOrderDetail _oProductionOrderDetail = new ProductionOrderDetail();
        List<ProductionOrderDetail> _oProductionOrderDetails = new List<ProductionOrderDetail>();
        List<PTUUnit2> _oPTUUnit2s = new List<PTUUnit2>();
        List<PTUUnit2Log> _oPTUUnit2Logs = new List<PTUUnit2Log>();
        PTUUnit2 _oPTUUnit2 = new PTUUnit2();
        List<PTUUnit2Distribution> _oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
        PTUUnit2Distribution _oPTUUnit2Distribution = new PTUUnit2Distribution();
        ExportPI _oExportPI = new ExportPI();
        ExportSC _oExportSC = new ExportSC();
        #endregion

        #region PTU Control
        public ActionResult ViewPTUControl(int nExportSCID, int ProductNature, int buid, int menuid)//Production Tracking Unit Control
        {
            _oPTUUnit2 = new PTUUnit2();
            _oExportSC = new ExportSC();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            if(nExportSCID>0)
            {
                _oExportSC = _oExportSC.Get(nExportSCID, (int)Session[SessionInfo.currentUserID]);
                _oExportSC.PTUUnit2s = PTUUnit2.Gets(nExportSCID, buid, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            return View(_oExportSC);
        }

        public ActionResult ViewPTUWaitForSubContract(int ProductNature, int buid, int menuid)//Production Tracking Unit Control
        {
            _oPTUUnit2s = new List<PTUUnit2>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPTUUnit2s = PTUUnit2.WaitFoSubcontractReceivePTU(buid,ProductNature, (int)Session[SessionInfo.currentUserID]);
            return View(_oPTUUnit2s);
        }
        public ActionResult ViewPTUReceive(int nPTUUnit2ID)
        {
            _oPTUUnit2 = new PTUUnit2();
            _oPTUUnit2 = _oPTUUnit2.Get(nPTUUnit2ID,(int)Session[SessionInfo.currentUserID]);
            return View(_oPTUUnit2);
        }
        public ActionResult ViewPTUReceiveInReadyStock(int nPTUUnit2ID)
        {
            _oPTUUnit2 = new PTUUnit2();
            _oPTUUnit2 = _oPTUUnit2.Get(nPTUUnit2ID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(_oPTUUnit2.BUID, EnumModuleName.QC, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oPTUUnit2);
        }
        public ActionResult ViewPTUReceiveInAvilableStock(int nPTUUnit2ID)
        {
            _oPTUUnit2 = new PTUUnit2();
            _oPTUUnit2 = _oPTUUnit2.Get(nPTUUnit2ID, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_WorkingUnit WHERE BUID = "+_oPTUUnit2.BUID+" AND ISNULL(UnitType,0) = "+(int)EnumWoringUnitType.Available;
            ViewBag.Stores = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oPTUUnit2);
        }

        public ActionResult ViewSubContractReceive(int nPTUUnit2ID, int nExportSCDetailID)
        {
            _oPTUUnit2 = new PTUUnit2();
            _oPTUUnit2 = _oPTUUnit2.Get(nPTUUnit2ID, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_PTUUnit2Distribution WHERE PTUUnit2ID IN (SELECT PTUUnit2ID FROM View_PTUUnit2 WHERE PTUType = " + (int)EnumPTUType.Subcontract + " AND ExportSCDetailID=" + nExportSCDetailID + " AND ISNULL(ReadyStockQty,0)>0)";
            _oPTUUnit2.PTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(_oPTUUnit2.BUID, EnumModuleName.QC, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oPTUUnit2);
        }
        #endregion

        #region Post functions       
        [HttpPost]
        public JsonResult GetsPTUUnit(PTUUnit2 oPTUUnit2)//use in Production Sheet
        {
            _oPTUUnit2s = new List<PTUUnit2>();
            try
            {
                string sSQL = "SELECT * FROM View_PTUUnit2 WHERE ISNULL(YetToProductionSheeteQty,0)>0 AND  BUID = " + oPTUUnit2.BUID + " AND ProductNature = " + oPTUUnit2.ProductNatureInInt;
                if (oPTUUnit2.ContractorID > 0)
                {
                    sSQL += " AND ContractorID = " + oPTUUnit2.ContractorID;
                }
                if (oPTUUnit2.ExportPINo != null && oPTUUnit2.ExportPINo != "")
                {
                    sSQL += " AND ExportPINo LIKE '%" + oPTUUnit2.ExportPINo + "%'";
                }
                _oPTUUnit2s = PTUUnit2.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2 = new PTUUnit2();
                _oPTUUnit2.ErrorMessage = ex.Message;
                _oPTUUnit2s.Add(_oPTUUnit2);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2s);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPTUForStockQty(PTUUnit2 oPTUUnit2)
        {
            _oPTUUnit2s = new List<PTUUnit2>();
            try
            {
                //string sSQL = "SELECT * FROM View_PTUUnit2 WHERE ISNULL(ReadyStockQty,0)>0 AND  BUID = " + oPTUUnit2.BUID + " AND ProductNature = " + oPTUUnit2.ProductNatureInInt + " AND ProductID = " + oPTUUnit2.ProductID + " AND ColorID = " + oPTUUnit2.ColorID + " AND PTUUnit2ID!=" + oPTUUnit2.PTUUnit2ID + " AND ExportSCID IN (SELECT MM.ExportSCID FROM ExportSC AS MM WHERE MM.ExportPIID IN (SELECT BB.ExportPIID FROM ExportPI AS BB WHERE BB.BUID="+oPTUUnit2.BUID+"))";
                string sSQL = "SELECT * FROM View_PTUUnit2 WHERE ISNULL(ReadyStockQty,0)>0 AND  BUID = " + oPTUUnit2.BUID + " AND ProductNature = " + oPTUUnit2.ProductNatureInInt + " AND ProductID = " + oPTUUnit2.ProductID + " AND ColorID = " + oPTUUnit2.ColorID + " AND PTUUnit2ID!=" + oPTUUnit2.PTUUnit2ID.ToString();
                if (oPTUUnit2.ExportPINo != null && oPTUUnit2.ExportPINo != "")
                {
                    sSQL += " AND ExportPINo LIKE '%" + oPTUUnit2.ExportPINo + "%'";
                }
                _oPTUUnit2s = PTUUnit2.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2 = new PTUUnit2();
                _oPTUUnit2.ErrorMessage = ex.Message;
                _oPTUUnit2s.Add(_oPTUUnit2);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2s);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        

        [HttpPost]
        public JsonResult GetPTUDistributions(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                string sSQL = "SELECT * FROM View_PTUUnit2Distribution WHERE  PTUUnit2ID = " + oPTUUnit2Distribution.PTUUnit2ID+" AND Isnull(Qty,0)>0 AND BUID = "+oPTUUnit2Distribution.BUID;
                if (oPTUUnit2Distribution.LotNo != "" && oPTUUnit2Distribution.LotNo != null)
                {
                    sSQL += " AND LotNo Like '%" + oPTUUnit2Distribution.LotNo + "%'";
                }
                _oPTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
                _oPTUUnit2Distributions.Add(_oPTUUnit2Distribution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateGraceQty(PTUUnit2 oPTUUnit2)
        {
            _oPTUUnit2 = new PTUUnit2();
            try
            {
                _oPTUUnit2 = oPTUUnit2.UpdateGrace((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2 = new PTUUnit2();
                _oPTUUnit2.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByPTUwithType(PTUUnit2Log oPTUUnit2Log)
        {
            _oPTUUnit2Logs = new List<PTUUnit2Log>();
            try
            {       
                    _oPTUUnit2Logs = PTUUnit2Log.Gets(oPTUUnit2Log.PTUUnit2ID,oPTUUnit2Log.PTUUnit2RefTypeInInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPTUUnit2Log = new PTUUnit2Log();
                oPTUUnit2Log.ErrorMessage = ex.Message;
                _oPTUUnit2Logs.Add(oPTUUnit2Log);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Logs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsReadyStockHistory(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                string sSQL = "SELECT * FROM View_PTUUnit2Distribution WHERE  PTUUnit2ID = " + oPTUUnit2Distribution.PTUUnit2ID;
                _oPTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
                _oPTUUnit2Distributions.Add(_oPTUUnit2Distribution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsSubContrctReceiveHistory(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                string sSQL = "SELECT * FROM View_PTUUnit2Distribution WHERE PTUUnit2DistributionID IN ( SELECT PTUUnit2DistributionID FROM PTUUnit2DistributionLog WHERE PTUUnit2DistributionRefType = " + (int)EnumPTUUnit2Ref.Subcontract_Receive + " AND  PTUUnit2DistributionRefID IN (SELECT PTUUnit2DistributionID FROM PTUUnit2Distribution WHERE PTUUnit2ID = " + oPTUUnit2Distribution.PTUUnit2ID + "))";
                _oPTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
                _oPTUUnit2Distributions.Add(_oPTUUnit2Distribution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        
        [HttpPost]
        public JsonResult CommitPTUUnit2Distribution(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distribution = new PTUUnit2Distribution();
            try
            {
                _oPTUUnit2Distribution = oPTUUnit2Distribution.PTUTransfer((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceiveInReadyeStock(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distribution = new PTUUnit2Distribution();
            try
            {
                _oPTUUnit2Distribution = oPTUUnit2Distribution.ReceiveInReadyeStock((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ReceiveInAvilableStock(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distribution = new PTUUnit2Distribution();
            try
            {
                _oPTUUnit2Distribution = oPTUUnit2Distribution.ReceiveInAvilableStock((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceiveInReadyStockFromSubContract(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            _oPTUUnit2Distribution = new PTUUnit2Distribution();
            try
            {
                _oPTUUnit2Distribution = oPTUUnit2Distribution.PTUTransferSubContract((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPTUUnit2Distribution = new PTUUnit2Distribution();
                _oPTUUnit2Distribution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUUnit2Distribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
   

        #region Utility Functions
        public Image GetCompanyLogo(Company oCompany)
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
        public Image GetSignature(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oAttachDocument.AttachFile);
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

        #region PI Search
        [HttpPost]
        public JsonResult GetsExportSCForPTU(ExportSC oExportSC)
        {
            List<ExportSC> oExportSCs = new List<ExportSC>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportSC WHERE ExportSCID IN (SELECT HH.ExportSCID FROM PTUUnit2 AS HH WHERE HH.BUID=" + oExportSC.BUID + ") AND ProductNature = " + oExportSC.ProductNatureInInt;
                if (oExportSC.PINo != null && oExportSC.PINo != "")
                {
                    sSQL += " And PINo Like '%" + oExportSC.PINo.Trim() + "%'";
                }

                if (oExportSC.ContractorID > 0)
                {
                    sSQL += " And ContractorID = " + oExportSC.ContractorID + "";
                }
                oExportSCs = ExportSC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportSCs = new List<ExportSC>();
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
                oExportSCs.Add(_oExportSC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Confirm PTU Distribution
        public ActionResult ViewConfirmPTUDistribution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            Lot oLot = new Lot();
            List<PTUUnit2Distribution> oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            ViewBag.PTUUnit2Distributions = oPTUUnit2Distributions;
            ViewBag.BUID = buid;
            return View(oLot);
        }
        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot >oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT * FROM View_Lot WHERE LotNo Like '%" + oLot.LotNo + "%' AND WorkingUnitID IN (145,146) AND BUID = " + oLot.BUID;
                //string sSQL = "SELECT * FROM View_Lot WHERE LotNo Like  '%" + oLot.LotNo + "%' AND BUID = " + oLot.BUID;
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsPTUUnit2DistributionByLotID(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            List<PTUUnit2Distribution> oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                string sSQL = "SELECT * from View_PTUUnit2Distribution WHERE LotID = " + oPTUUnit2Distribution.LotID;
                oPTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPTUUnit2Distribution = new PTUUnit2Distribution();
                oPTUUnit2Distribution.ErrorMessage = ex.Message;
                oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePTUUnit2List(List<PTUUnit2Distribution> oPTUUnit2Distributions)
        {
            PTUUnit2Distribution objPTUUnit2Distribution = new PTUUnit2Distribution();
            List<PTUUnit2Distribution> objPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            int nLotID = 0;
            try
            {
                if(oPTUUnit2Distributions.Count>0)
                {
                    nLotID = oPTUUnit2Distributions[0].LotID;
                    objPTUUnit2Distributions = PTUUnit2Distribution.ConfirmPTUUnit2Distribution(oPTUUnit2Distributions, nLotID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                objPTUUnit2Distribution = new PTUUnit2Distribution();
                objPTUUnit2Distribution.ErrorMessage = ex.Message;
                oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}