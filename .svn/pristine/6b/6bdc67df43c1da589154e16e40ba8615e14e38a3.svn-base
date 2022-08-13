using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region ReportLayout
    
    public class ReportLayout : BusinessObject
    {
        public ReportLayout()
        {
            ReportLayoutID = 0;
            ReportNo = "";
            ReportName = "";
            ReportType = EnumReportLayout.None;
            OperationType = EnumModuleName.None;
            ErrorMessage = "";

        }

        #region Properties
         
        public int ReportLayoutID { get; set; }
         
        public string ReportNo { get; set; }
         
        public string ReportName { get; set; }

        public EnumReportLayout ReportType { get; set; }
         
        public EnumModuleName OperationType { get; set; }
         
        public int ReportTypeInInt { get; set; }
         
        public int OperationTypeInInt { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ReportLayout> ReportLayouts { get; set; }
        public Company Company { get; set; }
        //public List<OperationType> OperationTypes { get; set; }
        //public List<ReportType> ReportTypes { get; set; }
        public string ReportTypeInString
        {
            get
            {
                return ReportType.ToString();
            }
        }

        public string OperationTypeInString
        {
            get
            {
                return OperationType.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<ReportLayout> Gets(long nUserID)
        {
            return ReportLayout.Service.Gets( nUserID);
        }

        public static List<ReportLayout> Gets(EnumModuleName eEnumOperationType, long nUserID)
        {
            return ReportLayout.Service.Gets((int)eEnumOperationType, nUserID);
        }
        public static List<ReportLayout> Gets(string sSQL, long nUserID)
        {
            return ReportLayout.Service.Gets(sSQL, nUserID);
        }

        public ReportLayout Get(int id, long nUserID)
        {
            return ReportLayout.Service.Get(id, nUserID);
        }

        public ReportLayout Save(long nUserID)
        {
            return ReportLayout.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ReportLayout.Service.Delete(id, nUserID);
        }
        

        #endregion

        #region ServiceFactory

     
        internal static IReportLayoutService Service
        {
            get { return (IReportLayoutService)Services.Factory.CreateService(typeof(IReportLayoutService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class ReportLayoutList : List<ReportLayout>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IReportLayout interface
     
    public interface IReportLayoutService
    {
         
        ReportLayout Get(int id, Int64 nUserID);
         
        List<ReportLayout> Gets(Int64 nUserID);
         
        List<ReportLayout> Gets(int eEnumOperationType, Int64 nUserID);
         
        List<ReportLayout> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        ReportLayout Save(ReportLayout oReportLayout, Int64 nUserID);
        
    }
    #endregion
    

}
