using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportBillRealized
    [DataContract]
    public class ExportBillRealized : BusinessObject
    {
        public ExportBillRealized()
        {
            ExportBillRealizedID = 0;
            ExportBillID = 0;
            Amount = 0;
            CurrencyID = 0;
            CurrencyName = "";
            CCRate = 0;
            InOutType = EnumInOutType.None;
            InOutTypeInt = 0;
            ParticularName = "";
            CCRate=0;
            CurrencyID = 0;
        }

        #region Properties
        public int ExportBillRealizedID { get; set; }
        public int ExportBillID { get; set; }
        public int ExportBillParticularID { get; set; }
        public double Amount { get; set; }
        public EnumInOutType InOutType { get; set; }
         public int InOutTypeInt { get; set; }
         public string ParticularName { get; set; }
         public double CCRate { get; set; }
         public int CurrencyID { get; set; }
         public string CurrencyName { get; set; }
         public string ErrorMessage { get; set; }


        #endregion

        #region Derived Property


        public string InOutTypeSt
        {
            get
            {
                if (this.InOutType == EnumInOutType.Receive) return "Add";
                else if (this.InOutType == EnumInOutType.Disburse) return "Deduct";
                else return "-";
            }
        }
        public string Amountst
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
     
        #endregion

        #region Functions

        public static List<ExportBillRealized> Gets(int nExportBillID, Int64 nUserID)
        {
            return ExportBillRealized.Service.Gets(nExportBillID,nUserID);
        }
        public static List<ExportBillRealized> Gets(string sSQL, Int64 nUserID)
        {
            return ExportBillRealized.Service.Gets(sSQL, nUserID);
        }

        public ExportBillRealized Get(int nId, Int64 nUserID)
        {
            return ExportBillRealized.Service.Get(nId, nUserID);
        }

        public ExportBillRealized Save(Int64 nUserID)
        {
            return ExportBillRealized.Service.Save(this, nUserID);
        }

        public string Delete( Int64 nUserID)
        {
            return ExportBillRealized.Service.Delete(this, nUserID);
        }
      
        #endregion

        #region ServiceFactory

    
        internal static IExportBillRealizedService Service
        {
            get { return (IExportBillRealizedService)Services.Factory.CreateService(typeof(IExportBillRealizedService)); }
        }

        #endregion
    }
    #endregion

    #region IExportBillRealized interface
    [ServiceContract]
    public interface IExportBillRealizedService
    {
        ExportBillRealized Get(int id, Int64 nUserID);        
        List<ExportBillRealized> Gets(int nExportBillID,Int64 nUserID);
        List<ExportBillRealized> Gets(string sSQL, Int64 nUserID);
        string Delete(ExportBillRealized oExportBillRealized, Int64 nUserID);
        ExportBillRealized Save(ExportBillRealized oExportBillRealized, Int64 nUserID);
   
    }
    
    #endregion
}
