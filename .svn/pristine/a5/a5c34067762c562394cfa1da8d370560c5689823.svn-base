using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region FabricQCParName
    public class FabricQCParName
    {
        public FabricQCParName()
        {
            FabricQCParNameID = 0;
            Name = "";
            Note = "";
            SL = 0;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";

        }
        #region Property
        public int FabricQCParNameID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int SL { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

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
        public static List<FabricQCParName> Gets(long nUserID)
        {
            return FabricQCParName.Service.Gets(nUserID);
        }
        public static List<FabricQCParName> Gets(string sSQL, long nUserID)
        {
            return FabricQCParName.Service.Gets(sSQL, nUserID);
        }
        public FabricQCParName Get(int id, long nUserID)
        {
            return FabricQCParName.Service.Get(id, nUserID);
        }
        public FabricQCParName Save(long nUserID)
        {
            return FabricQCParName.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricQCParName.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricQCParNameService Service
        {
            get { return (IFabricQCParNameService)Services.Factory.CreateService(typeof(IFabricQCParNameService)); }
        }
        #endregion
    }
    #endregion
    #region IFabricQCParName interface
    public interface IFabricQCParNameService
    {
        FabricQCParName Get(int id, Int64 nUserID);
        List<FabricQCParName> Gets(Int64 nUserID);
        List<FabricQCParName> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricQCParName Save(FabricQCParName oFabricQCParName, Int64 nUserID);
    }
    #endregion
}
