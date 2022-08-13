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
using System.Diagnostics;
namespace ESimSolFinancial.Controllers
{
    public class BudgetController : Controller
    {
        #region Declaration
        Budget _oBudget = new Budget();
        List<Budget> _oBudgets = new List<Budget>();
        List<TBudgetDetail> _oTBudgetDetails = new List<TBudgetDetail>();
        TBudgetDetail _oTBudgetDetail = new TBudgetDetail();
        List<BudgetDetail> _oBudgetDetails = new List<BudgetDetail>();
        BudgetDetail _oBudgetDetail = new BudgetDetail();
        #endregion

        #region Get Tree Detail With Amount

        private TBudgetDetail Get_TBudgetDetail(ChartsOfAccount oChartsOfAccount)
        {
            _oTBudgetDetail = new TBudgetDetail();
            _oTBudgetDetail.BudgetID = 0;
            _oTBudgetDetail.BudgetDetailID = 0;
            _oTBudgetDetail.id = oChartsOfAccount.AccountHeadID;
            _oTBudgetDetail.parentid = oChartsOfAccount.ParentHeadID;
            _oTBudgetDetail.text = oChartsOfAccount.AccountHeadName;
            _oTBudgetDetail.attributes = oChartsOfAccount.IsJVNode.ToString();
            _oTBudgetDetail.code = oChartsOfAccount.AccountCode;
            _oTBudgetDetail.AccountHeadID = oChartsOfAccount.AccountHeadID;
            _oTBudgetDetail.parentid = oChartsOfAccount.ParentHeadID;
            _oTBudgetDetail.BudgetAmount = oChartsOfAccount.Amount;
            _oTBudgetDetail.AccountHeadName = oChartsOfAccount.AccountHeadName;
            _oTBudgetDetail.AccountType = oChartsOfAccount.AccountType;
            _oTBudgetDetail.AccountTypeInInt = oChartsOfAccount.AccountTypeInInt;
            _oTBudgetDetail.IsjvNode = oChartsOfAccount.IsJVNode;
            _oTBudgetDetail.ComponentType = (EnumComponentType)oChartsOfAccount.ComponentID;

            return _oTBudgetDetail;
        }
        private TBudgetDetail Get_TBudgetDetail(ChartsOfAccount oChartsOfAccount, BudgetDetail oBudgetDetail)
        {
            _oTBudgetDetail = new TBudgetDetail();
            _oTBudgetDetail.BudgetID = 0;
            _oTBudgetDetail.BudgetDetailID = 0;
            _oTBudgetDetail.id = oChartsOfAccount.AccountHeadID;
            _oTBudgetDetail.parentid = oChartsOfAccount.ParentHeadID;
            _oTBudgetDetail.text = oChartsOfAccount.AccountHeadName;
            _oTBudgetDetail.attributes = oChartsOfAccount.IsJVNode.ToString();
            _oTBudgetDetail.code = oChartsOfAccount.AccountCode;
            _oTBudgetDetail.AccountHeadID = oChartsOfAccount.AccountHeadID;
            _oTBudgetDetail.parentid = oChartsOfAccount.ParentHeadID;
            _oTBudgetDetail.BudgetAmount = oChartsOfAccount.Amount;
            _oTBudgetDetail.ComponentType = (EnumComponentType)oChartsOfAccount.ComponentID;

            if (oBudgetDetail != null)
            {
                _oTBudgetDetail.BudgetID = oBudgetDetail.BudgetID;
                _oTBudgetDetail.BudgetDetailID = oBudgetDetail.BudgetDetailID;
                _oTBudgetDetail.BudgetAmount = oBudgetDetail.BudgetAmount;
                _oTBudgetDetail.Remarks = oBudgetDetail.Remarks;
            }

            _oTBudgetDetail.AccountHeadName = oChartsOfAccount.AccountHeadName;
            _oTBudgetDetail.AccountType = oChartsOfAccount.AccountType;
            _oTBudgetDetail.AccountTypeInInt = oChartsOfAccount.AccountTypeInInt;
            _oTBudgetDetail.IsjvNode = oChartsOfAccount.IsJVNode;

            return _oTBudgetDetail;
        }
        private List<TBudgetDetail> Get_TBudgetDetails(List<ChartsOfAccount> oChartsOfAccounts, List<BudgetDetail> oBudgetDetails)
        {
            List<TBudgetDetail> oTBudgetDetails = new List<TBudgetDetail>();
            List<ChartsOfAccount> ComponentBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Component).ToList();
            List<ChartsOfAccount> SegmentBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Segment).ToList();
            List<ChartsOfAccount> GroupBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Group).ToList();
            List<ChartsOfAccount> SubGroupBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.SubGroup).ToList();
            List<ChartsOfAccount> LedgerBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Ledger).ToList();

            foreach (ChartsOfAccount oItemLedger in LedgerBudgetDetails)
            {
                oTBudgetDetails.Add(Get_TBudgetDetail(oItemLedger, oBudgetDetails.Where(m => m.AccountHeadID == oItemLedger.AccountHeadID).FirstOrDefault()));
            }

            #region SubGroup
            foreach (ChartsOfAccount oItemSubGroup in SubGroupBudgetDetails)
            {
                double nAmount = oBudgetDetails.Where(m => m.ParentHeadID == oItemSubGroup.AccountHeadID).Sum(p => p.BudgetAmount);
                oItemSubGroup.Amount = nAmount;

                oTBudgetDetails.Add(Get_TBudgetDetail(oItemSubGroup));
                //oChartsOfAccounts.Where(x => x.AccountHeadID == oItemSubGroup.AccountHeadID).Select(x => x.Amount = nAmount);
            }
            #endregion

            #region Group
            foreach (ChartsOfAccount oItemGroupWise in GroupBudgetDetails)
            {
                double nAmount = SubGroupBudgetDetails.Where(m => m.ParentHeadID == oItemGroupWise.AccountHeadID).Sum(p => p.Amount);
                oItemGroupWise.Amount = nAmount;

                oTBudgetDetails.Add(Get_TBudgetDetail(oItemGroupWise));
                //oChartsOfAccounts.Where(x => x.AccountHeadID == oItemGroupWise.AccountHeadID).Select(x => x.Amount = nAmount);
            }
            #endregion

            #region Segment
            foreach (ChartsOfAccount oItemSegment in SegmentBudgetDetails)
            {
                double nAmount = GroupBudgetDetails.Where(m => m.ParentHeadID == oItemSegment.AccountHeadID).Sum(p => p.Amount);
                oItemSegment.Amount = nAmount;

                oTBudgetDetails.Add(Get_TBudgetDetail(oItemSegment));
                //oChartsOfAccounts.Where(x => x.AccountHeadID == oItemSegment.AccountHeadID).Select(x => x.Amount = nAmount);
            }
            #endregion

            #region Component
            foreach (ChartsOfAccount oItemComponent in ComponentBudgetDetails)
            {
                double nAmount = SegmentBudgetDetails.Where(m => m.ParentHeadID == oItemComponent.AccountHeadID).Sum(p => p.Amount);
                oItemComponent.Amount = nAmount;

                oTBudgetDetails.Add(Get_TBudgetDetail(oItemComponent));
                //oChartsOfAccounts.Where(x => x.AccountHeadID == oItemComponent.AccountHeadID).Select(x => x.Amount = nAmount);
            }
            #endregion

            oChartsOfAccounts.Where(x => x.AccountHeadID == 1).Select(x => x.Amount = ComponentBudgetDetails.Sum(p => p.Amount));
            oTBudgetDetails.Add(Get_TBudgetDetail(oChartsOfAccounts.Where(x => x.AccountHeadID == 1).FirstOrDefault()));

            return oTBudgetDetails;
        }
        public TBudgetDetail GetBudgetDetailsTree(Budget oBudget)
        {
            _oBudgetDetails = new List<BudgetDetail>();
            _oTBudgetDetails = new List<TBudgetDetail>();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            _oBudgetDetails = BudgetDetail.GetsByBID(_oBudget.BudgetID, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
            _oTBudgetDetails = Get_TBudgetDetails(oChartsOfAccounts, _oBudgetDetails);

            _oTBudgetDetail = new TBudgetDetail();
            _oTBudgetDetail = GetRoot(0);
            this.AddTreeNodes(ref _oTBudgetDetail);

            return _oTBudgetDetail;
        }
        #endregion

        #region Get Budget Details With Parents
        private void Get_BudgetDetail(ref List<BudgetDetail> oBudgetDetails, ChartsOfAccount oChartsOfAccount)
        {
            oBudgetDetails.Add(new BudgetDetail()
            {
                AccountHeadID = oChartsOfAccount.AccountHeadID,
                ParentHeadID = oChartsOfAccount.ParentHeadID,
                AccountType = oChartsOfAccount.AccountType,
                AccountTypeInInt = oChartsOfAccount.AccountTypeInInt,
                AccountCode = oChartsOfAccount.AccountCode,
                AccountHeadName = oChartsOfAccount.AccountHeadName,
                BudgetAmount = oChartsOfAccount.Amount,
                ComponentType = (EnumComponentType)oChartsOfAccount.ComponentID
            });
        }
        private List<BudgetDetail> Get_BudgetDetails(List<ChartsOfAccount> oChartsOfAccounts, List<BudgetDetail> oBudgetDetails)
        {
            _oBudgetDetails = new List<BudgetDetail>();
            List<ChartsOfAccount> ComponentBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Component).ToList();
            List<ChartsOfAccount> SegmentBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Segment).ToList();
            List<ChartsOfAccount> GroupBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Group).ToList();
            List<ChartsOfAccount> SubGroupBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.SubGroup).ToList();
            List<ChartsOfAccount> LedgerBudgetDetails = oChartsOfAccounts.Where(x => x.AccountType == EnumAccountType.Ledger).ToList();

            #region Ledger
            foreach (ChartsOfAccount oItemLedger in LedgerBudgetDetails)
            {
                _oBudgetDetail = new BudgetDetail();
                _oBudgetDetail = oBudgetDetails.Where(m => m.AccountHeadID == oItemLedger.AccountHeadID).FirstOrDefault();

                if (_oBudgetDetail != null)
                {
                    _oBudgetDetail.ComponentType = (EnumComponentType)oItemLedger.ComponentID;
                    _oBudgetDetails.Add(_oBudgetDetail);
                }
                else
                    Get_BudgetDetail(ref _oBudgetDetails, oItemLedger);
            }
            #endregion

            #region SubGroup
            foreach (ChartsOfAccount oItemSubGroup in SubGroupBudgetDetails)
            {
                double nAmount = oBudgetDetails.Where(m => m.ParentHeadID == oItemSubGroup.AccountHeadID).Sum(p => p.BudgetAmount);
                oItemSubGroup.Amount = nAmount;

                Get_BudgetDetail(ref _oBudgetDetails, oItemSubGroup);
            }
            #endregion

            #region Group
            foreach (ChartsOfAccount oItemGroupWise in GroupBudgetDetails)
            {
                double nAmount = SubGroupBudgetDetails.Where(m => m.ParentHeadID == oItemGroupWise.AccountHeadID).Sum(p => p.Amount);
                oItemGroupWise.Amount = nAmount;

                Get_BudgetDetail(ref _oBudgetDetails, oItemGroupWise);
            }
            #endregion

            #region Segment
            foreach (ChartsOfAccount oItemSegment in SegmentBudgetDetails)
            {
                double nAmount = GroupBudgetDetails.Where(m => m.ParentHeadID == oItemSegment.AccountHeadID).Sum(p => p.Amount);
                oItemSegment.Amount = nAmount;

                Get_BudgetDetail(ref _oBudgetDetails, oItemSegment);
            }
            #endregion

            #region Component
            foreach (ChartsOfAccount oItemComponent in ComponentBudgetDetails)
            {
                double nAmount = SegmentBudgetDetails.Where(m => m.ParentHeadID == oItemComponent.AccountHeadID).Sum(p => p.Amount);
                oItemComponent.Amount = nAmount;

                Get_BudgetDetail(ref _oBudgetDetails, oItemComponent);
            }
            #endregion

            Get_BudgetDetail(ref _oBudgetDetails, oChartsOfAccounts.Where(x => x.AccountHeadID == 1).FirstOrDefault());
            return _oBudgetDetails;
        }
        #endregion

        #region Action

        public ActionResult ViewBudgets(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oBudgets = new List<Budget>();
            string sSQL = "Select top(100)* from View_Budget";
            _oBudgets = Budget.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BudgetType = EnumObject.jGets(typeof(EnumBudgetType));



            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));

            return View(_oBudgets);
        }

        public ActionResult ViewBudget(int id)
        {
            List<Budget> _oBudgets = new List<Budget>();
            TBudgetDetail _oTBudgetDetail = new TBudgetDetail();
            List<TBudgetDetail> _oTBudgetDetails = new List<TBudgetDetail>();

            try
            {
                _oBudget = _oBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTBudgetDetail = new TBudgetDetail();
                _oTBudgetDetail = GetBudgetDetailsTree(_oBudget);

                //string sSQL = "SELECT * FROM View_BudgetDetail AS HH WHERE HH.ParentHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType=" + ((int)EnumCISSetup.Inventory_Head).ToString() + ") ORDER BY HH.AccountHeadName";
                //ViewBag.InventoryEffects = Budget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


                ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.AccountingSessions = AccountingSession.Gets("SELECT * FROM AccountingSession WHERE SessionType=1", (int)Session[SessionInfo.currentUserID]);
                ViewBag.BudgetType = EnumObject.jGets(typeof(EnumBudgetType));
                ViewBag.BudgetStatus = EnumObject.jGets(typeof(EnumBudgetStatus));

                ViewBag.TBudgetDetail = _oTBudgetDetail;
                return View(_oBudget);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oBudgetDetail);
            }
        }

        [HttpPost]
        public JsonResult Save(Budget oBudget)
        {
            _oBudget = new Budget();
            try
            {
                _oBudget = oBudget.Save(oBudget, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBudget = new Budget();
                _oBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Revise(Budget oBudget)
        {
            _oBudget = new Budget();
            try
            {
                _oBudget = oBudget.Revise(oBudget, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBudget = new Budget();
                _oBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Budget oBudget)
        {
            try
            {
                if (oBudget.BudgetID <= 0) { throw new Exception("Please select an valid item."); }
                oBudget.ErrorMessage = oBudget.Delete(oBudget.BudgetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oBudget = new Budget();
                oBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBudget.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tree
        private TBudgetDetail GetRoot(int nParentID)
        {
            TBudgetDetail oTBudgetDetail = new TBudgetDetail();
            foreach (TBudgetDetail oItem in _oTBudgetDetails)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTBudgetDetail;
        }
        private TBudgetDetail GetRootForMove(int nID)
        {
            TBudgetDetail oTBudgetDetail = new TBudgetDetail();
            foreach (TBudgetDetail oItem in _oTBudgetDetails)
            {
                if (oItem.BudgetID == nID)
                {
                    return oItem;
                }
            }
            return _oTBudgetDetail;
        }
        private List<TBudgetDetail> GetChild(int nAccountHeadID)
        {
            List<TBudgetDetail> oTBudgetDetails = new List<TBudgetDetail>();
            foreach (TBudgetDetail oItem in _oTBudgetDetails)
            {
                if (oItem.parentid == nAccountHeadID)
                {
                    oTBudgetDetails.Add(oItem);
                }
            }
            return oTBudgetDetails;
        }
        private void AddTreeNodes(ref TBudgetDetail oTBudgetDetail)
        {
            List<TBudgetDetail> oChildNodes;
            oChildNodes = GetChild(oTBudgetDetail.id);
            oTBudgetDetail.children = oChildNodes;

            foreach (TBudgetDetail oItem in oChildNodes)
            {
                TBudgetDetail oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        public JsonResult BudgetStatusChange(Budget oBudget)
        {
            _oBudget = new Budget();
            _oBudget = oBudget;
            try
            {
                if (oBudget.BudgetStatus == EnumBudgetStatus.Initialized)
                {
                    // ReqForApprove
                    _oBudget = _oBudget.BudgetStatusChange(oBudget, EnumDBOperation.Request, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (oBudget.BudgetStatus == EnumBudgetStatus.ReqForApprove)
                {
                    //Approve
                    _oBudget = _oBudget.BudgetStatusChange(oBudget, EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else if (oBudget.BudgetStatus == EnumBudgetStatus.Approved)
                {
                    //ReqForRevise
                    _oBudget = _oBudget.BudgetStatusChange(oBudget, EnumDBOperation.Revise, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                //_oBudget = SetORSStatus(_oBudget);
                //_oBudget = _oBudget.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBudget = new Budget();
                _oBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PrintBudget(int id)
        {
            Budget oBudget = new Budget();
            List<BudgetDetail> oBudgetDetails = new List<BudgetDetail>();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            oBudget = oBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
            oBudgetDetails = BudgetDetail.Gets("SELECT * FROM View_BudgetDetail WHERE BudgetID=" + id, (int)Session[SessionInfo.currentUserID]);

            oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
            oBudgetDetails = Get_BudgetDetails(oChartsOfAccounts, oBudgetDetails);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);

            rptBudget oReport = new rptBudget();
            byte[] abytes = oReport.PrepareReport(oCompany, oBusinessUnit, oBudget, oBudgetDetails);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintBudgetLedger(int id)
        {
            Budget oBudget = new Budget();
            List<BudgetDetail> oBudgetDetails = new List<BudgetDetail>();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            oBudget = oBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
            oBudgetDetails = BudgetDetail.Gets("SELECT * FROM View_BudgetDetail WHERE BudgetID=" + id + " AND BudgetAmount>0", (int)Session[SessionInfo.currentUserID]);

            oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
            oBudgetDetails = Get_BudgetDetails(oChartsOfAccounts, oBudgetDetails);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);

            try
            {
                rptBudget oReport = new rptBudget();
                byte[] abytes = oReport.PrepareReportLedger(oCompany, oBusinessUnit, oBudget, oBudgetDetails);
                return File(abytes, "application/pdf");
            }
            catch
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data");
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

        #endregion
        #region Excel
        public void ExcelReportYearly(string sParams)
        {
            int nCount = 0;
            int nBUID = 0;
            BudgetVariance oBudgetVariance = new BudgetVariance();
            List<BudgetVariance> oBudgetVariances = new List<BudgetVariance>();
            if (!string.IsNullOrEmpty(sParams))
            {
                nBUID = Convert.ToInt32(sParams.Split('~')[nCount++]);
                oBudgetVariance.BudgetID = Convert.ToInt32(sParams.Split('~')[nCount++]);
                oBudgetVariance.ReportType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                oBudgetVariance.IsApproved = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                oBudgetVariance.StartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                oBudgetVariance.EndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
            }
            oBudgetVariances = BudgetVariance.GetsReport(oBudgetVariance.BudgetID, oBudgetVariance.ReportType, oBudgetVariance.IsApproved, oBudgetVariance.StartDateSt, oBudgetVariance.EndDateSt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBudgetVariances.Count < 1) return;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "Code", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Account Name", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Budget Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Actual Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Budget Variance", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Achieve Percent", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Budget Variance");
                sheet.Name = "Budget Variance(Yearly)";

                

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 7;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Budget Variance(Yearly)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                nCount = 0; nEndCol = table_header.Count() + nStartCol;

                double dBudgetAmount = 0.0;
                double dAchiveAmount = 0.0;
                double dVarianceAmount = 0.0;
                double dAchievePercent = 0.0;
                string sAchievePercent = "";
                bool bFirstRow = false;
                int nSubGroupID = 0;
                ////////////////////

                List<BudgetVariance> oTempBudgetVariances = oBudgetVariances.OrderBy(x => x.SubGroupID).ToList();


                foreach (var obj in oTempBudgetVariances)
                {
                    nStartCol = 2;

                    ExcelTool.Formatter = "";
                    if (nSubGroupID != obj.SubGroupID && bFirstRow==true)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        ExcelTool.FillCell(sheet, nRowIndex, 4, dBudgetAmount.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, 5, dAchiveAmount.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, 6, dVarianceAmount.ToString(), true);
                        ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                        ExcelTool.FillCell(sheet, nRowIndex, 7, dAchievePercent.ToString(), true);
                        ExcelTool.Formatter = " ";
                        dBudgetAmount = 0.0;
                        dAchiveAmount = 0.0;
                        dVarianceAmount = 0.0;
                        dAchievePercent = 0.0;
                        sAchievePercent = dAchievePercent + " %";

                        nStartCol = 2;
                        nRowIndex++;
                    }
                    if (nSubGroupID != obj.SubGroupID)
                    {
                        EnumComponentType oName = (EnumComponentType)obj.ComponentID;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SubGroupCode.ToString(), false);
                        ExcelTool.FillCellMerge(ref sheet, obj.SubGroupName + "  (" + oName + ")", nRowIndex, nRowIndex++, nStartCol, 7, true, ExcelHorizontalAlignment.Left, true);
                        nStartCol = 2;
                    }
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.AccountCode.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "           " + obj.AccountHeadName.ToString(), false);
                    ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BudgetAmount.ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualAmount.ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.VarianceAmount.ToString(), true);
                    ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.AchievePercent.ToString(), true);
                    ExcelTool.Formatter = "";

                    nRowIndex++;
                    dBudgetAmount = obj.BudgetAmount + dBudgetAmount;
                    dAchiveAmount = obj.ActualAmount + dAchiveAmount;
                    dVarianceAmount = obj.VarianceAmount + dVarianceAmount;
                    dAchievePercent = dAchiveAmount / dBudgetAmount * 100;
                    nSubGroupID = obj.SubGroupID;
                    bFirstRow = true;
                    sheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                ExcelTool.FillCell(sheet, nRowIndex, 4, dBudgetAmount.ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, 5, dAchiveAmount.ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, 6, dVarianceAmount.ToString(), true);
                ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                ExcelTool.FillCell(sheet, nRowIndex, 7, dAchievePercent.ToString(), true);
                ExcelTool.Formatter = " ";



                #endregion

                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Budget Variance Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public void ExcelReportMonthly(string sParams)
        {
            int nCount = 0;
            int nBUID = 0;
            BudgetVariance oBudgetVariance = new BudgetVariance();
            BudgetVariance oTempBudgetVariance = new BudgetVariance();
            List<BudgetVariance> oBudgetVariances = new List<BudgetVariance>();
            if (!string.IsNullOrEmpty(sParams))
            {
                nBUID = Convert.ToInt32(sParams.Split('~')[nCount++]);
                oTempBudgetVariance.BudgetID = Convert.ToInt32(sParams.Split('~')[nCount++]);
                oTempBudgetVariance.ReportType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                oTempBudgetVariance.IsApproved = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                oTempBudgetVariance.StartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                oTempBudgetVariance.EndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
            }
            oBudgetVariances = BudgetVariance.GetsReport(oTempBudgetVariance.BudgetID, oTempBudgetVariance.ReportType, oTempBudgetVariance.IsApproved, oTempBudgetVariance.StartDateSt, oTempBudgetVariance.EndDateSt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBudgetVariances.Count < 1) return;
            oBudgetVariance = oBudgetVariances[0];
            List<BudgetVariance> oBudgetVarianceMonthNames = oBudgetVariances.Where(x => x.AccountHeadID == oBudgetVariance.AccountHeadID).ToList();            
            List<BudgetVariance> oBudgetVarianceMonthName = oBudgetVarianceMonthNames.OrderBy(x => x.MonthNo).ToList();            
            int nMonthDifference = oBudgetVarianceMonthName.Count;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(0, ((User)Session[SessionInfo.CurrentUser]).UserID);


            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = 6 + (nMonthDifference * 4) + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Budget Variance");
                sheet.Name = "Budget Variance(Monthly)";

                //ExcelTool.SetColumnWidth(Table_headerFirst, ref sheet, ref nStartCol, ref nEndCol);
                sheet.DefaultColWidth = 15;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Budget Variance(Monthly)   Date: (" + oTempBudgetVariance.StartDateSt + " to " + oTempBudgetVariance.StartDateSt + ")"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data


                nRowIndex++;
                nStartCol = 2;
                //ExcelTool.GenerateHeader(table_headerFirst, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                nCount = 0;


                #region first header
                int nFixedBlankCol = 3;
                nStartCol = 2;
                int nForFirst = 2;
                int SaveRowIndex = nRowIndex;
                ExcelTool.FillCellMerge(ref sheet, "Account Head", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Center, false);
                nStartCol = 4;
                for (int i = 1; i <= nMonthDifference; i++)
                {
                    ExcelTool.FillCellMerge(ref sheet, oBudgetVarianceMonthName[i - 1].NameofMonth, nRowIndex, nRowIndex, nStartCol , nStartCol + 3, true, ExcelHorizontalAlignment.Center, false);
                    nForFirst = 0;
                    nStartCol = nStartCol + 4;

                }
                ExcelTool.FillCellMerge(ref sheet, "Total Summary", nRowIndex, nRowIndex, nStartCol, nStartCol + 3, true, ExcelHorizontalAlignment.Center, false);

                nStartCol = 2;
                nRowIndex++;
                #endregion

                #region second header
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "A/C Code", false, true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "A/C Name", false, true);
                for (int i = 1; i <= nMonthDifference; i++)
                {
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Budget", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Actual", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Varience", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Achieve", false, true);
                }
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Total Budget", false, true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Total Actual", false, true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Total Varience", false, true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Total Achieve", false, true);

                nRowIndex++;
                nStartCol = 2;
                #endregion

                #region column width
                for(int i = 1; i<=7 +nMonthDifference*4; i++)
                {
                    sheet.Column(i).Width = 15;
                    //sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 35;
                //sheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //sheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                #endregion
                int nSubGroupID = 0;
                bool bFirstRow = true;

                double dFinalTotalBudgetAmount = 0.00;
                double dFinalTotalActualAmount = 0.00;
                double dFinalTotalVarianceAmount = 0.00;
                double dFinalTotalAchievePercent = 0.00;

                double dGrandFinalTotalBudgetAmount = 0.00;
                double dGranFinalTotalActualAmount = 0.00;
                double dGranFinalTotalVarianceAmount = 0.00;
                double dGranFinalTotalAchievePercent = 0.00;
                int nComponentID = 0;
                bool IsSubTotalPrint = false;
                bool IsTotalPrint = false;


                List<BudgetVariance> objBudgetVariances = oBudgetVariances.GroupBy(x => x.SubGroupID).Select(x => x.First()).ToList();
                foreach (BudgetVariance objRow in objBudgetVariances)
                {
                    List<BudgetVariance> objSubGroupWiseLedgers = oBudgetVariances.Where(x => x.SubGroupID == objRow.SubGroupID).ToList();
                    List<BudgetVariance> objDistinctLedgers = objSubGroupWiseLedgers.GroupBy(x => x.AccountHeadID).Select(x => x.First()).ToList();
                    List<BudgetVariance> objForMonth = objDistinctLedgers.OrderBy(x => x.MonthNo).ToList();  

                    foreach (BudgetVariance oLedger in objDistinctLedgers)
                    {

                        List<BudgetVariance> objTempForMonth = objSubGroupWiseLedgers.Where(x => x.AccountHeadID == oLedger.AccountHeadID).ToList();
                       
                        if (nSubGroupID != oLedger.SubGroupID && bFirstRow == false)
                        {
                            #region Sub Total
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, false);
                            nStartCol = 4;
                            foreach (BudgetVariance oTemp in objForMonth)
                            {
                                ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.BudgetAmount), 2).ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.ActualAmount), 2).ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.VarianceAmount), 2).ToString(), true);
                                ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.AchievePercent), 2).ToString(), true);
                            }
                            ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalBudgetAmount,2).ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalActualAmount,2).ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalVarianceAmount,2).ToString(), true);
                            ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalAchievePercent, 2).ToString(), true);
                            ExcelTool.Formatter = "";
                            //ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol + 3, true, ExcelHorizontalAlignment.Right, false);
                            dGrandFinalTotalBudgetAmount = dGrandFinalTotalBudgetAmount + dFinalTotalBudgetAmount;
                            dGranFinalTotalActualAmount = dGranFinalTotalActualAmount + dFinalTotalActualAmount;
                            dGranFinalTotalVarianceAmount = dGranFinalTotalVarianceAmount + dFinalTotalVarianceAmount;
                            dGranFinalTotalAchievePercent = dGranFinalTotalAchievePercent + dFinalTotalAchievePercent;

                            dFinalTotalBudgetAmount = 0.00;
                            dFinalTotalActualAmount = 0.00;
                            dFinalTotalVarianceAmount = 0.00;
                            dFinalTotalAchievePercent = 0.00;
                            nStartCol = 2;
                            nRowIndex++;
                            IsSubTotalPrint = true;

                            #endregion
                        }
                        if (nComponentID != oLedger.ComponentID && bFirstRow == false)
                        {
                            #region Total
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Total:", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, false);
                            nStartCol = 4;
                            foreach (BudgetVariance oTemp in objForMonth)
                            {
                                ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.BudgetAmount), 2).ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.ActualAmount), 2).ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.VarianceAmount), 2).ToString(), true);
                                ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.AchievePercent), 2).ToString(), true);
                            }
                            ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGrandFinalTotalBudgetAmount, 2).ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGranFinalTotalActualAmount, 2).ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGranFinalTotalVarianceAmount, 2).ToString(), true);
                            ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGranFinalTotalAchievePercent, 2).ToString(), true);
                            ExcelTool.Formatter = "";
                            //ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol + 3, true, ExcelHorizontalAlignment.Right, false);
                            dGrandFinalTotalBudgetAmount = 0.0;
                            dGranFinalTotalActualAmount = 0.0;
                            dGranFinalTotalVarianceAmount = 0.0;
                            dGranFinalTotalAchievePercent = 0.0;
                            dFinalTotalBudgetAmount = 0.00;
                            dFinalTotalActualAmount = 0.00;
                            dFinalTotalVarianceAmount = 0.00;
                            dFinalTotalAchievePercent = 0.00;
                            nStartCol = 2;
                            nRowIndex++;
                            IsTotalPrint = true;
                            IsSubTotalPrint = true;
                            #endregion
                        }
                        bFirstRow = false;
                        if (nSubGroupID != oLedger.SubGroupID)
                        {
                            EnumComponentType sComponentName = (EnumComponentType)oLedger.ComponentID;
                            nComponentID = oLedger.ComponentID; ;
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oLedger.SubGroupCode.ToString(), false, true);
                            ExcelTool.FillCellMerge(ref sheet, oLedger.SubGroupName + " (" + sComponentName.ToString() + ")", nRowIndex, nRowIndex++, nStartCol, 7 + nMonthDifference * 4, true, ExcelHorizontalAlignment.Left, false);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oLedger.AccountCode, false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, " "+oLedger.AccountHeadName, false);

                        foreach (BudgetVariance oTemp in objForMonth)
                        {
                            ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oTemp.BudgetAmount.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oTemp.ActualAmount.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oTemp.VarianceAmount.ToString(), true);
                            ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oTemp.AchievePercent.ToString(), true);
                        }
                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, objRow.TotalBudgetAmount.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, objRow.TotalActualAmount.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, objRow.TotalVarianceAmount.ToString(), true);
                        ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, objRow.TotalAchievePercent.ToString(), true);

                        dFinalTotalBudgetAmount = dFinalTotalBudgetAmount + objRow.TotalBudgetAmount;
                        dFinalTotalActualAmount = dFinalTotalActualAmount + objRow.TotalActualAmount;
                        dFinalTotalVarianceAmount = dFinalTotalVarianceAmount + objRow.TotalVarianceAmount;
                        dFinalTotalAchievePercent = dFinalTotalAchievePercent + objRow.TotalAchievePercent;

                        nRowIndex++;
                        nSubGroupID = oLedger.SubGroupID;
                        nStartCol = 2;
                        
                        
                    }
                    //if (IsSubTotalPrint == false) { 
                    //#region Sub Total
                    //nStartCol = 2;
                    //ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, false);
                    //nStartCol = 4;
                    //foreach (BudgetVariance oTemp in objForMonth)
                    //{
                    //    ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.BudgetAmount), 2).ToString(), true);
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.ActualAmount), 2).ToString(), true);
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.VarianceAmount), 2).ToString(), true);
                    //    ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.SubGroupID == nSubGroupID && x.MonthNo.Equals(oTemp.MonthNo)).Sum(x => x.AchievePercent), 2).ToString(), true);
                    //}
                    //ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalBudgetAmount, 2).ToString(), true);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalActualAmount, 2).ToString(), true);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalVarianceAmount, 2).ToString(), true);
                    //ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dFinalTotalAchievePercent, 2).ToString(), true);
                    //ExcelTool.Formatter = "";
                    ////ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol + 3, true, ExcelHorizontalAlignment.Right, false);
                    //dGrandFinalTotalBudgetAmount = dGrandFinalTotalBudgetAmount + dFinalTotalBudgetAmount;
                    //dGranFinalTotalActualAmount = dGranFinalTotalActualAmount + dFinalTotalActualAmount;
                    //dGranFinalTotalVarianceAmount = dGranFinalTotalVarianceAmount + dFinalTotalVarianceAmount;
                    //dGranFinalTotalAchievePercent = dGranFinalTotalAchievePercent + dFinalTotalAchievePercent;

                    //dFinalTotalBudgetAmount = 0.00;
                    //dFinalTotalActualAmount = 0.00;
                    //dFinalTotalVarianceAmount = 0.00;
                    //dFinalTotalAchievePercent = 0.00;
                    //nStartCol = 2;
                    //nRowIndex++;

                    //#endregion
                    //}
                    //IsSubTotalPrint = false;
                    //if (IsTotalPrint == false) { 
                    //#region Total
                    //nStartCol = 2;
                    //ExcelTool.FillCellMerge(ref sheet, "Total:", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, false);
                    //nStartCol = 4;
                    //foreach (BudgetVariance oTemp in objForMonth)
                    //{
                    //    ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.BudgetAmount), 2).ToString(), true);
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.ActualAmount), 2).ToString(), true);
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.VarianceAmount), 2).ToString(), true);
                    //    ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                    //    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oBudgetVariances.Where(x => x.ComponentID == nComponentID).Sum(x => x.AchievePercent), 2).ToString(), true);
                    //}
                    //ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGrandFinalTotalBudgetAmount, 2).ToString(), true);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGranFinalTotalActualAmount, 2).ToString(), true);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGranFinalTotalVarianceAmount, 2).ToString(), true);
                    //ExcelTool.Formatter = " ##0.00%;(##0.00%)";
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(dGranFinalTotalAchievePercent, 2).ToString(), true);
                    //ExcelTool.Formatter = "";
                    ////ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol + 3, true, ExcelHorizontalAlignment.Right, false);
                    //dGrandFinalTotalBudgetAmount = 0.0;
                    //dGranFinalTotalActualAmount = 0.0;
                    //dGranFinalTotalVarianceAmount = 0.0;
                    //dGranFinalTotalAchievePercent = 0.0;
                    //dFinalTotalBudgetAmount = 0.00;
                    //dFinalTotalActualAmount = 0.00;
                    //dFinalTotalVarianceAmount = 0.00;
                    //dFinalTotalAchievePercent = 0.00;
                    //nStartCol = 2;
                    //nRowIndex++;
                    //#endregion
                    //}
                    //IsTotalPrint = false;
                    //IsSubTotalPrint = false;
                }

                #endregion
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Budget Variance Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion
        }




        #endregion
       

    }
}


