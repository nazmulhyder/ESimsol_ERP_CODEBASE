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

    #region QCStep
    
    public class QCStep : BusinessObject
    {
        public QCStep()
        {
            QCStepID = 0;
            ParentID = 0;
            QCStepName = "";
            QCDataType = EnumQCDataType.Text;
            Sequence = 0;
            ProductionStepID = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int QCStepID { get; set; }
        public int ParentID { get; set; }
        public string QCStepName { get; set; }
        public EnumQCDataType QCDataType { get; set; }
        public List<QCStep> ChildQCSteps { get; set; }
        public int Sequence { get; set; }
        public int ProductionStepID { get; set; }
        public string ProductionStepName { get; set; }
        public string Note { get; set; }
        public IEnumerable<QCStep> ChildNodes { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<QCStep> QCSteps { get; set; }
         
     
        public Company Company { get; set; }
        public string QCDataTypeInString
        {
           get
           {
                   return QCDataType.ToString();
           }
            
        }
      
    
        #endregion

        #region Functions

        public static List<QCStep> Gets(long nUserID)
        {
            return QCStep.Service.Gets( nUserID);
        }

        public static List<QCStep> Gets(string sSQL, long nUserID)
        {
            return QCStep.Service.Gets(sSQL, nUserID);
        }

        public QCStep Get(int id, long nUserID)
        {
            return QCStep.Service.Get(id, nUserID);
        }

        public QCStep Save(long nUserID)
        {
            return QCStep.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return QCStep.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory

        internal static IQCStepService Service
        {
            get { return (IQCStepService)Services.Factory.CreateService(typeof(IQCStepService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class QCStepList : List<QCStep>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IQCStep interface
     
    public interface IQCStepService
    {
         
        QCStep Get(int id, Int64 nUserID);
         
        List<QCStep> Gets(Int64 nUserID);
         
        List<QCStep> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        QCStep Save(QCStep oQCStep, Int64 nUserID);

    }
    #endregion


    #region TQCStep
    public class TQCStep
    {
        public TQCStep()
        {
            id = 0;
            text = "";
            QCDataTypeInString = "";
            Sequence = 0;
            ProductionStepName = "";
            parentid = 0;
            ProductionStepID = 0;
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string QCDataTypeInString { get; set; }
        public int Sequence { get; set; }
        public string ProductionStepName { get; set; }
        public int parentid { get; set; }
        public int ProductionStepID { get; set; }
        public string SequenceInString
        {
            get
            {
                if (this.parentid > 1)
                {
                    return this.Sequence.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public IEnumerable<TQCStep> children { get; set; }//: an array nodes defines some children nodes
        public List<TQCStep> TQCSteps { get; set; }
    }
    #endregion
  
}
