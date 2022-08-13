using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility; 

namespace ESimSol.BusinessObjects
{
    #region DiagramIdentification
    
    public class DiagramIdentification : BusinessObject
    {
        public DiagramIdentification()
        {
            DiagramIdentificationID=0;
            MesurementPoint="";
            PointName ="";
            Note ="";
            Param = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int DiagramIdentificationID { get; set; }
         
        public string MesurementPoint { get; set; }
         
        public string PointName { get; set; }
         
        public string Note { get; set; }
        public string Param { get; set; }
         
        public string ErrorMessage { get; set; }
       
        #endregion

        #region Derived Propety

        public List<DiagramIdentification> DiagramIdentificationList { get; set; }
        #endregion

        #region Functions


        public DiagramIdentification Get(int id, long nUserID)
        {
            return DiagramIdentification.Service.Get(id, nUserID);
        }

        public static List<DiagramIdentification> Gets(long nUserID)
        {
            
            return DiagramIdentification.Service.Gets( nUserID);
        }
        public static List<DiagramIdentification> Gets_print(string sSQL, long nUserID)
        {
            
            return DiagramIdentification.Service.Gets_print(sSQL, nUserID);
        }
        public static List<DiagramIdentification> GetsByName(string sName,  long nUserID)
        {
            return DiagramIdentification.Service.GetsByName(sName, nUserID);
        }

        public DiagramIdentification Save(long nUserID)
        {
            return DiagramIdentification.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return DiagramIdentification.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IDiagramIdentificationService Service
        {
            get { return (IDiagramIdentificationService)Services.Factory.CreateService(typeof(IDiagramIdentificationService)); }
        }
        #endregion
    }
    #endregion


    #region IDiagramIdentificationService interface
     
    public interface IDiagramIdentificationService
    {
         
        DiagramIdentification Get(int id, Int64 nUserID);
         
        List<DiagramIdentification> Gets(Int64 nUserID);
         
        List<DiagramIdentification> Gets_print(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        DiagramIdentification Save(DiagramIdentification oDiagramIdentification, Int64 nUserID);
         
       List< DiagramIdentification> GetsByName(string sName, Int64 nUserID);
    }
    #endregion
}
