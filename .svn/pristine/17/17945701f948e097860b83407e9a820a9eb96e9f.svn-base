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

    #region TAPTemplateDetail
    
    public class TAPTemplateDetail : BusinessObject
    {
        public TAPTemplateDetail()
        {
            TAPTemplateDetailID = 0;
            TAPTemplateID = 0;
            OrderStepID = 0;
            OrderStepName = "";
            Sequence = 0;
            BeforeShipment = 0;
            Remarks = "";
            OrderStepParentID = 0;
            OrderStepSequence = 0;
            bIsUp = true;
            CalculationType = EnumCalculationType.Days;
            TemplateName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int TAPTemplateDetailID { get; set; }
         
        public int TAPTemplateID { get; set; }
         
        public int OrderStepID { get; set; }
         
        public string OrderStepName { get; set; }
        public string TemplateName { get; set; }
        public int Sequence { get; set; }
        public EnumCalculationType CalculationType { get; set; }
        public int BeforeShipment { get; set; }
         
        public string Remarks { get; set; }
        public int OrderStepParentID { get; set; }
        public int OrderStepSequence { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public bool bIsUp { get; set; }
        public TAPTemplate TAPTemplate { get; set; }
        public IEnumerable<TAPTemplateDetail> ChildNodes { get; set; }
        public string CalculationTypeInstring
        {
            get
            {
                return this.CalculationType.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<TAPTemplateDetail> Gets(int CourierServiceID, long nUserID)
        {           
            return TAPTemplateDetail.Service.Gets(CourierServiceID, nUserID);
        }
        public static List<TAPTemplateDetail> GetsByTampleteType(int nTempleteType, long nUserID)
        {
            return TAPTemplateDetail.Service.GetsByTampleteType(nTempleteType, nUserID);
        }

        public static List<TAPTemplateDetail> Gets(string sSQL, long nUserID)
        {
            return TAPTemplateDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<TAPTemplateDetail> Gets( long nUserID)
        {
            return TAPTemplateDetail.Service.Gets(nUserID);
        }
        public TAPTemplateDetail Get(int TAPTemplateDetailID, long nUserID)
        {
            return TAPTemplateDetail.Service.Get(TAPTemplateDetailID, nUserID);
        }

      
        
        #endregion

        #region ServiceFactory

    
        internal static ITAPTemplateDetailService Service
        {
            get { return (ITAPTemplateDetailService)Services.Factory.CreateService(typeof(ITAPTemplateDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITAPTemplateDetail interface
     
    public interface ITAPTemplateDetailService
    {
         
        TAPTemplateDetail Get(int TAPTemplateDetailID, Int64 nUserID);
         
        List<TAPTemplateDetail> Gets(int CourierServiceID, Int64 nUserID);
        List<TAPTemplateDetail> GetsByTampleteType(int nTempleteType, Int64 nUserID);

        List<TAPTemplateDetail> Gets(string sSQL, Int64 nUserID);
         
        List<TAPTemplateDetail> Gets(Int64 nUserID);

    }
    #endregion

    #region TTAPTemplateDetail
    public class TTAPTemplateDetail
    {
        public TTAPTemplateDetail()
        {
            id = 0;
            text = "";
            BeforeShipment = 0;
            Sequence = 0;
            Remarks = "";
            parentid = 0;
            TAPTemplateDetailID = 0;
            OrderStepID = 0;
            OrderStepSequence = 0;
            TAPTemplateID = 0;

        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public int BeforeShipment { get; set; }
        public int Sequence { get; set; }
        public string Remarks { get; set; }
        public int parentid { get; set; }
        public int TAPTemplateDetailID { get; set; }
        public int TAPTemplateID { get; set; }
        public int OrderStepID { get; set; }
        public int OrderStepSequence { get; set; }
        public string SequenceInString
        {
            get
            {
                return this.Sequence.ToString();
            }
        }
        public IEnumerable<TTAPTemplateDetail> children { get; set; }//: an array nodes defines some children nodes
        public List<TTAPTemplateDetail> TTAPTemplateDetails { get; set; }
    }
    #endregion
 
}
