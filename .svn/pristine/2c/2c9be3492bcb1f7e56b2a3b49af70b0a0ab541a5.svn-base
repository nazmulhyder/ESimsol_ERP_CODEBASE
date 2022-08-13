using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using ESimSol.BusinessObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;



namespace ESimSolFinancial.Controllers 
{
    public class DUOrderStatusController  : Controller
    {
        #region Declaration
        RptDUOrderStatus oRptDUOrderStatus = new RptDUOrderStatus();
        List<RptDUOrderStatus> _oRptDUOrderStatuss = new List<RptDUOrderStatus>();
        DyeingOrder oDyeingOrder = new DyeingOrder();
        List<DyeingOrder> _DyeingOrders = new List<DyeingOrder>();
        DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
        List<DyeingOrderDetail> _DyeingOrderDetails = new List<DyeingOrderDetail>();
        List<DUOrderSetup> OrderTypes = new List<DUOrderSetup>();
        DataSet _oDataSet = new DataSet();
        public bool LSDUReq = false;
        #endregion
        public ActionResult ViewDUOrderStatus(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptDUOrderStatus> oDUOrderStatuss = new List<RptDUOrderStatus>();
            ViewBag.BUID = buid;
            ViewBag.LSDUReq = true;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = OrderTypes.Where(x => x.OrderType != 11 && x.OrderType != 12 && x.OrderType != 9 && x.OrderType != 5 && x.OrderType != 8 && x.OrderType != 10);
            //ViewBag.OrderTypes = OrderTypes.Where(x => x.OrderType == 2 && x.OrderType == 7);

            return View(oDUOrderStatuss);
        }
        public ActionResult ViewDUOrderStatusLoan(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptDUOrderStatus> oDUOrderStatuss = new List<RptDUOrderStatus>();
            ViewBag.BUID = buid;
            ViewBag.LSDUReq = true;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = OrderTypes.Where(x => x.OrderType != 11 && x.OrderType != 12 && x.OrderType != 9 && x.OrderType != 5 && x.OrderType != 2 && x.OrderType != 7 && x.OrderType != 10);
            //ViewBag.OrderTypes = OrderTypes.Where(x => x.OrderType == 2 && x.OrderType == 7);
            return View(oDUOrderStatuss);
        }
        [HttpPost]
        public JsonResult SearchDUOrderStatus(RptDUOrderStatus oRptDUOrderStatus)
        {
            List<RptDUOrderStatus> oRPCAs = new List<RptDUOrderStatus>();
            string ProductIDs = oRptDUOrderStatus.ErrorMessage;
            DateTime StartTime = Convert.ToDateTime(oRptDUOrderStatus.Startdate);
            DateTime EndTime = Convert.ToDateTime(oRptDUOrderStatus.Enddate);
            int nReportType = Convert.ToInt32(oRptDUOrderStatus.ReportLayout);
            int BUID = Convert.ToInt32(oRptDUOrderStatus.BUID);


            oRPCAs = GetsDUOrderStatus(ProductIDs,nReportType, StartTime, EndTime,BUID);
            _oRptDUOrderStatuss = oRPCAs.GroupBy(x => new { x.ProductCategoryID, x.CategoryName }, (key, grp) =>
                                   new RptDUOrderStatus
                                   {
                                       ProductCategoryID = key.ProductCategoryID,
                                       ProductID = 0,
                                       ProductName = "Category",
                                       CategoryName = key.CategoryName,
                                       DyeingOrderID = 0,                                    
                                       ProductBaseID = 0,                                 
                                       ProductBaseName = "", 
                                        OrderQty = 0,
                                        SRSQty = 0,
                                        SRMQty = 0, 
                                        OrderType = 0, 
                                        QtyQC = 0,
                                        QtyDC = 0,
                                        QtyRC = 0,
                                       QtyDyeing = 0,
                                       Qty_Hydro = 0,
                                       Qty_Drier = 0,
                                       Qty_WQC = 0,
                                   }).ToList();

            _oRptDUOrderStatuss.ForEach(x => oRPCAs.Add(x));
            oRPCAs = oRPCAs.OrderBy(o => o.CategoryName).ThenBy(a => a.ProductID).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRPCAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public List<RptDUOrderStatus> GetsDUOrderStatus(string ProductIDs,int nReportType, DateTime StartTime, DateTime EndTime, int BUID)
        {
            List<RptDUOrderStatus> oRPCAs = new List<RptDUOrderStatus>();
            RptDUOrderStatus oRPCA = new RptDUOrderStatus();
            try
            {
                if (ProductIDs =="") { throw new Exception("Please select searching criteria."); }              
                oRPCAs = RptDUOrderStatus.MailContent(ProductIDs,nReportType, StartTime, EndTime,BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRPCAs.Count() <= 0) { throw new Exception("No Information Found."); }
            }
            catch (Exception ex)
            {
                oRPCA.ErrorMessage = ex.Message;
                oRPCAs = new List<RptDUOrderStatus>();
                oRPCAs.Add(oRPCA);
            }

            return oRPCAs;
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriterias(List<RptDUOrderStatus> RptDUOrderStatusList)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, RptDUOrderStatusList);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchDUOrderStatusPrint(string sTempValue, double nts)
        {
            _oRptDUOrderStatuss = new List<RptDUOrderStatus>();
            _oRptDUOrderStatuss = (List<RptDUOrderStatus>)Session[SessionInfo.ParamObj];
            DateTime StartTime = Convert.ToDateTime(sTempValue.Split('~')[1]);
            DateTime EndTime = Convert.ToDateTime(sTempValue.Split('~')[2]);
            int nReportType = Convert.ToInt32(sTempValue.Split('~')[6]);
            LSDUReq = Convert.ToBoolean(sTempValue.Split('~')[7]);

            List<RptDUOrderStatus> _oTempRptDUOrderStatusList1 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList2 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList3 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList4 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oRptDUOrderStatusList = new List<RptDUOrderStatus>();
            DataSet _oDataSet2 = new DataSet();
            DataSet _oDataSet3 = new DataSet();
            DataSet _oDataSet4 = new DataSet();
            if (nReportType == 0 || nReportType == 1)// Product wise 
            {
                string sSql = GetSQLForProductWiseSearch(sTempValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForProductWiseSearch(sTempValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForProductWiseSearch(sTempValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _sSql = GetSQLForProductWiseSearch(sTempValue, 4);
                _oDataSet4 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                    oRptDUOrderStatus.ProductCode = (oRow["ProductCode"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCode"]);
                    oRptDUOrderStatus.CategoryName = (oRow["ProductCategoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCategoryName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    //oRptDUOrderStatus.SRSQty = (oRow["SRSQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRSQty"]);
                    //oRptDUOrderStatus.SRMQty = (oRow["SRMQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRMQty"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    _oRptDUOrderStatusList.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["OrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["OrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet4.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.SRSQty = (oRow["SRSQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRSQty"]);
                    oRptDUOrderStatus.SRMQty = (oRow["SRMQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRMQty"]);
                    _oTempRptDUOrderStatusList4.Add(oRptDUOrderStatus);
                    var a = _oTempRptDUOrderStatusList4.Exists(x => x.ProductID == oRptDUOrderStatus.ProductID);

                }

                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oRptDUOrderStatusList.Count > 0)
                {
                    foreach (RptDUOrderStatus oItem in _oRptDUOrderStatusList)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.FirstOrDefault().Qty_RS;
                            oItem.QtyQC = _oTempList.FirstOrDefault().QtyQC;
                            oItem.QtyDC = _oTempList.FirstOrDefault().QtyDC;
                            oItem.QtyRC = _oTempList.FirstOrDefault().QtyRC;
                            oItem.YarnOut = _oTempList.FirstOrDefault().YarnOut;
                            oItem.WastageQty = _oTempList.FirstOrDefault().WastageQty;
                            oItem.RecycleQty = _oTempList.FirstOrDefault().RecycleQty;
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.FirstOrDefault().QtyDyeing;
                            oItem.Qty_Hydro = _oTempList.FirstOrDefault().Qty_Hydro;
                            oItem.Qty_Drier = _oTempList.FirstOrDefault().Qty_Drier;
                            oItem.Qty_WQC = _oTempList.FirstOrDefault().Qty_WQC;
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList4.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.SRSQty = _oTempList.FirstOrDefault().SRSQty;
                            oItem.SRMQty = _oTempList.FirstOrDefault().SRMQty;
                        }

                        //_oRptDUOrderStatusList.Add(oItem);
                    }

                }
            }

            if (nReportType == 2)/// Customer Wise
            {
                string sSql = GetSQLForCustomerWiseSearch(sTempValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForCustomerWiseSearch(sTempValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForCustomerWiseSearch(sTempValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.ContractorName = (oRow["ContractorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ContractorName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    oRptDUOrderStatus.SRSQty = (oRow["QtySRS"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRS"]);
                    oRptDUOrderStatus.SRMQty = (oRow["QtySRM"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRM"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    _oTempRptDUOrderStatusList1.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }
                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList2.Count && _oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList3.Count)
                {
                    foreach (RptDUOrderStatus oItem in _oTempRptDUOrderStatusList1)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.FirstOrDefault().Qty_RS;
                            oItem.QtyQC = _oTempList.FirstOrDefault().QtyQC;
                            oItem.QtyDC = _oTempList.FirstOrDefault().QtyDC;
                            oItem.YarnOut = _oTempList.FirstOrDefault().YarnOut;
                            oItem.QtyRC = _oTempList.FirstOrDefault().QtyRC;
                            oItem.WastageQty = _oTempList.FirstOrDefault().WastageQty;
                            oItem.RecycleQty = _oTempList.FirstOrDefault().RecycleQty;
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.FirstOrDefault().QtyDyeing;
                            oItem.Qty_Hydro = _oTempList.FirstOrDefault().Qty_Hydro;
                            oItem.Qty_Drier = _oTempList.FirstOrDefault().Qty_Drier;
                            oItem.Qty_WQC = _oTempList.FirstOrDefault().Qty_WQC;
                        }
                        _oRptDUOrderStatusList.Add(oItem);
                    }
                }
            }
            string sReprotPrintStatus = "";
            string sDateRange = StartTime.ToString("dd MMM yyyy") + " To " + EndTime.ToString("dd MMM yyyy");
            try
            {
                if (sTempValue != "" && nReportType>0)
                {
                    if (nReportType == 1)
                    {
                        sReprotPrintStatus = "Product Wise Order Status Print";
                    }
                    if (nReportType == 2)
                    {
                        sReprotPrintStatus = "Customer Wise Order Status Print";
                    }                  
                }
                else
                {
                    throw new Exception("Nothing to Print. Print Type was Not Select");
                }               
            }
            catch (Exception ex)
            {
                oRptDUOrderStatus = new RptDUOrderStatus();
                oRptDUOrderStatus.ErrorMessage = ex.Message;
            }
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);            
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDUOrderStatus oReport = new rptDUOrderStatus();
            byte[] abytes = oReport.PrepareReport(_oRptDUOrderStatusList, oCompany, sDateRange, sReprotPrintStatus, nReportType, LSDUReq);
            return File(abytes, "application/pdf");

        }
        public void FullPreviewInExcel(string sTempValue, double nts)
        {
            _oRptDUOrderStatuss = new List<RptDUOrderStatus>();
            _oRptDUOrderStatuss = (List<RptDUOrderStatus>)Session[SessionInfo.ParamObj];
            DateTime StartTime = Convert.ToDateTime(sTempValue.Split('~')[1]);
            DateTime EndTime = Convert.ToDateTime(sTempValue.Split('~')[2]);
            int nReportType = Convert.ToInt32(sTempValue.Split('~')[6]);
            LSDUReq = Convert.ToBoolean(sTempValue.Split('~')[7]);

            List<RptDUOrderStatus> _oTempRptDUOrderStatusList1 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList2 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList3 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList4 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oRptDUOrderStatusList = new List<RptDUOrderStatus>();
            DataSet _oDataSet2 = new DataSet();
            DataSet _oDataSet3 = new DataSet();
            DataSet _oDataSet4 = new DataSet();
            if (nReportType == 0 || nReportType == 1)// Product wise 
            {
                string sSql = GetSQLForProductWiseSearch(sTempValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForProductWiseSearch(sTempValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForProductWiseSearch(sTempValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _sSql = GetSQLForProductWiseSearch(sTempValue, 4);
                _oDataSet4 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                    oRptDUOrderStatus.ProductCode = (oRow["ProductCode"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCode"]);
                    oRptDUOrderStatus.CategoryName = (oRow["ProductCategoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCategoryName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    //oRptDUOrderStatus.SRSQty = (oRow["SRSQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRSQty"]);
                    //oRptDUOrderStatus.SRMQty = (oRow["SRMQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRMQty"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    _oRptDUOrderStatusList.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["OrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["OrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet4.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.SRSQty = (oRow["SRSQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRSQty"]);
                    oRptDUOrderStatus.SRMQty = (oRow["SRMQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRMQty"]);
                    _oTempRptDUOrderStatusList4.Add(oRptDUOrderStatus);
                    var a = _oTempRptDUOrderStatusList4.Exists(x => x.ProductID == oRptDUOrderStatus.ProductID);

                }

                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oRptDUOrderStatusList.Count > 0)
                {
                    foreach (RptDUOrderStatus oItem in _oRptDUOrderStatusList)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.FirstOrDefault().Qty_RS;
                            oItem.QtyQC = _oTempList.FirstOrDefault().QtyQC;
                            oItem.QtyDC = _oTempList.FirstOrDefault().QtyDC;
                            oItem.QtyRC = _oTempList.FirstOrDefault().QtyRC;
                            oItem.YarnOut = _oTempList.FirstOrDefault().YarnOut;
                            oItem.WastageQty = _oTempList.FirstOrDefault().WastageQty;
                            oItem.RecycleQty = _oTempList.FirstOrDefault().RecycleQty;
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.FirstOrDefault().QtyDyeing;
                            oItem.Qty_Hydro = _oTempList.FirstOrDefault().Qty_Hydro;
                            oItem.Qty_Drier = _oTempList.FirstOrDefault().Qty_Drier;
                            oItem.Qty_WQC = _oTempList.FirstOrDefault().Qty_WQC;
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList4.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.SRSQty = _oTempList.FirstOrDefault().SRSQty;
                            oItem.SRMQty = _oTempList.FirstOrDefault().SRMQty;
                        }

                        //_oRptDUOrderStatusList.Add(oItem);
                    }

                }
            }

            if (nReportType == 2)/// Customer Wise
            {
                string sSql = GetSQLForCustomerWiseSearch(sTempValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForCustomerWiseSearch(sTempValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForCustomerWiseSearch(sTempValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.ContractorName = (oRow["ContractorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ContractorName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    oRptDUOrderStatus.SRSQty = (oRow["QtySRS"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRS"]);
                    oRptDUOrderStatus.SRMQty = (oRow["QtySRM"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRM"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    _oTempRptDUOrderStatusList1.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }
                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList2.Count && _oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList3.Count)
                {
                    foreach (RptDUOrderStatus oItem in _oTempRptDUOrderStatusList1)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.FirstOrDefault().Qty_RS;
                            oItem.QtyQC = _oTempList.FirstOrDefault().QtyQC;
                            oItem.QtyDC = _oTempList.FirstOrDefault().QtyDC;
                            oItem.YarnOut = _oTempList.FirstOrDefault().YarnOut;
                            oItem.QtyRC = _oTempList.FirstOrDefault().QtyRC;
                            oItem.WastageQty = _oTempList.FirstOrDefault().WastageQty;
                            oItem.RecycleQty = _oTempList.FirstOrDefault().RecycleQty;
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.FirstOrDefault().QtyDyeing;
                            oItem.Qty_Hydro = _oTempList.FirstOrDefault().Qty_Hydro;
                            oItem.Qty_Drier = _oTempList.FirstOrDefault().Qty_Drier;
                            oItem.Qty_WQC = _oTempList.FirstOrDefault().Qty_WQC;
                        }
                        _oRptDUOrderStatusList.Add(oItem);
                    }
                }
            }    
            //write here       
            try
            {
                if (_oRptDUOrderStatusList.Count > 0)
                {
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);            
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int nTotalCol = 0;
                    int nCount = 0;
                    int colIndex = 2;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("DU Order Status");
                        sheet.Name = "DU Order Status Preview";
                        sheet.Column(colIndex++).Width = 10; //SL   
                        sheet.Column(colIndex++).Width = 20; //Product Code  
                        sheet.Column(colIndex++).Width = 30; //Product Name                  
                        sheet.Column(colIndex++).Width = 25; //Order Type
                        sheet.Column(colIndex++).Width = 20; //Order Qty
                        sheet.Column(colIndex++).Width = 20; //SRS Qty
                        sheet.Column(colIndex++).Width = 20; //SQM Qty
                        sheet.Column(colIndex++).Width = 20; //Pending SRS                  
                        sheet.Column(colIndex++).Width = 20; //Yarn Out
                        sheet.Column(colIndex++).Width = 20; //pENDING Yarn Out
                        sheet.Column(colIndex++).Width = 20; //IN MACHINE
                        sheet.Column(colIndex++).Width = 20; //IN Hydro
                        sheet.Column(colIndex++).Width = 20; //IN_Drier
                        sheet.Column(colIndex++).Width = 20; // IN WAIT FOR QC
                        sheet.Column(colIndex++).Width = 20; //WIP
                        sheet.Column(colIndex++).Width = 20; //Qty QC
                        sheet.Column(colIndex++).Width = 20; //Recycle Qty
                        sheet.Column(colIndex++).Width = 20; //WASTAGE Qty                  
                        sheet.Column(colIndex++).Width = 20; // Delivery Qty
                        sheet.Column(colIndex++).Width = 20; // Return Qty
                        sheet.Column(colIndex++).Width = 20; //Pending Delivery Qty

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "sReprotPrintStatus"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion                    
                           #region DATA : PRODUCT WISE 
                            int nSL = 1;
                            colIndex = 2;
                            if (nReportType == 1)
                            {
                                var RptDUOrderStatuss = _oRptDUOrderStatusList.GroupBy(x => new { x.CategoryName })
                                                              .OrderBy(x => x.Key.CategoryName)
                                                              .Select(x => new
                                                              {
                                                                  CategoryName = x.Key.CategoryName,
                                                                  _RptDUOrderStatusList = x.OrderBy(c => c.ProductID),
                                                                  SubTotalOrderQty = x.Sum(y => y.OrderQty),
                                                                  SubTotalSRSQty = x.Sum(y => y.SRSQty),
                                                                  SubTotalSRMQty = x.Sum(y => y.SRMQty),
                                                                  SubTotalYarnOut = x.Sum(y => y.YarnOut),
                                                                  PendingSubTotalYarnOut = x.Sum(y => y.PendingYarnOutST),
                                                                  SubTotalPendingSRS = x.Sum(y => y.PendingSRSST),
                                                                  SubTotalQtyQC = x.Sum(y => y.QtyQC),
                                                                  SubTotalQtyDC = x.Sum(y => y.QtyDC),
                                                                  SubTotalQtyRC = x.Sum(y => y.QtyRC),
                                                                  subTotalWIP = x.Sum(y => y.WIPST),
                                                                  SubTotalDyeingQty = x.Sum(y => y.QtyDyeing),
                                                                  SubTotalQtyHydro = x.Sum(y => y.Qty_Hydro),
                                                                  SubTotalQtyDrier = x.Sum(y => y.Qty_Drier),
                                                                  SubTotalQtyWQC = x.Sum(y => y.Qty_WQC),
                                                                  SubTotalRecycleQty = x.Sum(y => y.RecycleQty),
                                                                  SubTotalWastageQty = x.Sum(y => y.WastageQty),
                                                                  PendingDeliverySubTotal = x.Sum(y => y.PendingDeliveryST),
                                                              });

                                foreach (var oData in RptDUOrderStatuss)
                                {
                                    nSL = 1;
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 22]; cell.Merge = true; cell.Value = "Category Name: " + oData.CategoryName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    rowIndex++;
                                    #region Column Name : Product Wise
                                    if (nReportType == 1)
                                    {
                                        colIndex = 2;
                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        if (LSDUReq == false)
                                        {
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                            cell = sheet.Cells[rowIndex, colIndex++, rowIndex, colIndex + 2]; cell.Merge = true; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            colIndex++;
                                            cell = sheet.Cells[rowIndex, colIndex++, rowIndex, colIndex]; cell.Merge = true; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            colIndex++;
                                        }
                                        else
                                        {
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        }

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        if (LSDUReq == true)
                                        {
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SRS Qty"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SRM Qty"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending SRS"; cell.Style.Font.Bold = true;
                                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        }
                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Out"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending Yarn Out"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Dyeing"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Hydro"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Drier"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty WQC"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "WIP"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty QC"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Recycle Qty"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Wastage Qty"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty DC"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty RC"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending Delivery Qty"; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        rowIndex++;
                                    #endregion
                                    foreach (var oItem in oData._RptDUOrderStatusList)
                                    {
                                        colIndex = 2;
                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                        if (LSDUReq == false)
                                        {
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductCode; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                            //COLSPAN = 3
                                            cell = sheet.Cells[rowIndex, colIndex++,rowIndex,colIndex+2]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            colIndex++;
                                            //COLSPAN = 2
                                            cell = sheet.Cells[rowIndex, colIndex++,rowIndex,colIndex]; cell.Value = oItem.OrderName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            colIndex++;
                                        }

                                      else
                                        {
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductCode; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                        }
                                       

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        if (LSDUReq == true)
                                        {
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SRSQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SRMQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingSRSST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                        }
                                        
                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.YarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingYarnOutST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDyeing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_Hydro; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_Drier; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_WQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WIPST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RecycleQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WastageQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyRC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingDeliveryST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                              
                                        nSL++;
                                        rowIndex++;
                                    }

                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oData.SubTotalOrderQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oData.SubTotalSRSQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oData.SubTotalSRMQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oData.SubTotalPendingSRS; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oData.SubTotalYarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oData.PendingSubTotalYarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Value = oData.SubTotalDyeingQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 13]; cell.Value = oData.SubTotalQtyHydro; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 14]; cell.Value = oData.SubTotalQtyDrier; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 15]; cell.Value = oData.SubTotalQtyWQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 16]; cell.Value = oData.subTotalWIP; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 17]; cell.Value = oData.SubTotalQtyQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 18]; cell.Value = oData.SubTotalRecycleQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 19]; cell.Value = oData.SubTotalWastageQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 20]; cell.Value = oData.SubTotalQtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 21]; cell.Value = oData.SubTotalQtyRC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, 22]; cell.Value = oData.PendingDeliverySubTotal; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    rowIndex++;
                                }
                            }
                            #endregion
                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=RPT_DUOrderStatus.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();

                        }

                    }
                
                }
                else
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
            }
            catch (Exception ex)
            {
                _oRptDUOrderStatusList = new List<RptDUOrderStatus>();
                oRptDUOrderStatus = new RptDUOrderStatus();
                oRptDUOrderStatus.ErrorMessage = ex.Message;
                _oRptDUOrderStatusList.Add(oRptDUOrderStatus);
            }


        }
        [HttpGet]
        public void SearchDUOrderStatusExcel(string sTempValue, double nts)
        {
            _oRptDUOrderStatuss = new List<RptDUOrderStatus>();
            _oRptDUOrderStatuss = (List<RptDUOrderStatus>)Session[SessionInfo.ParamObj];
            DateTime StartTime = Convert.ToDateTime(sTempValue.Split('~')[0]);
            DateTime EndTime = Convert.ToDateTime(sTempValue.Split('~')[1]);
            int nReportType = Convert.ToInt32(sTempValue.Split('~')[2]);
            int BUID = Convert.ToInt32(sTempValue.Split('~')[3]);
            string sReprotPrintStatus = "";
            string sDateRange = StartTime + " ~ " + EndTime;
            try
            {
                if (sTempValue != "" && nReportType > 0)
                {

                    if (nReportType == 1)
                    {
                        sReprotPrintStatus = "Product Wise Order Status Print";
                    }

                    if (nReportType == 2)
                    {
                        sReprotPrintStatus = "Customer Wise Order Status Print";
                    }

                }
                Company oCompany = new Company();            
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (_oRptDUOrderStatuss == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                int nTotalCol = 0;
                int nCount = 0;
                int colIndex = 2;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DU Order Status");
                    sheet.Name = "DU Order Status List";
                    sheet.Column(colIndex++).Width = 10; //SL   
                    sheet.Column(colIndex++).Width = 20; //Product Code  
                    sheet.Column(colIndex++).Width = 30; //Product Name                  
                    sheet.Column(colIndex++).Width = 25; //Order Type
                    sheet.Column(colIndex++).Width = 20; //Order Qty
                    sheet.Column(colIndex++).Width = 20; //SRS Qty
                    sheet.Column(colIndex++).Width = 20; //SQM Qty
                    sheet.Column(colIndex++).Width = 20; //Pending SRS                  
                    sheet.Column(colIndex++).Width = 20; //Yarn Out
                    sheet.Column(colIndex++).Width = 20; //pENDING Yarn Out
                    sheet.Column(colIndex++).Width = 20; //IN MACHINE
                    sheet.Column(colIndex++).Width = 20; //IN Hydro
                    sheet.Column(colIndex++).Width = 20; //IN_Drier
                    sheet.Column(colIndex++).Width = 20; // IN WAIT FOR QC
                    sheet.Column(colIndex++).Width = 20; //WIP
                    sheet.Column(colIndex++).Width = 20; //Qty QC
                    sheet.Column(colIndex++).Width = 20; //rECYCLE
                    sheet.Column(colIndex++).Width = 20; //WASTAGE                  
                    sheet.Column(colIndex++).Width = 20; //Qty DC
                    sheet.Column(colIndex++).Width = 20; //Qty RC
                    sheet.Column(colIndex++).Width = 20; //Pending Delivery
               
                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = sReprotPrintStatus; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Header 1
                    if (nReportType == 1)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SRS Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SRM Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending SRS"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Out"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending Yarn Out"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Dyeing"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Hydro"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Drier"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty WQC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "WIP"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty QC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Recycle Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Wastage Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty DC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty RC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending Delivery Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;

                    }
                    if (nReportType == 2)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                     
                        cell = sheet.Cells[rowIndex, colIndex++,rowIndex,colIndex]; cell.Merge = true; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SRS Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SRM Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending SRS"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Out"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending Yarn Out"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Dyeing"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Hydro"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Drier"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty WQC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "WIP"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty QC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Recycle Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Wastage Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty DC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty RC"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pending Delivery Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;

                    }                   
                    #endregion
                    #region Report Body
                    int nSL = 1;
                    colIndex = 2;
                    if (nReportType == 1)
                    {
                        var RptDUOrderStatuss = _oRptDUOrderStatuss.GroupBy(x => new { x.CategoryName })
                                                      .OrderBy(x => x.Key.CategoryName)
                                                      .Select(x => new
                                                      {
                                                          CategoryName = x.Key.CategoryName,
                                                          _RptDUOrderStatusList = x.OrderBy(c => c.ProductID),
                                                          SubTotalOrderQty = x.Sum(y => y.OrderQty),
                                                          SubTotalSRSQty = x.Sum(y => y.SRSQty),
                                                          SubTotalSRMQty = x.Sum(y => y.SRMQty),
                                                          SubTotalYarnOut = x.Sum(y => y.YarnOut),
                                                          PendingSubTotalYarnOut = x.Sum(y => y.PendingYarnOutST),
                                                          SubTotalPendingSRS = x.Sum(y => y.PendingSRSST),
                                                          SubTotalQtyQC = x.Sum(y => y.QtyQC),
                                                          SubTotalQtyDC = x.Sum(y => y.QtyDC),
                                                          SubTotalQtyRC = x.Sum(y => y.QtyRC),
                                                          subTotalWIP = x.Sum(y => y.WIPST),
                                                          SubTotalDyeingQty = x.Sum(y => y.QtyDyeing),
                                                          SubTotalQtyHydro = x.Sum(y => y.Qty_Hydro),
                                                          SubTotalQtyDrier = x.Sum(y => y.Qty_Drier),
                                                          SubTotalQtyWQC = x.Sum(y => y.Qty_WQC),
                                                          SubTotalRecycleQty = x.Sum(y => y.RecycleQty),
                                                          SubTotalWastageQty = x.Sum(y => y.WastageQty),
                                                          PendingDeliverySubTotal = x.Sum(y => y.PendingDeliveryST),
                                                      });

                        foreach (var oData in RptDUOrderStatuss)
                        {
                            nSL = 1;
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 22]; cell.Merge = true; cell.Value = "Category Name: " + oData.CategoryName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex++;
                            foreach (var oItem in oData._RptDUOrderStatusList)
                            {
                                colIndex = 2;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductCode; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SRSQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SRMQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingSRSST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.YarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingYarnOutST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDyeing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_Hydro; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_Drier; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_WQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WIPST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RecycleQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WastageQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyRC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingDeliveryST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                              
                                nSL++;
                                rowIndex++;
                            }

                            cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oData.SubTotalOrderQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oData.SubTotalSRSQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oData.SubTotalSRMQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oData.SubTotalPendingSRS; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = oData.SubTotalYarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = oData.PendingSubTotalYarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = oData.SubTotalDyeingQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = oData.SubTotalQtyHydro; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = oData.SubTotalQtyDrier; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = oData.SubTotalQtyWQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 16]; cell.Value = oData.subTotalWIP; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 17]; cell.Value = oData.SubTotalQtyQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 18]; cell.Value = oData.SubTotalRecycleQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 19]; cell.Value = oData.SubTotalWastageQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 20]; cell.Value = oData.SubTotalQtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 21]; cell.Value = oData.SubTotalQtyRC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 22]; cell.Value = oData.PendingDeliverySubTotal; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rowIndex++;
                        }
                    }
                    if (nReportType == 2)
                    {
                        var RptDUOrderStatuss = _oRptDUOrderStatuss.GroupBy(x => new { x.ContractorName })
                                                    .OrderBy(x => x.Key.ContractorName)
                                                    .Select(x => new
                                                    {
                                                        contractorName = x.Key.ContractorName,
                                                        _RptDUOrderStatusList = x.OrderBy(c => c.ContractorID),
                                                        SubTotalOrderQty = x.Sum(y => y.OrderQty),
                                                        SubTotalSRSQty = x.Sum(y => y.SRSQty),
                                                        SubTotalSRMQty = x.Sum(y => y.SRMQty),
                                                        SubTotalYarnOut = x.Sum(y => y.YarnOut),
                                                        PendingSubTotalYarnOut = x.Sum(y => y.PendingYarnOutST),
                                                        SubTotalPendingSRS = x.Sum(y => y.PendingSRSST),
                                                        SubTotalQtyQC = x.Sum(y => y.QtyQC),
                                                        SubTotalQtyDC = x.Sum(y => y.QtyDC),
                                                        SubTotalQtyRC = x.Sum(y => y.QtyRC),
                                                        subTotalWIP = x.Sum(y => y.WIPST),
                                                        SubTotalDyeingQty = x.Sum(y => y.QtyDyeing),
                                                        SubTotalQtyHydro = x.Sum(y => y.Qty_Hydro),
                                                        SubTotalQtyDrier = x.Sum(y => y.Qty_Drier),
                                                        SubTotalQtyWQC = x.Sum(y => y.Qty_WQC),
                                                        SubTotalRecycleQty = x.Sum(y => y.RecycleQty),
                                                        SubTotalWastageQty = x.Sum(y => y.WastageQty),
                                                        PendingDeliverySubTotal = x.Sum(y => y.PendingDeliveryST),
                                                    });
                        foreach (var oData in RptDUOrderStatuss)
                        {
                            nSL = 1;
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 21]; cell.Merge = true; cell.Value = "Customer: " + oData.contractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex++;
                            foreach (var oItem in oData._RptDUOrderStatusList)
                            {
                                colIndex = 2;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++,rowIndex,colIndex];cell.Merge = true; cell.Value = oItem.OrderName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                colIndex++;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SRSQty; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SRMQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingSRSST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.YarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingYarnOutST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDyeing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_Hydro; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_Drier; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_WQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WIPST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RecycleQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WastageQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyRC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PendingDeliveryST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                nSL++;
                                rowIndex++;
                            }

                            cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oData.SubTotalOrderQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oData.SubTotalSRSQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oData.SubTotalSRMQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oData.SubTotalPendingSRS; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oData.SubTotalYarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = oData.PendingSubTotalYarnOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = oData.SubTotalDyeingQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = oData.SubTotalQtyHydro; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = oData.SubTotalQtyDrier; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = oData.SubTotalQtyWQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = oData.subTotalWIP; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 16]; cell.Value = oData.SubTotalQtyQC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 17]; cell.Value = oData.SubTotalRecycleQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 18]; cell.Value = oData.SubTotalWastageQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 19]; cell.Value = oData.SubTotalQtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 20]; cell.Value = oData.SubTotalQtyRC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 21]; cell.Value = oData.PendingDeliverySubTotal; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rowIndex++;
                        }
                    }
                   

                   
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=RPT_DUOrderStatus.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();

                }

            }
            catch (Exception ex)
            {
                _oRptDUOrderStatuss = new List<RptDUOrderStatus>();
                oRptDUOrderStatus = new RptDUOrderStatus();
                oRptDUOrderStatus.ErrorMessage = ex.Message;
                _oRptDUOrderStatuss.Add(oRptDUOrderStatus);
            }
                    #endregion
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

        #region

        [HttpPost]
        public ActionResult AdvSearchDUOrderStatus(string sValue)
        {
            int nReportType = Convert.ToInt32(sValue.Split('~')[6]);
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList1 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList2 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList3 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList4 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList5 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oRptDUOrderStatusList = new List<RptDUOrderStatus>();
            DataSet _oDataSet2 = new DataSet();
            DataSet _oDataSet3 = new DataSet();
            DataSet _oDataSet4 = new DataSet();
            DataSet _oDataSet5 = new DataSet();

            #region Product wise
            if (nReportType == 0 || nReportType == 1)// Product wise 
            {
                string sSql = GetSQLForProductWiseSearch(sValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForProductWiseSearch(sValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForProductWiseSearch(sValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSql = GetSQLForProductWiseSearch(sValue, 4);
                 _oDataSet4 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSql = GetSQLForProductWiseSearch(sValue, 5);
                 _oDataSet5 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                    oRptDUOrderStatus.ProductCode = (oRow["ProductCode"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCode"]);
                    oRptDUOrderStatus.CategoryName = (oRow["ProductCategoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCategoryName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    _oRptDUOrderStatusList.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["OrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["OrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet4.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.SRSQty = (oRow["SRSQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRSQty"]);
                    oRptDUOrderStatus.SRMQty = (oRow["SRMQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRMQty"]);
                    //oRptDUOrderStatus.YarnReceive = (oRow["YarnReceive"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnReceive"]);
                    _oTempRptDUOrderStatusList4.Add(oRptDUOrderStatus);
                    var a = _oTempRptDUOrderStatusList4.Exists(x => x.ProductID == oRptDUOrderStatus.ProductID);

                }
                foreach (DataRow oRow in _oDataSet5.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnReceive = (oRow["YarnReceive"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnReceive"]);
                    _oTempRptDUOrderStatusList5.Add(oRptDUOrderStatus);
                    //var a = _oTempRptDUOrderStatusList4.Exists(x => x.ProductID == oRptDUOrderStatus.ProductID);
                }

                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oRptDUOrderStatusList.Count > 0)
                {
                    foreach (RptDUOrderStatus oItem in _oRptDUOrderStatusList)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.Sum(x => x.Qty_RS);
                            oItem.QtyQC = _oTempList.Sum(x => x.QtyQC);
                            oItem.QtyDC = _oTempList.Sum(x => x.QtyDC);
                            oItem.QtyRC = _oTempList.Sum(x => x.QtyRC);
                            oItem.YarnOut = _oTempList.Sum(x => x.YarnOut);
                            oItem.WastageQty = _oTempList.Sum(x => x.WastageQty);
                            oItem.RecycleQty = _oTempList.Sum(x => x.RecycleQty);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.Sum(x => x.QtyDyeing);
                            oItem.Qty_Hydro = _oTempList.Sum(x => x.Qty_Hydro);
                            oItem.Qty_Drier = _oTempList.Sum(x => x.Qty_Drier);
                            oItem.Qty_WQC = _oTempList.Sum(x => x.Qty_WQC);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList4.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.SRSQty = _oTempList.Sum(x => x.SRSQty);
                            oItem.SRMQty = _oTempList.Sum(x => x.SRMQty);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList5.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.YarnReceive = _oTempList.Sum(x => x.YarnReceive);
                        }
                        //_oRptDUOrderStatusList.Add(oItem);
                    }

                }
            }
            #endregion

            #region Customer Wise
            if (nReportType == 2)/// Customer Wise
            {
                string sSql = GetSQLForCustomerWiseSearch(sValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForCustomerWiseSearch(sValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForCustomerWiseSearch(sValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSql = GetSQLForCustomerWiseSearch(sValue, 4);
                _oDataSet4 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.ContractorName = (oRow["ContractorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ContractorName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    oRptDUOrderStatus.SRSQty = (oRow["QtySRS"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRS"]);
                    oRptDUOrderStatus.SRMQty = (oRow["QtySRM"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRM"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    _oTempRptDUOrderStatusList1.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet4.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.YarnReceive = (oRow["YarnReceive"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnReceive"]);
                    _oTempRptDUOrderStatusList4.Add(oRptDUOrderStatus);
                }
                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList2.Count && _oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList3.Count)
                {
                    foreach (RptDUOrderStatus oItem in _oTempRptDUOrderStatusList1)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.Sum(x => x.Qty_RS);
                            oItem.QtyQC = _oTempList.Sum(x => x.QtyQC);
                            oItem.QtyDC = _oTempList.Sum(x => x.QtyDC);
                            oItem.YarnOut = _oTempList.Sum(x => x.YarnOut);
                            oItem.QtyRC = _oTempList.Sum(x => x.QtyRC);
                            oItem.WastageQty = _oTempList.Sum(x => x.WastageQty);
                            oItem.RecycleQty = _oTempList.Sum(x => x.RecycleQty);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.Sum(x => x.QtyDyeing);
                            oItem.Qty_Hydro = _oTempList.Sum(x => x.Qty_Hydro);
                            oItem.Qty_Drier = _oTempList.Sum(x => x.Qty_Drier);
                            oItem.Qty_WQC = _oTempList.Sum(x => x.Qty_WQC);
                        }
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList4.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.YarnReceive = _oTempList.Sum(x => x.YarnReceive);
                        }
                        _oRptDUOrderStatusList.Add(oItem);
                    }

                }
            }
            #endregion

            #region Order Wise
            if (nReportType == 3)// Order wise 
            {
                string sSql = GetSQLForOrderWiseSearch(sValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForOrderWiseSearch(sValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForOrderWiseSearch(sValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sSql = GetSQLForOrderWiseSearch(sValue, 4);
                _oDataSet4 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sSql = GetSQLForOrderWiseSearch(sValue, 5);
                _oDataSet5 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    oRptDUOrderStatus.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.ContractorName = (oRow["ContractorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ContractorName"]);
                    _oRptDUOrderStatusList.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.OrderType = (oRow["OrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["OrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet4.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.SRSQty = (oRow["SRSQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRSQty"]);
                    oRptDUOrderStatus.SRMQty = (oRow["SRMQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["SRMQty"]);
                    //oRptDUOrderStatus.YarnReceive = (oRow["YarnReceive"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnReceive"]);
                    _oTempRptDUOrderStatusList4.Add(oRptDUOrderStatus);
                    var a = _oTempRptDUOrderStatusList4.Exists(x => x.ProductID == oRptDUOrderStatus.ProductID);
                }

                foreach (DataRow oRow in _oDataSet5.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnReceive = (oRow["YarnReceive"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnReceive"]);
                    _oTempRptDUOrderStatusList5.Add(oRptDUOrderStatus);
                }

                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oRptDUOrderStatusList.Count > 0)
                {
                    foreach (RptDUOrderStatus oItem in _oRptDUOrderStatusList)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.Sum(x => x.Qty_RS);
                            oItem.QtyQC = _oTempList.Sum(x => x.QtyQC);
                            oItem.QtyDC = _oTempList.Sum(x => x.QtyDC);
                            oItem.QtyRC = _oTempList.Sum(x => x.QtyRC);
                            oItem.YarnOut = _oTempList.Sum(x => x.YarnOut);
                            oItem.WastageQty = _oTempList.Sum(x => x.WastageQty);
                            oItem.RecycleQty = _oTempList.Sum(x => x.RecycleQty);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.Sum(x => x.QtyDyeing);
                            oItem.Qty_Hydro = _oTempList.Sum(x => x.Qty_Hydro);
                            oItem.Qty_Drier = _oTempList.Sum(x => x.Qty_Drier);
                            oItem.Qty_WQC = _oTempList.Sum(x => x.Qty_WQC);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList4.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.SRSQty = _oTempList.Sum(x => x.SRSQty);
                            oItem.SRMQty = _oTempList.Sum(x => x.SRMQty);
                            //oItem.YarnReceive = _oTempList.Sum(x => x.YarnReceive);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList5.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.YarnReceive = _oTempList.Sum(x => x.YarnReceive);
                        }
                        //_oRptDUOrderStatusList.Add(oItem);
                    }

                }
            }
            #endregion

            //else if(_oTempRptDUOrderStatusList2.Count > _oTempRptDUOrderStatusList2.Count && _oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList3.Count)            
            var jsonResult = Json(_oRptDUOrderStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQLForProductWiseSearch(string sTempValue,int type)
        {
            EnumCompareOperator eOrderDate = (EnumCompareOperator)Convert.ToInt32(sTempValue.Split('~')[0]);
            DateTime dOrderStartDate = Convert.ToDateTime(sTempValue.Split('~')[1]);
            DateTime dOrderEndDate = Convert.ToDateTime(sTempValue.Split('~')[2]);
            var nOrderType = Convert.ToInt32(sTempValue.Split('~')[3]);
            var ProductIDs = Convert.ToString(sTempValue.Split('~')[4]);
            var CustomerIDs = Convert.ToString(sTempValue.Split('~')[5]);
            var nReportType = Convert.ToInt32(sTempValue.Split('~')[6]);

            string sReturn1 = "";
            string sReturn = "";

            if (type == 1)
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = "Select DyeingOrderType,DUOrderSetup.OrderName, DyeingOrderDetail.ProductID,ProductName,ProductCode,ProductBaseID,ProductCategoryName,Product.ProductCategoryID,SUM(Qty) as OrderQty   from DyeingOrderDetail "
                                  + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DyeingOrderDetail.DyeingOrderID"
                                   + " left join Product on Product.ProductID=DyeingOrderDetail.ProductID"
                                    + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID"
                                     + " left join DUOrderSetup on DUOrderSetup.OrderType = DyeingOrder.DyeingOrderType"
                                      + " where DyeingOrder.[Status]<>9 ";
              //+ "group by DyeingOrder.DyeingOrderType,OrderName,  DyeingOrderDetail.ProductID,ProductName,ProductCategoryName,ProductCode,ProductBaseID,Product.ProductCategoryID";

              //      sReturn1 = "Select DyeingOrderType,OrderName,ProductID, SUM(OrderQty) AS OrderQty,SUM(QtySRS) AS SRSQty,SUM(QtySRM) AS SRMQty,ProductName,ProductCode,ProductBaseID,ProductCategoryID,ProductCategoryName from ("
              //+ "Select DyeingOrderType,OrderName, ProductName,ProductCode,ProductBaseID,ProductCategoryID, ProductID,ProductCategoryName,OrderQty"
              //+ ",(select SUM(Qty) from DURequisitionDetail where DyeingOrderID=HH.DyeingOrderID and ProductID=HH.ProductID and DURequisitionID in (Select DURequisitionID from DURequisition where  DURequisition.RequisitionType=101) ) as QtySRS"
              //+ ",(select SUM(Qty) from DURequisitionDetail where DyeingOrderID=HH.DyeingOrderID and ProductID=HH.ProductID and DURequisitionID in (Select DURequisitionID from DURequisition where  DURequisition.RequisitionType=102) ) as QtySRM"
              //+ " from("
              //+ "Select DyeingOrderType,DUOrderSetup.OrderName,DyeingOrder.DyeingOrderID, DyeingOrderDetail.ProductID,ProductName,ProductCode,ProductBaseID,ProductCategoryName,Product.ProductCategoryID,SUM(Qty) as OrderQty from DyeingOrderDetail"
              //+ " left join DyeingOrder on DyeingOrder.DyeingOrderID=DyeingOrderDetail.DyeingOrderID"
              // + " left join Product on Product.ProductID=DyeingOrderDetail.ProductID"
              //  + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID"
              //   + " left join DUOrderSetup on DUOrderSetup.OrderType = DyeingOrder.DyeingOrderType"
              //  + " where  DyeingOrder.DyeingOrderID= 12172 and DyeingOrder.[Status]<>9";

                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrderDetail.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs!= "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {
                          
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "group by DyeingOrder.DyeingOrderType,OrderName,  DyeingOrderDetail.ProductID,ProductName,ProductCategoryName,ProductCode,ProductBaseID,Product.ProductCategoryID";
                }            

            }

            if (type == 2)
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = "Select DyeingOrder.DyeingOrderType, DOD.ProductID,SUM(Qty_RS) as YarnOut ,SUM(Qty_QC) as Qty_QC,SUM(QtyDC) as QtyDC,SUM(QtyRC) as QtyRC ,SUM(WastageQty) as WastageQty ,SUM(TT.RecycleQty) as RecycleQty "
                 + " from RouteSheetDO as TT"
                 + " left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                 + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID"
                 + "  where DyeingOrder.DyeingOrderType >0 AND DyeingOrder.[Status]<>9 ";
                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DOD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region CustomerID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {
                           
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   group by DyeingOrder.DyeingOrderType, DOD.ProductID";
                }

            }
            if (type == 3)
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = "Select GG.OrderType, GG.ProductID,SUM(QtyDyeing) as QtyDYeing,SUM(Qty_Hydro) as Qty_Hydro,SUM(Qty_Drier) as Qty_Drier,SUM(Qty_WQC) as Qty_WQC from ( "
                 + " Select DOD.DyeingOrderID, DOD.ProductID,Qty_RS as Qty_RS,Qty_QC,QtyDC,QtyRC as QtyRC"
                 + " ,Case when RS.RSState in (4,5,6,7) then Qty_RS else 0 end QtyDYeing"
                 + " ,Case when RS.RSState in (8,9) then Qty_RS else 0 end Qty_Hydro"
                 + " ,Case when RS.RSState in (10,11) then Qty_RS else 0 end Qty_Drier "
                  + " ,Case when RS.RSState in (12,13,14,15,16) then Qty_RS-Qty_QC else 0 end Qty_WQC "
                   + " ,DyeingOrder.DyeingOrderType as OrderType "
                    + "  from RouteSheetDO as TT "
                     + "  left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                      + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID "
                        + "  left join Routesheet as RS on RS.RoutesheetID=TT.RoutesheetID  "
                        + "  where TT.IsOut=1 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DOD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {
                         
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   ) as GG group by  GG.ProductID,GG.OrderType";
                }

            }
            if (type ==4)// DU equsation
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = " Select DyeingOrderType,ProductCategoryID,ProductCategoryName,ProductCode,ProductName, ProductID ,Sum(QtySRS) as SRSQty ,Sum(QtySRM) as SRMQty from ("
	                          + "Select Product.ProductName,Product.ProductCode,ProductCategory.ProductCategoryName,ProductCategory.ProductCategoryID, DyeingOrder.DyeingOrderType, ReqD.DyeingOrderID,ReqD.ProductID, Req.RequisitionType, Qty "
	                           + ", case when Req.RequisitionType in (0,101)  then ReqD.Qty else 0 end as QtySRS"
                               + " , case when Req.RequisitionType in (102)  then ReqD.Qty else 0 end as QtySRM"
	                         + " from DURequisitionDetail as ReqD"
	                        + "   left join DURequisition as Req on Req.DURequisitionID=ReqD.DURequisitionID"
	                         + "  left join DyeingOrder on DyeingOrder.DyeingOrderID=ReqD.DyeingOrderID"
	                         + "  left join Product on Product.ProductID=ReqD.ProductID"
                           + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID where DyeingOrder.DyeingOrderID>0 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND ReqD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region CustomerID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   ) as HH  group by DyeingOrderType, ProductID,ProductCategoryID,ProductCategoryName,ProductCode,ProductName";
                }

            }
            if (type == 5)// Yarn Receive
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = "  Select DyeingOrder.DyeingOrderType, DUPGD.ProductID, SUM(Qty) as YarnReceive "                    
                             + " from DUProGuideLineDetail as DUPGD  "
                            + "   left join DUProGuideLine as DUPG on DUPG.DUProGuideLineID=DUPGD.DUProGuideLineID  "
                             + "  left join DyeingOrder on DyeingOrder.DyeingOrderID=DUPG.DyeingOrderID  "
                             + "   left join Product on Product.ProductID=DUPGD.ProductID "
                           + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID Where DyeingOrder.DyeingOrderID>0 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + " AND DUPGD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + " group by DyeingOrderType, DUPGD.ProductID";
                }

            }
            
            return sReturn;
        }
        private string GetSQLForCustomerWiseSearch(string sTempValue, int type)
        {
            EnumCompareOperator eOrderDate = (EnumCompareOperator)Convert.ToInt32(sTempValue.Split('~')[0]);
            DateTime dOrderStartDate = Convert.ToDateTime(sTempValue.Split('~')[1]);
            DateTime dOrderEndDate = Convert.ToDateTime(sTempValue.Split('~')[2]);
            var nOrderType = Convert.ToInt32(sTempValue.Split('~')[3]);
            var ProductIDs = Convert.ToString(sTempValue.Split('~')[4]);
            var CustomerIDs = Convert.ToString(sTempValue.Split('~')[5]);
            var nReportType = Convert.ToInt32(sTempValue.Split('~')[6]);

            string sReturn1 = "";
            string sReturn = "";

            if (type == 1)
            {
                sReturn1 = "Select DyeingOrderID,DyeingOrderType,OrderName,ContractorID,ContractorName, SUM(OrderQty) OrderQty,SUM(QtySRS) QtySRS,SUM(QtySRM) QtySRM from ("
                 + " Select DyeingOrderID,DyeingOrderType,OrderName, ProductID, ContractorID,ContractorName,OrderQty"
                 + ",(select SUM(Qty) from DURequisitionDetail where DyeingOrderID=HH.DyeingOrderID and ProductID=HH.ProductID and DURequisitionID in (Select DURequisitionID from DURequisition where  DURequisition.RequisitionType=101) ) as QtySRS"
                 + ",(select SUM(Qty) from DURequisitionDetail where DyeingOrderID=HH.DyeingOrderID and ProductID=HH.ProductID and DURequisitionID in (Select DURequisitionID from DURequisition where  DURequisition.RequisitionType=102) ) as QtySRM"
                 + " from("
                 + "Select DyeingOrderType,DUOrderSetup.OrderName,DyeingOrder.DyeingOrderID,DyeingOrder.ContractorID,Contractor.Name as ContractorName, ProductID ,SUM(Qty) as OrderQty from DyeingOrderDetail"
                 + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DyeingOrderDetail.DyeingOrderID"
                  + " left join Contractor on Contractor.ContractorID = DyeingOrder.ContractorID"
                   + " left join DUOrderSetup on DUOrderSetup.OrderType = DyeingOrder.DyeingOrderType"
                 + " where DyeingOrder.[Status]<>9 and IsClose=0 ";
                    #region ProductID
                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrderDetail.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {
                        
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "  group by DyeingOrder.DyeingOrderType,OrderName, DyeingOrder.DyeingOrderID , ProductID,DyeingOrder.ContractorID,Contractor.Name) as HH ) as GG group by DyeingOrderType,OrderName,ProductID,ContractorID,DyeingOrderID,ContractorName";
                }

            

            if (type == 2)
            {
                sReturn1 = " Select DyeingOrder.DyeingOrderID, DyeingOrder.DyeingOrderType, DOD.ProductID,ContractorID,SUM(Qty_RS) as YarnOut ,SUM(Qty_QC) as Qty_QC,SUM(QtyDC) as QtyDC,SUM(QtyRC) as QtyRC,SUM(WastageQty) as WastageQty ,SUM(TT.RecycleQty) as RecycleQty "
                 + " from RouteSheetDO as TT"
                 + " left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                 + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID"
                 + "   where DyeingOrder.DyeingOrderType >0 AND DyeingOrder.[Status]<>9 and IsClose=0 ";
                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DOD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {
                           
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   group by DyeingOrder.DyeingOrderID, DyeingOrder.DyeingOrderType, DOD.ProductID,ContractorID";
                }

            
            if (type == 3)
            {

                sReturn1 = "Select GG.DyeingOrderID,GG.OrderType as DyeingOrderType, GG.ProductID,ContractorID,SUM(QtyDyeing) as QtyDYeing,SUM(Qty_Hydro) as Qty_Hydro,SUM(Qty_Drier) as Qty_Drier,SUM(Qty_WQC) as Qty_WQC from ( "
                 + " Select DOD.DyeingOrderID, DOD.ProductID,ContractorID,Qty_RS as Qty_RS,Qty_QC,QtyDC,QtyRC as QtyRC"
                 + " ,Case when RS.RSState in (4,5,6,7) then Qty_RS else 0 end QtyDYeing"
                 + " ,Case when RS.RSState in (8,9) then Qty_RS else 0 end Qty_Hydro"
                 + " ,Case when RS.RSState in (10,11) then Qty_RS else 0 end Qty_Drier "
                  + " ,Case when RS.RSState in (12,13,14,15,16) then Qty_RS-Qty_QC else 0 end Qty_WQC "
                   + " ,DyeingOrder.DyeingOrderType as OrderType "
                    + "  from RouteSheetDO as TT "
                     + "  left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                      + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID "
                        + "  left join Routesheet as RS on RS.RoutesheetID=TT.RoutesheetID  "
                        + "  where TT.IsOut=1 and isnull(TT.RSState,0)<>13 and  DyeingOrder.[Status]<>9 and IsClose=0  ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DOD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {
                           
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   ) as GG group by GG.DyeingOrderID,  GG.ProductID,GG.OrderType,ContractorID";
                
            }
            if (type == 4)// Yarn Receive
            {
                sReturn1 = "  Select DUPG.DyeingOrderID,DUPG.ContractorID,DyeingOrder.DyeingOrderType, DUPGD.ProductID, SUM(Qty) as YarnReceive "
                             + " from DUProGuideLineDetail as DUPGD  "
                            + "   left join DUProGuideLine as DUPG on DUPG.DUProGuideLineID=DUPGD.DUProGuideLineID  "
                             + "  left join DyeingOrder on DyeingOrder.DyeingOrderID=DUPG.DyeingOrderID  "
                             + "   left join Product on Product.ProductID=DUPGD.ProductID "
                           + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID Where DyeingOrder.DyeingOrderID>0 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + " AND DUPGD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + " group by DUPG.DyeingOrderID,DUPG.ContractorID,DyeingOrderType, DUPGD.ProductID";                
            }

            return sReturn;
        }
        private string GetSQLForOrderWiseSearch(string sTempValue, int type)
        {
            EnumCompareOperator eOrderDate = (EnumCompareOperator)Convert.ToInt32(sTempValue.Split('~')[0]);
            DateTime dOrderStartDate = Convert.ToDateTime(sTempValue.Split('~')[1]);
            DateTime dOrderEndDate = Convert.ToDateTime(sTempValue.Split('~')[2]);
            var nOrderType = Convert.ToInt32(sTempValue.Split('~')[3]);
            var ProductIDs = Convert.ToString(sTempValue.Split('~')[4]);
            var CustomerIDs = Convert.ToString(sTempValue.Split('~')[5]);
            var nReportType = Convert.ToInt32(sTempValue.Split('~')[6]);
            string sReturn1 = "";
            string sReturn = "";
            if (type == 1)
            {
                if (nReportType == 3)
                {
                    sReturn1 = "Select  DyeingOrder.DyeingOrderID,DyeingOrderType,DyeingOrder.OrderNo,DyeingOrder.ContractorID,Contractor.Name as ContractorName,DUOrderSetup.OrderName, DyeingOrderDetail.ProductID,SUM(Qty) as OrderQty from DyeingOrderDetail  "
                                  + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DyeingOrderDetail.DyeingOrderID "
                                   + " left join Product on Product.ProductID=DyeingOrderDetail.ProductID"
                                    + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID"
                                     + " left join DUOrderSetup on DUOrderSetup.OrderType = DyeingOrder.DyeingOrderType"
                                      + " left join Contractor on Contractor.ContractorID = DyeingOrder.ContractorID "
                                      + " where DyeingOrder.[Status]<>9";
                

                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrderDetail.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ContractorID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "group by  DyeingOrder.DyeingOrderID,DyeingOrder.DyeingOrderType,OrderName,DyeingOrder.OrderNo,DyeingOrder.ContractorID,Contractor.Name,DyeingOrderDetail.ProductID";
                }

            }

            if (type == 2)
            {
                if (nReportType == 3)
                {
                    sReturn1 = "Select  DyeingOrder.DyeingOrderID,DyeingOrder.DyeingOrderType, DOD.ProductID,SUM(Qty_RS) as YarnOut ,SUM(Qty_QC) as Qty_QC,SUM(QtyDC) as QtyDC,SUM(QtyRC) as QtyRC ,SUM(WastageQty) as WastageQty ,SUM(TT.RecycleQty) as RecycleQty "
                 + " from RouteSheetDO as TT"
                 + " left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                 + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID"
                 + "  where DyeingOrder.DyeingOrderType >0 AND DyeingOrder.[Status]<>9 ";
                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DOD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   group by  DyeingOrder.DyeingOrderID,DyeingOrder.DyeingOrderType, DOD.ProductID";
                }

            }
            if (type == 3)
            {
                if (nReportType == 3)
                {
                    sReturn1 = "Select GG.DyeingOrderID,GG.OrderType, GG.ProductID,SUM(QtyDyeing) as QtyDYeing,SUM(Qty_Hydro) as Qty_Hydro,SUM(Qty_Drier) as Qty_Drier,SUM(Qty_WQC) as Qty_WQC from ( "
                 + " Select DOD.DyeingOrderID, DOD.ProductID,Qty_RS as Qty_RS,Qty_QC,QtyDC,QtyRC as QtyRC"
                 + " ,Case when RS.RSState in (4,5,6,7) then Qty_RS else 0 end QtyDYeing"
                 + " ,Case when RS.RSState in (8,9) then Qty_RS else 0 end Qty_Hydro"
                 + " ,Case when RS.RSState in (10,11) then Qty_RS else 0 end Qty_Drier "
                  + " ,Case when RS.RSState in (12,13,14,15,16) then Qty_RS-Qty_QC else 0 end Qty_WQC "
                   + " ,DyeingOrder.DyeingOrderType as OrderType "
                    + "  from RouteSheetDO as TT "
                     + "  left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                      + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID "
                        + "  left join Routesheet as RS on RS.RoutesheetID=TT.RoutesheetID  "
                        + "  where TT.IsOut=1 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND DOD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   ) as GG group by  GG.DyeingOrderID,GG.ProductID,GG.OrderType";
                }

            }
            if (type == 4)// DU equsation
            {
                if (nReportType == 3)
                {
                    sReturn1 = " Select DyeingOrderID,DyeingOrderType, ProductID ,Sum(QtySRS) as SRSQty ,Sum(QtySRM) as SRMQty from ("
                              + "Select DyeingOrder.DyeingOrderType, ReqD.DyeingOrderID,ReqD.ProductID, Req.RequisitionType, Qty  "
                               + ", case when Req.RequisitionType in (0,101)  then ReqD.Qty else 0 end as QtySRS"
                               + " , case when Req.RequisitionType in (102)  then ReqD.Qty else 0 end as QtySRM"
                             + " from DURequisitionDetail as ReqD"
                            + "   left join DURequisition as Req on Req.DURequisitionID=ReqD.DURequisitionID"
                             + "  left join DyeingOrder on DyeingOrder.DyeingOrderID=ReqD.DyeingOrderID"
                             + "  left join Product on Product.ProductID=ReqD.ProductID"
                           + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID where DyeingOrder.DyeingOrderID>0 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + "  AND ReqD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + "   ) as HH  group by DyeingOrderID,DyeingOrderType, ProductID";
                }

            }
            if (type == 5)// Yarn Receive
            {
                if (nReportType == 3)
                {
                    sReturn1 = "  Select DUPG.DyeingOrderID,DyeingOrder.DyeingOrderType, DUPGD.ProductID, SUM(Qty) as YarnReceive "
                             + " from DUProGuideLineDetail as DUPGD  "
                            + "   left join DUProGuideLine as DUPG on DUPG.DUProGuideLineID=DUPGD.DUProGuideLineID  "
                             + "  left join DyeingOrder on DyeingOrder.DyeingOrderID=DUPG.DyeingOrderID  "
                             + "   left join Product on Product.ProductID=DUPGD.ProductID "
                           + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID Where DyeingOrder.DyeingOrderID>0 ";


                    #region ProductID
                    if (ProductIDs != "")
                    {

                        sReturn = sReturn + " AND DUPGD.ProductID IN (" + ProductIDs + ") ";
                    }
                    #endregion
                    #region ProductID
                    if (CustomerIDs != "")
                    {

                        sReturn = sReturn + "  AND DyeingOrder.ContractorID IN (" + CustomerIDs + ") ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + " group by DUPG.DyeingOrderID,DyeingOrderType, DUPGD.ProductID";
                }
            }

                return sReturn;
            }
        #endregion

        [HttpGet]
        public ActionResult PrintDetailPreview(string sValue,double nts)
        {
            string sDateRange = "";
            var ProductName = Convert.ToString(sValue.Split('~')[1]);
            var CustomerName = Convert.ToString(sValue.Split('~')[3]);
            var nOrderType = Convert.ToInt32(sValue.Split('~')[4]);
            int nReportType = Convert.ToInt32(sValue.Split('~')[8]);
            DateTime dOrderStartDate = Convert.ToDateTime(sValue.Split('~')[6]);
            DateTime dOrderEndDate = Convert.ToDateTime(sValue.Split('~')[7]);
            var BUID = Convert.ToInt32(sValue.Split('~')[9]);

            sDateRange = dOrderStartDate.ToString("dd MMM yyyy") + " To " + dOrderEndDate.ToString("dd MMM yyyy");

            List<RptDUOrderStatus> _oTempRptDUOrderStatusList1 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList2 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oTempRptDUOrderStatusList3 = new List<RptDUOrderStatus>();
            List<RptDUOrderStatus> _oRptDUOrderStatusList = new List<RptDUOrderStatus>();
            DataSet _oDataSet2 = new DataSet();
            DataSet _oDataSet3 = new DataSet();

            if (nReportType == 0 || nReportType == 1)
            {
                string sSql = GetSQLForProductWiseDetail(sValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForProductWiseDetail(sValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForProductWiseDetail(sValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                    oRptDUOrderStatus.ProductCode = (oRow["ProductCode"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCode"]);
                    oRptDUOrderStatus.CategoryName = (oRow["ProductCategoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductCategoryName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                  
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    oRptDUOrderStatus.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
                    oRptDUOrderStatus.OrderDate = (oRow["OrderDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["OrderDate"]);
                    oRptDUOrderStatus.ContractorName = (oRow["ContractorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ContractorName"]);
                    oRptDUOrderStatus.PINo = (oRow["PINo"] == DBNull.Value) ? "" : Convert.ToString(oRow["PINo"]);

                    _oTempRptDUOrderStatusList1.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.SRSQty = (oRow["QtySRS"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRS"]);
                    oRptDUOrderStatus.SRMQty = (oRow["QtySRM"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRM"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oRptDUOrderStatus.OrderType = (oRow["OrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["OrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC   = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);

                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }
                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oTempRptDUOrderStatusList1.Count >0)
                {
                    foreach (RptDUOrderStatus oItem in _oTempRptDUOrderStatusList1)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID && x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.SRSQty = _oTempList.Sum(x => x.SRSQty);
                            oItem.SRMQty = _oTempList.Sum(x => x.SRMQty);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.OrderType == oItem.OrderType && x.ProductID == oItem.ProductID && x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.YarnOut = _oTempList.Sum(x => x.YarnOut);
                            oItem.QtyDyeing = _oTempList.Sum(x => x.QtyDyeing);
                            oItem.Qty_Hydro = _oTempList.Sum(x => x.Qty_Hydro);
                            oItem.Qty_Drier = _oTempList.Sum(x => x.Qty_Drier);
                            oItem.Qty_WQC = _oTempList.Sum(x => x.Qty_WQC);
                            oItem.Qty_RS = _oTempList.Sum(x => x.Qty_RS);
                            oItem.QtyQC = _oTempList.Sum(x => x.QtyQC);
                            oItem.QtyDC = _oTempList.Sum(x => x.QtyDC);
                            oItem.QtyRC = _oTempList.Sum(x => x.QtyRC);
                            oItem.WastageQty = _oTempList.Sum(x => x.WastageQty);
                            oItem.RecycleQty = _oTempList.Sum(x => x.RecycleQty);
                        }

                        _oRptDUOrderStatusList.Add(oItem);
                    }

                }
            }

            if (nReportType == 2)
            {
                string sSql = GetSQLForCustomerWiseDetail(sValue, 1);
                _oDataSet = RptDUOrderStatus.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string ssSql = GetSQLForCustomerWiseDetail(sValue, 2);
                _oDataSet2 = RptDUOrderStatus.Gets(ssSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string _sSql = GetSQLForCustomerWiseDetail(sValue, 3);
                _oDataSet3 = RptDUOrderStatus.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.ContractorName = (oRow["ContractorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ContractorName"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.OrderQty = (oRow["OrderQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["OrderQty"]);
                    oRptDUOrderStatus.SRSQty = (oRow["QtySRS"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRS"]);
                    oRptDUOrderStatus.SRMQty = (oRow["QtySRM"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtySRM"]);
                    oRptDUOrderStatus.OrderName = (oRow["OrderName"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderName"]);
                    oRptDUOrderStatus.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
                    oRptDUOrderStatus.OrderDate = (oRow["OrderDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["OrderDate"]);
                    oRptDUOrderStatus.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                    oRptDUOrderStatus.PINo = (oRow["PINo"] == DBNull.Value) ? "" : Convert.ToString(oRow["PINo"]);

                    _oTempRptDUOrderStatusList1.Add(oRptDUOrderStatus);
                }
                foreach (DataRow oRow in _oDataSet2.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.YarnOut = (oRow["YarnOut"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YarnOut"]);
                    oRptDUOrderStatus.QtyQC = (oRow["Qty_QC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_QC"]);
                    oRptDUOrderStatus.QtyDC = (oRow["QtyDC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDC"]);
                    oRptDUOrderStatus.QtyRC = (oRow["QtyRC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyRC"]);
                    oRptDUOrderStatus.WastageQty = (oRow["WastageQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WastageQty"]);
                    oRptDUOrderStatus.RecycleQty = (oRow["RecycleQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["RecycleQty"]);
                    _oTempRptDUOrderStatusList2.Add(oRptDUOrderStatus);
                }

                foreach (DataRow oRow in _oDataSet3.Tables[0].Rows)
                {
                    oRptDUOrderStatus = new RptDUOrderStatus();
                    oRptDUOrderStatus.DyeingOrderID = (oRow["DyeingOrderID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderID"]);
                    oRptDUOrderStatus.ContractorID = (oRow["ContractorID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ContractorID"]);
                    oRptDUOrderStatus.OrderType = (oRow["DyeingOrderType"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DyeingOrderType"]);
                    oRptDUOrderStatus.QtyDyeing = (oRow["QtyDYeing"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["QtyDYeing"]);
                    oRptDUOrderStatus.Qty_Hydro = (oRow["Qty_Hydro"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Hydro"]);
                    oRptDUOrderStatus.Qty_Drier = (oRow["Qty_Drier"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_Drier"]);
                    oRptDUOrderStatus.Qty_WQC = (oRow["Qty_WQC"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["Qty_WQC"]);
                    _oTempRptDUOrderStatusList3.Add(oRptDUOrderStatus);
                }
                List<RptDUOrderStatus> _oTempList = new List<RptDUOrderStatus>();
                if (_oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList2.Count && _oTempRptDUOrderStatusList1.Count > _oTempRptDUOrderStatusList3.Count)
                {
                    foreach (RptDUOrderStatus oItem in _oTempRptDUOrderStatusList1)
                    {
                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList2.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.Qty_RS = _oTempList.Sum(x => x.Qty_RS);
                            oItem.QtyQC = _oTempList.Sum(x => x.QtyQC);
                            oItem.QtyDC = _oTempList.Sum(x => x.QtyDC);
                            oItem.QtyRC = _oTempList.Sum(x => x.QtyRC);
                            oItem.WastageQty = _oTempList.Sum(x => x.WastageQty);
                            oItem.RecycleQty = _oTempList.Sum(x => x.RecycleQty);
                        }

                        _oTempList = new List<RptDUOrderStatus>();
                        _oTempList = _oTempRptDUOrderStatusList3.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ContractorID == oItem.ContractorID).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.QtyDyeing = _oTempList.Sum(x => x.QtyDyeing);
                            oItem.Qty_Hydro = _oTempList.Sum(x => x.Qty_Hydro);
                            oItem.Qty_Drier = _oTempList.Sum(x => x.Qty_Drier);
                            oItem.Qty_WQC = _oTempList.Sum(x => x.Qty_WQC);
                        }

                        _oRptDUOrderStatusList.Add(oItem);
                    }
                }
            }

            byte[] abytes = new byte[0];;
            if (nReportType == 1)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptDUOrderStatusPreviewDetail oReport = new rptDUOrderStatusPreviewDetail();
                abytes = oReport.PrepareReport(_oRptDUOrderStatusList, oCompany, CustomerName, ProductName, sDateRange, nReportType, oBusinessUnit);
            }
            if (nReportType == 2)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptDUOrderStatusPreviewDetail oReport = new rptDUOrderStatusPreviewDetail();
                abytes = oReport.PrepareReport(_oRptDUOrderStatusList, oCompany, CustomerName, ProductName, sDateRange, nReportType, oBusinessUnit);
            }
           
            return File(abytes, "application/pdf");
        }
        private string GetSQLForProductWiseDetail(string sTempValue, int type)
        {
            var ProductID = Convert.ToInt32(sTempValue.Split('~')[0]);
            var ProductName = Convert.ToString(sTempValue.Split('~')[1]);
            var CustomerID = Convert.ToInt32(sTempValue.Split('~')[2]);
            var CustomerName = Convert.ToString(sTempValue.Split('~')[3]);
            var nOrderType = Convert.ToInt32(sTempValue.Split('~')[4]);
            EnumCompareOperator eOrderDate = (EnumCompareOperator)Convert.ToInt32(sTempValue.Split('~')[5]);
            DateTime dOrderStartDate = Convert.ToDateTime(sTempValue.Split('~')[6]);
            DateTime dOrderEndDate = Convert.ToDateTime(sTempValue.Split('~')[7]);
            var nReportType = Convert.ToInt32(sTempValue.Split('~')[8]);
            var BUID = Convert.ToInt32(sTempValue.Split('~')[9]);
            string sReturn1 = "";
            string sReturn = "";

            if (type == 1)
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = "Select DyeingOrder.DyeingOrderID, DyeingOrder.ContractorID,Contractor.Name AS ContractorName, DyeingOrder.OrderNo,DyeingOrder.OrderDate,ExportPI.PINo,SUM(DyeingOrderDetail.Qty) AS OrderQty, DyeingOrderType,OrderName, ProductName,ProductCode,ProductBaseID,Product.ProductCategoryID, Product.ProductID,ProductCategoryName"
                                +" from DyeingOrderDetail left join DyeingOrder on DyeingOrder.DyeingOrderID=DyeingOrderDetail.DyeingOrderID  left join Product on Product.ProductID=DyeingOrderDetail.ProductID left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID  left join DUOrderSetup on DUOrderSetup.OrderType = DyeingOrder.DyeingOrderType  Left Join ExportPI On DyeingOrder.ExportPIID= ExportPI.ExportPIID Left Join Contractor On Contractor.ContractorID= DyeingOrder.ContractorID where DyeingOrder.[Status]<>9  ";
                    #region ProductID
                    if (ProductID >0)
                    {
                        sReturn = sReturn + "  AND DyeingOrderDetail.ProductID =" +ProductID+" ";
                    }
                    #endregion                    
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType + " ";
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + " group by DyeingOrder.DyeingOrderID, DyeingOrder.ContractorID,Contractor.Name , DyeingOrder.OrderNo,DyeingOrder.OrderDate,ExportPI.PINo,DyeingOrderType,OrderName, ProductName,ProductCode,ProductBaseID,Product.ProductCategoryID, Product.ProductID,ProductCategoryName";
                }

            }

            if (type == 2)
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = " Select DyeingOrderID,DyeingOrderType,ProductCategoryID,ProductCategoryName,ProductCode,ProductName, ProductID ,Sum(QtySRS) as QtySRS ,Sum(QtySRM) as QtySRM from ("
	                +"Select Product.ProductName,Product.ProductCode,ProductCategory.ProductCategoryName,ProductCategory.ProductCategoryID, DyeingOrder.DyeingOrderType, ReqD.DyeingOrderID,ReqD.ProductID, Req.RequisitionType, Qty "
	                +" , case when Req.RequisitionType in (0,101)  then ReqD.Qty else 0 end as QtySRS"
                    +", case when Req.RequisitionType in (102)  then ReqD.Qty else 0 end as QtySRM 	  from DURequisitionDetail as ReqD 	   left join DURequisition as Req on Req.DURequisitionID=ReqD.DURequisitionID    left join DyeingOrder on DyeingOrder.DyeingOrderID=ReqD.DyeingOrderID    left join Product on Product.ProductID=ReqD.ProductID    left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID    where DyeingOrder.[Status]<>9 ";
                    #region ProductID
                    if (ProductID > 0)
                    {
                        sReturn = sReturn + "  AND ReqD.ProductID =" + ProductID + " ";
                    }
                    #endregion                                      
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + " ) as HH  group by  DyeingOrderID, DyeingOrderType, ProductID,ProductCategoryID,ProductCategoryName,ProductCode,ProductName";
                }

            }

            if (type == 3)
            {
                if (nReportType == 0 || nReportType == 1)
                {
                    sReturn1 = " Select GG.OrderType, GG.ProductID,GG.DyeingOrderID, sum(Qty_RS)as YarnOut,Sum(Qty_QC) as Qty_QC, sum(QtyDC)as QtyDC,sum(QtyRC)as QtyRC, sum(WastageQty) as WastageQty,sum(RecycleQty) as RecycleQty,Sum(QtyDyeing) as QtyDYeing,Sum(Qty_Hydro) as Qty_Hydro,Sum(Qty_Drier) as Qty_Drier,Sum(Qty_WQC) as Qty_WQC from ( Select DOD.DyeingOrderID, DOD.ProductID,Qty_RS as Qty_RS,       Case when RS.RSState in (4,5,6,7) then Qty_RS else 0 end QtyDYeing     ,Case when RS.RSState in (8,9) then Qty_RS else 0 end Qty_Hydro   , Case when RS.RSState in (10,11) then Qty_RS else 0 end Qty_Drier   , Case when RS.RSState in (12,13,14,15,16) then Qty_RS-Qty_QC else 0 end Qty_WQC      ,  DyeingOrder.DyeingOrderType as OrderType "
				 +",TT.Qty_QC			 ,TT.QtyDC				 ,TT.QtyRC,TT.Qty_RS as YarnOut ,TT.WastageQty		 ,TT.RecycleQty              from RouteSheetDO as TT     left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID      left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID             left join Routesheet as RS on RS.RoutesheetID=TT.RoutesheetID    where DyeingOrder.[Status]<>9 and TT.IsOut=1 ";

                    #region ProductID
                    if (ProductID > 0)
                    {

                        sReturn = sReturn + " AND DOD.ProductID=" + ProductID + " ";
                    }
                    #endregion
                    #region DyeingOrderType
                    if (nOrderType != null && nOrderType > 0)
                    {
                        sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                    }
                    #endregion
                    #region RequestDate
                    if (eOrderDate != EnumCompareOperator.None)
                    {
                        if (eOrderDate == EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                        }
                        else if (eOrderDate == EnumCompareOperator.Between)
                        {

                            sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                        }
                    }
                    #endregion
                    sReturn = sReturn1 + sReturn + ") AS GG group by GG.OrderType, GG.ProductID,GG.DyeingOrderID";
                }

            }
            return sReturn;
        }
        private string GetSQLForCustomerWiseDetail(string sTempValue, int type)
        {
            var ProductID = Convert.ToInt32(sTempValue.Split('~')[0]);
            var ProductName = Convert.ToString(sTempValue.Split('~')[1]);
            var CustomerID = Convert.ToInt32(sTempValue.Split('~')[2]);
            var CustomerName = Convert.ToString(sTempValue.Split('~')[3]);
            var nOrderType = Convert.ToInt32(sTempValue.Split('~')[4]);
            EnumCompareOperator eOrderDate = (EnumCompareOperator)Convert.ToInt32(sTempValue.Split('~')[5]);
            DateTime dOrderStartDate = Convert.ToDateTime(sTempValue.Split('~')[6]);
            DateTime dOrderEndDate = Convert.ToDateTime(sTempValue.Split('~')[7]);
            var nReportType = Convert.ToInt32(sTempValue.Split('~')[8]);
            var BUID = Convert.ToInt32(sTempValue.Split('~')[9]);
            string sReturn1 = "";
            string sReturn = "";
            if (type == 1)
            {
                sReturn1 = "Select DyeingOrder.DyeingOrderID,Contractor.Name as ContractorName,DyeingOrder.ContractorID, DyeingOrder.OrderNo,DyeingOrder.OrderDate,ExportPI.PINo,DyeingOrderDetail.Qty AS OrderQty, DyeingOrderType,OrderName, ProductName,ProductCode,ProductBaseID,Product.ProductCategoryID, Product.ProductID,ProductCategoryName,"
                + "(select SUM(Qty) from DURequisitionDetail where DyeingOrderID=DyeingOrder.DyeingOrderID and ProductID=DyeingOrderDetail.ProductID and DURequisitionID in (Select DURequisitionID from DURequisition where  DURequisition.RequisitionType=101) ) as QtySRS"
                + ",(select SUM(Qty) from DURequisitionDetail where DyeingOrderID=DyeingOrderDetail.DyeingOrderID and ProductID=DyeingOrderDetail.ProductID and DURequisitionID in (Select DURequisitionID from DURequisition where  DURequisition.RequisitionType=102)) as QtySRM"
                + " from DyeingOrderDetail"
                + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DyeingOrderDetail.DyeingOrderID "
                + " left join Product on Product.ProductID=DyeingOrderDetail.ProductID"
                + " left join ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID "
                 + " left join DUOrderSetup on DUOrderSetup.OrderType = DyeingOrder.DyeingOrderType "
                  + " Left Join ExportPI On DyeingOrder.ExportPIID= ExportPI.ExportPIID"
                  + " Left Join Contractor On Contractor.ContractorID=DyeingOrder.ContractorID"
                + " where DyeingOrder.[Status]<>9 and IsClose=0 ";
                #region CustomerID
                if (CustomerID >0)
                {
                    sReturn = sReturn + "  AND DyeingOrder.ContractorID =" + CustomerID +"  ";
                }
                #endregion               
                #region DyeingOrderType
                if (nOrderType != null && nOrderType > 0)
                {
                    sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                }
                #endregion
                #region RequestDate
                if (eOrderDate != EnumCompareOperator.None)
                {
                    if (eOrderDate == EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eOrderDate == EnumCompareOperator.Between)
                    {

                        sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                    }
                }
                #endregion
                sReturn = sReturn1 + sReturn;
            }

            if (type == 2)
            {
                sReturn1 = " Select DyeingOrder.DyeingOrderID, DyeingOrder.DyeingOrderType, DOD.ProductID,DyeingOrder.ContractorID,Qty_RS as YarnOut ,Qty_QC as Qty_QC,QtyDC as QtyDC,QtyRC as QtyRC,WastageQty as WastageQty ,TT.RecycleQty as RecycleQty "
                 + " from RouteSheetDO as TT"
                 + " left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                 + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID"
                 + "   where DyeingOrder.DyeingOrderType >0 AND DyeingOrder.[Status]<>9 and IsClose=0 ";
                #region ProductID
                if (CustomerID >0)
                {

                    sReturn = sReturn + " AND DyeingOrder.ContractorID =" + CustomerID;
                }
                #endregion     
                #region DyeingOrderType
                if (nOrderType != null && nOrderType > 0)
                {
                    sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                }
                #endregion
                #region RequestDate
                if (eOrderDate != EnumCompareOperator.None)
                {
                    if (eOrderDate == EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eOrderDate == EnumCompareOperator.Between)
                    {

                        sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                    }
                }
                #endregion
                sReturn = sReturn1 + sReturn ;
            }


            if (type == 3)
            {

                sReturn1 = "Select GG.DyeingOrderID,GG.OrderType as DyeingOrderType, GG.ProductID,ContractorID,QtyDyeing as QtyDYeing,Qty_Hydro as Qty_Hydro,Qty_Drier as Qty_Drier,Qty_WQC as Qty_WQC from ( "
                 + " Select DOD.DyeingOrderID, DOD.ProductID,ContractorID,Qty_RS as Qty_RS,Qty_QC,QtyDC,QtyRC as QtyRC"
                 + " ,Case when RS.RSState in (4,5,6,7) then Qty_RS else 0 end QtyDYeing"
                 + " ,Case when RS.RSState in (8,9) then Qty_RS else 0 end Qty_Hydro"
                 + " ,Case when RS.RSState in (10,11) then Qty_RS else 0 end Qty_Drier "
                  + " ,Case when RS.RSState in (12,13,14,15,16) then Qty_RS-Qty_QC else 0 end Qty_WQC "
                   + " ,DyeingOrder.DyeingOrderType as OrderType "
                    + "  from RouteSheetDO as TT "
                     + "  left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=TT.DyeingOrderDetailID"
                      + " left join DyeingOrder on DyeingOrder.DyeingOrderID=DOD.DyeingOrderID "
                        + "  left join Routesheet as RS on RS.RoutesheetID=TT.RoutesheetID  "
                        + "  where TT.IsOut=1 and isnull(TT.RSState,0)<>13 and  DyeingOrder.[Status]<>9 and IsClose=0  ";


             
                #region CustomerID
                if (CustomerID>0)
                {

                    sReturn = sReturn + "  AND DyeingOrder.ContractorID = " + CustomerID + " ";
                }
                #endregion
                #region DyeingOrderType
                if (nOrderType != null && nOrderType > 0)
                {
                    sReturn = sReturn + " AND DyeingOrder.DyeingOrderType =" + nOrderType;
                }
                #endregion
                #region RequestDate
                if (eOrderDate != EnumCompareOperator.None)
                {
                    if (eOrderDate == EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eOrderDate == EnumCompareOperator.Between)
                    {

                        sReturn = sReturn + " AND CONVERT(DATE,CONVERT(VARCHAR(12),DyeingOrder.OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dOrderEndDate.ToString("dd MMM yyyy") + "',106))";
                    }
                }
                #endregion
                sReturn = sReturn1 + sReturn + "   ) as GG";

            }

            return sReturn;
        }
    }
}