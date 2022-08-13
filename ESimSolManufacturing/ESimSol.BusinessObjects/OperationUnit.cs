using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{
    #region OperationUnit
    public class OperationUnit : BusinessObject
    {
        public OperationUnit()
        {
            OperationUnitID = 0;
            OperationUnitName = "";
            Description = "";
            IsStore = false;
            ContainingProduct = 0;
            ShortName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int OperationUnitID { get; set; }
        public string OperationUnitName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool IsStore { get; set; }
        public int ContainingProduct { get; set; }
        public string ErrorMessage { get; set; }
        public string ContainingProductName { get; set; }        
        public string StoreStatus
        {
            get
            {
                if (IsStore)
                {
                    return "Store";
                }
                else
                {
                    return "Not Store";
                }
            }
        }
        #endregion

        #region Functions
        public OperationUnit Get(int id, int nUserID)
        {
            return OperationUnit.Service.Get(id, nUserID);
        }
        public OperationUnit Save(int nUserID)
        {
            return OperationUnit.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return OperationUnit.Service.Delete(id, nUserID);
        }
        public static List<OperationUnit> Gets(int nUserID)
        {
            return OperationUnit.Service.Gets(nUserID);
        }
        public static List<OperationUnit> Gets(string sSQL, int nUserID)
        {
            return OperationUnit.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IOperationUnitService Service
        {
            get { return (IOperationUnitService)Services.Factory.CreateService(typeof(IOperationUnitService)); }
        }
        #endregion

    }
    #endregion
       

    #region IOperationUnit interface
    public interface IOperationUnitService
    {
        OperationUnit Get(int id, int nUserID);
        List<OperationUnit> Gets(int nUserID);
        List<OperationUnit> Gets(string sSQL, int nUserID);
        string Delete(int id,int nUserID);
        OperationUnit Save(OperationUnit oOperationUnit, int nUserID);
    }
    #endregion
}