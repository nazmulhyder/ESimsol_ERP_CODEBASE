using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region GratuityScheme

    public class GratuityScheme : BusinessObject
    {
        public GratuityScheme()
        {
            GSID = 0;
            Name="";
            Description="";
            ApproveBy = 0;
            ApproveByDate= DateTime.Now;
            InactiveDate = DateTime.Now;
            ErrorMessage = "";
            EncryptedID = "";
            ApproveByNameCode = "";
        }

        #region Properties

        public int GSID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ApproveBy { get; set; }

        public DateTime ApproveByDate { get; set; }

        public DateTime InactiveDate { get; set; }

        public string ErrorMessage { get; set; }
        public string ApproveByNameCode { get; set; }

        #endregion

        #region Derived Property
        public string ApproveByDateInString { get { return (this.ApproveByDate == DateTime.MinValue) ? "" : ApproveByDate.ToString("dd MMM yyyy"); } }
        public string InactiveDateInString { get { return (this.InactiveDate == DateTime.MinValue) ? "" : InactiveDate.ToString("dd MMM yyyy"); } }
        public string ActivityStatus { get { return (this.InactiveDate == DateTime.MinValue) ? "Active" : "InActive"; } }
        public string EncryptedID { get; set; }
        public List<GratuitySchemeDetail> GratuitySchemeDetails { get; set; }
        #endregion

        #region Functions
        public static GratuityScheme Get(int Id, long nUserID)
        {
            return GratuityScheme.Service.Get(Id, nUserID);
        }
        public static GratuityScheme Get(string sSQL, long nUserID)
        {
            return GratuityScheme.Service.Get(sSQL, nUserID);
        }
        public static List<GratuityScheme> Gets(long nUserID)
        {
            return GratuityScheme.Service.Gets(nUserID);
        }

        public static List<GratuityScheme> Gets(string sSQL, long nUserID)
        {
            return GratuityScheme.Service.Gets(sSQL, nUserID);
        }

        public GratuityScheme IUD(int nDBOperation, long nUserID)
        {
            return GratuityScheme.Service.IUD(this, nDBOperation, nUserID);
        }

        public  GratuityScheme Activity( long nUserID)
        {
            return GratuityScheme.Service.Activity(this, nUserID);
        }

        public  GratuityScheme Approve(long nUserID)
        {
            return GratuityScheme.Service.Approve(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IGratuitySchemeService Service
        {
            get { return (IGratuitySchemeService)Services.Factory.CreateService(typeof(IGratuitySchemeService)); }
        }

        #endregion
    }
    #endregion

    #region IGratuityScheme interface

    public interface IGratuitySchemeService
    {
        GratuityScheme Get(int id, Int64 nUserID);
        GratuityScheme Get(string sSQL, Int64 nUserID);
        List<GratuityScheme> Gets(Int64 nUserID);
        List<GratuityScheme> Gets(string sSQL, Int64 nUserID);
        GratuityScheme IUD(GratuityScheme oGratuityScheme, int nDBOperation, Int64 nUserID);
        GratuityScheme Activity(GratuityScheme oGratuityScheme, Int64 nUserID);
        GratuityScheme Approve(GratuityScheme oGratuityScheme, Int64 nUserID);

    }
    #endregion
}
