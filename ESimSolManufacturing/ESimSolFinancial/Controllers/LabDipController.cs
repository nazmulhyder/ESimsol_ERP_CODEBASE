using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSol.Controllers
{
    public class LabDipController : Controller
    {
        #region Declaration
        LabDip _oLabDip = new LabDip();
        LabDipDetail _oLabDipDetail = new LabDipDetail();
        List<LabDip> _oLabDips = new List<LabDip>();
        string _sDateRange = "";
        #endregion

        #region Labdip
        public ActionResult ViewLabDips(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oLabDips = new List<LabDip>();
            _oLabDips = LabDip.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.EnumLabdipOrderStatuss = Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            ViewBag.LabDipSetup = oLabDipSetup;
            return View(_oLabDips);
        }
        public ActionResult ViewLabDip_Challan(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oLabDips = new List<LabDip>();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.EnumLabdipOrderStatuss = Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            return View(_oLabDips);
        }
        public ActionResult ViewLabDip(int nId, double ts)
        {
            _oLabDip = new LabDip();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            if (nId > 0)
            {
                _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oLabDip.LabDipID > 0)
                {
                    string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                    _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipDetails.Count > 0)
                    {
                        if (_oLabDip.LabDipDetails.FirstOrDefault() != null && _oLabDip.LabDipDetails.FirstOrDefault().LabDipDetailID > 0 && _oLabDip.LabDipDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
                            _oLabDip.LabDipDetails.ForEach((item) => { oLabDipDetails.Add(item); });
                            _oLabDip.LabDipDetails = this.TwistedLabdipDetails(_oLabDip.LabDipDetails);
                            _oLabDip.LabDipDetails[0].CellRowSpans = this.RowMerge(oLabDipDetails);
                        }
                    }
                    oCPIssueTos=ContactPersonnel.Gets(_oLabDip.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oLabDip.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            else
            {
                _oLabDip.PriorityLevel = EnumPriorityLevel.High;
            }
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat));   
            ViewBag.EnumPriorityLevels = EnumObject.jGets(typeof(EnumPriorityLevel));
            ViewBag.EnumKnitPlyYarns = EnumObject.jGets(typeof(EnumKnitPlyYarn));
            ViewBag.LabDipSetup = oLabDipSetup;
            return View(_oLabDip);
        }
        #endregion

        #region Re Lab (FSC)
        public ActionResult ViewLabDips_FSC(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            int type = 4; //1 For HandLoom//
            int[] OrderTypes = this.GetOrderTypes(type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oLabDips = new List<LabDip>();

            //SELECT TOP 200 * FROM View_Labdip_FSCD WHERE ISNULL(FSCDetailID,0)<>0
            _oLabDips = LabDip.Gets("SELECT TOP 100 * FROM View_Labdip_FSCD WHERE  OrderStatus = 1 ORDER BY DBServerDateTime", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.EnumLabdipOrderStatuss = Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            ViewBag.LabDipSetup = oLabDipSetup;


            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.cboEnumWarpWeft = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
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

            return View(_oLabDips);
        }
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
                        (int)EnumFabricRequestType.Local_Sample
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
            return OrderTypes;
        }

        [HttpPost]
        public JsonResult AdvanchSearchTwo(FabricSCReport oFabricSCReport)
        {
            _oLabDips = new List<LabDip>();
            try
            {
                string sSQL = MakeSQL(oFabricSCReport);
                _oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLabDips = new List<LabDip>();
                _oLabDip = new LabDip();
                _oLabDip.ErrorMessage = ex.Message;
                _oLabDips.Add(_oLabDip);
            }
            var jsonResult = Json(_oLabDips, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricSCReport oFabricSCReport)
        {
            string sTemp = oFabricSCReport.ErrorMessage;
            string sReturn1 = "";
            string sReturn = "";
            if (!string.IsNullOrEmpty(sTemp) && sTemp != null)
            {
                sReturn1 = "SELECT * FROM View_Labdip_FSCD";

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
                int nBUID = 0;
                if (sTemp.Split('~').Length > 15)
                    Int32.TryParse(sTemp.Split('~')[15], out nBUID);
                oFabricSCReport.SCNoFull = string.Empty;
                if (sTemp.Split('~').Length > 16)
                    oFabricSCReport.SCNoFull = sTemp.Split('~')[16];
                //int ncboProductionType = Convert.ToInt32(sTemp.Split('~')[11]);
                //int ncboPrinted = Convert.ToInt32(sTemp.Split('~')[12]);
                //string sOrderTypeIDs = Convert.ToString(sTemp.Split('~')[13]);
                //string sCurrentStatus = Convert.ToString(sTemp.Split('~')[14]);
                //int nBUID = Convert.ToInt32(sTemp.Split('~')[15]);
                //oFabricSCReport.SCNoFull = sTemp.Split('~')[16];

                int nType = 0;
                if (sTemp.Split('~').Length > 17)
                    Int32.TryParse(sTemp.Split('~')[17], out nType);

                string sHLNo = string.Empty;
                if (sTemp.Split('~').Length > 18)
                    sHLNo = sTemp.Split('~')[18];

                bool IsReceived = false;
                if (sTemp.Split('~').Length > 19)
                    bool.TryParse(sTemp.Split('~')[19], out IsReceived);

                bool IsSubmitHO = false;
                if (sTemp.Split('~').Length > 20)
                    bool.TryParse(sTemp.Split('~')[20], out IsSubmitHO);

                bool IsWatingForLab = false;
                if (sTemp.Split('~').Length > 21)
                    bool.TryParse(sTemp.Split('~')[21], out IsWatingForLab);

                bool IsYetToSubmitHO = false;
                if (sTemp.Split('~').Length > 22)
                    bool.TryParse(sTemp.Split('~')[22], out IsYetToSubmitHO);

                string sLabStatus = string.Empty;
                if (sTemp.Split('~').Length > 23)
                    sLabStatus = sTemp.Split('~')[23];

                int ncboReceivedDate = 0;
                if (sTemp.Split('~').Length > 24)
                    Int32.TryParse(sTemp.Split('~')[24], out ncboReceivedDate);

                DateTime dFromReceivedDate = DateTime.Now;
                DateTime dToReceivedDate = DateTime.Now;
                if (ncboReceivedDate > 0)
                {
                    dFromReceivedDate = Convert.ToDateTime(sTemp.Split('~')[25]);
                    dToReceivedDate = Convert.ToDateTime(sTemp.Split('~')[26]);
                }
                int ncboExcDate = 0;
                if (sTemp.Split('~').Length > 27)
                    Int32.TryParse(sTemp.Split('~')[27], out ncboExcDate);

                DateTime dFromExcDate = DateTime.Now;
                DateTime dToExcDate = DateTime.Now;
                if (ncboExcDate > 0)
                {
                    dFromExcDate = Convert.ToDateTime(sTemp.Split('~')[28]);
                    dToExcDate = Convert.ToDateTime(sTemp.Split('~')[29]);
                }

                int ncboLabStatus = 0;
                DateTime dFromLabDate = DateTime.Now;
                DateTime dToLabDate = DateTime.Now;
                int ncboLabDate = 0;
                if (sTemp.Split('~').Length > 30)
                {
                    if (!string.IsNullOrEmpty(sTemp.Split('~')[30]))
                    {
                        ncboLabStatus = Convert.ToInt32(sTemp.Split('~')[30]);
                        ncboLabDate = Convert.ToInt32(sTemp.Split('~')[31]);
                        if (ncboLabDate > 0)
                        {
                            dFromLabDate = Convert.ToDateTime(sTemp.Split('~')[32]);
                            dToLabDate = Convert.ToDateTime(sTemp.Split('~')[33]);
                        }
                    }
                }



                #endregion

                #region Make Query

                #region F SC NO
                if (!string.IsNullOrEmpty(oFabricSCReport.SCNoFull))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SCNo LIKE '%" + oFabricSCReport.SCNoFull + "%' ";
                }
                #endregion

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "PINo like '%" + sExportPINo + "%'";
                }
                #endregion

                #region sFabricID
                if (!string.IsNullOrEmpty(sFabricID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricNo like '%" + sFabricID + "%'";
                }
                #endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion

                #region BBuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion

                #region sMAccountIDs
                if (!String.IsNullOrEmpty(sMAccountIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(" + sMAccountIDs + ") ";
                }
                #endregion

                #region F SC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region ReceivedDate
                if (ncboReceivedDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceivedDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region ncboExcDate
                if (ncboExcDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboExcDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "FSCDetailID in (Select FSCDID from FabricExecutionOrderSpecification where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "FSCDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "FSCDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "FSCDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "FSCDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "FSCDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                }
                #endregion

                #region ncboProductionType
                if (ncboProductionType > 0)
                {
                    if (ncboProductionType == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =1";
                    }
                    else if (ncboProductionType == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =0";
                    }

                }
                #endregion

                #region nIsPrinting
                if (ncboPrinted > 0)
                {
                    if (ncboPrinted == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsPrint =1";
                    }
                    else if (ncboPrinted == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "IsPrint =0";
                    }
                }
                #endregion

                #region Order Type Wise
                if (!string.IsNullOrEmpty(sOrderTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType IN (" + sOrderTypeIDs + ") ";
                }
                #endregion

                #region Custom Type R&D Menu Wise
                //nType => 1; // Analysis
                //nType => 2; // H/L PO
                //nType => 3; // Bulk 

                if (nType > 0 && nType <= 3)
                {
                    int[] OrderTypes = this.GetOrderTypes(nType);

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(ApproveBy,0)<>0 AND  OrderType In (" + string.Join(",", OrderTypes) + ")";
                }

                #endregion

                #region Status Wise
                if (!string.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Currentstatus IN (" + sCurrentStatus + ") ";
                }
                #endregion

                #region HL NO
                if (!string.IsNullOrEmpty(sHLNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExeNo Like '%" + sHLNo + "%'";
                }
                #endregion
                #region Received
                if (IsReceived)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(FabricReceiveBy,0)!=0";
                }
                #endregion

                #region Submit To HO
                if (IsSubmitHO)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SubmissionDate != '' AND SubmissionDate IS NOT NULL ";
                }
                #endregion

                #region IsWatingForLab
                if (IsWatingForLab)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricID not in (Select FabricID from Labdip where isnull(FabricID,0)>0)";
                }
                #endregion

                #region Yet Submit To HO
                if (IsYetToSubmitHO)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "( SubmissionDate = '' OR SubmissionDate IS NULL )";
                }
                #endregion

                #region sLabStatus
                if (!string.IsNullOrEmpty(sLabStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabStatus in (" + sLabStatus + ")";
                }

                #endregion

                #region FSC Lab History
                //--SELECT * FROM FabricSalesContractDetail AS FSCDetail WHERE LabStatus=4 AND FabricSalesContractDetailID IN ((Select Top(1)Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.FSCDetailID = FSCDetail.FabricSalesContractDetailID AND Lab.CurrentStatus=4 ORDER BY DBServerDateTime DESC ))
                if (ncboLabDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboLabDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " FSCDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "  FSCDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "  FSCDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "  FSCDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "  FSCDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "  FSCDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                }
                #endregion

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }
                #endregion
            }
            else
            {
                sReturn1 = "SELECT top(150)* FROM View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ";
                //_oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            sReturn = sReturn1 + sReturn + " order by SCDate DESC"; //, Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC
            return sReturn;
        }

        #endregion

        #region Labdip From WU
        public ActionResult ViewWULabDips(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oLabDips = new List<LabDip>();
            _oLabDips = LabDip.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.EnumLabdipOrderStatuss = Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            ViewBag.LabDipSetup = oLabDipSetup;
            return View(_oLabDips);
        }
        public ActionResult ViewWULabDipFSC(int nId, double ts)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            _oLabDip = new LabDip();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            if (nId > 0)
            {
                _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReport.Get(_oLabDip.FSCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                if (_oLabDip.LabDipID > 0)
                {
                    string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID + " order by WarpWeftType";
                    _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oLabDip.LabDipDetailFabrics = LabDipDetailFabric.Gets("Select * from View_LabDipDetailFabric Where LabDipID=" + _oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipDetails.Count > 0)
                    {
                        if (_oLabDip.LabDipDetails.FirstOrDefault() != null && _oLabDip.LabDipDetails.FirstOrDefault().LabDipDetailID > 0 && _oLabDip.LabDipDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
                            _oLabDip.LabDipDetails.ForEach((item) => { oLabDipDetails.Add(item); });
                            _oLabDip.LabDipDetails = this.TwistedLabdipDetails(_oLabDip.LabDipDetails);
                            _oLabDip.LabDipDetails[0].CellRowSpans = this.RowMerge(oLabDipDetails);
                        }
                    }
                    oCPIssueTos = ContactPersonnel.Gets(_oLabDip.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oLabDip.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                   // oFabric = oFabric.Get(_oLabDip.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oLabDip.PriorityLevel = EnumPriorityLevel.High;
                    _oLabDip.ContractorID = oFabricSCReport.ContractorID;
                    _oLabDip.DeliveryToID = oFabricSCReport.BuyerID;
                    _oLabDip.ContractorName = oFabricSCReport.ContractorName;
                    _oLabDip.DeliveryToName = oFabricSCReport.BuyerName;
                    _oLabDip.MktPersonID = oFabricSCReport.MktAccountID;
                    _oLabDip.MktPerson = oFabricSCReport.MKTPName;
                    _oLabDip.LightSourceID = oFabricSCReport.LightSourceID;
                    _oLabDip.LightSourceName = oFabricSCReport.LightSourceName;
                    _oLabDip.ISTwisted = false;
                    _oLabDip.LabDipFormat = EnumLabdipFormat.YarnForm;
                    _oLabDip.LDTwistType = EnumLDTwistType.Generale;
                    _oLabDip.BuyerRefNo = oFabricSCReport.BuyerReference;
                    _oLabDip.SCNo = oFabricSCReport.SCNoFull;
                    _oLabDip.FabricID = oFabricSCReport.FabricID;
                    _oLabDip.FSCDetailID = oFabricSCReport.FabricSalesContractDetailID;
                    _oLabDip.FSCID = oFabricSCReport.FabricSalesContractID;
                  
                }
                _oLabDip.FabricNo = oFabricSCReport.FabricNo;
                _oLabDip.OrderNo = oFabricSCReport.ExeNo;
            }
            else
            {
                _oLabDip.PriorityLevel = EnumPriorityLevel.High;
            }
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.LDTwistTypes = EnumObject.jGets(typeof(EnumLDTwistType));
            ViewBag.FabricRequestType = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat));
            ViewBag.EnumPriorityLevels = EnumObject.jGets(typeof(EnumPriorityLevel));
            ViewBag.EnumKnitPlyYarns = EnumObject.jGets(typeof(EnumKnitPlyYarn));
            ViewBag.LabDipSetup = oLabDipSetup;
            ViewBag.FabricSCReport = oFabricSCReport;
            return View(_oLabDip);
        }
        public ActionResult ViewWULabDip(int nId, double ts)
        {
            Fabric oFabric = new Fabric();
            _oLabDip = new LabDip();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            if (nId > 0)
            {
                _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oLabDip.LabDipID > 0)
                {
                    string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID + " order by WarpWeftType";
                    _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oLabDip.LabDipDetailFabrics = LabDipDetailFabric.Gets("Select * from View_LabDipDetailFabric Where LabDipID=" + _oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipDetails.Count > 0)
                    {
                        if (_oLabDip.LabDipDetails.FirstOrDefault() != null && _oLabDip.LabDipDetails.FirstOrDefault().LabDipDetailID > 0 && _oLabDip.LabDipDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
                            _oLabDip.LabDipDetails.ForEach((item) => { oLabDipDetails.Add(item); });
                            _oLabDip.LabDipDetails = this.TwistedLabdipDetails(_oLabDip.LabDipDetails);
                            _oLabDip.LabDipDetails[0].CellRowSpans = this.RowMerge(oLabDipDetails);
                        }
                    }
                    oCPIssueTos = ContactPersonnel.Gets(_oLabDip.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oLabDip.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFabric = oFabric.Get(_oLabDip.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            else
            {
                _oLabDip.PriorityLevel = EnumPriorityLevel.High;
            }
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.FabricRequestType = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat));
            ViewBag.EnumPriorityLevels = EnumObject.jGets(typeof(EnumPriorityLevel));
            ViewBag.EnumKnitPlyYarns = EnumObject.jGets(typeof(EnumKnitPlyYarn));
            ViewBag.LabDipSetup = oLabDipSetup;
            ViewBag.Fabric = oFabric;
            return View(_oLabDip);
        }
        public ActionResult ViewLabdipFromFabric(int nId, int buid) // Here, Id comes from fabric;
        {
            LabDip oLabDip = new LabDip();
            List<ContactPersonnel> oCPs = new List<ContactPersonnel>();
            LabDipSetup oLabDipSetup = new LabDipSetup();

            Fabric oFabric = new Fabric();
            List<FabricPatternDetail> oFPDetails = new List<FabricPatternDetail>();
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();

            if (nId > 0)
            {
                oLabDip = LabDip.GetByFSD(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFabric = oFabric.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                string sSQL = "Select * from View_FabricPatternDetail Where FPID In (Select FPID from FabricPattern Where FabricID=" + oFabric.FabricID + " And IsActive=1)";
                oFPDetails = FabricPatternDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oLabDip.LabDipID <= 0)
                {
                    oLabDip = new LabDip
                    {
                        LabDipID = 0,
                        LabdipNo = string.Empty,
                        ContractorID = oFabric.BuyerID,
                        MktPerson = oFabric.MKTPersonName,
                        ContractorName = oFabric.BuyerName,
                        DeliveryToID = oFabric.BuyerID,
                        DeliveryToName = oFabric.BuyerName,
                        BuyerRefNo = oFabric.BuyerReference,
                        Note = string.Empty,
                        OrderStatus = EnumLabdipOrderStatus.Initialized,
                        OrderReferenceType = EnumOrderType.LabDipOrder,
                        LabDipFormat = EnumLabdipFormat.YarnForm,
                        
                        OrderReferenceID = 0,
                        SeekingDate = DateTime.Today,
                        OrderDate = DateTime.Today,
                        MktPersonID = oFabric.MKTPersonID,
                        LightSourceID = oFabric.PrimaryLightSourceID,
                        ISTwisted = false,
                        FabricID = oFabric.FabricID,
                        PriorityLevel = EnumPriorityLevel.High //)oFabric.PriorityLevel
                    };


                    oLabDipDetails.AddRange(
                        oFPDetails.GroupBy(x => new { x.ProductID, x.ColorName }, (key, grp) => new LabDipDetail
                        {
                            LabDipDetailID = 0,
                            LabDipID = oLabDip.LabDipID,
                            LabdipColorID = 0,
                            ProductID = key.ProductID,
                            ColorName = key.ColorName,
                            ProductCode = grp.First().ProductCode,
                            ProductName = grp.First().ProductName,
                            ColorSet = 3,
                            ShadeCount = 3,
                            Gauge = 1,
                            Combo = 1
                        })
                    );

                    oLabDip.LabDipDetails = oLabDipDetails;
                }
                else
                {
                    oLabDip.LabDipDetails = LabDipDetail.Gets("Select * from View_LabdipDetail Where LabDipID=" + oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                //oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if(oMarketingAccounts.Any() && oMarketingAccounts.First().MarketingAccountID>0)
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            if (_oLabDip.ContractorID > 0)
            {
                oCPIssueTos = ContactPersonnel.Gets(_oLabDip.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oLabDip.DeliveryToID > 0)
            {
                oCPDeliveryTos = ContactPersonnel.Gets(_oLabDip.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID_Dyeing = oBusinessUnit.BusinessUnitID;
            ViewBag.BUID = buid;
            ViewBag.MarketingAccounts = oMarketingAccounts;
           // ViewBag.LabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat));
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.PriorityLevels = EnumObject.jGets(typeof(EnumPriorityLevel));
            ViewBag.EnumPriorityLevels = EnumObject.jGets(typeof(EnumPriorityLevel));
           // ViewBag.KnitPlyYarns = EnumObject.jGets(typeof(EnumKnitPlyYarn));
            ViewBag.EnumKnitPlyYarns = EnumObject.jGets(typeof(EnumKnitPlyYarn));
            ViewBag.LabDipSetup = oLabDipSetup;
            ViewBag.Fabric = oFabric;
            return View(oLabDip);
        }
        [HttpPost]
        public JsonResult SaveByFabric(FabricSalesContractDetail oFabricSalesContractDetail)
        {
           
            LabDip oLabDip = new LabDip();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            try
            {
                //if (oFabricSalesContractDetail.FabricID > 0)
                //{
                //    oLabDip = LabDip.GetByFSD(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 
                //    oFabricSalesContractDetail=oFabricSalesContractDetail.Get(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    oFabricSalesContract=oFabricSalesContract.Get(oFabricSalesContractDetail.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    string sSQL = "Select * from View_FabricPatternDetail Where FPID In (Select FPID from FabricPattern Where FabricID=" + oFabric.FabricID + " And IsActive=1)";
                //    oFPDetails = FabricPatternDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //    if (oLabDip.LabDipID <= 0)
                //    {
                //        oLabDip = new LabDip
                //        {
                //            LabDipID = 0,
                //            LabdipNo = string.Empty,
                //            ContractorID = oFabricSalesContract.ContractorID,
                //            MktPersonID = oFabricSalesContract.MktAccountID,
                //            ContractorName = oFabricSalesContract.ContractorName,
                //            DeliveryToID = oFabricSalesContract.BuyerID,
                //            DeliveryToName = oFabric.BuyerName,
                //            BuyerRefNo = oFabric.BuyerReference,
                //            Note = string.Empty,
                //            OrderStatus = EnumLabdipOrderStatus.Initialized,
                //            OrderReferenceType = EnumOrderType.LabDipOrder,
                //            LabDipFormat = EnumLabdipFormat.YarnForm,
                //            OrderReferenceID = 0,
                //            SeekingDate = DateTime.Today,
                //            OrderDate = DateTime.Today,
                //            LightSourceID = (oFabricSalesContract.LightSourceID == 0 ? oFabric.PrimaryLightSourceID : oFabricSalesContract.LightSourceID),
                //            ISTwisted = false,
                //            FabricID = oFabricSalesContractDetail.FabricID,
                //            FSCDetailID = oFabricSalesContractDetail.FabricSalesContractDetailID,
                //            PriorityLevel = EnumPriorityLevel.High //)oFabric.PriorityLevel
                //        };
                //        oLabDipDetails.AddRange(
                //            oFPDetails.GroupBy(x => new { x.ProductID, x.ColorName }, (key, grp) => new LabDipDetail
                //            {
                //                LabDipDetailID = 0,
                //                LabDipID = oLabDip.LabDipID,
                //                LabdipColorID = 0,
                //                ProductID = key.ProductID,
                //                ColorName = key.ColorName,
                //                ProductCode = grp.First().ProductCode,
                //                ProductName = grp.First().ProductName,
                //                ColorSet = 3,
                //                ShadeCount = 3,
                //                Gauge = 1,
                //                Combo = 1
                //            })
                //        );

                //        oLabDip.LabDipDetails = oLabDipDetails;
                //    }
                //    else
                //    {
                //        oLabDip.LabDipDetails = LabDipDetail.Gets("Select * from View_LabdipDetail Where LabDipID=" + oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    }
                   
                //    if (oLabDip.LabDipID <= 0)
                //    {
                //        oLabDip = oLabDip.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    }
                //    else
                //    {
                //        oLabDip = oLabDip.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    }
                //    if (oLabDip.LabDipID > 0)
                //    {
                //        oLabDip.LabdipHistorys = LabdipHistory.Gets(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    }
                //}
                //if (oLabDip.LabDipID <= 0)
                //{
                //    oLabDip = oLabDip.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                //else
                //{
                oLabDip.FabricID = oFabricSalesContractDetail.FabricID;
                oLabDip.FSCDetailID = oFabricSalesContractDetail.FabricSalesContractDetailID;
                oLabDip = oLabDip.IUD_LD_Fabric(((User)Session[SessionInfo.CurrentUser]).UserID);
                
                if (oLabDip.LabDipID > 0)
                {
                    oLabDip.LabdipHistorys = LabdipHistory.Gets(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
              
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLDByFabric(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            LabDip oLabDip = new LabDip();
            try
            {
                if (oFabricSalesContractDetail.FabricID > 0)
                {
                    oLabDip = LabDip.GetByFSD(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oLabDip = LabDip.GetByFSD(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDip.LabDipID > 0)
                {
                    oLabDip.LabdipHistorys = LabdipHistory.Gets(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLDDetailFabric(LabDipDetailFabric oLabDipDetailFabric)
        {
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            try
            {
                if (!string.IsNullOrEmpty(oLabDipDetailFabric.Params))
                {
                    oLabDipDetailFabrics = LabDipDetailFabric.Gets("SELECT * FROM View_LabDipDetailFabric Where LabDipDetailID IN (" + oLabDipDetailFabric.Params+")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDipDetailFabric = new LabDipDetailFabric() { ErrorMessage=ex.Message };
                oLabDipDetailFabrics = new List<LabDipDetailFabric>();
                oLabDipDetailFabrics.Add(oLabDipDetailFabric);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetailFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLDDetailByFSC(LabDipDetailFabric oLabDipDetailFabric)
        {
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            try
            {
                if (!string.IsNullOrEmpty(oLabDipDetailFabric.Params))
                {
                    oLabDipDetailFabrics = LabDipDetailFabric.Gets("SELECT * FROM View_LabDipDetailFabric Where FSCDetailID IN (" + oLabDipDetailFabric.Params + ") AND ISNULL(ReviseNo,0) =" + oLabDipDetailFabric.ReviseNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oLabDipDetailFabrics.Count > 0)
                    {
                        if (oLabDipDetailFabrics.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LDFID > 0 && oLabDipDetailFabrics.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<LabDipDetailFabric> oLabDipDetailFabrics_Temp = new List<LabDipDetailFabric>();
                            oLabDipDetailFabrics.ForEach((item) => { oLabDipDetailFabrics_Temp.Add(item); });
                            oLabDipDetailFabrics = this.TwistedLabdipDetailFabrics(oLabDipDetailFabrics);
                            oLabDipDetailFabrics[0].CellRowSpans = this.RowMerge(oLabDipDetailFabrics_Temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oLabDipDetailFabric = new LabDipDetailFabric() { ErrorMessage = ex.Message };
                oLabDipDetailFabrics = new List<LabDipDetailFabric>();
                oLabDipDetailFabrics.Add(oLabDipDetailFabric);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetailFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetLDDetailByFSCnRevise(LabDipDetailFabric oLabDipDetailFabric)
        //{
        //    List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(oLabDipDetailFabric.Params))
        //        {
        //            oLabDipDetailFabrics = LabDipDetailFabric.Gets("SELECT * FROM View_LabDipDetailFabric Where FSCDetailID IN (" + oLabDipDetailFabric.Params + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (oLabDipDetailFabrics.Count > 0)
        //            {
        //                if (oLabDipDetailFabrics.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LDFID > 0 && oLabDipDetailFabrics.Where(x => x.TwistedGroup > 0).Count() > 0)
        //                {
        //                    List<LabDipDetailFabric> oLabDipDetailFabrics_Temp = new List<LabDipDetailFabric>();
        //                    oLabDipDetailFabrics.ForEach((item) => { oLabDipDetailFabrics_Temp.Add(item); });
        //                    oLabDipDetailFabrics = this.TwistedLabdipDetailFabrics(oLabDipDetailFabrics);
        //                    oLabDipDetailFabrics[0].CellRowSpans = this.RowMerge(oLabDipDetailFabrics_Temp);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oLabDipDetailFabric = new LabDipDetailFabric() { ErrorMessage = ex.Message };
        //        oLabDipDetailFabrics = new List<LabDipDetailFabric>();
        //        oLabDipDetailFabrics.Add(oLabDipDetailFabric);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oLabDipDetailFabrics);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ChangeStatusFromFabric(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0) throw new Exception("Please select a valid labdip.");
                EnumLabdipOrderStatus OrderStatus;
                EnumLabdipOrderStatus.TryParse(oLabDip.Params, out OrderStatus);

                oLabDip = oLabDip.ChangeOrderStatus((short)OrderStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDip.LabDipID > 0)
                {
                    oLabDip.LabdipHistorys = LabdipHistory.Gets(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DirectApproval(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0) throw new Exception("Please select a valid labdip.");
                EnumLabdipOrderStatus OrderStatus;
                EnumLabdipOrderStatus.TryParse(oLabDip.Params, out OrderStatus);

                oLabDip = oLabDip.DirectApproval((short)OrderStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDip.LabDipID > 0)
                {
                    oLabDip.LabdipHistorys = LabdipHistory.Gets(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsDetailForPI(FabricSCReport oFabricSCReport)
        {
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<FabricSCReport> _oFabricSCReport = new List<FabricSCReport>();

            string sReturn1 = "SELECT TOP 150 * FROM View_FabricSalesContractReport ";
            string sReturn = "";

            #region OrderType
            if (oFabricSCReport.OrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType =" + oFabricSCReport.OrderType + "";
            }
            #endregion
            #region ID
            if (oFabricSCReport.FabricSalesContractID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractID=" + oFabricSCReport.FabricSalesContractID + "";
            }
            #endregion

            #region FabricNo
            if (!String.IsNullOrEmpty(oFabricSCReport.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricNo like '%" + oFabricSCReport.FabricNo + "%'";
            }
            #endregion

            #region ExeNo
            if (!String.IsNullOrEmpty(oFabricSCReport.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo like '%" + oFabricSCReport.ExeNo + "%'";
            }
            #endregion

            string sSQL = sReturn1 + sReturn + " ORDER BY IssueDate DESC ";
            _oFabricSCReport = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ErrorMessage))
            //{
            //    oExportPIDetails = ExportPIDetail.Gets("Select * from view_ExportPIDetail as EPID where  isnull(OrderSheetDetailID,0)>0 and OrderSheetDetailID in (" + oFabricSalesContractDetail.ErrorMessage + ") and ExportPIID=" + oFabricSalesContractDetail.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    if (oExportPIDetails.Any() && oExportPIDetails.FirstOrDefault().FabricID > 0)
            //    {
            //        // _oFabricSCReport.ForEach(x => { x.Qty_PI = 0+ _oFSCDetails_PI.Where(p => p.FabricID == x.FabricID).Sum(o => o.Qty_PI); });
            //        _oFabricSCReport.ForEach(x => { x.Qty_PI = x.Qty_PI - oExportPIDetails.Where(p => p.OrderSheetDetailID == x.FabricSalesContractDetailID).Sum(o => o.Qty); });
            //    }
            //}
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region JSON FUNCTION
        [HttpPost]
        public JsonResult Save(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0)
                {
                    oLabDip = oLabDip.IUD((int)EnumDBOperation.Insert,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLabDip = oLabDip.IUD((int)EnumDBOperation.Update,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Challan(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0)
                {
                    oLabDip = oLabDip.Save_Challan( ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLabDip = oLabDip.Save_Challan(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_FabricDetail(LabDipDetailFabric oLabDipDetailFabric)
        {
            try
            {
                if (oLabDipDetailFabric.LDFID <= 0) 
                {
                    oLabDipDetailFabric = oLabDipDetailFabric.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                    oLabDipDetailFabric = oLabDipDetailFabric.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetailFabric = new LabDipDetailFabric();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetailFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_FabricDetail(LabDipDetailFabric oLabDipDetailFabric)
        {
            try
            {
                if (oLabDipDetailFabric.LDFID > 0)
                {
                    oLabDipDetailFabric = oLabDipDetailFabric.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDipDetailFabric = new LabDipDetailFabric();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetailFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0) { throw new Exception("Please select an valid item."); }
                oLabDip = oLabDip.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDetail(LabDipDetail oLabDipDetail)
        {
            try
            {
                if (oLabDipDetail.LabDipDetailID <= 0)
                {
                    oLabDipDetail = oLabDipDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLabDipDetail = oLabDipDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReviseDetail(LabDipDetail oLabDipDetail)
        {
            _oLabDipDetail = new LabDipDetail();
            try
            {
                if (oLabDipDetail.LabDipDetailID > 0)
                {
                    _oLabDipDetail = oLabDipDetail.Revise((int)EnumDBOperation.Revise, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else throw new Exception("Invalid Details!");
            }
            catch (Exception ex)
            {
                _oLabDipDetail = new LabDipDetail();
                _oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateLot(LabDipDetail oLabDipDetail)
        {
            try
            {
               if (oLabDipDetail.LabDipDetailID <= 0) { throw new Exception("Please select an valid item."); }
                else
                {
                    oLabDipDetail = oLabDipDetail.UpdateLot(((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDetail(LabDipDetail oLabDipDetail)
        {
            try
            {
                if (oLabDipDetail.LabDipDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oLabDipDetail = oLabDipDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetail=new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public string MakeAdvSQL(LabDip oLabDip) //throws
        {
            string sLabdipNo = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[0])) ? "" : oLabDip.Params.Split('~')[0].Trim();
            string sContractorIDs = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[1])) ? "" : oLabDip.Params.Split('~')[1].Trim();
            string sDeliveryToIDs = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[2])) ? "" : oLabDip.Params.Split('~')[2].Trim();
            string sMktPersonIDs = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[3])) ? "" : oLabDip.Params.Split('~')[3].Trim();
            int nLabDipFormat = Convert.ToInt16(oLabDip.Params.Split('~')[4]);
            int nLightSourceID = Convert.ToInt32(oLabDip.Params.Split('~')[5]);
            DateTime dtOrderFrom = Convert.ToDateTime(oLabDip.Params.Split('~')[6]);
            DateTime dtOrderTo = Convert.ToDateTime(oLabDip.Params.Split('~')[7]);
            bool IsdtOrderSearch = Convert.ToBoolean(oLabDip.Params.Split('~')[8]);
            DateTime dtSeekingFrom = Convert.ToDateTime(oLabDip.Params.Split('~')[9]);
            DateTime dtSeekingTo = Convert.ToDateTime(oLabDip.Params.Split('~')[10]);
            bool IsdtSeekingSearch = Convert.ToBoolean(oLabDip.Params.Split('~')[11]);
            DateTime dtHistoryFrom = Convert.ToDateTime(oLabDip.Params.Split('~')[12]);
            DateTime dtHistoryTo = Convert.ToDateTime(oLabDip.Params.Split('~')[13]);
            bool IsdtHistorySearch = Convert.ToBoolean(oLabDip.Params.Split('~')[14]);
            string sOrderStatus = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[15])) ? "" : oLabDip.Params.Split('~')[15].Trim();
            string sColorNo = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[16])) ? "" : oLabDip.Params.Split('~')[16].Trim();
            string sOrderIDs = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[17])) ? "" : oLabDip.Params.Split('~')[17].Trim();
            //string sFabricNo = (string.IsNullOrEmpty(oLabDip.Params.Split('~')[18])) ? "" : oLabDip.Params.Split('~')[18].Trim();
            string sFabricNo = string.Empty;
            if (oLabDip.Params.Split('~').Length > 18)
                sFabricNo = oLabDip.Params.Split('~')[18];

            string sSQL = "SELECT * FROM View_LabDip Where LabDipID<>0 ";
            if (sLabdipNo != "") sSQL = sSQL + " And LabdipNo Like '%" + sLabdipNo + "%'";
            if (sContractorIDs != "") sSQL = sSQL + " And ContractorID In (" + sContractorIDs + ")";
            if (sDeliveryToIDs != "") sSQL = sSQL + " And DeliveryToID In (" + sDeliveryToIDs + ")";
            if (sMktPersonIDs != "") sSQL = sSQL + " And MktPersonID In (" + sMktPersonIDs + ")";
            if (nLabDipFormat > 0) sSQL = sSQL + " And LabDipFormat = " + nLabDipFormat + "";
            if (nLightSourceID > 0) sSQL = sSQL + " And LightSourceID = " + nLightSourceID + " ";
            if (IsdtOrderSearch) sSQL = sSQL + " And Convert(Date,(OrderDate))  Between '" + dtOrderFrom.ToString("dd MMM yyyy") + "' And '" + dtOrderTo.ToString("dd MMM yyyy") + "'";
            if (IsdtSeekingSearch) sSQL = sSQL + " And Convert(Date,(SeekingDate))  Between '" + dtSeekingFrom.ToString("dd MMM yyyy") + "' And '" + dtSeekingTo.ToString("dd MMM yyyy") + "'";
            if (IsdtHistorySearch) sSQL = sSQL + " And LabDipID In (Select LabdipID from LabdipHistory Where LabdipHistoryID<>0  " + ((sOrderStatus != "") ? " And CurrentStatus In (" + sOrderStatus + ")" : "") + "  And CONVERT(date,DBServerDateTime) Between '" + dtHistoryFrom.ToString("dd MMM yyyy") + "' And '" + dtHistoryTo.ToString("dd MMM yyyy") + "')";
            if (sOrderStatus != "" && !IsdtHistorySearch) sSQL = sSQL + " And OrderStatus In (" + sOrderStatus + ")";
            if (sColorNo != "") sSQL = sSQL + " And LabDipID In (SELECT HH.LabDipID FROM LabdipDetail AS HH WHERE HH.ColorNo like '%" + sColorNo + "%')";
            if (sOrderIDs != "") sSQL = sSQL + " And OrderReferenceID In (" + sOrderIDs + ")";
            if (sFabricNo != "") sSQL = sSQL + " And LabDipID IN (SELECT LabDipID FROM FSCLabMapping WHERE FabricID IN (SELECT FabricID FROM Fabric WHERE FabricNo LIKE '%" + sFabricNo + "%') )";  //" And LabDipID In (SELECT HH.LabDipID FROM View_LabdipDetailFabric AS HH WHERE HH.FabricNo like '%" + sFabricNo + "%')";
   
            return sSQL;
        }

        [HttpPost]
        public JsonResult Gets(LabDip oLabDip)
        {
            _oLabDips = new List<LabDip>();
            try
            {
                string sSQL=MakeAdvSQL(oLabDip);
                _oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLabDips = new List<LabDip>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLabDips);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLabDipDetailRnD(LabDipDetail oLabDipDetail)
        {
            List<LabDipReport> oLabDipReports = new List<LabDipReport>();
            try
            {
                string sSQL = "";
                string sReturn = "";
                
                if (!String.IsNullOrEmpty(oLabDipDetail.LabdipNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.LabdipNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ISNULL(LDNo,'')+ISNULL(ColorNo,'')!='' and LabdipNo Like'%" + oLabDipDetail.LabdipNo + "%'";
                }
                if (!String.IsNullOrEmpty(oLabDipDetail.ColorNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.ColorNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ISNULL(LDNo,'')+ISNULL(ColorNo,'')!='' and ColorNo Like'%" + oLabDipDetail.ColorNo + "'";
                }
                if (!String.IsNullOrEmpty(oLabDipDetail.PantonNo))
                {
                    oLabDipDetail.PantonNo = oLabDipDetail.PantonNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ISNULL(LDNo,'')+ISNULL(ColorNo,'')!='' and PantonNo Like'%" + oLabDipDetail.PantonNo + "%'";
                }
                 //Global.TagSQL(ref sReturn);
                 //sReturn = sReturn + "LabdipID in (Select LabdipID from Labdip where Labdip.OrderStatus in (5,6,7,8,9))";
               
                sSQL = sSQL + "" + sReturn;
                oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipReports = new List<LabDipReport>();
            }

            var jsonResult = Json(oLabDipReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetsLabDipDetail(LabDipDetail oLabDipDetail)
        {
            List<LabDipReport> oLabDipReports = new List<LabDipReport>();
            try
            {
                string sSQL = "";
                string sReturn = "";

                if (!String.IsNullOrEmpty(oLabDipDetail.LabdipNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.LabdipNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ISNULL(LDNo,'')+ISNULL(ColorNo,'')!='' and LabdipNo Like'%" + oLabDipDetail.LabdipNo + "%'";
                }
                if (!String.IsNullOrEmpty(oLabDipDetail.ColorNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.ColorNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ISNULL(LDNo,'')+ISNULL(ColorNo,'')!='' and ISNULL(LDNo,'')+ColorNo Like'%" + oLabDipDetail.ColorNo + "%'";
                }

                //Global.TagSQL(ref sReturn);
                //sReturn = sReturn + "LabdipID in (Select LabdipID from Labdip where Labdip.OrderStatus in (5,6,7,8,9))";

                sSQL = sSQL + "" + sReturn;
                oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipReports = new List<LabDipReport>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oLabDipReports);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(oLabDipReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetLabDipDetail(LabDipDetail oLabDipDetail)
        {
            try
            {
                if (oLabDipDetail.LabDipDetailID <= 0)
                    throw new Exception("Please select a valid item.");
                oLabDipDetail = LabDipDetail.Get(oLabDipDetail.LabDipDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeOrderStatus(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0) throw new Exception("Please select a valid labdip.");
                EnumLabdipOrderStatus OrderStatus;
                EnumLabdipOrderStatus.TryParse(oLabDip.Params, out OrderStatus);

                oLabDip = oLabDip.ChangeOrderStatus((short)OrderStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MakeTwistedGroup(LabDipDetail oLabDipDetail)
        {
            LabDip oLabDip = new LabDip();
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sLabDipDetailID = string.IsNullOrEmpty(oLabDipDetail.Params) ? "" : oLabDipDetail.Params;
                if (sLabDipDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oLabDipDetail.LabDipID<=0)
                    throw new Exception("No valid labdip found.");

                oLabDipDetails = LabDipDetail.MakeTwistedGroup(sLabDipDetailID, oLabDipDetail.LabDipID, oLabDipDetail.TwistedGroup, oLabDipDetail.ParentID, (int)EnumDBOperation.Insert,((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDipDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID > 0 && oLabDipDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<LabDipDetail> oTempLabDipDetails = new List<LabDipDetail>();
                    oLabDipDetails.ForEach((item) => { oTempLabDipDetails.Add(item); });
                    oLabDipDetails = this.TwistedLabdipDetails(oLabDipDetails);
                    oLabDipDetails[0].CellRowSpans = this.RowMerge(oTempLabDipDetails);
                }
                oLabDip.LabDipDetails = oLabDipDetails;

            }
            catch (Exception ex)
            {
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveTwistedGroup(LabDipDetail oLabDipDetail)
        {
            LabDip oLabDip = new LabDip();
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sLabDipDetailID = string.IsNullOrEmpty(oLabDipDetail.Params) ? "" : oLabDipDetail.Params;
                if (sLabDipDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oLabDipDetail.LabDipID <= 0)
                    throw new Exception("No valid labdip found.");

                oLabDipDetails = LabDipDetail.MakeTwistedGroup(sLabDipDetailID, oLabDipDetail.LabDipID, oLabDipDetail.TwistedGroup, oLabDipDetail.ParentID, (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oLabDipDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID > 0 && oLabDipDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                      List<LabDipDetail> oTempLabDipDetails = new List<LabDipDetail>();
                    oLabDipDetails.ForEach((item) => { oTempLabDipDetails.Add(item); });
                    oLabDipDetails = this.TwistedLabdipDetails(oLabDipDetails);
                    oLabDipDetails[0].CellRowSpans = this.RowMerge(oTempLabDipDetails);
                }
                oLabDip.LabDipDetails = oLabDipDetails;
               
            }
            catch (Exception ex)
            {
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    
        [HttpPost]
        public JsonResult Relab(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipID <= 0) throw new Exception("Please select a valid labdip.");
                oLabDip = LabDip.Relab(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PRINT PDF
        public ActionResult PrintLabDipWU(int nId, int nView, double nts)
        {
            _oLabDip = new LabDip();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            Fabric oFabric = new Fabric();
            try
            {
                if (nId > 0)
                {
                    _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oLabDip.LabDipDetailFabrics = LabDipDetailFabric.Gets("Select * from View_LabDipDetailFabric Where LabDipID=" + _oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if (_oLabDip.FSCDetailID > 0)
                        {
                            oFabricSalesContractDetail = oFabricSalesContractDetail.Get(_oLabDip.FSCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                    oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptLabDipWU oReport = new rptLabDipWU();
            byte[] abytes = oReport.PrepareReport(_oLabDip.LabDipDetails, nView, oCompany, oBusinessUnit, _oLabDip, oFabricSalesContractDetail);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintLabDipWU_Fabric(int nId, int vno, int nView, double nts)
        {
            _oLabDip = new LabDip();

            List<LabDip> oLabDips = new List<LabDip>();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            Fabric oFabric = new Fabric();
            LabDipDetail oLabDipDetail = new LabDipDetail();
            try
            {
                if (nId > 0)
                {
                    _oLabDip.LabDipDetailFabrics = LabDipDetailFabric.Gets("Select * from View_LabDipDetailFabric Where FSCDetailID=" + nId + " AND ReviseNo =" + vno, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipDetailFabrics.Any()) 
                    {
                        _oLabDip.LabDipDetails = new List<LabDipDetail>();
                        foreach (var oItem in _oLabDip.LabDipDetailFabrics)
                        {
                            oLabDipDetail = new LabDipDetail();
                            oLabDipDetail.ColorName = oItem.ColorName;
                            oLabDipDetail.WarpWeftType =oItem.WarpWeftType;
                            oLabDipDetail.TwistedGroup = oItem.TwistedGroup;
                            oLabDipDetail.ProductName = oItem.ProductName;
                            oLabDipDetail.PantonNo = oItem.PantonNo;
                            oLabDipDetail.ColorNo = oItem.ColorNo;
                            oLabDipDetail.LDNo = oItem.LDNo;
                            oLabDipDetail.RefNo = oItem.Remarks;
                            oLabDipDetail.ProductID = oItem.ProductID;
                            oLabDipDetail.LabDipID = oItem.LabDipID;
                            _oLabDip.LabDipDetails.Add(oLabDipDetail);
                        }

                        //foreach (var oItem in _oLabDip.LabDipDetails) 
                        //{
                        //    oItem.ColorName = _oLabDip.LabDipDetailFabrics.Where(x => x.LabDipDetailID == oItem.LabDipDetailID).Select(x => x.ColorName).FirstOrDefault();
                        //    oItem.WarpWeftType = _oLabDip.LabDipDetailFabrics.Where(x => x.LabDipDetailID == oItem.LabDipDetailID).Select(x => x.WarpWeftType).FirstOrDefault();
                        //    oItem.TwistedGroup = _oLabDip.LabDipDetailFabrics.Where(x => x.LabDipDetailID == oItem.LabDipDetailID).Select(x => x.TwistedGroup).FirstOrDefault();
                        //    oItem.ProductName = _oLabDip.LabDipDetailFabrics.Where(x => x.LabDipDetailID == oItem.LabDipDetailID).Select(x => x.ProductName).FirstOrDefault();
                        //}

                        oLabDips = LabDip.Gets("Select * from View_Labdip Where LabdipID IN (" + string.Join(",", _oLabDip.LabDipDetails.Select(x => x.LabDipID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                       // _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //if (_oLabDip.FSCDetailID > 0)
                        //{
                        //    oFabricSalesContractDetail = oFabricSalesContractDetail.Get(_oLabDip.FSCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //}
                        oFabricSCReport = oFabricSCReport.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        _oLabDip.LabdipNo = string.Join(" ,", oLabDips.Select(x => x.LabdipNo).Distinct());
                        _oLabDip.LDTwistType = oLabDips.Select(x => x.LDTwistType).FirstOrDefault();
                        if (oFabricSCReport.OrderType == (int)EnumFabricRequestType.HandLoom) { _oLabDip.ContractorName = oFabricSCReport.BuyerName; }
                        else { _oLabDip.ContractorName = oFabricSCReport.ContractorName; } //string.Join(" ,", oLabDips.Select(x => x.ContractorName).Distinct());
                        _oLabDip.OrderDateStr = string.Join(" ,", oLabDips.Select(x => x.OrderDate).Distinct());
                    }
                    
                    oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptLabDipWU oReport = new rptLabDipWU();
            byte[] abytes = oReport.PrepareReport(_oLabDip.LabDipDetails, nView, oCompany, oBusinessUnit, _oLabDip, oFabricSalesContractDetail);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintLabDip(int nId, double nts)
        {
            _oLabDip = new LabDip();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            FabricSalesContractDetail oFabricSCDetail = new FabricSalesContractDetail();
            try
            {
                if (nId > 0)
                {
                    _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if(_oLabDip.FSCDetailID>0)
                    {
                        oFabricSCDetail = oFabricSCDetail.Get(_oLabDip.FSCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                } 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLabDip oReport = new rptLabDip();

            if (oLabDipSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
            else if (oLabDipSetup.PrintNo == (int)EnumExcellColumn.B)
            {

                byte[] abytes = oReport.PrepareReport_B(_oLabDip, oCompany, oBusinessUnit, oFabricSCDetail);
                return File(abytes, "application/pdf");
            }
            else
            {
                byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintLabDipByFabric(int nId, double nts)
        {
            
            _oLabDip = new LabDip();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            try
            {
                if (nId > 0)
                {
                    oFabricSalesContractDetail = oFabricSalesContractDetail.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oLabDip = LabDip.GetByFSD(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLabDip oReport = new rptLabDip();

            if (oLabDipSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
            else if (oLabDipSetup.PrintNo == (int)EnumExcellColumn.B)
            {

                byte[] abytes = oReport.PrepareReport_B(_oLabDip, oCompany, oBusinessUnit, oFabricSalesContractDetail);
                return File(abytes, "application/pdf");
            }
            else
            {
                byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintLabDipDeliveryChallan(int nId, double nts, bool bPrintFormat, int nTitleType)
        {
            _oLabDip = new LabDip();
            Contractor oContractor = new Contractor();
            LabdipHistory oLabdipHistory = new LabdipHistory();
            try
            {
                if (nId > 0)
                {
                    _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oContractor = oContractor.Get(_oLabDip.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oLabdipHistory = oLabdipHistory.Getby(nId, (int)EnumLabdipOrderStatus.LabdipInBuyerHand, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oLabDip.SeekingDate = oLabdipHistory.DateTime;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLabDipDeliveryChallan oReport = new rptLabDipDeliveryChallan();
            byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany, oBusinessUnit, nTitleType, oContractor);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintLabdipTestCard(int nId, double nts)
        {
            _oLabDip = new LabDip();
            try
            {
                if (nId > 0)
                {
                    _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLabdipTestCard oReport = new rptLabdipTestCard();
            byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany);
            return File(abytes, "application/pdf");

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

        #endregion

        #region EXCEL
        public void PrintLabDipsXL(string Params, double nts)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            _oLabDips = new List<LabDip>();
            try
            {
                if (string.IsNullOrEmpty(Params))
                {
                    string sSQL = "SELECT top(200)* FROM View_LabDip Where LabDipID<>0 And OrderStatus=" + (int)EnumLabdipOrderStatus.InLab + " Order By OrderDate Desc";
                    _oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else {
                    string sSQL = MakeAdvSQL(new LabDip() { Params = Params });
                    _oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oLabDips = new List<LabDip>();
            }

            #region Print XL

            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0, nCount = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LabDip");
                sheet.Name = "Lab Dip";

                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#", Width = 8f, IsRotate = false});
                table_header.Add(new TableHeader { Header = "LD Order No", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Issue Date", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Seeking Date", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Contractor", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Deliver To", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Mkt Person", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Source", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Status", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Priority", Width = 8f, IsRotate = false });
                
                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 12]; cell.Merge = true;
                cell.Value = "Lab Dip"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 12]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 12]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10);

                #region Report Data

                #region Body
                foreach (var oItem in _oLabDips)
                {
                    nStartCol = 2;
                    nCount++;
                    FillCell(sheet, nRowIndex, nStartCol++, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.LabdipNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderDateStr, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.SeekingDateStr, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.DeliveryToName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.MktPerson, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderReferenceTypeSt, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderStatusStr, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.PriorityLevelStr, false);
                    
                    sheet.Row(nRowIndex).Style.Font.Size = 10;
                    nRowIndex++;
                }
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 13];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LabDipList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            cell = sheet.Cells[nRowIndex, nStartCol++];
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            if (IsNumber)
                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        #endregion

        #region Gets Dyeing Order
        [HttpPost]
        public JsonResult GetsDO(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
            try
            {
                string sSQL = "Select top(100)* from View_DyeingOrder Where DyeingOrderID<>0 ";

                if (!string.IsNullOrEmpty(oDyeingOrder.OrderNo))
                    sSQL = sSQL + " And NoCode+OrderNo Like '%" + oDyeingOrder.OrderNo + "%'";
                #region Contractor
                if (!String.IsNullOrEmpty(oDyeingOrder.ContractorName))
                {
                    sSQL = sSQL + " and ContractorID in(" + oDyeingOrder.ContractorName + ")";
                }
                #endregion
                sSQL = sSQL + " order by OrderDate DESC";
                _oDyeingOrders = DyeingOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
                _oDyeingOrders = new List<DyeingOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Laboratory
        public ActionResult ViewLabDipInLaboratorys( int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
         
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oLabDips = new List<LabDip>();
            string sSQL = " SELECT top(200)* FROM View_LabDip Where LabDipID<>0 And OrderStatus=" + (int)EnumLabdipOrderStatus.InLab + " Order By OrderDate Desc";
            _oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.EnumLabdipOrderStatuss = Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            ViewBag.LabDipSetup = oLabDipSetup;
            return View(_oLabDips);
        }

        public ActionResult ViewLabDipInLaboratory(int nId, int buid)
        {
            _oLabDip = new LabDip();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (nId > 0)
            {
                _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oLabDip.LabDipID > 0)
                {
                    string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                    _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oLabDip.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oLabDip.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.EnumLabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //Enum.GetValues(typeof(EnumLabdipFormat)).Cast<EnumLabdipFormat>().Where(x => (int)x != 0).Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumPriorityLevels = EnumObject.jGets(typeof(EnumPriorityLevel));// Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Where(x => (int)x != 0).Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumShades = EnumObject.jGets(typeof(EnumShade));// Enum.GetValues(typeof(EnumShade)).Cast<EnumShade>().Where(x => (int)x != 0).Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LabDipSetup = oLabDipSetup;
            ViewBag.BUID = buid;
            return View(_oLabDip);
        }

        #region Color Issue

        [HttpPost]
        public JsonResult IssueColor(LabDipDetail oLabDipDetail)
        {
            try
            {
                if (oLabDipDetail.LabDipDetailID <= 0) throw new Exception("Please select an item form list.");
                oLabDipDetail = LabDipDetail.IssueColor(oLabDipDetail.LabDipDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult IssueColorMultiple(LabDip oLabDip)
        {
            try
            {
                if (oLabDip.LabDipDetails.Count() <= 0) throw new Exception("No items found to assign color. Please select items from list.");

                List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
                int[] LabDipDetailIDs = oLabDip.LabDipDetails.Select(x => x.LabDipDetailID).ToArray();
                string sSQL = "SELECT * FROM  View_LabdipDetail Where ColorCreateBy>0 And ColorNo='' And LabDipDetailID In (" + string.Join(",", LabDipDetailIDs) + ")";
                oLabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oLabDipDetails.Count() > 0 && oLabDipDetails.FirstOrDefault().LabDipDetailID > 0)
                    throw new Exception("Please unchecked the color item first.");


                oLabDipDetails = new List<LabDipDetail>();
                oLabDipDetails = LabDipDetail.IssueColorMultiple(LabDipDetailIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oLabDip = new LabDip();
                if (oLabDipDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID > 0)
                {
                    oLabDip.LabDipDetails = oLabDipDetails;
                }
            }
            catch (Exception ex)
            {
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save_ColorNo(LabDipDetail oLabDipDetail)
        {
            oLabDipDetail.RemoveNulls();
            try
            {
                oLabDipDetail = oLabDipDetail.Save_ColorNo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_PantonNo(LabDipDetail oLabDipDetail)
        {
            oLabDipDetail.RemoveNulls();
            try
            {
                if (oLabDipDetail.LabDipDetailID <= 0) throw new Exception("Invalid Color in Lab!");

                oLabDipDetail = oLabDipDetail.Save_PantonNo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateLot_Recipe(LabdipRecipe oLabdipRecipe)
        {
            oLabdipRecipe.RemoveNulls();
            try
            {
                oLabdipRecipe = oLabdipRecipe.UpdateLot(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabdipRecipe = new LabdipRecipe();
                oLabdipRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    
        #endregion

        #region Assigned Person

        [HttpPost]
        public JsonResult GetsLabdipAssignedPersonnel(LabDipDetail oLabDipDetail)
        {
            List<LabdipAssignedPersonnel> oLabdipAssignedPersonnels = new List<LabdipAssignedPersonnel>();
            try
            {
                string sSQL = "SELECT * FROM View_LabdipAssignedPersonnel WHERE LabdipDetailID = " + oLabDipDetail.LabDipDetailID;
                oLabdipAssignedPersonnels = LabdipAssignedPersonnel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oLabdipAssignedPersonnels = new List<LabdipAssignedPersonnel>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipAssignedPersonnels);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeForLabdipAssignedPersonnel(LabDipDetail oLabDipDetail)
        {
            List<Employee> oEmployees = new List<Employee>();
            try
            {
                string sSQL = "Select * from View_Employee Where EmployeeID Not In (Select EmployeeID from LabdipAssignedPersonnel Where LabdipDetailID= "+oLabDipDetail.LabDipDetailID+")";
                if (!string.IsNullOrEmpty(oLabDipDetail.Params))
                {
                    sSQL = sSQL + " And (Code Like '%" + oLabDipDetail.Params + "%' Or Name Like '%" + oLabDipDetail.Params + "%')";
                }
                oEmployees = Employee.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oEmployees = new List<Employee>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveLabdipAssignedPersonnel(LabdipAssignedPersonnel oLabdipAssignedPersonnel)
        {
            try
            {
                if (oLabdipAssignedPersonnel.LabdipAssignedPersonnelID <= 0)
                {
                    oLabdipAssignedPersonnel = oLabdipAssignedPersonnel.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLabdipAssignedPersonnel = oLabdipAssignedPersonnel.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipAssignedPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLabdipAssignedPersonnel(LabdipAssignedPersonnel oLabdipAssignedPersonnel)
        {
            try
            {
                if (oLabdipAssignedPersonnel.LabdipAssignedPersonnelID <= 0) { throw new Exception("Please select an valid item."); }
                oLabdipAssignedPersonnel = oLabdipAssignedPersonnel.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();
                oLabdipAssignedPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipAssignedPersonnel.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Labdip Shade & Recipe

        [HttpPost]
        public JsonResult SaveLabdipShade(LabdipShade oLabdipShade)
        {
            try
            {
                if (oLabdipShade.LabdipShadeID <= 0)
                {
                    oLabdipShade = oLabdipShade.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLabdipShade = oLabdipShade.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabdipShade = new LabdipShade();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLabdipShade(LabdipShade oLabdipShade)
        {
            try
            {
                if (oLabdipShade.LabdipShadeID <= 0) { throw new Exception("Please select an valid item."); }
                oLabdipShade = oLabdipShade.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabdipShade = new LabdipShade();
                oLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipShade.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveLabdipShade(LabdipShade oLabdipShade)
        {
            try
            {
                if (oLabdipShade.LabdipShadeID <= 0) { throw new Exception("Please select an valid item."); }
                oLabdipShade = oLabdipShade.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabdipShade = new LabdipShade();
                oLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CopyLabdipShade(LabdipShade oLabdipShade)
        {
            try
            {

                if (oLabdipShade.LabdipShadeID <= 0 || oLabdipShade.LabdipDetailID<=0) { throw new Exception("Please select an valid item."); }

                LabDipDetail oLabDipDetail = new LabDipDetail();
                oLabDipDetail = LabDipDetail.Get(oLabdipShade.LabdipDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<LabdipShade> oLabdipShades = new List<LabdipShade>();
                string sSQL = "Select * from View_LabdipShade Where LabdipDetailID=" + oLabDipDetail.LabDipDetailID;
                oLabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oLabdipShade = oLabdipShades.FirstOrDefault(x => x.LabdipShadeID == oLabdipShade.LabdipShadeID);

                if (oLabdipShade == null || oLabdipShade.LabdipShadeID <= 0) throw new Exception("Invalid labdip shade. Please refresh & try again.");

                if (oLabdipShade.LabdipShadeID > 0)
                {
                    List<LabdipRecipe> oLabdipRecipes=new List<LabdipRecipe>();
                    sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID=" + oLabdipShade.LabdipShadeID;
                    oLabdipRecipes=LabdipRecipe.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oLabdipRecipes.Count() > 0)
                    {
                        oLabdipRecipes.ForEach(x => { x.LabdipRecipeID = 0; x.LabdipShadeID = 0; });
                        oLabdipShade.RecipeDyes = oLabdipRecipes.Where(x => x.IsDyes == true).ToList();
                        oLabdipShade.RecipeChemicals = oLabdipRecipes.Where(x => x.IsDyes == false).ToList();
                    }

                    int nShadeCount = oLabDipDetail.ShadeCount;
                    EnumShade previousShade = oLabdipShades.Max(x => x.ShadeID);
                    EnumShade maxShade= Enum.GetValues(typeof(EnumShade)).Cast<EnumShade>().Max();
                    nShadeCount = ((int)maxShade < nShadeCount) ? (int)maxShade : nShadeCount;
                    if (previousShade == maxShade || nShadeCount == (int)previousShade) throw new Exception("No remaining shade available to create.");

                    oLabdipShade.LabdipShadeID = 0;
                    oLabdipShade.ShadeID = Enum.GetValues(typeof(EnumShade)).Cast<EnumShade>().Where(x => (int)x > (int)previousShade).First();

                    oLabdipShade = oLabdipShade.CopyLabdipShade(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabdipShade = new LabdipShade();
                oLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLabdipRecipe(LabdipRecipe oLabdipRecipe)
        {
            try
            {
                LabdipShade oLabdipShade = LabdipShade.Get(oLabdipRecipe.LabdipShadeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabdipRecipe.LabdipRecipeID <= 0)
                {
                    oLabdipRecipe = oLabdipRecipe.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLabdipRecipe = oLabdipRecipe.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oLabdipShade.Qty > 0)
                {
                    oLabdipRecipe.PerTage = (oLabdipRecipe.Qty * 100) / oLabdipShade.Qty;
                }
            }
            catch (Exception ex)
            {
                oLabdipRecipe = new LabdipRecipe();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLabdipRecipe(LabdipRecipe oLabdipRecipe)
        {
            try
            {
                if (oLabdipRecipe.LabdipRecipeID <= 0) { throw new Exception("Please select an valid item."); }
                oLabdipRecipe = oLabdipRecipe.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabdipRecipe = new LabdipRecipe();
                oLabdipRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipRecipe.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsLabdipShade(LabDipDetail oLabDipDetail)
        {
            LabdipShade oLabdipShade = new LabdipShade();
            try
            {
                string sSQL = "Select * from View_LabdipShade Where LabdipDetailID = " + oLabDipDetail.LabDipDetailID;
                oLabdipShade.LabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabdipShade.LabdipShades.Count() > 0 && oLabdipShade.LabdipShades[0].LabdipShadeID > 0)
                {
                    List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();

                    sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID In (" + string.Join(",", oLabdipShade.LabdipShades.Select(x => x.LabdipShadeID)) + ")";
                    oLabdipRecipes = LabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oLabdipShade.RecipeDyes = oLabdipRecipes.Where(x => x.IsDyes == true).ToList();
                    oLabdipShade.RecipeChemicals = oLabdipRecipes.Where(x => x.IsDyes == false).ToList();
                }
            }
            catch (Exception ex)
            {
                oLabdipShade = new LabdipShade();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult PrintLabDipRecipe(int nId, int nDetailID, double nts)
        {
            _oLabDip = new LabDip();
            List<LabdipShade> oLabdipShades = new List<LabdipShade>();
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();
            try
            {
                if (nId > 0)
                {
                    _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        
                        if (nDetailID > 0)
                        {
                            sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID + " And LabDipDetailID=" + nDetailID;
                            _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipShade Where LabdipDetailID =" + nDetailID;
                            oLabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID in (Select LabdipShadeID from LabdipShade Where LabdipDetailID =" + nDetailID+")";
                            oLabdipRecipes = LabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else 
                        {
                            sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                            _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipShade Where LabdipDetailID in (Select LabdipDetailID from LabdipDetail where LabdipID=" + _oLabDip.LabDipID + ")";
                            oLabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID in (Select LabdipShadeID from LabdipShade Where LabdipDetailID in (Select LabdipDetailID from LabdipDetail where LabdipID=" + _oLabDip.LabDipID + "))";
                            oLabdipRecipes = LabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Fabric oFabric = new Fabric();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oLabDip.FabricID > 0)
            {
                oFabric = oFabric.Get(_oLabDip.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            rptLabDipRecipe oReport = new rptLabDipRecipe();
            byte[] abytes = oReport.PrepareReport(_oLabDip, oCompany, oBusinessUnit, oLabdipShades, oLabdipRecipes, oFabric);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintLabDipSubmission(int nId, int nDetailID, double nts)
        {
            _oLabDip = new LabDip();
            List<LabdipShade> oLabdipShades = new List<LabdipShade>();
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();
            try
            {
                if (nId > 0)
                {
                    _oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                        
                        if (nDetailID > 0)
                        {
                            sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID + " And LabDipDetailID=" + nDetailID;
                            _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipShade Where LabdipDetailID =" + nDetailID;
                            oLabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID in (Select LabdipShadeID from LabdipShade Where LabdipDetailID =" + nDetailID+")";
                            oLabdipRecipes = LabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else 
                        {
                            sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;
                            _oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipShade Where LabdipDetailID in (Select LabdipDetailID from LabdipDetail where LabdipID=" + _oLabDip.LabDipID + ")";
                            oLabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID in (Select LabdipShadeID from LabdipShade Where LabdipDetailID in (Select LabdipDetailID from LabdipDetail where LabdipID=" + _oLabDip.LabDipID + "))";
                            oLabdipRecipes = LabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Fabric oFabric = new Fabric();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptLabDipSubCard oReport = new rptLabDipSubCard();
            byte[] abytes = oReport.PrepareReport(_oLabDip.LabDipDetails, oCompany, oBusinessUnit, _oLabDip, oLabdipShades);
            return File(abytes, "application/pdf");
        }
          
        #endregion

        #region Merger Row
        private List<CellRowSpan> RowMerge(List<LabDipDetail> oLabDipDetails)
        {

            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<LabDipDetail> oTWGLDDetails = new List<LabDipDetail>();
            List<LabDipDetail> oLDDetails = new List<LabDipDetail>();
            List<LabDipDetail> oTempLDDetails = new List<LabDipDetail>();

            oTWGLDDetails = oLabDipDetails.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails=oLabDipDetails.Where(x=>x.TwistedGroup==0).ToList();

            while (oLabDipDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID == oTWGLDDetails.FirstOrDefault().LabDipDetailID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();

                    oLabDipDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID == oLDDetails.FirstOrDefault().LabDipDetailID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.LabDipDetailID == oLDDetails.FirstOrDefault().LabDipDetailID).ToList();

                    oLabDipDetails.RemoveAll(x => x.LabDipDetailID == oTempLDDetails.FirstOrDefault().LabDipDetailID);
                    oLDDetails.RemoveAll(x => x.LabDipDetailID == oTempLDDetails.FirstOrDefault().LabDipDetailID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0]; 
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("TwistedGroup", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<LabDipDetail> TwistedLabdipDetails(List<LabDipDetail> oLabDipDetails)
        {
            List<LabDipDetail> oTwistedLDDetails = new List<LabDipDetail>();
            List<LabDipDetail> oTWGLDDetails = new List<LabDipDetail>();
            List<LabDipDetail> oLDDetails = new List<LabDipDetail>();
            List<LabDipDetail> oTempLDDetails = new List<LabDipDetail>();

            oTWGLDDetails = oLabDipDetails.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oLabDipDetails.Where(x => x.TwistedGroup == 0).ToList();

            while (oLabDipDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID == oTWGLDDetails.FirstOrDefault().LabDipDetailID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();
                    oLabDipDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oLabDipDetails.FirstOrDefault().LabDipDetailID == oLDDetails.FirstOrDefault().LabDipDetailID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.LabDipDetailID == oLDDetails.FirstOrDefault().LabDipDetailID).ToList();

                    oLabDipDetails.RemoveAll(x => x.LabDipDetailID == oTempLDDetails.FirstOrDefault().LabDipDetailID);
                    oLDDetails.RemoveAll(x => x.LabDipDetailID == oTempLDDetails.FirstOrDefault().LabDipDetailID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        #endregion

        #region Merger Row : LabDipDetailFabric

        [HttpPost]
        public JsonResult MakeTwistedGroup_Fabric(LabDipDetailFabric oLabDipDetailFabric)
        {
            LabDip oLabDip = new LabDip();
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            try
            {
                string sLabDipDetailID = string.IsNullOrEmpty(oLabDipDetailFabric.Params) ? "" : oLabDipDetailFabric.Params;
                if (sLabDipDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oLabDipDetailFabric.FSCDetailID <= 0)
                    throw new Exception("No valid labdip found.");

                oLabDipDetailFabrics = LabDipDetailFabric.MakeTwistedGroup(sLabDipDetailID, oLabDipDetailFabric.FSCDetailID, oLabDipDetailFabric.TwistedGroup, 0, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDipDetailFabrics.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LabDipDetailID > 0 && oLabDipDetailFabrics.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<LabDipDetailFabric> oTempLabDipDetails = new List<LabDipDetailFabric>();
                    oLabDipDetailFabrics.ForEach((item) => { oTempLabDipDetails.Add(item); });
                    oLabDipDetailFabrics = this.TwistedLabdipDetailFabrics(oLabDipDetailFabrics);
                    oLabDipDetailFabrics[0].CellRowSpans = this.RowMerge(oTempLabDipDetails);
                }
                oLabDip.LabDipDetailFabrics = oLabDipDetailFabrics;
            }
            catch (Exception ex)
            {
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveTwistedGroup_Fabric(LabDipDetailFabric LabDipDetailFabric)
        {
            LabDip oLabDip = new LabDip();
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            try
            {
                string sLabDipDetailID = string.IsNullOrEmpty(LabDipDetailFabric.Params) ? "" : LabDipDetailFabric.Params;
                if(sLabDipDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (LabDipDetailFabric.FSCDetailID <= 0)
                    throw new Exception("No valid labdip found.");

                oLabDipDetailFabrics = LabDipDetailFabric.MakeTwistedGroup(sLabDipDetailID, LabDipDetailFabric.FSCDetailID, LabDipDetailFabric.TwistedGroup, 0, (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDipDetailFabrics.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LabDipDetailID > 0 && oLabDipDetailFabrics.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<LabDipDetailFabric> oTempLabDipDetails = new List<LabDipDetailFabric>();
                    oLabDipDetailFabrics.ForEach((item) => { oTempLabDipDetails.Add(item); });
                    oLabDipDetailFabrics = this.TwistedLabdipDetailFabrics(oLabDipDetailFabrics);
                    oLabDipDetailFabrics[0].CellRowSpans = this.RowMerge(oTempLabDipDetails);
                }
                oLabDip.LabDipDetailFabrics = oLabDipDetailFabrics;
            }
            catch (Exception ex)
            {
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<CellRowSpan> RowMerge(List<LabDipDetailFabric> oLabDipDetailFabrics)
        {
            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<LabDipDetailFabric> oTWGLDDetails = new List<LabDipDetailFabric>();
            List<LabDipDetailFabric> oLDDetails = new List<LabDipDetailFabric>();
            List<LabDipDetailFabric> oTempLDDetails = new List<LabDipDetailFabric>();

            oTWGLDDetails = oLabDipDetailFabrics.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oLabDipDetailFabrics.Where(x => x.TwistedGroup == 0).ToList();

            while (oLabDipDetailFabrics.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LDFID == oTWGLDDetails.FirstOrDefault().LDFID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();

                    oLabDipDetailFabrics.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LDFID == oLDDetails.FirstOrDefault().LDFID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.LDFID == oLDDetails.FirstOrDefault().LDFID).ToList();

                    oLabDipDetailFabrics.RemoveAll(x => x.LDFID == oTempLDDetails.FirstOrDefault().LDFID);
                    oLDDetails.RemoveAll(x => x.LDFID == oTempLDDetails.FirstOrDefault().LDFID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0];
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("TwistedGroup", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<LabDipDetailFabric> TwistedLabdipDetailFabrics(List<LabDipDetailFabric> oLabDipDetailFabrics)
        {
            List<LabDipDetailFabric> oTwistedLDDetails = new List<LabDipDetailFabric>();
            List<LabDipDetailFabric> oTWGLDDetails = new List<LabDipDetailFabric>();
            List<LabDipDetailFabric> oLDDetails = new List<LabDipDetailFabric>();
            List<LabDipDetailFabric> oTempLDDetails = new List<LabDipDetailFabric>();

            oTWGLDDetails = oLabDipDetailFabrics.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oLabDipDetailFabrics.Where(x => x.TwistedGroup == 0).ToList();

            while (oLabDipDetailFabrics.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LDFID == oTWGLDDetails.FirstOrDefault().LDFID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();
                    oLabDipDetailFabrics.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oLabDipDetailFabrics.FirstOrDefault().LDFID == oLDDetails.FirstOrDefault().LDFID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.LDFID == oLDDetails.FirstOrDefault().LDFID).ToList();

                    oLabDipDetailFabrics.RemoveAll(x => x.LDFID == oTempLDDetails.FirstOrDefault().LDFID);
                    oLDDetails.RemoveAll(x => x.LDFID == oTempLDDetails.FirstOrDefault().LDFID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        #endregion

        #region LapDipColor Code
        public ActionResult ViewoLabdipColors(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LabdipColor> oLapdipColors = new List<LabdipColor>();
            string sSQL = "Select * from LabdipColor";
            oLapdipColors = LabdipColor.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            ViewBag.BUID = buid;
            return View(oLapdipColors);

        }

        [HttpPost]
        public JsonResult SaveLapdipColor(LabdipColor oLapdipColor)
        {
            try
            {
                if (oLapdipColor.LabdipColorID <= 0)
                {
                    oLapdipColor = oLapdipColor.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLapdipColor = oLapdipColor.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oLapdipColor = new LabdipColor();
                oLapdipColor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLapdipColor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLapdipColor(LabdipColor oLapdipColor)
        {
            try
            {
                if (oLapdipColor.LabdipColorID <= 0) { throw new Exception("Please select an valid item."); }
                oLapdipColor = oLapdipColor.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLapdipColor = new LabdipColor();
                oLapdipColor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLapdipColor.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLapdipColor(LabdipColor oLapdipColor)
        {
            try
            {
                if (oLapdipColor.LabdipColorID <= 0) { throw new Exception("Please select an valid item."); }
                oLapdipColor = LabdipColor.Get(oLapdipColor.LabdipColorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLapdipColor = new LabdipColor();
                oLapdipColor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLapdipColor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLabDipLolors(LabdipColor oLapdipColor)
        {
            string sSQL = "Select * from LabdipColor";
            if(!String.IsNullOrEmpty(oLapdipColor.Code))
            {
                sSQL = sSQL+" where LabdipColor.Code like '%" +oLapdipColor.Code+ "%'";
            }
            List<LabdipColor> oLapdipColors = new List<LabdipColor>();
            try
            {
                
                oLapdipColors = LabdipColor.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLapdipColor = new LabdipColor();
                oLapdipColor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLapdipColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDistingDeliveryNote(LabDip oLabDip)
        {
            string sSQL = "";
            List<LabDip> oLabDips = new List<LabDip>();
            try
            {

                if (!string.IsNullOrEmpty(oLabDip.DeliveryNote))
                {
                    sSQL = "SELECT DISTINCT(DeliveryNote) As DeliveryNote  FROM LabDip where LEN(DeliveryNote) > 1 and DeliveryNote LIKE '%" + oLabDip.DeliveryNote + "%'";
                }
                else
                {
                    sSQL = "SELECT DISTINCT(DeliveryNote) As DeliveryNote  FROM LabDip where LEN(DeliveryNote) > 1";
                }


                oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabDips.Count <= 0)
                {
                    oLabDip = new LabDip();
                    oLabDip.ErrorMessage = "";
                    oLabDips.Add(oLabDip);
                }

            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
                oLabDips.Add(oLabDip);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDips);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Labdip Detail
        public ActionResult ViewLabDipDetails(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            LabDipSetup oLabDipSetup=new LabDipSetup();

            oLabDipDetails = LabDipDetail.Gets("SELECT top(120)* FROM  View_LabdipDetail where Isnull(ColorCreateBy,0)=0 order by OrderDate desc", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumShades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.BUID = buid;

            ViewBag.FabricRequestType = EnumObject.jGets(typeof(EnumFabricRequestType));
            return View(oLabDipDetails);
        }
        [HttpPost]
        public JsonResult Save_Receive(LabDipDetail oLabDipDetail)
        {
            try
            {
                oLabDipDetail = oLabDipDetail.LabDip_Receive_Submit((int)EnumDBOperation.Receive, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_LabDipDone(LabDipDetail oLabDipDetail)
        {
            try
            {
                oLabDipDetail = oLabDipDetail.LabDip_Receive_Submit((int)EnumDBOperation.Delivered, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Submit(LabDipDetail oLabDipDetail)
        {
            try
            {
                oLabDipDetail = oLabDipDetail.LabDip_Receive_Submit((int)EnumDBOperation.Disburse, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #region Search
        [HttpPost]
        public JsonResult GetbyLDNo(LabDipDetail oFNLabDipDetail)
        {
            string sContractorIDs = "";
            if (!string.IsNullOrEmpty(oFNLabDipDetail.Params))
            {
                sContractorIDs = Convert.ToString(oFNLabDipDetail.Params.Split('~')[0]);
            }

            string sSQL = "";
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            string sReturn1 = "SELECT top(100)* FROM View_LabdipDetail";
            string sReturn = "";

            #region LD No
            if (!string.IsNullOrEmpty(oFNLabDipDetail.LabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipNo LIKE '%" + oFNLabDipDetail.LabdipNo + "%'";
            }
            #endregion

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "isnull(LabdipNo,'')<>''";

            #region Contractor
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID in (" + sContractorIDs + ")";
            }
            #endregion
            #region Fabric
            if (!string.IsNullOrEmpty(oFNLabDipDetail.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricNo Like %" + oFNLabDipDetail.FabricNo + "%";
            }
            #endregion
            //#region PantonNo
            //if (!string.IsNullOrEmpty(oFNLabDipDetail.PantonNo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " PantonNo Like %" + oFNLabDipDetail.PantonNo + "%";
            //}
            //#endregion
            //#region BUID
            //if (oExportLC.BUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID=" + oExportLC.BUID;
            //}
            //#endregion
            sSQL = sReturn1 + sReturn;
            oLabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AdvSearch(LabDipDetail oLabDipDetail)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sSQL = MakeSQL(oLabDipDetail);
                oLabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetails = new List<LabDipDetail>();
                oLabDipDetail.ErrorMessage = ex.Message;
                oLabDipDetails.Add(oLabDipDetail);
            }
            var jsonResult = Json(oLabDipDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(LabDipDetail oLabDipDetail)
        {
            string sParams = oLabDipDetail.ErrorMessage;

            string sContractorIDs = "";
            string sProductIDs = "";

            int ncboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;

            int ncboReceiveDate = 0;
            DateTime dFromReceiveDate = DateTime.Today;
            DateTime dToReceiveDate = DateTime.Today;
            int ncboSubmitDate = 0;
            DateTime dFromSubmitDate = DateTime.Today;
            DateTime dToSubmitDate = DateTime.Today;
            int nCboMkPerson = 0;

            string sLabdipNo = "";
            string sFabricNo = "";
            string sColorName = "";
            string sConstruction = "";
            string sOrderNo = "";
            string sLDNo = "";
            string sColorNo = "";
            string sPantonNo = "";
            int nInOutSide = 0;
            int nOrderStatus = 0;
            if (!string.IsNullOrEmpty(sParams))
            {

                sContractorIDs = Convert.ToString(sParams.Split('~')[0]);
                sProductIDs = Convert.ToString(sParams.Split('~')[1]);

                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[2]);
                if (ncboOrderDate > 0)
                {
                    dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[3]);
                    dToOrderDate = Convert.ToDateTime(sParams.Split('~')[4]);
                }

                ncboReceiveDate = Convert.ToInt32(sParams.Split('~')[5]);
                if (ncboReceiveDate > 0)
                {
                    dFromReceiveDate = Convert.ToDateTime(sParams.Split('~')[6]);
                    dToReceiveDate = Convert.ToDateTime(sParams.Split('~')[7]);
                }

                ncboSubmitDate = Convert.ToInt32(sParams.Split('~')[8]);
                if (ncboSubmitDate > 0)
                {
                    dFromSubmitDate = Convert.ToDateTime(sParams.Split('~')[9]);
                    dToSubmitDate = Convert.ToDateTime(sParams.Split('~')[10]);
                }
                nCboMkPerson = 0;

                sLabdipNo = Convert.ToString(sParams.Split('~')[12]);
                sFabricNo = Convert.ToString(sParams.Split('~')[13]);
                sColorName = Convert.ToString(sParams.Split('~')[14]);
                sConstruction = Convert.ToString(sParams.Split('~')[15]);
                sOrderNo = Convert.ToString(sParams.Split('~')[16]);
                sLDNo = Convert.ToString(sParams.Split('~')[17]);
                sColorNo = Convert.ToString(sParams.Split('~')[18]);
                nInOutSide = Convert.ToInt16(sParams.Split('~')[19]);

                if (sParams.Split('~').Length > 20)
                    nOrderStatus = Convert.ToInt16(sParams.Split('~')[20]); 
                if (sParams.Split('~').Length > 21)
                    sPantonNo = sParams.Split('~')[21];
            }


            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "SELECT * FROM View_LabDipDetail";

            #region Contractor
            if (!String.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + sContractorIDs + ")";
            }
            #endregion

            #region ProductName
            if (!String.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + sProductIDs + ")";
            }
            #endregion

            #region Issue Date
            if (ncboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region ncboReceiveDate
            if (ncboReceiveDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                Global.TagSQL(ref sReturn);
                if (ncboReceiveDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ColorCreateDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ColorCreateDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ColorCreateDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ColorCreateDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ColorCreateDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromReceiveDate.ToString("dd MMM yyyy") + " To " + dToReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ColorCreateDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromReceiveDate.ToString("dd MMM yyyy") + " To " + dToReceiveDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Submit Date
            if (ncboSubmitDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboSubmitDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region sConstruction
            if (!string.IsNullOrEmpty(sConstruction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Construction LIKE '%" + sConstruction + "%'";
            }
            #endregion

            #region sLabdipNo
            if (!string.IsNullOrEmpty(sLabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipNo LIKE '%" + sLabdipNo + "%'";
            }
            #endregion

            #region FabricNo
            if (!string.IsNullOrEmpty(sFabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  LabDipID IN (SELECT LabDipID FROM FSCLabMapping WHERE FabricID IN (SELECT FabricID FROM Fabric WHERE FabricNo LIKE '%"+ sFabricNo + "%') )";
            }
            #endregion
            #region ColorName
            if (!string.IsNullOrEmpty(sColorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorName LIKE '%" + sColorName + "%' ";
            }
            #endregion
            #region sOrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderNo LIKE '%" + sOrderNo + "%' ";
            }
            #endregion
            #region sLDNo
            if (!string.IsNullOrEmpty(sLDNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LDNo LIKE '%" + sLDNo + "%' ";
            }
            #endregion
            #region  sColorNo;
            if (!string.IsNullOrEmpty(sColorNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorNo LIKE '%" + sColorNo + "%' ";
            }
            #endregion
            #region  sPantonNo;
            if (!string.IsNullOrEmpty(sPantonNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PantonNo LIKE '%" + sPantonNo + "%' ";
            }
            #endregion
            #region InOutSide
            if (nInOutSide> 0)
            {
                Global.TagSQL(ref sReturn);
                if (nInOutSide == 1)
                {
                    sReturn = sReturn + "isnull(IsInHouse,0)=1";
                }
                if (nInOutSide == 2)
                {
                    sReturn = sReturn + "isnull(IsInHouse,0)=0";
                }
            }
            #endregion


            #region Order Status
            if (nOrderStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nOrderStatus == 1)
                {
                    sReturn = sReturn + "isnull(OrderStatus,0)=1";
                }
                if (nOrderStatus == 8)
                {
                    sReturn = sReturn + "isnull(OrderStatus,0)=8";
                }
                if (nOrderStatus == 2)// "Issue" (this.ColorCreateBy == 0)
                {
                    sReturn = sReturn + "isnull(OrderStatus,0)>=3 AND isnull(OrderStatus,0)<5 AND isnull(ColorCreateBy,0) = 0 ";
                }
                if (nOrderStatus == 3)// "Received"     else if (this.ColorCreateBy != 0 && this.SubmitBy == 0)
                {
                    sReturn = sReturn + "isnull(OrderStatus,0)>=3 AND isnull(OrderStatus,0)<5 AND isnull(ColorCreateBy,0) != 0 AND  isnull(SubmitBy,0) = 0 ";
                }
                if (nOrderStatus == 4)// "LabdipDone"    else if (this.SubmitBy != 0)
                {
                    sReturn = sReturn + "isnull(OrderStatus,0)>=3 AND isnull(OrderStatus,0)<5 AND AND  isnull(SubmitBy,0) != 0 ";
                }
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " Order BY LabdipID DESC";
            return sSQL;
        }
        #endregion
        #region 
        public void PrintLabDipsXLTwo(string Params, double nts)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sSQL = "";
                if (string.IsNullOrEmpty(Params))
                    sSQL = "SELECT * FROM  View_LabdipDetail where Isnull(ColorCreateBy,0)=0";
                else
                    sSQL = MakeSQL(new LabDipDetail { ErrorMessage = Params });

                oLabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLabDipDetail = new LabDipDetail();
                _oLabDipDetail.ErrorMessage = ex.Message;
                oLabDipDetails.Add(_oLabDipDetail);
            }

            #region Print XL

            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0, nCount = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FNLabDip");
                sheet.Name = "Lab Dip";

                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Issue Date", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric No", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Composition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LD Order No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LD No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color Name", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Panton No", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Status", Width = 8f, IsRotate = false });

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);

                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                cell.Value = "Lab Dip"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Blank
                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 10]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10);

                #region Report Data

                #region Body
                foreach (var oItem in oLabDipDetails)
                {
                    nStartCol = 2;
                    nCount++;
                    FillCell(sheet, nRowIndex, nStartCol++, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderDateSt, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.Construction, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.LabdipNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.PantonNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.Status, false);

                    sheet.Row(nRowIndex).Style.Font.Size = 10;
                    nRowIndex++;
                }
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 13];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FNLabDipList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
    
        #endregion

        #endregion

        #region LabDip Fabric Pattern
        public ActionResult ViewLabDipFabricPattern(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FabricPatternDetail> oFabricPatternDetails = new List<FabricPatternDetail>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oFabricPatternDetails = FabricPatternDetail.Gets("SELECT top(120)* FROM  View_FabricPatternDetail where isnull(LabdipDetailID,0)<=0 and FPID in (select FabricPattern.FPID from FabricPattern where FabricPattern.IsActive=1)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oFabricPatternDetails);
        }
        #endregion
    }
}