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

    #region QCTemplateDetail
    
    public class QCTemplateDetail : BusinessObject
    {
        public QCTemplateDetail()
        {
            QCTemplateDetailID = 0;
            QCTemplateID = 0;
            QCStepID = 0;
            QCStepName = "";
            Sequence = 0;
            Remarks = "";
            QCStepParentID = 0;
            QCStepSequence = 0;
            bIsUp = true;
            TemplateName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int QCTemplateDetailID { get; set; }        
        public int QCTemplateID { get; set; }
        public int QCStepID { get; set; }
        public string QCStepName { get; set; }
        public string TemplateName { get; set; }
        public int Sequence { get; set; }
        public string Remarks { get; set; }
        public int QCStepParentID { get; set; }
        public int QCStepSequence { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public bool bIsUp { get; set; }
        public QCTemplate QCTemplate { get; set; }
        public IEnumerable<QCTemplateDetail> ChildNodes { get; set; }
      
        #endregion

        #region Functions

        public static List<QCTemplateDetail> Gets(int CourierServiceID, long nUserID)
        {           
            return QCTemplateDetail.Service.Gets(CourierServiceID, nUserID);
        }
      

        public static List<QCTemplateDetail> Gets(string sSQL, long nUserID)
        {
            return QCTemplateDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<QCTemplateDetail> Gets( long nUserID)
        {
            return QCTemplateDetail.Service.Gets(nUserID);
        }
        public QCTemplateDetail Get(int QCTemplateDetailID, long nUserID)
        {
            return QCTemplateDetail.Service.Get(QCTemplateDetailID, nUserID);
        }

      
        
        #endregion

        #region ServiceFactory

    
        internal static IQCTemplateDetailService Service
        {
            get { return (IQCTemplateDetailService)Services.Factory.CreateService(typeof(IQCTemplateDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IQCTemplateDetail interface
     
    public interface IQCTemplateDetailService
    {
         
        QCTemplateDetail Get(int QCTemplateDetailID, Int64 nUserID);
         
        List<QCTemplateDetail> Gets(int CourierServiceID, Int64 nUserID);
     
        List<QCTemplateDetail> Gets(string sSQL, Int64 nUserID);
         
        List<QCTemplateDetail> Gets(Int64 nUserID);

    }
    #endregion

    #region TQCTemplateDetail
    public class TQCTemplateDetail
    {
        public TQCTemplateDetail()
        {
            id = 0;
            text = "";
            BeforeShipment = 0;
            Sequence = 0;
            parentid = 0;
            QCTemplateDetailID = 0;
            QCStepID = 0;
            QCStepSequence = 0;
            QCTemplateID = 0;

        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public int BeforeShipment { get; set; }
        public int Sequence { get; set; }
        public int parentid { get; set; }
        public int QCTemplateDetailID { get; set; }
        public int QCTemplateID { get; set; }
        public int QCStepID { get; set; }
        public int QCStepSequence { get; set; }
        public string SequenceInString
        {
            get
            {
                return this.Sequence.ToString();
            }
        }
        public IEnumerable<TQCTemplateDetail> children { get; set; }//: an array nodes defines some children nodes
        public List<TQCTemplateDetail> TQCTemplateDetails { get; set; }
    }
    #endregion
 
}
