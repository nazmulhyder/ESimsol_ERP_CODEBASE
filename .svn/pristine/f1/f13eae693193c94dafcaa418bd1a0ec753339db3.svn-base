using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region DBOperationArchive

    public class DBOperationArchive : BusinessObject
    {
        public DBOperationArchive()
        {
            DBOperationArchiveID = 0;
            BUID = 0;
            DBOperationType = EnumDBOperation.None;
            ModuleName = EnumModuleName.None;
            DBRefObjID = 0;
            RefText = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
            UserName = "";
            BUName = "";
            IsDateSearch = false;
        }

        #region Properties

        public int DBOperationArchiveID { get; set; }
        public int BUID { get; set; }
        public EnumDBOperation DBOperationType { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int DBRefObjID { get; set; }
        public string RefText { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string UserName { get; set; }
        public string BUName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDateSearch { get; set; }

        #endregion

        #region Derived Property

        public string DBServerDateTimeSt
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy HH:mm:ss");
            }
        }
        public string ModuleNameSt
        {
            get
            {
                return EnumObject.jGet(this.ModuleName);
            }
        }
        public string DBOperationTypeSt
        {
            get
            {
                return EnumObject.jGet(this.DBOperationType);
            }
        }
        #endregion

        #region Functions

        public static List<DBOperationArchive> Gets(string sSQL, int nCurrentUserID)
        {
            return DBOperationArchive.Service.Gets(sSQL, nCurrentUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IDBOperationArchiveService Service
        {
            get { return (IDBOperationArchiveService)Services.Factory.CreateService(typeof(IDBOperationArchiveService)); }
        }

        #endregion
    }

    #endregion

    #region IDBOperationArchive interface

    public interface IDBOperationArchiveService
    {
        List<DBOperationArchive> Gets(string sSQL, int nCurrentUserID);
    }

    #endregion
}
