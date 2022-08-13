using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class InspectionCertificateController : Controller
    {

        #region Declartion
        InspectionCertificate _oInspectionCertificate = new InspectionCertificate();
        List<InspectionCertificate> _oInspectionCertificates = new List<InspectionCertificate>();
        InspectionCertificateDetail _oInspectionCertificateDetail = new InspectionCertificateDetail();
        List<InspectionCertificateDetail> _oInspectionCertificateDetails = new List<InspectionCertificateDetail>();
        CommercialInvoice _oCommercialInvoice = new CommercialInvoice();
        List<CommercialInvoiceDetail> _oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();

        ApprovalRequest _oApprovalRequest = new ApprovalRequest();

        #endregion

        #region function
        private bool HaveRateViewPermission(EnumRoleOperationType OperationType)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InspectionCertificate).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            if ((int)Session[SessionInfo.currentUserID] == -9)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < oAuthorizationRoleMappings.Count; i++)
                {
                    if (oAuthorizationRoleMappings[i].OperationType == OperationType && oAuthorizationRoleMappings[i].ModuleName == EnumModuleName.InspectionCertificate)
                    {
                        return true;

                    }

                }
            }

            return false;
        }
        #endregion

        #region Add, Edit, Delete

        #region Inspection Certificate Entry Module 
        public ActionResult ViewInspectionCertificate(int id) //commercial Invoice ID
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser( ((int)EnumModuleName.InspectionCertificate).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oInspectionCertificate = new InspectionCertificate();
            _oCommercialInvoice = new CommercialInvoice();
            _oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
            _oInspectionCertificateDetails = new List<InspectionCertificateDetail>();

            _oInspectionCertificate = _oInspectionCertificate.Get(id, (int)Session[SessionInfo.currentUserID]); ///commercial Invoice ID
            if (_oInspectionCertificate.InspectionCertificateID > 0)
            {
                _oInspectionCertificate.InspectionCertificateDetails = InspectionCertificateDetail.Gets(_oInspectionCertificate.InspectionCertificateID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oCommercialInvoice = _oCommercialInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCommercialInvoiceDetails = CommercialInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                _oInspectionCertificate.CommercialInvoiceID = _oCommercialInvoice.CommercialInvoiceID;
                _oInspectionCertificate.InvoiceNo = _oCommercialInvoice.InvoiceNo;
                _oInspectionCertificate.InvoiceDate = _oCommercialInvoice.InvoiceDate;
                _oInspectionCertificate.InvoiceValue = _oCommercialInvoice.NetInvoiceAmount;
                _oInspectionCertificate.MasterLCNo = _oCommercialInvoice.MasterLCNo;
                _oInspectionCertificate.MasterLCDate = _oCommercialInvoice.LCDate;
                _oInspectionCertificate.ShipperID = _oCommercialInvoice.ProductionFactoryID;
                _oInspectionCertificate.ShipperName= _oCommercialInvoice.ProductionFactoryName;
                _oInspectionCertificate.ManufacturerID = _oCommercialInvoice.ProductionFactoryID;
                _oInspectionCertificate.MenufacturerName = _oCommercialInvoice.ProductionFactoryName;
                _oInspectionCertificate.ShipmentMode = _oCommercialInvoice.ShipmentMode;

                foreach(CommercialInvoiceDetail oItem in _oCommercialInvoiceDetails )
                {
                    _oInspectionCertificateDetail = new InspectionCertificateDetail();
                    _oInspectionCertificateDetail.CommercialInvoiceDetailID = oItem.CommercialInvoiceDetailID;
                    _oInspectionCertificateDetail.StyleNo = oItem.StyleNo;
                    _oInspectionCertificateDetail.OrderNo = oItem.OrderNo;
                    _oInspectionCertificateDetail.OrderRecapNo = oItem.OrderRecapNo;
                    _oInspectionCertificateDetail.OrderQty = oItem.InvoiceQty;
                    _oInspectionCertificateDetail.ShipedQty = oItem.InvoiceQty;
                    _oInspectionCertificateDetails.Add(_oInspectionCertificateDetail);
                }
                _oInspectionCertificate.InspectionCertificateDetails = _oInspectionCertificateDetails;

            }
            _oInspectionCertificate.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);                        
            return View(_oInspectionCertificate);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(InspectionCertificate oInspectionCertificate)
        {
            _oInspectionCertificate = new InspectionCertificate();
            try
            {
               _oInspectionCertificate = oInspectionCertificate.Save((int)Session[SessionInfo.currentUserID]);
               if (_oInspectionCertificate.CommercialInvoiceID > 0)
               {
                   _oInspectionCertificate.CommercialInvoice = _oCommercialInvoice.Get(_oInspectionCertificate.CommercialInvoiceID, (int)Session[SessionInfo.currentUserID]);
               }
            }
            catch (Exception ex)
            {
                _oInspectionCertificate = new InspectionCertificate();
                _oInspectionCertificate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oInspectionCertificate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ICPreview 

        public ActionResult ICPreview(int id)
        {
            _oInspectionCertificate = new InspectionCertificate();
            _oInspectionCertificate = _oInspectionCertificate.GetIC(id, (int)Session[SessionInfo.currentUserID]);
            _oInspectionCertificate.InspectionCertificateDetails = InspectionCertificateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptIC oReport = new rptIC();
            byte[] abytes = oReport.PrepareReport(_oInspectionCertificate, oCompany);
            return File(abytes, "application/pdf");
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

        #endregion
    }
}
