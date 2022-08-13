using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Collections;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeLeaveLedgerController : PdfViewController
    {
        #region Declaration
        EmployeeLeaveLedger _oEmployeeLeaveLedger = new EmployeeLeaveLedger();
        List<EmployeeLeaveLedger> _oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
        #endregion

        #region   EmployeeLeaveLedger

        [HttpPost]
        public JsonResult Save(EmployeeLeaveLedger oEmployeeLeaveLedger)
        {
            try
            {
                if (oEmployeeLeaveLedger.EmpLeaveLedgerID <= 0)
                {
                    oEmployeeLeaveLedger = oEmployeeLeaveLedger.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oEmployeeLeaveLedger = oEmployeeLeaveLedger.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oEmployeeLeaveLedger = new EmployeeLeaveLedger();
                oEmployeeLeaveLedger.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLeaveLedger);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(EmployeeLeaveLedger oEmployeeLeaveLedger)
        {
            try
            {
                if (oEmployeeLeaveLedger.EmpLeaveLedgerID <= 0) { throw new Exception("Please select an valid item."); }
                oEmployeeLeaveLedger = oEmployeeLeaveLedger.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLeaveLedger = new EmployeeLeaveLedger();
                oEmployeeLeaveLedger.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLeaveLedger.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

  

        [HttpPost]
        public ActionResult Get(EmployeeLeaveLedger oEmployeeLeaveLedger)
        {
            try
            {
                if (oEmployeeLeaveLedger.EmpLeaveLedgerID <= 0) { throw new Exception("Please select an valid item."); }
                oEmployeeLeaveLedger = oEmployeeLeaveLedger.Get(oEmployeeLeaveLedger.EmpLeaveLedgerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLeaveLedger = new EmployeeLeaveLedger();
                oEmployeeLeaveLedger.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLeaveLedger);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Gets(EmployeeLeaveLedger oEmployeeLeaveLedger)
        {
            _oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            try
            {
                _oEmployeeLeaveLedgers = EmployeeLeaveLedger.GetsActiveLeaveLedger(oEmployeeLeaveLedger.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeLeaveLedgers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region Leave Transfer
        [HttpPost]
        public JsonResult TransferLeave(EmployeeLeaveLedger oEmployeeLeaveLedger)
        {
            List<EmployeeLeaveLedger> oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            try
            {
                int nELLIDFrom = Convert.ToInt32(oEmployeeLeaveLedger.Params.Split('~')[0]);
                int nELLIDTo = Convert.ToInt32(oEmployeeLeaveLedger.Params.Split('~')[1]);
                int nDays = Convert.ToInt32(oEmployeeLeaveLedger.Params.Split('~')[2]);
                string sNote = oEmployeeLeaveLedger.Params.Split('~')[3];

                if (nELLIDFrom <= 0) { throw new Exception("Please select, from where you want to transfer."); }
                else if (nELLIDTo <= 0) { throw new Exception("Please select, where you want to transfer."); }
                else if (nDays <= 0) { throw new Exception("Please enter number of days to transfer."); }

                oEmployeeLeaveLedgers = EmployeeLeaveLedger.TransferLeave(nELLIDFrom, nELLIDTo, nDays, sNote, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLeaveLedger = new EmployeeLeaveLedger();
                oEmployeeLeaveLedger.ErrorMessage = ex.Message;
                oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
                oEmployeeLeaveLedgers.Add(oEmployeeLeaveLedger);
            }
            oEmployeeLeaveLedger = new EmployeeLeaveLedger();
            oEmployeeLeaveLedger.EmployeeLeaveLedgers = oEmployeeLeaveLedgers;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLeaveLedger);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LeaveTransferGets(EmployeeLeaveTransfer oEmployeeLeaveTransfer)
        {
            List<EmployeeLeaveTransfer> oEmployeeLeaveTransfers = new List<EmployeeLeaveTransfer>();
            try
            {
                int nEmpLeaveLedgerID=Convert.ToInt32(oEmployeeLeaveTransfer.Params.Split('~')[0]);
                oEmployeeLeaveTransfers = EmployeeLeaveTransfer.Gets(nEmpLeaveLedgerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oEmployeeLeaveTransfers.Count() <= 0) { throw new Exception("No transfer record found."); }
                else
                {
                    foreach (EmployeeLeaveTransfer oItem in oEmployeeLeaveTransfers)
                    {
                        if (oItem.ELLIDFrom == nEmpLeaveLedgerID) { oItem.TransferStatus = "Out"; oItem.TransferLeaveName = oItem.TransferTo; }
                        if (oItem.ELLIDTo == nEmpLeaveLedgerID) { oItem.TransferStatus = "In"; oItem.TransferLeaveName = oItem.TransferFrom; }
                    }

                    oEmployeeLeaveTransfers = oEmployeeLeaveTransfers.OrderBy(x => x.TransferStatus).ThenBy(x => x.TransferLeaveName).ToList();
                }
            
            }
            catch (Exception ex)
            {
                oEmployeeLeaveTransfers = new List<EmployeeLeaveTransfer>();
                oEmployeeLeaveTransfer = new EmployeeLeaveTransfer();
                oEmployeeLeaveTransfer.ErrorMessage = ex.Message;
                oEmployeeLeaveTransfers.Add(oEmployeeLeaveTransfer);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLeaveTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion 
    }
}