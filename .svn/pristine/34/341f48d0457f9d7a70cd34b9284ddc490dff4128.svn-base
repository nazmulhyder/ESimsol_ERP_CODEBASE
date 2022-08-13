using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region FabricBatchQCCheck
    public class FabricBatchQCCheck
    {
        public FabricBatchQCCheck()
        {
            FabricBatchQCCheckID = 0;
            FabricQCParNameID = 0;
            FabricBatchQCID = 0;
            Note = "";
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";
            Name = "";

        }
        #region Property
        public int FabricBatchQCCheckID { get; set; }
        public int FabricQCParNameID { get; set; }
        public int FabricBatchQCID { get; set; }
        public string Note { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }
        public string Name { get; set; }

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
        public static List<FabricBatchQCCheck> Gets(long nUserID)
        {
            return FabricBatchQCCheck.Service.Gets(nUserID);
        }
        public static List<FabricBatchQCCheck> Gets(string sSQL, long nUserID)
        {
            return FabricBatchQCCheck.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchQCCheck Get(int id, long nUserID)
        {
            return FabricBatchQCCheck.Service.Get(id, nUserID);
        }
        public FabricBatchQCCheck Save(long nUserID)
        {
            return FabricBatchQCCheck.Service.Save(this, nUserID);
        }
        public List<FabricBatchQCCheck> SaveList(List<FabricBatchQCCheck> oFabricBatchQCChecks, long nUserID)
        {
            return FabricBatchQCCheck.Service.SaveList(oFabricBatchQCChecks, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricBatchQCCheck.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricBatchQCCheckService Service
        {
            get { return (IFabricBatchQCCheckService)Services.Factory.CreateService(typeof(IFabricBatchQCCheckService)); }
        }
        #endregion
    }
    #endregion
    #region IFabricQCParName interface
    public interface IFabricBatchQCCheckService
    {
        FabricBatchQCCheck Get(int id, Int64 nUserID);
        List<FabricBatchQCCheck> Gets(Int64 nUserID);
        List<FabricBatchQCCheck> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricBatchQCCheck Save(FabricBatchQCCheck oFabricBatchQCCheck, Int64 nUserID);
        List<FabricBatchQCCheck> SaveList(List<FabricBatchQCCheck> oFabricBatchQCChecks, Int64 nUserID);
    }
    #endregion
}
