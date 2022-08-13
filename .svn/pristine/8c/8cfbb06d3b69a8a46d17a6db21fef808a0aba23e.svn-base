using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
   
    
    #region Knitting
    
    public class Knitting:BusinessObject
    {
        public Knitting()
        {
            KnittingID = 0;             
            Name = "";
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
         
        public int KnittingID { get; set; }
      
         
        public string Name { get; set; }
       
         
        public string Note { get; set; }
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<Knitting> Knittings { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<Knitting> Gets(long nUserID)
        {
            return Knitting.Service.Gets( nUserID);
        }

        public static List<Knitting> Gets(string sSQL, long nUserID)
        {
            return Knitting.Service.Gets(sSQL, nUserID);
        }

        public Knitting Get(int id, long nUserID)
        {
            return Knitting.Service.Get(id, nUserID);
        }

        public Knitting Save(long nUserID)
        {
            return Knitting.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Knitting.Service.Delete(id, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static IKnittingService Service
        {
            get { return (IKnittingService)Services.Factory.CreateService(typeof(IKnittingService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class KnittingList : List<Knitting>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IKnitting interface
     
    public interface IKnittingService
    {
         
        Knitting Get(int id, Int64 nUserID);
         
        List<Knitting> Gets(Int64 nUserID);
         
        List<Knitting> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        Knitting Save(Knitting oKnitting, Int64 nUserID);
       
    }
    #endregion
    
  
}
