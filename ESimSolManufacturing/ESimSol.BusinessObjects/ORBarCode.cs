using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;


namespace ESimSol.BusinessObjects
{
    #region ORBarCode

    public class ORBarCode : BusinessObject
    {
        public ORBarCode()
        {
            ORBarCodeID = 0;
            OrderRecapID = 0;
            ColorID = 0;
            SizeID = 0;
            BarCode = "";
            ColorName = "";
            SizeName = "";
            OrderRecapNo = "";
            ORBarCodeLogID = 0;
            OrderRecapLogID = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int ORBarCodeID { get; set; }

        public int OrderRecapID { get; set; }

        public int ColorID { get; set; }

        public int SizeID { get; set; }

        public int ORBarCodeLogID { get; set; }

        public int OrderRecapLogID { get; set; }

        public string BarCode { get; set; }

        public string ColorName { get; set; }

        public string SizeName { get; set; }

        public string OrderRecapNo { get; set; }


        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions

        public static List<ORBarCode> Gets(int nOrderRecapID, long nUserID)
        {
            return ORBarCode.Service.Gets(nOrderRecapID, nUserID);
        }
        public static List<ORBarCode> GetsByLog(int id, long nUserID) //OrderRecapLogId
        {
            return ORBarCode.Service.GetsByLog(id, nUserID);
        }
        public static List<ORBarCode> Gets(string sSQL, long nUserID)
        {
            return ORBarCode.Service.Gets(sSQL, nUserID);
        }
        public ORBarCode Get(int nORBarCodeID, long nUserID)
        {
            return ORBarCode.Service.Get(nORBarCodeID, nUserID);
        }
        public ORBarCode Save(long nUserID)
        {
            return ORBarCode.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IORBarCodeService Service
        {
            get { return (IORBarCodeService)Services.Factory.CreateService(typeof(IORBarCodeService)); }
        }

        #endregion
    }
    #endregion

    #region IORBarCode interface

    public interface IORBarCodeService
    {

        ORBarCode Get(int nORBarCodeID, Int64 nUserID);

        List<ORBarCode> Gets(int nOrderRecapID, Int64 nUserID);

        List<ORBarCode> Gets(string sSQL, Int64 nUserID);

        List<ORBarCode> GetsByLog(int id, Int64 nUserID);   //ORBarCodeLogID

        ORBarCode Save(ORBarCode oORBarCode, Int64 nUserID);
    }
    #endregion   
}
