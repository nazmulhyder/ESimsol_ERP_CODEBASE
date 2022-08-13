using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region COAChartOfAccountNameAlternative
    public class COAChartOfAccountNameAlternative : BusinessObject
    {
        public COAChartOfAccountNameAlternative()
        {
            AlternativeAccountHeadID = 0;
            AccountHeadID = 0;
            Name = "";
            Description = "";
        }

        #region Properties
        public int AlternativeAccountHeadID { get; set; }
        public int AccountHeadID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int ParentHeadID { get; set; }
        public string DisplayMessage { get; set; }  
        #endregion

        #region Derive Properties
        public string ErrorMessage { get; set; } // To catch Error Message from Controller and Carry it to view      
        public IEnumerable<COAChartOfAccountNameAlternative> ChildNodes { get; set; }        
        public List<COAChartOfAccountNameAlternative> COAChartOfAccountNameAlternatives { get; set; }
        public string AccountHeadNameCode
        {
            get
            {
                return AccountHeadName + "[" + AccountCode + "]" + " @" + AccountType;
            }
        }
        #endregion

        #region Functions
        public static List<COAChartOfAccountNameAlternative> GetsByAccountHeadID(int nAccountHeadID, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.GetsByAccountHeadID(nAccountHeadID, nUserID);
        }
        public List<COAChartOfAccountNameAlternative> SearchbyAlternativeName(string Search, int ParentID, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.SearchbyAlternativeName(Search,ParentID, nUserID);
        }
        public static List<COAChartOfAccountNameAlternative> GetsbyParentID(int ParentID, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.GetsbyParentID(ParentID, nUserID);
        }
        public COAChartOfAccountNameAlternative Get(int id, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.Get(id, nUserID);
        }
        public COAChartOfAccountNameAlternative Save(int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.Delete(id, nUserID);
        }
        public static List<COAChartOfAccountNameAlternative> Gets(int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.Gets(nUserID);
        }
        public static List<COAChartOfAccountNameAlternative> Gets(string sSQL, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.Gets(sSQL, nUserID);
        }
        public COAChartOfAccountNameAlternative SaveForDocumentLeaf(COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternativeint, int nUserID)
        {
            return COAChartOfAccountNameAlternative.Service.SaveForDocumentLeaf(oCOAChartOfAccountNameAlternativeint, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static ICOAChartOfAccountNameAlternativeService Service
        {
            get { return (ICOAChartOfAccountNameAlternativeService)Services.Factory.CreateService(typeof(ICOAChartOfAccountNameAlternativeService)); }
        }
        #endregion
        public static List<COAChartOfAccountNameAlternative> Getsjason(int p, int nUserID)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    //#region COAChartOfAccountNameAlternative
    //public class COAChartOfAccountNameAlternatives : IndexedBusinessObjects
    //{
    //    #region Collection Class Methods
    //    public void Add(COAChartOfAccountNameAlternative oItem)
    //    {
    //        base.AddItem(oItem);
    //    }
    //    #endregion
    //}
    //#endregion

    #region ICOAChartOfAccountNameAlternative
    public interface ICOAChartOfAccountNameAlternativeService
    {
        List<COAChartOfAccountNameAlternative> Gets(int nUserID);
        List<COAChartOfAccountNameAlternative> GetsByAccountHeadID(int nAccountHeadID, int nUserID);
        COAChartOfAccountNameAlternative Save(COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative, int nUserID);
        COAChartOfAccountNameAlternative Get(int id, int nUserID);
        string Delete(int id, int nUserID);
        COAChartOfAccountNameAlternative SaveForDocumentLeaf(COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative, int nUserID);
        List<COAChartOfAccountNameAlternative> SearchbyAlternativeName(string Search, int ParentID, int nUserID);
        List<COAChartOfAccountNameAlternative> GetsbyParentID(int ParentID, int nUserID);
        List<COAChartOfAccountNameAlternative> Gets(string sSQL, int nUserID);
    }
    #endregion
}