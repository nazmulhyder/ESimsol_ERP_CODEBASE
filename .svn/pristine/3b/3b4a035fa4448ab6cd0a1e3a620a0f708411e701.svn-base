using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class MachineLiquor
    {
         public MachineLiquor()
        {
            MachineLiquorID = 0;
            MachineID = 0;
            Label = 0;
            Liquor = 0;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";

        }

        #region Property
        public int MachineLiquorID { get; set; }
        public int MachineID { get; set; }
        public int Label { get; set; }
        public double Liquor { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

        #endregion

        #region DERIVED Property
        public string LiquorST
        {
            get
            {
                return this.Liquor.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string LabelST
        {
            get
            {
                return this.Label.ToString("#,##0;(#,##0)");
            }
        }
        #endregion

        #region Functions
        public static List<MachineLiquor> Gets(long nUserID)
        {
            return MachineLiquor.Service.Gets(nUserID);
        }
        public static List<MachineLiquor> Gets(string sSQL, long nUserID)
        {
            return MachineLiquor.Service.Gets(sSQL, nUserID);
        }
        public MachineLiquor Get(int id, long nUserID)
        {
            return MachineLiquor.Service.Get(id, nUserID);
        }
        public MachineLiquor Save(long nUserID)
        {
            return MachineLiquor.Service.Save(this, nUserID);
        }
        public List<MachineLiquor> SaveList(List<MachineLiquor> oMachineLiquors, long nUserID)
        {
            return MachineLiquor.Service.SaveList(oMachineLiquors, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return MachineLiquor.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IMachineLiquorService Service
        {
            get { return (IMachineLiquorService)Services.Factory.CreateService(typeof(IMachineLiquorService)); }
        }
        #endregion
    }

    #region IFabricQCParName interface
    public interface IMachineLiquorService
    {
        MachineLiquor Get(int id, Int64 nUserID);
        List<MachineLiquor> Gets(Int64 nUserID);
        List<MachineLiquor> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        MachineLiquor Save(MachineLiquor oMachineLiquor, Int64 nUserID);
        List<MachineLiquor> SaveList(List<MachineLiquor> oMachineLiquors, Int64 nUserID);
    }
    #endregion
}
