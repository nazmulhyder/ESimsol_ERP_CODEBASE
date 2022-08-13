using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
   public class OperationStage
    {
       public OperationStage()
        {
        OperationStageID =0;
        Name = string.Empty;
        OperationStageEnum =EnumOperationStage.None;
        Params = "";
        ErrorMessage = "";

        }
        #region properties
        public int OperationStageID { get; set; }
        public string Name { get; set; }
        public EnumOperationStage OperationStageEnum { get; set; }

        #endregion
        #region derivedproperties
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public string OperationStageEnumStr { get { return this.OperationStageEnum.ToString(); } }
        #endregion

        #region Functions
        public static OperationStage Get(int nOperationStageID, long nUserID)
        {
            return OperationStage.Service.Get( nOperationStageID, nUserID);
        }
        public static List<OperationStage> Gets(string sSQL, long nUserID)
        {
            return OperationStage.Service.Gets(sSQL, nUserID);
        }
        public static List<OperationStage> Gets( long nUserID)
        {
            return OperationStage.Service.Gets( nUserID);
        }
        public OperationStage IUD(int nDBOperation, long nUserID)
        {
            return OperationStage.Service.IUD(this, nDBOperation, nUserID);
        }

        #region Non DB Members
        public static string GetsName(EnumOperationStage eEnumOperationStage,List<OperationStage> oOperationStages)
        {
            string sName = "";
            foreach (OperationStage oitem in oOperationStages)
            {
              if(oitem.OperationStageEnum==eEnumOperationStage)
              { sName = oitem.Name; break; }
            }
            return sName;
        }


        #endregion
        #endregion

        #region ServiceFactory
        internal static IOperationStageService Service
        {
            get { return (IOperationStageService)Services.Factory.CreateService(typeof(IOperationStageService)); }
        }
        #endregion

        
    }
   #region IOperationStageService interface

   public interface IOperationStageService
   {

       OperationStage Get(int nOperationStageID, Int64 nUserID);
       List<OperationStage> Gets(string sSQL, Int64 nUserID);
       List<OperationStage> Gets( Int64 nUserID);
       OperationStage IUD(OperationStage oOperationStage, int nDBOperation, Int64 nUserID);
   }
   #endregion
}
