using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region ExportPIShipment
    public class ExportPIShipment
    {
        public ExportPIShipment()
        {
            ExportPIShipmentID = 0;
            ExportPIID = 0;
            ExportBillID = 0;
            ShipmentBy = "";
            DestinationPort = "";
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";
            Remarks = "";

        }
        #region Property
        public int ExportPIShipmentID { get; set; }
        public int ExportPIID { get; set; }
        public int ExportBillID { get; set; }
        public string ShipmentBy{ get; set; }
        public string DestinationPort { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }
        public string Remarks { get; set; }

        #endregion
        #region Derived Property
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region Functions
        public static List<ExportPIShipment> Gets(long nUserID)
        {
            return ExportPIShipment.Service.Gets(nUserID);
        }
        public static List<ExportPIShipment> Gets(string sSQL, long nUserID)
        {
            return ExportPIShipment.Service.Gets(sSQL, nUserID);
        }
        public ExportPIShipment Get(int id, long nUserID)
        {
            return ExportPIShipment.Service.Get(id, nUserID);
        }
        public ExportPIShipment GetByExportPIID(int ExportPIID, long nUserID)
        {
            return ExportPIShipment.Service.GetByExportPIID(ExportPIID, nUserID);
        }
        public ExportPIShipment Save(long nUserID)
        {
            return ExportPIShipment.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ExportPIShipment.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IExportPIShipmentService Service
        {
            get { return (IExportPIShipmentService)Services.Factory.CreateService(typeof(IExportPIShipmentService)); }
        }
        #endregion
    }
    #endregion
    #region IExportPIShipment interface
    public interface IExportPIShipmentService
    {
        ExportPIShipment Get(int id, Int64 nUserID);
        ExportPIShipment GetByExportPIID(int ExportPIID, Int64 nUserID);
        List<ExportPIShipment> Gets(Int64 nUserID);
        List<ExportPIShipment> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ExportPIShipment Save(ExportPIShipment oExportPIShipment, Int64 nUserID);
    }
    #endregion
}
