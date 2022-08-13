using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;



namespace ESimSol.BusinessObjects
{
    #region WUFunctionality
    
    public class WUFunctionality : BusinessObject
    {
        public WUFunctionality()
        {
            WUFunctionalityID = 0;
            WorkingUnitID = 0;
            OperationFunctionality = EnumOperationFunctionality.None;
            WorkingUnitFunctionality = EnumWorkingUnitFunctionality.None;
            ErrorMessage = "";
        }

        #region Properties
        
        public int WUFunctionalityID { get; set; }
        
        public int WorkingUnitID { get; set; }
        
        public EnumOperationFunctionality OperationFunctionality { get; set; }
        
        public EnumWorkingUnitFunctionality WorkingUnitFunctionality { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public int EnumWorkingUnitFunctionalityint { get; set; }
        
        public string SelectedWorkingUnit { get; set; }
        #endregion

        #region Derived Property

        public List<WUFunctionality> WUFunctionalityForSelectedWorkingUnit { get; set; }
        
        public int WorkingUnitFunctionalityint { get; set; }
        public string WorkingUnitFunctionalityInString
        {
            get
            {
                return WorkingUnitFunctionality.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<WUFunctionality> Gets(long nUserID)
        {
            return WUFunctionality.Service.Gets(nUserID);
        }
        public static List<WUFunctionality> GetsByWorkingUnit(int nWorkingUnitID, long nUserID)
        {
            return WUFunctionality.Service.GetsByWorkingUnit(nWorkingUnitID, nUserID);
        }

        public WUFunctionality Get(int nId, long nUserID)
        {
            return WUFunctionality.Service.Get(nId, nUserID);
        }

        public WUFunctionality Save(long nUserID)
        {
            return WUFunctionality.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return WUFunctionality.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IWUFunctionalityService Service
        {
            get { return (IWUFunctionalityService)Services.Factory.CreateService(typeof(IWUFunctionalityService)); }
        }
        #endregion
    }
    #endregion

    #region WUFunctionalitys
    public class WUFunctionalitys : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(WUFunctionality item)
        {
            base.AddItem(item);
        }
        public void Remove(WUFunctionality item)
        {
            base.RemoveItem(item);
        }
        public WUFunctionality this[int index]
        {
            get { return (WUFunctionality)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IWUFunctionality interface
    
    public interface IWUFunctionalityService
    {
        
        WUFunctionality Get(int id, long nUserID);
        
        List<WUFunctionality> Gets(long nUserID);
        
        List<WUFunctionality> GetsByWorkingUnit(int nWorkingUnitID, long nUserID);
        
        string Delete(int id, long nUserID);
        
        WUFunctionality Save(WUFunctionality oWUFunctionality, long nUserID);
    }
    #endregion
}