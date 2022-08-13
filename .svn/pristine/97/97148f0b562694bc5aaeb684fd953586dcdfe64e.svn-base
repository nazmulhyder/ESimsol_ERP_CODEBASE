using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DispoProductionController : Controller
    {
        #region Declaration

        DispoProduction _oDispoProduction = new DispoProduction();
        List<DispoProduction> _oDispoProductions = new List<DispoProduction>();
        #endregion

        #region Actions
        public ActionResult ViewDispoProduction(int buid, int menuid)
        {
            int type = 3; //1 For RnD//
            //int[] OrderTypes = this.GetOrderTypes(type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oDispoProductions = DispoProduction.Gets("Select top(50)* from View_FabricSalesContractReport where  isnull(SCDetailType,0) not in (" + (int)EnumSCDetailType.AddCharge + "," + (int)EnumSCDetailType.DeductCharge + ") and  ordertype in (" + (int)EnumFabricRequestType.Buffer + "," + (int)EnumFabricRequestType.Bulk + "," + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.SampleFOC + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus));

            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Products = oProducts;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = type;
            return View(_oDispoProductions);
        }

        #endregion

        #region Functions
        private int[] GetOrderTypes(int type)
        {
            int[] OrderTypes = new int[] { };
            if (type == 1)// Analysis PO
                OrderTypes = new int[]
                { 
                        (int)EnumFabricRequestType.Analysis, 
                        (int)EnumFabricRequestType.CAD,
                        (int)EnumFabricRequestType.Color,
                        (int)EnumFabricRequestType.Labdip,
                        (int)EnumFabricRequestType.YarnSkein
                };
            else if (type == 2) //HL
                OrderTypes = new int[]
                { 
                        (int)EnumFabricRequestType.HandLoom
                };
            else if (type == 3) //Bulk, Sample
                OrderTypes = new int[]
                { 
                        (int)EnumFabricRequestType.Bulk,
                        (int)EnumFabricRequestType.Sample,
                        (int)EnumFabricRequestType.SampleFOC,
                        (int)EnumFabricRequestType.Local_Bulk,
                        (int)EnumFabricRequestType.Local_Sample,
                        (int)EnumFabricRequestType.Buffer
                };
            else if (type == 4) //Bulk, Sample,HO
                OrderTypes = new int[]
                {        (int)EnumFabricRequestType.HandLoom,
                        (int)EnumFabricRequestType.Bulk,
                        (int)EnumFabricRequestType.Sample,
                        (int)EnumFabricRequestType.SampleFOC,
                        (int)EnumFabricRequestType.Local_Bulk,
                        (int)EnumFabricRequestType.Local_Sample
                };
            else if (type == 5) //Bulk, Sample,HO
                OrderTypes = new int[]
                {        (int)EnumFabricRequestType.Analysis, 
                        (int)EnumFabricRequestType.CAD
                };
            return OrderTypes;
        }
        #endregion

        #region Adv Search
        public JsonResult AdvanchSearch(DispoProduction oDispoProduction)
        {
            _oDispoProductions = new List<DispoProduction>();
            try
            {
                string sSQL = MakeSQL(oDispoProduction);
                _oDispoProductions = DispoProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDispoProductions = new List<DispoProduction>();
                _oDispoProduction.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(_oDispoProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(DispoProduction oDispoProduction)
        {
            int nBUID = 0;
            string sTemp = oDispoProduction.ErrorMessage;
            nBUID = oDispoProduction.BUID;
            string sReturn1 = "";
            string sReturn = " ";
            if (!string.IsNullOrEmpty(sTemp) && sTemp != null)
            {
                sReturn1 = " WHERE ISNULL(FabricSalesContractDetailID,0) <> 0"; //SELECT * FROM FabricSalesContractDetail 

                #region Set Values

                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFabricID = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[3]);
                string sMAccountIDs = Convert.ToString(sTemp.Split('~')[4]);

                int nCboSCDate = Convert.ToInt32(sTemp.Split('~')[5]);
                DateTime dFromSCDate = DateTime.Now;
                DateTime dToSCDate = DateTime.Now;
                if (nCboSCDate > 0)
                {
                    dFromSCDate = Convert.ToDateTime(sTemp.Split('~')[6]);
                    dToSCDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                }

                int ncboApproveDate = Convert.ToInt32(sTemp.Split('~')[8]);
                DateTime dFromApproveDate = DateTime.Now;
                DateTime dToApproveDate = DateTime.Now;
                if (ncboApproveDate > 0)
                {
                    dFromApproveDate = Convert.ToDateTime(sTemp.Split('~')[9]);
                    dToApproveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                }

                int ncboProductionType = 0;
                if (sTemp.Split('~').Length > 11)
                    Int32.TryParse(sTemp.Split('~')[11], out ncboProductionType);

                int ncboPrinted = 0;
                if (sTemp.Split('~').Length > 12)
                    Int32.TryParse(sTemp.Split('~')[12], out ncboPrinted);

                string sOrderTypeIDs = string.Empty;
                if (sTemp.Split('~').Length > 13)
                    sOrderTypeIDs = sTemp.Split('~')[13];

                string sCurrentStatus = string.Empty;
                if (sTemp.Split('~').Length > 14)
                    sCurrentStatus = sTemp.Split('~')[14];

                if (sTemp.Split('~').Length > 15)
                    Int32.TryParse(sTemp.Split('~')[15], out nBUID);

                oDispoProduction.SCNoFull = string.Empty;
                if (sTemp.Split('~').Length > 16)
                    oDispoProduction.SCNoFull = sTemp.Split('~')[16];

                int nType = 0;
                if (sTemp.Split('~').Length > 17)
                    Int32.TryParse(sTemp.Split('~')[17], out nType);

                string sHLNo = string.Empty;
                if (sTemp.Split('~').Length > 18)
                    sHLNo = sTemp.Split('~')[18];

                string sLabStatus = string.Empty;
                if (sTemp.Split('~').Length > 19)
                    sLabStatus = sTemp.Split('~')[19];

                int ncboReceivedDate = 0;
                if (sTemp.Split('~').Length > 20)
                    Int32.TryParse(sTemp.Split('~')[20], out ncboReceivedDate);
                DateTime dFromReceivedDate = DateTime.Now;
                DateTime dToReceivedDate = DateTime.Now;
                if (ncboReceivedDate > 0)
                {
                    dFromReceivedDate = Convert.ToDateTime(sTemp.Split('~')[21]);
                    dToReceivedDate = Convert.ToDateTime(sTemp.Split('~')[22]);
                }

                int ncboExcDate = 0;
                if (sTemp.Split('~').Length > 23)
                    Int32.TryParse(sTemp.Split('~')[23], out ncboExcDate);
                DateTime dFromExcDate = DateTime.Now;
                DateTime dToExcDate = DateTime.Now;
                if (ncboExcDate > 0)
                {
                    dFromExcDate = Convert.ToDateTime(sTemp.Split('~')[24]);
                    dToExcDate = Convert.ToDateTime(sTemp.Split('~')[25]);
                }

                string sConstruction = string.Empty;
                if (sTemp.Split('~').Length > 26)
                    sConstruction = sTemp.Split('~')[26];

                string sComposition = string.Empty;
                if (sTemp.Split('~').Length > 27)
                    sComposition = sTemp.Split('~')[27];

                string sFinishType = string.Empty;
                if (sTemp.Split('~').Length > 28)
                    sFinishType = sTemp.Split('~')[28];

                string sWeave = string.Empty;
                if (sTemp.Split('~').Length > 29)
                    sWeave = sTemp.Split('~')[29];

                //bool IsReceived = false;
                //if (sTemp.Split('~').Length > 19)
                //    bool.TryParse(sTemp.Split('~')[19], out IsReceived);

                //bool IsSubmitHO = false;
                //if (sTemp.Split('~').Length > 20)
                //    bool.TryParse(sTemp.Split('~')[20], out IsSubmitHO);

                //bool IsWatingForLab = false;
                //if (sTemp.Split('~').Length > 21)
                //    bool.TryParse(sTemp.Split('~')[21], out IsWatingForLab);

                //bool IsYetToSubmitHO = false;
                //if (sTemp.Split('~').Length > 22)
                //    bool.TryParse(sTemp.Split('~')[22], out IsYetToSubmitHO);

                //int ncboLabStatus = 0;
                //DateTime dFromLabDate = DateTime.Now;
                //DateTime dToLabDate = DateTime.Now;
                //int ncboLabDate = 0;
                //if (sTemp.Split('~').Length > 30)
                //{
                //    if (!string.IsNullOrEmpty(sTemp.Split('~')[30]))
                //    {
                //        ncboLabStatus = Convert.ToInt32(sTemp.Split('~')[30]);
                //        ncboLabDate = Convert.ToInt32(sTemp.Split('~')[31]);
                //        if (ncboLabDate > 0)
                //        {
                //            dFromLabDate = Convert.ToDateTime(sTemp.Split('~')[32]);
                //            dToLabDate = Convert.ToDateTime(sTemp.Split('~')[33]);
                //        }
                //    }
                //}
                //string sMAGroupIDs = string.Empty;
                //if (sTemp.Split('~').Length > 34)
                //    sMAGroupIDs = sTemp.Split('~')[34];

                #endregion

                #region Make Query

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINO LIKE '%" + sExportPINo + "%')";
                }
                #endregion

                #region sFabricID
                if (!string.IsNullOrEmpty(sFabricID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricID IN (SELECT FabricID FROM Fabric WHERE FabricNo LIKE '%" + sFabricID + "%')";
                }
                #endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE ContractorID IN (" + sContractorIDs + ") )";
                }
                #endregion

                #region BuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE BuyerID IN (" + sBuyerIDs + ") )";
                }
                #endregion

                #region sMAccountIDs
                if (!String.IsNullOrEmpty(sMAccountIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE MktAccountID IN (" + sMAccountIDs + ") )";
                }
                #endregion

                #region FSC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                }
                #endregion

                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                }
                #endregion

                #region ProductionType
                if (ncboProductionType > 0)
                {
                    if (ncboProductionType == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE IsInHouse = 1)";
                    }
                    else if (ncboProductionType == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE IsInHouse = 0)";
                    }
                }
                #endregion

                #region IsPrinting
                if (ncboPrinted > 0)
                {
                    if (ncboPrinted == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE IsPrint = 1)";
                    }
                    else if (ncboPrinted == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE IsPrint = 0)";
                    }
                }
                #endregion

                #region Order Type Wise
                if (!string.IsNullOrEmpty(sOrderTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE OrderType IN (" + sOrderTypeIDs + "))";
                }
                #endregion

                #region Status Wise
                if (!string.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CurrentStatus IN (" + sCurrentStatus + "))";
                }
                #endregion

                #region SC NO
                if (!string.IsNullOrEmpty(oDispoProduction.SCNoFull))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE SCNo LIKE '%" + oDispoProduction.SCNoFull + "%')";
                }
                #endregion

                #region Custom Type R&D Menu Wise
                //nType => 1; // Analysis
                //nType => 2; // H/L PO
                //nType => 3; // Bulk 
                //if (nType > 0 && nType <= 3)
                //{
                //    int[] OrderTypes = this.GetOrderTypes(nType);
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE ISNULL(ApproveBy,0)<>0 AND OrderType IN (" + string.Join(",", OrderTypes) + ")) ";
                //}

                #endregion

                #region HL NO
                if (!string.IsNullOrEmpty(sHLNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExeNo Like '%" + sHLNo + "%'";
                }
                #endregion

                #region sLabStatus
                if (!string.IsNullOrEmpty(sLabStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabStatus in (" + sLabStatus + ")";
                }

                #endregion

                #region ReceivedDate
                if (ncboReceivedDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceivedDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                }
                #endregion

                #region ncboExcDate
                if (ncboExcDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboExcDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                }
                #endregion

                #region Construction
                if (!string.IsNullOrEmpty(sConstruction))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Construction Like '%" + sConstruction + "%'";
                }
                #endregion

                #region Composition
                if (!string.IsNullOrEmpty(sComposition))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductName Like '%" + sComposition + "%'";
                }
                #endregion

                #region Composition
                if (!string.IsNullOrEmpty(sComposition))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductID IN (SELECT ProductID FROM Product WHERE ProductName LIKE '%" + sComposition + "%')";
                }
                #endregion

                #region FinishType
                if (!string.IsNullOrEmpty(sFinishType))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FinishType IN (SELECT FabricProcessID FROM FabricProcess WHERE Name LIKE '%" + sFinishType + "%')";
                }
                #endregion

                #region Fabric Weave
                if (!string.IsNullOrEmpty(sWeave))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricWeave IN (SELECT FabricProcessID FROM FabricProcess WHERE Name LIKE '%" + sWeave + "%')";
                }
                #endregion

                #region Received
                //if (IsReceived)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "ISNULL(FabricReceiveBy,0)!=0";
                //}
                #endregion

                #region Submit To HO
                //if (IsSubmitHO)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " SubmissionDate != '' AND SubmissionDate IS NOT NULL ";
                //}
                #endregion

                #region IsWatingForLab
                //if (IsWatingForLab)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "FabricID not in (Select FabricID from Labdip where isnull(FabricID,0)>0)";
                //}
                #endregion

                #region Yet Submit To HO
                //if (IsYetToSubmitHO)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " SCDetailType=" + (int)EnumSCDetailType.ExtraOrder;
                //}
                #endregion

                #region FSC Lab History
                //--SELECT * FROM FabricSalesContractDetail AS FSCDetail WHERE LabStatus=4 AND FabricSalesContractDetailID IN ((Select Top(1)Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.FSCDetailID = FSCDetail.FabricSalesContractDetailID AND Lab.CurrentStatus=4 ORDER BY DBServerDateTime DESC ))
                //if (ncboLabDate != (int)EnumCompareOperator.None)
                //{
                //    Global.TagSQL(ref sReturn);
                //    if (ncboLabDate == (int)EnumCompareOperator.EqualTo)
                //    {
                //        sReturn = sReturn + " FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                //    }
                //    else if (ncboLabDate == (int)EnumCompareOperator.NotEqualTo)
                //    {
                //        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                //    }
                //    else if (ncboLabDate == (int)EnumCompareOperator.GreaterThan)
                //    {
                //        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                //    }
                //    else if (ncboLabDate == (int)EnumCompareOperator.SmallerThan)
                //    {
                //        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                //    }
                //    else if (ncboLabDate == (int)EnumCompareOperator.Between)
                //    {
                //        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDate.ToString("dd MMM yyyy") + "',106)) )";
                //    }
                //    else if (ncboLabDate == (int)EnumCompareOperator.NotBetween)
                //    {
                //        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDate.ToString("dd MMM yyyy") + "',106)) )";
                //    }
                //}
                #endregion

                #region sMKTGroupIDs
                //if (!String.IsNullOrEmpty(sMAGroupIDs))
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE MktGroupID IN (" + sMAGroupIDs + ") )";
                //}
                #endregion

                //List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                //if (oMarketingAccounts.Count > 0)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                //}
                #endregion

            }
            sReturn1 += sReturn;
            return sReturn1;
        }

        #endregion

        #region Excel
        public void ExportXL(string sTempString, int nBUID)
        {
            _oDispoProductions = new List<DispoProduction>();
            DispoProduction oDispoProduction = new DispoProduction();
            oDispoProduction.ErrorMessage = sTempString;
            oDispoProduction.BUID = nBUID;

            string sSQL = MakeSQL(oDispoProduction);
            _oDispoProductions = DispoProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDispoProductions.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO Issue Date", Width = 14f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buying House", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "MKT Team", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Concern Person", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "MKT Ref", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Receive Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Style No", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Ref", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No/ H/L Ref", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Composition", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric type ", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weave", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Size", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Type", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Width", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Status", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Lab Status", Width = 12f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sizing Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weaving Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Grey Rec.", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Store Rec. Qty", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Return Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Stock in Hand", Width = 12f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DISPO PRODUCTION REPORT");

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = "DISPO PRODUCTION REPORT"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion


                    #region Data
                    int nSL = 0;
                    foreach (DispoProduction oItem in _oDispoProductions)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nSL).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.SCDateSt, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.MKTGroup, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.MKTPName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FabricNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FabricReceiveDateStr, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.StyleNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BuyerReference, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.HLReference, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ColorInfo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProcessTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FabricWeaveName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Size, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FinishTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FabricWidth, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.CurrentStatusSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LabStatusST, false, ExcelHorizontalAlignment.Left, false, false);

                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyDispo.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyWarp.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtySizing.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyWeaving.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.GreyRecd.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.StoreRcvQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DCQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RCQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.StockInHand.ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        nRowIndex++;

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total  ", nRowIndex, nRowIndex, nStartCol, nStartCol += 21, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.Qty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.QtyDispo).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.QtyWarp).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.QtySizing).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.QtyWeaving).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.GreyRecd).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.StoreRcvQty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.DCQty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.RCQty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oDispoProductions.Sum(c => c.StockInHand).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DISPO_PRODUCTION_REPORT.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Comment
        public ActionResult ViewDispoProductionComments(int nFabricSalesContractDetailID, int nBUID)
        {
            List<DispoProductionComment> _oDispoProductionComments = new List<DispoProductionComment>();
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            //_oDispoProductionComments = DispoProductionComment.Gets("SELECT * FROM [View_DispoProductionComment] WHERE FSCDID = " + nFabricSalesContractDetailID + " AND ISNULL(IsOwn,0)=0 ORDER BY CommentDate DESC", (int)Session[SessionInfo.currentUserID]);
            _oDispoProductionComments = DispoProductionComment.GetsBySP(nFabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
            oFSCD = oFSCD.Get(nFabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FabricSalesContractDetail = oFSCD;
            return View(_oDispoProductionComments);
        }

        [HttpPost]
        public JsonResult SaveComment(DispoProductionComment oDispoProductionComment)
        {
            DispoProductionComment _oDispoProductionComment = new DispoProductionComment();
            List<DispoProductionComment> _oDispoProductionComments = new List<DispoProductionComment>();
            try
            {
                _oDispoProductionComment = oDispoProductionComment.Save((int)Session[SessionInfo.currentUserID]);
                if (_oDispoProductionComment.DispoProductionCommentID > 0 && _oDispoProductionComment.FSCDID > 0)
                {
                    //_oDispoProductionComments = DispoProductionComment.Gets("SELECT * FROM [View_DispoProductionComment] WHERE FSCDID = " + _oDispoProductionComment.FSCDID + " AND ISNULL(IsOwn,0)=0 ORDER BY CommentDate DESC", (int)Session[SessionInfo.currentUserID]);
                    _oDispoProductionComments = DispoProductionComment.Gets("SELECT * FROM (SELECT *  FROM View_DispoProductionComment WHERE FSCDID = " + _oDispoProductionComment.FSCDID + " AND ISNULL(IsOwn,0) = 0 UNION SELECT *  FROM View_DispoProductionComment WHERE FSCDID = " + _oDispoProductionComment.FSCDID + " AND ISNULL(IsOwn,0) = 1 AND UserID = " + (int)Session[SessionInfo.currentUserID] + " ) AS EE ORDER BY ISNULL(EE.IsOwn,0) DESC, CommentDate DESC", (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oDispoProductionComment = new DispoProductionComment();
                _oDispoProductionComment.ErrorMessage = ex.Message;
                _oDispoProductionComments = new List<DispoProductionComment>();
                _oDispoProductionComments.Add(_oDispoProductionComment);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDispoProductionComments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteComment(DispoProductionComment oDispoProductionComment)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDispoProductionComment.Delete(oDispoProductionComment.DispoProductionCommentID, (int)Session[SessionInfo.currentUserID]);
                if (sFeedBackMessage == "Deleted")
                {
                    List<DispoProductionAttachment> fileNames = DispoProductionAttachment.Gets("SELECT * FROM DispoProductionAttachment WHERE DispoProductionCommentID = " + oDispoProductionComment.DispoProductionCommentID, (int)Session[SessionInfo.currentUserID]);
                    foreach (DispoProductionAttachment oObj in fileNames)
                    {
                        string sYear = oObj.FileName.Trim().Substring(4, 4);
                        string sMon = oObj.FileName.Trim().Substring(2, 2);
                        string filePath = Path.Combine(Server.MapPath("~/ProjectAttachment/" + sYear + "/" + sMon), oObj.FileName.Trim());
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();
                        sFeedBackMessage = oDispoProductionAttachment.Delete(oObj.DispoProductionAttachmentID, (int)Session[SessionInfo.currentUserID]);
                    }
                }
                
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MakeOwnOrAll(DispoProductionComment oDispoProductionComment)
        {
            DispoProductionComment _oDispoProductionComment = new DispoProductionComment();
            try
            {
                _oDispoProductionComment = oDispoProductionComment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDispoProductionComment = new DispoProductionComment();
                _oDispoProductionComment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDispoProductionComment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewDispoProductionSeenUnseenComments(int nBUID)
        {
            List<DispoProductionComment> _oDispoProductionComments = new List<DispoProductionComment>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oDispoProductionComments);
        }

        [HttpPost]
        public JsonResult CommentAdvSearch(DispoProductionComment oDispoProductionComment)
        {
            List<DispoProductionComment> oDispoProductionComments = new List<DispoProductionComment>();
            try
            {
                if (!string.IsNullOrEmpty(oDispoProductionComment.ErrorMessage))
                {
                    string sCommentAdv =Convert.ToString(oDispoProductionComment.ErrorMessage.Split('~')[0]);
                    int nCommentDateAdv =Convert.ToInt32(oDispoProductionComment.ErrorMessage.Split('~')[1]);
                    DateTime dFromCommentDateAdv =Convert.ToDateTime(oDispoProductionComment.ErrorMessage.Split('~')[2]);
                    DateTime dToCommentDateAdv =Convert.ToDateTime(oDispoProductionComment.ErrorMessage.Split('~')[3]);
                    int nSeenUnseenAdv =Convert.ToInt32(oDispoProductionComment.ErrorMessage.Split('~')[4]);
                    string sUserIDs = Convert.ToString(oDispoProductionComment.ErrorMessage.Split('~')[5]);

                    string sSQL = "SELECT * FROM View_DispoProductionComment WHERE ISNULL(IsOwn,0) != 1 ", sReturn = " ";
                    #region Comment Search
                    if (!string.IsNullOrEmpty(sCommentAdv))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " Comment LIKE '%" + sCommentAdv + "%' ";
                    }
                    #endregion
                    #region Comment Date
                    if (nCommentDateAdv != (int)EnumCompareOperator.None)
                    {
                        DateObject.CompareDateQuery(ref sReturn, "CommentDate", nCommentDateAdv, dFromCommentDateAdv, dToCommentDateAdv);
                    }
                    #endregion
                    #region User
                    if (!string.IsNullOrEmpty(sUserIDs))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " UserID IN (" + sUserIDs + ") ";
                    }
                    #endregion

                    #region Seen/Unseen
                    if (nSeenUnseenAdv == 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " DispoProductionCommentID IN (SELECT DispoProductionCommentID FROM DispoProductionCommentViewer WHERE DBUserID=" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                    }
                    else
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " DispoProductionCommentID NOT IN (SELECT DispoProductionCommentID FROM DispoProductionCommentViewer WHERE DBUserID=" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                    }
                    #endregion

                    sSQL += sReturn + " ORDER BY ExeNo, CommentDate"; //+ " UNION SELECT * FROM View_DispoProductionComment WHERE UserID=" + ((User)Session[SessionInfo.CurrentUser]).UserID + " AND ISNULL(IsOwn,0) = 1";
                    oDispoProductionComments = DispoProductionComment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (nSeenUnseenAdv != 0 && oDispoProductionComments.Count() > 0)
                    {
                        List<DispoProductionCommentViewer> oDispoProductionCommentViewers = new List<DispoProductionCommentViewer>();
                        foreach (DispoProductionComment oItem in oDispoProductionComments)
                        {
                            DispoProductionCommentViewer oObj = new DispoProductionCommentViewer();
                            oObj.DispoProductionCommentID = oItem.DispoProductionCommentID;
                            oDispoProductionCommentViewers.Add(oObj);
                        }
                        DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
                        oDispoProductionCommentViewers = oDispoProductionCommentViewer.Save(oDispoProductionCommentViewers, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                oDispoProductionComments = new List<DispoProductionComment>();
                oDispoProductionComment = new DispoProductionComment();
                oDispoProductionComment.ErrorMessage = ex.Message;
                oDispoProductionComments.Add(oDispoProductionComment);
            }
            var jsonResult = Json(oDispoProductionComments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Pdf
        public ActionResult PrintCommentViewr(int nFSCDID, string nts)
        {
            List<DispoProductionComment> _oDispoProductionComments = new List<DispoProductionComment>();
            List<DispoProductionCommentViewer> oDispoProductionCommentViewers = new List<DispoProductionCommentViewer>();
            Company oCompany = new Company();
            if (nFSCDID > 0)
            {
                _oDispoProductionComments = DispoProductionComment.Gets("SELECT * FROM (SELECT *  FROM View_DispoProductionComment WHERE FSCDID = " + nFSCDID + " AND ISNULL(IsOwn,0) = 0 UNION SELECT *  FROM View_DispoProductionComment WHERE FSCDID = " + nFSCDID + " AND ISNULL(IsOwn,0) = 1 AND UserID = " + (int)Session[SessionInfo.currentUserID] + " ) AS EE ORDER BY ISNULL(EE.IsOwn,0) DESC, CommentDate DESC", (int)Session[SessionInfo.currentUserID]);
                foreach (DispoProductionComment oItem in _oDispoProductionComments)
                {
                    oItem.DispoProductionCommentViewers = DispoProductionCommentViewer.Gets("SELECT * FROM View_DispoProductionCommentViewer WHERE DispoProductionCommentID = " + oItem.DispoProductionCommentID + " ORDER BY DBServerDateTime ASC", (int)Session[SessionInfo.currentUserID]);
                }
            }

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oDispoProductionComments.Count > 0)
            {
                byte[] abytes;
                rptPrintCommentViewr oReport = new rptPrintCommentViewr();
                abytes = oReport.PrepareReport(_oDispoProductionComments, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

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

        public System.Drawing.Image GetImage(DispoProductionAttachment oDPA)
        {
            if (oDPA.FileName != null)
            {
                string sYear = oDPA.FileName.Trim().Substring(4, 4);
                string sMon = oDPA.FileName.Trim().Substring(2, 2);
                string fileDirectory = Path.Combine(Server.MapPath("~/ProjectAttachment/" + sYear + "/" + sMon), oDPA.FileName.Trim());
                //string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                //if (System.IO.File.Exists(fileDirectory))
                //{
                //    System.IO.File.Delete(fileDirectory);
                //}
                oDPA.File = GetFile(fileDirectory);
                MemoryStream m = new MemoryStream(oDPA.File);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public ActionResult PrintImageView(int nFSCDID, string nts)
        {
            List<DispoProductionComment> _oDispoProductionComments = new List<DispoProductionComment>();
            List<DispoProductionCommentViewer> oDispoProductionCommentViewers = new List<DispoProductionCommentViewer>();
            Company oCompany = new Company();
            if (nFSCDID > 0)
            {
                _oDispoProductionComments = DispoProductionComment.Gets("SELECT * FROM (SELECT *  FROM View_DispoProductionComment WHERE FSCDID = " + nFSCDID + " AND ISNULL(IsOwn,0) = 0 UNION SELECT *  FROM View_DispoProductionComment WHERE FSCDID = " + nFSCDID + " AND ISNULL(IsOwn,0) = 1 AND UserID = " + (int)Session[SessionInfo.currentUserID] + " ) AS EE ORDER BY ISNULL(EE.IsOwn,0) DESC, CommentDate DESC", (int)Session[SessionInfo.currentUserID]);
                foreach (DispoProductionComment oItem in _oDispoProductionComments)
                {
                    oItem.DispoProductionAttachments = DispoProductionAttachment.Gets("SELECT * FROM DispoProductionAttachment WHERE DispoProductionCommentID = " + oItem.DispoProductionCommentID + " ORDER BY DBServerDateTime ASC", (int)Session[SessionInfo.currentUserID]);
                    foreach (DispoProductionAttachment oDPA in oItem.DispoProductionAttachments)
                    {
                        oDPA.Image = GetImage(oDPA);
                    }
                }
            }

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oDispoProductionComments.Count > 0)
            {
                byte[] abytes;
                rptPrintImageView oReport = new rptPrintImageView();
                abytes = oReport.PrepareReport(_oDispoProductionComments, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        #region Attachment
        public ActionResult ViewDispoProductionAttachments(int nDispoProductionCommentID)
        {
            List<DispoProductionAttachment>  oDispoProductionAttachments = new List<DispoProductionAttachment>();
            if (nDispoProductionCommentID > 0)
            {
                string sSQL = "SELECT * FROM DispoProductionAttachment WHERE DispoProductionCommentID = '" + nDispoProductionCommentID + "' ";
                oDispoProductionAttachments = DispoProductionAttachment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (DispoProductionAttachment oAttach in oDispoProductionAttachments)
                {
                    oAttach.SubFileName = oAttach.FileName.Split('`')[0];
                }
            }
            DispoProductionComment oDispoProductionComment = new DispoProductionComment();
            ViewBag.DispoProductionComment = oDispoProductionComment.Get(nDispoProductionCommentID, (int)Session[SessionInfo.currentUserID]);
            return View(oDispoProductionAttachments);
        }

        public JsonResult SaveAttachment(double nts)
        {
            string sMessage = "";
            DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();

            try
            {
                string today = "", ext = "";
                oDispoProductionAttachment.DispoProductionAttachmentID = Convert.ToInt32(Request.Headers["DispoProductionAttachmentID"]);
                oDispoProductionAttachment.DispoProductionCommentID = Convert.ToInt32(Request.Headers["DispoProductionCommentID"]);
                string fileName = Convert.ToString(Request.Headers["FileName"]);
                fileName = fileName.Split('\\')[2] + "`";

                byte[] data;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = null;
                    if (Request.Files.Count == 1)
                    {
                        file = Request.Files[0];
                    }
                    else
                    {
                        if (Convert.ToBoolean(Request.Headers["IsFile"]) == true)
                        {
                            file = Request.Files[0];
                        }
                    }
                    if (file != null)
                    {
                        using (Stream inputStream = file.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                            double nMaxLength = 1024 * 1024;
                            if (data.Length > nMaxLength)
                            {
                                throw new Exception("Your File " + ((double)data.Length / nMaxLength).ToString("#,###.##") + "MB! You can selecte maximum 1MB Attachment");
                            }
                            ////File Upload
                            DateTime time = DateTime.Now;
                            //DateTime time = DateTime.Parse("02 Feb 2021 10:31:55");
                            today = time.ToString("dd") + time.ToString("MM") + time.ToString("yyyy") + time.ToString("hh") + time.ToString("mm") + time.ToString("ffff");
                            ext = Path.GetExtension(file.FileName);

                            string subPath = "~/ProjectAttachment";
                            bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                            subPath = "~/ProjectAttachment/" + time.ToString("yyyy");
                            exists = System.IO.Directory.Exists(Server.MapPath(subPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                            subPath = "~/ProjectAttachment/" + time.ToString("yyyy") + "/" + time.ToString("MM");
                            exists = System.IO.Directory.Exists(Server.MapPath(subPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                            string filePath = Path.Combine(Server.MapPath(subPath), today.Trim() + ext);
                            file.SaveAs(filePath);
                        }
                        oDispoProductionAttachment.File = data;
                        oDispoProductionAttachment.FileName = today.Trim() + ext;
                    }
                }
                oDispoProductionAttachment = oDispoProductionAttachment.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDispoProductionAttachment.DispoProductionAttachmentID > 0 && (oDispoProductionAttachment.ErrorMessage == null || oDispoProductionAttachment.ErrorMessage == ""))
                {
                    sMessage = "Save Successfully" + "~" + oDispoProductionAttachment.DispoProductionAttachmentID + "~" + oDispoProductionAttachment.DispoProductionCommentID + "~" + oDispoProductionAttachment.FileName;
                }
                else
                {
                    if ((oDispoProductionAttachment.ErrorMessage == null || oDispoProductionAttachment.ErrorMessage == "")) { throw new Exception("Unable to Upload."); }
                    else { throw new Exception(oDispoProductionAttachment.ErrorMessage); }

                }
            }
            catch (Exception ex)
            {
                oDispoProductionAttachment = new DispoProductionAttachment();
                oDispoProductionAttachment.ErrorMessage = ex.Message;
                sMessage = ex.Message + "~" + 0 + "~" + 0 + "~" + "";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult DeleteAttachment(int nDispoProductionAttachmentID, string fileName)
        {
            string sFeedBackMessage = "";
            try
            {
                DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();
                sFeedBackMessage = oDispoProductionAttachment.Delete(nDispoProductionAttachmentID, (int)Session[SessionInfo.currentUserID]);
                if (sFeedBackMessage == "Data delete successfully")
                {
                    List<DispoProductionAttachment> fileNames = DispoProductionAttachment.Gets("SELECT FileName FROM DispoProductionAttachment WHERE DispoProductionAttachmentID = " + nDispoProductionAttachmentID, (int)Session[SessionInfo.currentUserID]);
                    foreach (DispoProductionAttachment name in fileNames)
                    {
                        string sYear = name.FileName.Trim().Substring(4, 4);
                        string sMon = name.FileName.Trim().Substring(2, 2);
                        string filePath = Path.Combine(Server.MapPath("~/ProjectAttachment/" + sYear + "/" + sMon), name.FileName.Trim());
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                    }
                }
                

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadAttachment(int nDispoProductionAttachmentID, double ts)
        {
            DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();
            try
            {
                oDispoProductionAttachment = oDispoProductionAttachment.Get(nDispoProductionAttachmentID, (int)Session[SessionInfo.currentUserID]);
                if (oDispoProductionAttachment.FileName != null)
                {
                    string sYear = oDispoProductionAttachment.FileName.Substring(4, 4);
                    string sMon = oDispoProductionAttachment.FileName.Substring(2, 2);
                    string filePath = Path.Combine(Server.MapPath("~/ProjectAttachment/" + sYear + "/" + sMon), oDispoProductionAttachment.FileName.Trim());
                    //string ext = Path.GetExtension(oDispoProductionAttachment.FileName);
                    //var file = File(Server.MapPath("~/ProjectAttachment"), oDispoProductionAttachment.FileName.Trim());
                    //file.FileDownloadName = oDispoProductionAttachment.FileName;

                    byte[] fileBytes = GetFile(filePath);                    
                    TempData["message"] = "";
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, oDispoProductionAttachment.FileName.Trim());
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oDispoProductionAttachment.FileName);
            }
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            fs.Close();
            return data;
        }

        #endregion



    }

}
