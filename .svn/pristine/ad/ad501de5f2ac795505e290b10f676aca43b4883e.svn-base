using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region FabricPlanRepeat
    public class FabricPlanRepeat
    {
        public FabricPlanRepeat()
        {
            FabricPlanRepeatID = 0;
            FabricPlanOrderID = 0;
            RepeatNo = 0;
            StartCol = 0;
            EndCol = 0;
            SLNo = 0;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            WarpWeftType = EnumWarpWeft.None;
            ErrorMessage = "";
            LastUpdateByName = "";

        }
        #region Property
        public int FabricPlanRepeatID { get; set; }
        public int FabricPlanOrderID { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public int RepeatNo { get; set; }
        public int StartCol { get; set; }
        public int EndCol { get; set; }
        public int SLNo { get; set; }
        public int LastUpdateBy { get; set; }       
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

        #endregion
        #region Derived Property
        public string WarpWeftTypeST
        {
            get
            {
                return EnumObject.jGet(this.WarpWeftType);
            }
        }
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        public string StartColST
        {
            get
            {
                return "Count " + this.StartCol;
            }
        }
        public string EndColST
        {
            get
            {
                return "Count " + this.EndCol;
            }
        }

        #endregion
        #region Functions
        public static List<FabricPlanRepeat> Gets(long nUserID)
        {
            return FabricPlanRepeat.Service.Gets(nUserID);
        }
        public static List<FabricPlanRepeat> Gets(string sSQL, long nUserID)
        {
            return FabricPlanRepeat.Service.Gets(sSQL, nUserID);
        }
        public FabricPlanRepeat Get(int id, long nUserID)
        {
            return FabricPlanRepeat.Service.Get(id, nUserID);
        }
        public FabricPlanRepeat Save(long nUserID)
        {
            return FabricPlanRepeat.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricPlanRepeat.Service.Delete(id, nUserID);
        }
        public List<FabricPlanRepeat> SaveFabricPlanRepeats(List<FabricPlanRepeat> oFabricPlanRepeats, long nUserID)
        {
            return FabricPlanRepeat.Service.SaveFabricPlanRepeats(oFabricPlanRepeats, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricPlanRepeatService Service
        {
            get { return (IFabricPlanRepeatService)Services.Factory.CreateService(typeof(IFabricPlanRepeatService)); }
        }
        #endregion
    }
    #endregion
    #region IFabricPlanRepeat interface
    public interface IFabricPlanRepeatService
    {
        FabricPlanRepeat Get(int id, Int64 nUserID);
        List<FabricPlanRepeat> Gets(Int64 nUserID);
        List<FabricPlanRepeat> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricPlanRepeat Save(FabricPlanRepeat oFabricPlanRepeat, Int64 nUserID);
        List<FabricPlanRepeat> SaveFabricPlanRepeats(List<FabricPlanRepeat> oFabricPlanRepeats, Int64 nUserID);

    }
    #endregion
}
