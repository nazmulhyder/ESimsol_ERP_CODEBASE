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
    public class SalarySchemeGradeController : PdfViewController
    {
        #region Declaration
        SalarySchemeGrade _oSalarySchemeGrade = new SalarySchemeGrade();
        List<SalarySchemeGrade> _oSalarySchemeGrades = new List<SalarySchemeGrade>();
        #endregion

        #region   SalarySchemeGrade

        public ActionResult ViewSalarySchemeGrades(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oSalarySchemeGrades = new List<SalarySchemeGrade>();
            string sSQL = "Select * from SalarySchemeGrade";
            _oSalarySchemeGrades = SalarySchemeGrade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oSalarySchemeGrade = this.MakeTree(_oSalarySchemeGrades);

            sSQL = "Select * from SalaryScheme Where IsActive=1";
            ViewBag.SalarySchemes = SalaryScheme.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oSalarySchemeGrade);
        }


        [HttpPost]
        public JsonResult Save(SalarySchemeGrade oSalarySchemeGrade)
        {
            try
            {
                if (oSalarySchemeGrade.SSGradeID <= 0)
                {
                    oSalarySchemeGrade = oSalarySchemeGrade.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oSalarySchemeGrade = oSalarySchemeGrade.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oSalarySchemeGrade = new SalarySchemeGrade();
                oSalarySchemeGrade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemeGrade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SalarySchemeGrade oSalarySchemeGrade)
        {
            try
            {
                if (oSalarySchemeGrade.SSGradeID <= 0) { throw new Exception("Please select an valid item."); }
                oSalarySchemeGrade = oSalarySchemeGrade.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalarySchemeGrade = new SalarySchemeGrade();
                oSalarySchemeGrade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemeGrade.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChange(SalarySchemeGrade oSalarySchemeGrade)
        {
            try
            {
                if (oSalarySchemeGrade.SSGradeID <= 0) { throw new Exception("Please select an valid item."); }
                oSalarySchemeGrade.IsActive = !oSalarySchemeGrade.IsActive;
                oSalarySchemeGrade = oSalarySchemeGrade.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalarySchemeGrade = new SalarySchemeGrade();
                oSalarySchemeGrade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemeGrade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Get(SalarySchemeGrade oSalarySchemeGrade)
        {
            try
            {
                if (oSalarySchemeGrade.SSGradeID <= 0) { throw new Exception("Please select an valid item."); }
                oSalarySchemeGrade = SalarySchemeGrade.Get(oSalarySchemeGrade.SSGradeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalarySchemeGrade = new SalarySchemeGrade();
                oSalarySchemeGrade.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemeGrade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Gets(SalarySchemeGrade oSalarySchemeGrade)
        {
            _oSalarySchemeGrades = new List<SalarySchemeGrade>();
            try
            {
                string sSQL = "Select * from SalarySchemeGrade Where SSGradeID <> 0  ";
                _oSalarySchemeGrades = SalarySchemeGrade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSalarySchemeGrades = new List<SalarySchemeGrade>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalarySchemeGrades);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetApproveTree(SalarySchemeGrade oSalarySchemeGrade)
        {
            _oSalarySchemeGrades = new List<SalarySchemeGrade>();
            _oSalarySchemeGrade = new SalarySchemeGrade();
            try
            {
                string sSQL = "Select * from SalarySchemeGrade Where IsActive=1 And SSGradeID Not IN (Select SSGradeID From SalarySchemeGrade Where ParentID In (Select SSGradeID from SalarySchemeGrade Where IsActive=0 AND IsLastLayer=0))";
                _oSalarySchemeGrades = SalarySchemeGrade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oSalarySchemeGrade = this.MakeTree(_oSalarySchemeGrades);
            }
            catch (Exception ex)
            {
                _oSalarySchemeGrade = new SalarySchemeGrade();
                _oSalarySchemeGrade.ErrorMessage=ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalarySchemeGrade);
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

        #region Tree Generator


        private SalarySchemeGrade MakeTree(List<SalarySchemeGrade> oSSGs)
        {
            SalarySchemeGrade oSSG=new SalarySchemeGrade();
            if (oSSGs.Count() > 0)
            {
                oSSG = oSSGs.Where(x => x.SSGradeID == 1 && x.ParentID == 0).ElementAtOrDefault(0);
                this.AddChildNode(ref oSSG, oSSGs.Where(x => x.SSGradeID != oSSG.SSGradeID).ToList());
            }
            return oSSG;
        }

        public void AddChildNode(ref SalarySchemeGrade oSSG, List<SalarySchemeGrade> oSSGs)
        {
            int nParentID=oSSG.SSGradeID;
            oSSG.children = oSSGs.Where(x => x.ParentID == nParentID).ToList();
            if (oSSG.children.Count() > 0 && oSSGs.Count() > 0)
            {
                oSSGs.RemoveAll(x => x.ParentID == nParentID);
                foreach (SalarySchemeGrade OItem in oSSG.children)
                {
                    SalarySchemeGrade oTempNode = OItem;
                    this.AddChildNode(ref oTempNode, oSSGs);
                }
            }
            
        }


        #endregion
    }
}