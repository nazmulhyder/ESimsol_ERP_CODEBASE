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
    #region MaterialTransactionRule
    
    public class MaterialTransactionRule : BusinessObject
    {
        #region  Constructor
        public MaterialTransactionRule()
        {
            MaterialTransactionRuleID = 0;
            LocationID = 0;
            WorkingUnitID = 0;
            TriggerParentType = EnumTriggerParentsType.None;
            InOutType = EnumInOutType.None;
            Direction = false;
            ProductType = EnumProductType.None;
            ProductCategoryID = 0;
            IsActive = false;
            Note = "";
            MAP = "";
           
        }

        #endregion

        #region Properties  
        
        public int MaterialTransactionRuleID { get; set; }

        
        public int LocationID { get; set; }


        
        public int WorkingUnitID { get; set; }
        

        
        public EnumTriggerParentsType TriggerParentType { get; set; }
        

        
        public EnumInOutType InOutType { get; set; }

        
        public EnumProductType ProductType { get; set; }

        
        public int ProductCategoryID { get; set; }

        
        public bool Direction { get; set; }
        
        
        public bool IsActive { get; set; }
                
        
        public string Note{ get; set; }

        
        public string OperationUnitName { get; set; }

        
        public string LocationName { get; set; }

        
        public string MAP { get; set; }


        #endregion
   
        #region Derived Property
        public string Activity
        {
            get
            {
                if (IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "Inactive";
                }

            }

        }
        public string TriggerParentTypeInString
        {
            get
            {
                return TriggerParentType.ToString();
            }
        }
        public string InOutName
        {
            get
            {
                return InOutType.ToString();
            }
        }
        public string DirectionName
        {
            get
            {
                if (Direction)
                {
                    return "Bi";
                }
                else
                {
                    return "Uni";
                }

            }

        }

        
        public string ErrorMessage { get; set; }

        
        public string IDs { get; set; }

        
        public int InOutTypeInt { get; set; }

        
        public int TriggerParentTypeInt { get; set; }

        
        public int ProductTypeInt { get; set; }
        
        public string ProductCategoryName { get; set; }
        public List<Location> SelectedLocations { get; set; }
        public List<WorkingUnit> SelectedWorkingUnits { get; set; }
        public List<MaterialTransactionRule> BiDirectedRules { get; set; }
      
        #endregion

        #region Functions
        public static List<MaterialTransactionRule> Gets(long nUserID)
        {
            return MaterialTransactionRule.Service.Gets(nUserID);
        }
        public static List<MaterialTransactionRule> GetsForBiDirectionalRule(string sSQL, long nUserID)
        {
            return MaterialTransactionRule.Service.GetsForBiDirectionalRule(sSQL, nUserID);
        }
        public static List<MaterialTransactionRule> Gets(string sSQL, long nUserID)
        {
            return MaterialTransactionRule.Service.Gets(sSQL, nUserID);
        }
       
        public static MaterialTransactionRule Get(int nId, long nUserID)
        {
            return MaterialTransactionRule.Service.Get(nId, nUserID);
        }

        public MaterialTransactionRule Save(long nUserID)
        {
            return MaterialTransactionRule.Service.Save(this, nUserID);
        }
        public string SaveMAP(long nUserID)
        {
            return MaterialTransactionRule.Service.SaveMAP(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return MaterialTransactionRule.Service.Delete(this, nUserID);
        }
        public static MaterialTransactionRule Get(EnumTriggerParentsType eTPT, EnumInOutType eInOutType, bool bDirection, int nProductType, int nWorkingUnitID, int nOEvalue, string sDbObjectName, long nUserID)
        {
            return MaterialTransactionRule.Service.Get((int)eTPT, (int)eInOutType, bDirection, nProductType, nWorkingUnitID, nOEvalue, sDbObjectName, nUserID);
        }
        
        #endregion
       
        #region ServiceFactory
        internal static IMaterialTransactionRuleService Service
        {
            get { return (IMaterialTransactionRuleService)Services.Factory.CreateService(typeof(IMaterialTransactionRuleService)); }
        }
        #endregion
    }
    #endregion

    #region MaterialTransactionRules
    public class MaterialTransactionRules : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(MaterialTransactionRule item)
        {
            base.AddItem(item);
        }
        public void Remove(MaterialTransactionRule item)
        {
            base.RemoveItem(item);
        }
        public MaterialTransactionRule this[int index]
        {
            get { return (MaterialTransactionRule)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IMaterialTransactionRule interface
    
    public interface IMaterialTransactionRuleService
    {
        
        MaterialTransactionRule Get(int id, long nUserID);
        
        MaterialTransactionRule Get(int eTPT, int eInOutType, bool bDirection, int nProductType, int nWorkingUnitID, int nOEvalue, string sDbObjectName, long nUserID);
        
        List<MaterialTransactionRule> Gets(long nUserID);
        
        List<MaterialTransactionRule> GetsForBiDirectionalRule(string sSQL, long nUserID);
        
        string Delete(MaterialTransactionRule oMaterialTransactionRule, long nUserID);
        
        MaterialTransactionRule Save(MaterialTransactionRule oMaterialTransactionRule, long nUserID);
        
        string SaveMAP(MaterialTransactionRule oMaterialTransactionRule, long nUserID);
        
        List<MaterialTransactionRule> Gets(string sSQL, long nUserID);

    }
    #endregion
}