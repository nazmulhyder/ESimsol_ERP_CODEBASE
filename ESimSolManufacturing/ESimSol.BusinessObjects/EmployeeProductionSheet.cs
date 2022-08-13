using System;
using System.IO;
using ICS.Base.Client.BOFoundation;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Base.Client.ServiceVessel;
using ICS.Base.Client.Utility;


namespace ESimSol.BusinessObjects
{
    #region EmployeeProductionSheet
    [DataContract]
    public class EmployeeProductionSheet : BusinessObject
    {
        public EmployeeProductionSheet()
        {
            EPSID = 0;
            EPSNO="";
            EmployeeID=0;
            OrderRecapDetailID=0;
            ProductionProcess = EnumProductionProcess.None;
            GarmentPart = EnumGarmentPart.None;
            ErrorMessage = "";
            
        }

        #region Properties
        [DataMember]
        public int EPSID { get; set; }
        [DataMember]
        public string EPSNO { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public int OrderRecapDetailID { get; set; }
        [DataMember]
        public EnumProductionProcess ProductionProcess { get; set; }
        [DataMember]
        public EnumGarmentPart GarmentPart { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
       
        public int ProductionProcessInt { get; set; }
        public string ProductionProcessInString
        {
            get
            {
                return ProductionProcess.ToString();
            }
        }

        public int GarmentPartInt { get; set; }
        public string GarmentPartInString
        {
            get
            {
                return GarmentPart.ToString();
            }
        }

        #endregion

        #region Functions
        public static EmployeeProductionSheet Get(int id, Guid wcfSessionid)
        {
            return (EmployeeProductionSheet)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Get", id)[0];
        }

        public static List<EmployeeProductionSheet> Gets(Guid wcfSessionid)
        {
            return (List<EmployeeProductionSheet>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets")[0];
        }

        public static List<EmployeeProductionSheet> Gets(string sSQL, Guid wcfSessionid)
        {
            return (List<EmployeeProductionSheet>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets", sSQL)[0];
        }

        public EmployeeProductionSheet IUD(int nDBOperation, Guid wcfSessionid)
        {
            return (EmployeeProductionSheet)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "IUD", this, nDBOperation)[0];
        }

       

        #endregion

        #region ServiceFactory

        internal static Type ServiceType
        {
            get
            {
                return typeof(IEmployeeProductionSheetService);
            }
        }
        #endregion
    }
    #endregion

    #region IEmployeeProductionSheet interface
    [ServiceContract]
    public interface IEmployeeProductionSheetService
    {
        [OperationContract]
        EmployeeProductionSheet Get(int id, Int64 nUserID);

        [OperationContract]
        List<EmployeeProductionSheet> Gets(Int64 nUserID);

        [OperationContract]
        List<EmployeeProductionSheet> Gets(string sSQL, Int64 nUserID);

        [OperationContract]
        EmployeeProductionSheet IUD(EmployeeProductionSheet oEmployeeProductionSheet, int nDBOperation, Int64 nUserID);

        
    }
    #endregion
}
