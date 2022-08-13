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
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class LotController : Controller
    {
        #region Declaration
        Lot _oLot = new Lot();
        List<Lot> _oLots = new List<Lot>();

        LotLocation _oLotLocation = new LotLocation();
        List<LotLocation> _oLotLocations = new List<LotLocation>();
        #endregion
        
        #region Lot Creation
        public ActionResult ViewLots(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];

            _oLots = new List<Lot>();
            //string sSQL = "SELECT * FROM View_Lot AS MM WHERE MM.WorkingUnitID IN(SELECT NN.WorkingUnitID FROM StorePermission AS NN WHERE NN.UserID=" + nUserID.ToString() + " AND NN.ModuleName = " + ((int)EnumModuleName.Lot).ToString() + ") AND MM.BUID=" + buid.ToString() + " AND LotID NOT IN (SELECT LotID FROM ITransaction) AND ProductID IN (SELECT HH.ProductID FROM View_Product AS HH WHERE HH.ProductCategoryID IN (SELECT PC.ProductCategoryID FROM [dbo].[FN_GetProductCategoryByBUModuleAndUser](" + nUserID.ToString() + "," + ((int)EnumModuleName.Lot).ToString() + "," + ((int)EnumProductUsages.Regular).ToString() + "," + buid.ToString() + ") AS PC))";
            //_oLots = Lot.Gets(sSQL, nUserID);

            ViewBag.BUID = buid;
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            return View(_oLots);
        }
        public ActionResult ViewLot(int id, int buid, double ts)
        {
            _oLot = new Lot();
            if (id > 0)
            {
                _oLot = _oLot.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.Stores=WorkingUnit.GetsPermittedStore(buid, EnumModuleName.Lot, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oLot);
        }

        [HttpPost]
        public JsonResult Save(Lot oLot)
        {
            try
            {
                _oLot = oLot.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLot = new Lot();
                _oLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLot);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateRack(Lot oLot)
        {
            _oLot = new Lot();
            try
            {
                if(oLot.RackID>0)
                {
                    _oLot = oLot.UpdateRack((int)Session[SessionInfo.currentUserID]);
                    _oLot = oLot.Get(oLot.LotID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oLot = new Lot();
                _oLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLot);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateDate
        [HttpPost]
        public JsonResult UpdateTransaction(ITransaction oITransaction)
        {
            try
            {
                oITransaction = oITransaction.UpdateTransaction((int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                oITransaction = new ITransaction();
                oITransaction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITransaction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region UpdateLotPrice
        [HttpPost]
        public JsonResult UpdateLotPrice(Lot oLot)
        {
            try
            {
                _oLot = oLot.UpdateLotPrice((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLot = new Lot();
                _oLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLot);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region Gets
        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            _oLots = new List<Lot>();
            try
            {
                string sLotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim(): "";
                int nProductID = oLot.ProductID;
                int nWorkingUnitID = oLot.WorkingUnitID;

                string sSQL = "Select * from View_Lot Where LotID<>0 AND ISNULL(Balance,0)>0 ";
                if (!string.IsNullOrEmpty(sLotNo))
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                if (nProductID > 0)//Requirement of Shohel Rana of B007
                    sSQL = sSQL + " And ProductID=" + nProductID;
                if (nWorkingUnitID>0)
                    sSQL = sSQL + " And WorkingUnitID=" + nWorkingUnitID;
                if(oLot.StyleID>0)
                    sSQL = sSQL + " And StyleID=" + oLot.StyleID;
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID=" + oLot.BUID;
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLotForBalnce(Lot oLot)      // edited by akram
        {
            _oLots = new List<Lot>();
            try
            {
                string sSQL = "Select * from View_Lot Where LotID<>0 AND Balance>0.1 ";

                #region LotNo
                if (!string.IsNullOrEmpty(oLot.LotNo) && oLot.LotNo !="undefined")
                {
                    sSQL = sSQL + " And LotNo Like '%" + oLot.LotNo + "%'";
                }
                #endregion

                #region ProductID
                if (oLot.ProductID > 0)
                {
                    sSQL = sSQL + " And ProductID = " + oLot.ProductID;
                }
                #endregion

                #region Working UnitID
                if (oLot.WorkingUnitID > 0)
                {
                    sSQL = sSQL + " And WorkingUnitID = " + oLot.WorkingUnitID;
                }
                #endregion

                #region StyleID
                if (oLot.StyleID > 0)
                {
                    sSQL = sSQL + " And StyleID = " + oLot.StyleID;
                }
                #endregion

                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLotByProduct(Lot objLot)
        {
            int nBUID = Convert.ToInt32(objLot.Params.Split('~')[0]);
            string sLotNo = objLot.Params.Split('~')[1];

            List<Lot> oLots = new List<Lot>();
            Lot oLot = new Lot();
            try
            {
                string str = "SELECT top 180 * FROM View_Lot WHERE LotNo LIKE '%" + sLotNo + "%' AND ParentType IN (101,103)";
                if (objLot.ProductName != "" && objLot.ProductName != null)
                {
                    str = str + " AND ProductID IN (" + objLot.ProductName + ")";
                }
                if (objLot.BUID>0)
                {
                    str = str + " AND BUID=" + objLot.BUID;
                }
                oLots = Lot.Gets(str, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Import and Export Adjustment Lot
        public ActionResult ImportFromExcel()
        {
            ViewBag.FeedBack = "";
            return View();
        }
        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase fileLots)
        {
            List<Lot> oLots = new List<Lot>();
            Lot oLot = new Lot();

            try
            {
                if (fileLots == null) { throw new Exception("File not Found"); }
                oLots = this.GetLotsFromExcel(fileLots);
                foreach (Lot oItem in oLots)
                {
                    oLot = oItem.UploadLot((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View();
            }
           
            //return View(_oLots);
            return RedirectToAction("View_Lots", "Lot", new { menuid = (int)Session[SessionInfo.MenuID], buid =1 });
        }
        private List<Lot> GetLotsFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<Lot> oLots = new List<Lot>();
            Lot oLot = new Lot();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    bool bFlag = true;
                    int nMeasurementUnitID = 0; string sMeasurementUnitID = "";
                    List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
                    oMeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);

                    int nProductD = 0; string sProductName = "";
                    List<Product> oProducts = new List<Product>();
                    oProducts = Product.Gets("SELECT * FROM View_Product", (int)Session[SessionInfo.currentUserID]);

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        //sMeasurementUnitID = Convert.ToString(oRows[i]["MUnit"] == DBNull.Value ? "0" : oRows[i]["MUnit"]);
                        nMeasurementUnitID = this.GetMUnitID(sMeasurementUnitID, oMeasurementUnits);

                        sProductName = Convert.ToString(oRows[i]["ProductName"] == DBNull.Value ? "0" : oRows[i]["ProductName"]);
                        nProductD = this.GetProductID(sProductName, oProducts);

                        //if (nMeasurementUnitID > 0 && nProductD > 0)
                        //{
                           
                                oLot = new Lot();
                                oLot.ProductName = Convert.ToString(oRows[i]["ProductName"] == DBNull.Value ? "" : oRows[i]["ProductName"]);
                                oLot.MUnitID = nMeasurementUnitID;
                                oLot.ProductID = nProductD;// Convert.ToString(oRows[i]["PartName"] == DBNull.Value ? "" : oRows[i]["PartName"]);
                                oLot.LotNo = Convert.ToString(oRows[i]["LotNo"] == DBNull.Value ? "" : oRows[i]["LotNo"]);
                                oLot.LogNo = Convert.ToString(oRows[i]["LogNo"] == DBNull.Value ? "" : oRows[i]["LogNo"]);
                                oLot.WorkingUnitID = Convert.ToInt32(oRows[i]["WorkingUnitID"] == DBNull.Value ? 0 : oRows[i]["WorkingUnitID"]);
                                oLot.Balance = Convert.ToDouble(oRows[i]["Balance"] == DBNull.Value ? 0 : oRows[i]["Balance"]);
                                //oLot.UnitPrice = Convert.ToDouble(oRows[i]["UnitPrice"] == DBNull.Value ? 0 : oRows[i]["UnitPrice"]);
                                oLot.BUID = Convert.ToInt32(oRows[i]["BUID"] == DBNull.Value ? 0 : oRows[i]["BUID"]);
                                oLot.ProductID = Convert.ToInt32(oRows[i]["ProductID"] == DBNull.Value ? 0 : oRows[i]["ProductID"]);

                                oLots.Add(oLot);
                            
                        //}
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oLots;
        }
        private int GetMUnitID(string sMUnitID, List<MeasurementUnit> oMeasurementUnits)
        {
            int nMUnitID;
            bool isNumeric = int.TryParse(sMUnitID, out nMUnitID);
            if (isNumeric)
            {
                foreach (MeasurementUnit oItem in oMeasurementUnits)
                {
                    if (oItem.MeasurementUnitID == nMUnitID)
                    {
                        return oItem.MeasurementUnitID;
                    }
                }
                return 0;
            }
            else
            {
                nMUnitID = 0;
            }
            return nMUnitID;
        }
        private int GetProductID(string sProductID, List<Product> oProducts)
        {
            int nProductID;
            bool isNumeric = int.TryParse(sProductID, out nProductID);
            if (isNumeric)
            {
                foreach (Product oItem in oProducts)
                {
                    if (oItem.ProductID == nProductID)
                    {
                        return oItem.ProductID;
                    }
                }
                return 0;
            }
            else
            {
                nProductID = 0;
            }
            return nProductID;
        }

      
        #endregion

        #region Print and Preview
        public ActionResult PrintTransectiorn(int id, int buid, double ts)
        {
            List<ITransaction> oITransactions = new List<ITransaction>();
            string sSQL = "";
            sSQL = "SELECT * FROM  View_ITransactionReport WHERE LotID IN ( " + id + " ) Order By ITransactionID";
            oITransactions = ITransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
           
            if (oITransactions.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("TransactionTimeSt", "Date", 60, SelectedField.FieldType.Data, SelectedField.Alginment.CENTER); oSelectedFields.Add(oSelectedField);                
                oSelectedField = new SelectedField("RefNo", "Ref No", 130, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("UnitName", "Unit", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_In", "In Qty", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Out", "Out Qty", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("CurrentBalance", "Current Balance", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                
                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 0;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(oITransactions.Cast<object>().ToList(), oBusinessUnit, oCompany, "Transaction Report \n(" + oITransactions[0].ProductName + ", " + oITransactions[0].LotNo + ")", oSelectedFields);
                return File(abytes, "application/pdf");
            }
        }
        #endregion Print

        #region Search
        [HttpPost]
        public JsonResult GetLotByNo(Lot oLot)
        {
            _oLots = new List<Lot>();
            try
            {
                string sSQL = "";

                sSQL = "SELECT * FROM View_Lot AS HH WHERE (ISNULL(HH.LotNo,'')+ISNULL(HH.ProductName,'')+ISNULL(HH.ProductCode,'')) LIKE '%" + oLot.LotNo + "%'";

                #region BUID
                if (oLot.BUID>0)
                {
                    sSQL+=" AND BUID ="+oLot.BUID;
                }
                #endregion
              
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLots = new List<Lot>();
                _oLot.ErrorMessage = ex.Message;
                _oLots.Add(_oLot);
            }
            var jsonResult = Json(_oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvSearch(Lot oLot)
        {
            _oLots = new List<Lot>();
            try
            {
                string sSQL = MakeSQL(oLot);
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLots = new List<Lot>();
                _oLot.ErrorMessage = ex.Message;
                _oLots.Add(_oLot);
            }
            var jsonResult = Json(_oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(Lot oLot)
        {
            string sParams = oLot.ErrorMessage;


            string sProductIDs = "";
            string sWorkingUnitIDs = "";
            string sLotNo = "";
            int nBUID = 0;



            if (!string.IsNullOrEmpty(sParams))
            {

                sProductIDs = Convert.ToString(sParams.Split('~')[0]);
                sWorkingUnitIDs = Convert.ToString(sParams.Split('~')[1]);
                sLotNo = Convert.ToString(sParams.Split('~')[2]);
               

            }


            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_Lot ";

            #region ProductID
            if (!String.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + sProductIDs + ")";
            }
            #endregion
            #region ProductID
            if (!String.IsNullOrEmpty(sWorkingUnitIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "WorkingUnitID in(" + sWorkingUnitIDs + ")";
            }
            #endregion
           
            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo LIKE '%" + sLotNo + "%'";
            }
            #endregion

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "Balance>0.3";

            string sSQL = sReturn1 + " " + sReturn+" Order by ProductID";
            return sSQL;
        }
      
        
        #endregion

        #region Gets Lot By Product & Working Unit for different location
        [HttpPost]
        public JsonResult GetsLotByBUWise(PTUUnit2Distribution oPTUUnit2Distribution)
        {
            List<PTUUnit2Distribution> oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                string sSQL = "Select * from View_PTUUnit2Distribution Where Qty>0 And PTUUnit2ID=" + oPTUUnit2Distribution.PTUUnit2ID + " And WorkingUnitID=" + oPTUUnit2Distribution.WorkingUnitID + " And BUID=" + oPTUUnit2Distribution.BUID + "";
                oPTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPTUUnit2Distribution = new PTUUnit2Distribution();
                oPTUUnit2Distribution.ErrorMessage = ex.Message;
                oPTUUnit2Distributions.Add(oPTUUnit2Distribution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CommitIsRunning(Lot oLot)
        {
            _oLot = new Lot();
            _oLot = oLot.CommitIsRunning( ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLot);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateStatus(Lot oLot)
        {
            string sMsg = oLot.UpdateStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMsg);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets
        [HttpPost]
        public JsonResult GetsLotForAvilableStock(Lot oLot)
        {
            _oLots = new List<Lot>();
            try
            {
                string sLotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";
                int nProductID = oLot.ProductID;

                string sSQL = "Select * from View_Lot Where LotID<>0 ";

                if (!string.IsNullOrEmpty(sLotNo))
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                if (nProductID > 0)
                    sSQL = sSQL + " And ProductID=" + nProductID;
                if (oLot.ColorID> 0)
                    sSQL = sSQL + " And ColorID=" + oLot.ColorID;
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID=" + oLot.BUID;
                if (oLot.UnitTypeInInt > 0)
                    sSQL = sSQL + " And UnitType=" + oLot.UnitTypeInInt;

                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stock Report
        public ActionResult View_Lots(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            _oLots = new List<Lot>();
            ViewBag.BUID = buid;
            ViewBag.LotStatusList = EnumObject.jGets(typeof(EnumLotStatus)).Where(x => x.id != (int)EnumLotStatus.Running).ToList();
            return View(_oLots);
        }
        #endregion

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

        #region Aging
        public ActionResult View_Lots_Aging(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.WorkingUnits = WorkingUnit.BUWiseGets(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oLots);
        }
        private string MakeSQL_LotAging(Lot oLot)
        {
            string sParams = oLot.ErrorMessage;


            string sProductIDs = "";
            int nWorkingUnitID = 0;
            string sLotNo = "";
            int nAgingDays = 0;
            int nBUID = 0;

            if (!string.IsNullOrEmpty(sParams))
            {

                sProductIDs = Convert.ToString(sParams.Split('~')[0]);
                nWorkingUnitID = Convert.ToInt32(sParams.Split('~')[1]);
                sLotNo = Convert.ToString(sParams.Split('~')[2]);
                nAgingDays = Convert.ToInt32(sParams.Split('~')[3]);
                nBUID = Convert.ToInt32(sParams.Split('~')[4]);
            }


            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "SELECT * FROM View_LotAging";
            #region ProductID
            if (!String.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + sProductIDs + ")";
            }
            #endregion
            #region ProductID
            if (nWorkingUnitID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "WorkingUnitID in(" + nWorkingUnitID + ")";
            }
            #endregion
            #region ProductID
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID in(" + nBUID + ")";
            }
            #endregion
            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo LIKE '%" + sLotNo + "%'";
            }
            #endregion
            #region nAgingDays
            if (nAgingDays>0)
            {
                Global.TagSQL(ref sReturn);

                if (nAgingDays == 0)
                {
                    sReturn = sReturn +"AgingDays>1";
                }
                if (nAgingDays == 1)
                {
                    sReturn = sReturn + "AgingDays>1 and AgingDays<=30";
                }
                if (nAgingDays == 30)
                {
                    sReturn = sReturn + "AgingDays>30 and AgingDays<=60";
                }
                if (nAgingDays == 60)
                {
                    sReturn = sReturn + "AgingDays>60 and AgingDays<=90";
                }
                if (nAgingDays == 90)
                {
                    sReturn = sReturn + "AgingDays>90 and AgingDays<=120";
                }
                if (nAgingDays == 120)
                {
                    sReturn = sReturn + "AgingDays>120 and AgingDays<=365";
                }
                if (nAgingDays == 182)
                {
                    sReturn = sReturn + "AgingDays>182";
                }
                if (nAgingDays == 365)
                {
                    sReturn = sReturn + "AgingDays>365 ";
                }
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "  order by LastDate ASC";
            return sSQL;
        }
        [HttpPost]
        public JsonResult Gets_AgingLot(Lot oLot)
        {
            _oLots = new List<Lot>();
            try
            {
                string sSQL = MakeSQL_LotAging(oLot);
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLots = new List<Lot>();
                _oLot.ErrorMessage = ex.Message;
                _oLots.Add(_oLot);
            }
            var jsonResult = Json(_oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult PrintAgingLots(string sTempString)
        {
            _oLot.ErrorMessage = sTempString;
            _oLots = new List<Lot>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string sSQL = "";
            try
            {
                sSQL = MakeSQL_LotAging(_oLot);
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLots = new List<Lot>();
                _oLot.ErrorMessage = ex.Message;
                _oLots.Add(_oLot);
            }
           
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptLotAging oReport = new rptLotAging();
            string sMessage = "Aging lots";
            byte[] abytes = oReport.PrepareReport(_oLots, oCompany,oBusinessUnit, sMessage);
            return File(abytes, "application/pdf");

        }
        #endregion
        public ActionResult LotPrintList(string sIDs, double ts)
        {
            string sSQL = "";
            _oLot = new Lot();
            _oLots = new List<Lot>();
            //string sSql = "SELECT * FROM View_Lot WHERE LotID IN (" + sIDs + ") ORDER BY LotID ASC";
            //_oLot.Lots= Lot.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oLot.ErrorMessage = sIDs;
            if (!string.IsNullOrEmpty(_oLot.ErrorMessage))
            {
                sSQL = MakeSQL(_oLot);
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //_oLots = Lot.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            if (_oLots.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oLot.Company = oCompany;
                rptLotList oReport = new rptLotList();
                byte[] abytes = oReport.PrepareReport(_oLots, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }
        public void ExportToExcel(string sIDs, double ts)
        {
            string sSQL = "";
            _oLot = new Lot();
            _oLots = new List<Lot>();
            //string sSql = "SELECT * FROM View_Lot WHERE LotID IN (" + sIDs + ") ORDER BY LotID ASC";
            _oLot.ErrorMessage = sIDs;
            if (!string.IsNullOrEmpty(_oLot.ErrorMessage))
            {
                 sSQL = MakeSQL(_oLot);
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //_oLots = Lot.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
          
            if (_oLots.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oLot.Company = oCompany;


                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap List");
                    sheet.Name = "Lot List";

                    sheet.Column(2).Width = 10; //SL
                    sheet.Column(3).Width = 15; //Code
                    sheet.Column(4).Width = 25; //Product Name
                    sheet.Column(5).Width = 18; //Lot NO
                    sheet.Column(8).Width = 20; //Color
                    sheet.Column(6).Width = 25; //Buyer
                    sheet.Column(7).Width = 30; //Store
                    sheet.Column(9).Width = 10; //unit
                    sheet.Column(10).Width = 15; //Balance
                  
                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Lot List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Store"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;
                    #endregion

                    #region Report Data
                    int nSL = 0;
                    foreach (Lot oItem in _oLots)
                    {

                        nSL++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;   

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.WorkingUnitName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.MUName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.Balance; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0)";

                        rowIndex++;
                    }
                    #endregion


                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Lot Current Stock List.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }



            }

        }


        #region Actions Lot Location

        public ActionResult ViewLotLocations(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oLotLocations = new List<LotLocation>();
            _oLotLocations = LotLocation.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oLotLocations);
        }

        public ActionResult ViewLotLocation(int id)
        {
            _oLotLocation = new LotLocation();
            if (id > 0)
            {
                _oLotLocation = _oLotLocation.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oLotLocation);
        }

        [HttpPost]
        public JsonResult SaveLotLocation(LotLocation oLotLocation)
        {
            _oLotLocation = new LotLocation();
            try
            {
                _oLotLocation = oLotLocation;
                _oLotLocation = _oLotLocation.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLotLocation = new LotLocation();
                _oLotLocation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotLocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLotLocation(LotLocation oLotLocation)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLotLocation.Delete(oLotLocation.LotLocationID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}