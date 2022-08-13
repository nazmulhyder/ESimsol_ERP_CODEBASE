using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region LCTerm    
    public class LCTerm : BusinessObject
    {
        public LCTerm()
        {
            LCTermID = 0;
            Name = "";
            Description = "";
            Days = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int LCTermID { get; set; }        
        public string Name { get; set; }        
        public string Description { get; set; }        
        public int Days { get; set; }        
        public string ErrorMessage { get; set; }        
        public List<LCTerm> LCTerms { get; set; }        
        public Company Company { get; set; }

        #region Derived Property
        public string NameDaysInString
        {
            get
            {
                if (this.Days <= 0)
                {
                    return this.Name ;
                }
                else
                {
                    return "At " + this.Days.ToString() + " Days " + this.Name;
                }
            }
        }
        #endregion

        #endregion
        
        #region Functions
        public static List<LCTerm> Gets(Int64 nUserID)
        {
            return LCTerm.Service.Gets(nUserID);
        }
        public LCTerm Get(int id, Int64 nUserID)
        {
            return LCTerm.Service.Get(id, nUserID);
        }
        public LCTerm Save(Int64 nUserID)
        {
            return LCTerm.Service.Save(this, nUserID);
        }
        public string Delete(int id, Int64 nUserID)
        {
            return LCTerm.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILCTermService Service
        {
            get { return (ILCTermService)Services.Factory.CreateService(typeof(ILCTermService)); }
        }
        #endregion
    }
    #endregion

    #region ILCTerms interface
    [ServiceContract]
    public interface ILCTermService
    {
        [OperationContract]
        LCTerm Get(int id, Int64 nUserID);
        [OperationContract]
        List<LCTerm> Gets(Int64 nUserID);
        [OperationContract]
        string Delete(int id, Int64 nUserID);
        [OperationContract]
        LCTerm Save(LCTerm oLCTerm, Int64 nUserID);
    }
    #endregion
}