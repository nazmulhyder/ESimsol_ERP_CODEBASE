using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;

namespace ESimSol.BusinessObjects
{
    #region ApprovalHead
    public class ApprovalHead
    {
        public ApprovalHead()
        {
            ApprovalHeadID = 0;
            ModuleID = EnumModuleName.None;
            Name = "";
            Sequence = 0;
            BUID = 0;
            ErrorMessage = "";
            RefColName = "";
            RefID = 0;
            ApprovalHeadPersons = new List<ApprovalHeadPerson>();
            DesignationName = "";
            SignatureImage = null;
        }

        #region Properties
        public int ApprovalHeadID { get; set; }
        public bool IsUp { get; set; }
        public EnumModuleName ModuleID { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int BUID { get; set; }
        public string RefColName { get; set; }
        public int RefID { get; set; }
        public string DesignationName { get; set; }
        public System.Drawing.Image SignatureImage { get; set; }
        public string ErrorMessage { get; set; }
        public List<ApprovalHeadPerson> ApprovalHeadPersons { get; set; }
        #endregion

        #region Functions


        public static List<ApprovalHead> Gets(string sSQL, long nUserID)
        {
            return ApprovalHead.Service.Gets(sSQL, nUserID);
        }
        public static ApprovalHead Get(string sSQL, long nUserID)
        {
            return ApprovalHead.Service.Get(sSQL, nUserID);
        }
        public ApprovalHead IUD(int nDBOperation, long nUserID)
        {
            return ApprovalHead.Service.IUD(this, nDBOperation, nUserID);
        }
        public ApprovalHead UpDown(ApprovalHead oApprovalHead, long nUserID)
        {
            return ApprovalHead.Service.UpDown(oApprovalHead, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static IApprovalHeadService Service
        {
            get { return (IApprovalHeadService)Services.Factory.CreateService(typeof(IApprovalHeadService)); }
        }
        #endregion

        public static Image GetImage(AttachDocument oAttachDocument)
        {
            if (oAttachDocument != null)
            {
                if (oAttachDocument.AttachFile != null)
                {
                    string fileDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Content/CompanyLogo.jpg");
                    //string fileDirectory = "~/Content/CompanyLogo.jpg";
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
            else
            {
                return null;
            }
        }
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IApprovalHeadService
    {
        List<ApprovalHead> Gets(string sSQL, Int64 nUserID);
        ApprovalHead Get(string sSQL, Int64 nUserID);
        ApprovalHead IUD(ApprovalHead oApprovalHead, int nDBOperation, Int64 nUserID);
        ApprovalHead UpDown(ApprovalHead oApprovalHead, Int64 nUserID);
       
      
    }
    #endregion

    
}

