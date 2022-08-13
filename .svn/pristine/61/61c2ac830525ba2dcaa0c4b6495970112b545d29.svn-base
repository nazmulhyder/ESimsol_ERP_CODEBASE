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
   public class LabdipColor

    {
       public LabdipColor() 
       {
        LabdipColorID =0;
        Code=string.Empty;
        CodeNo=string.Empty;
        Name=string.Empty;
        Note=string.Empty;
        ErrorMessage =string.Empty;
        Params =string.Empty;

       }
        #region Properties
        public int  LabdipColorID {get; set;}
        public string Code {get; set;}
        public string CodeNo {get; set;}
        public string  Name {get; set;}
        public string Note { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public static LabdipColor Get(int nLapdipColorID, long nUserID)
        {
            return LabdipColor.Service.Get(nLapdipColorID, nUserID);
        }
        public static List<LabdipColor> Gets(string sSQL, long nUserID)
        {
            return LabdipColor.Service.Gets(sSQL, nUserID);
        }
        public LabdipColor IUD(int nDBOperation, long nUserID)
        {
            return LabdipColor.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILabdipColorService Service
        {
            get { return (ILabdipColorService)Services.Factory.CreateService(typeof(ILabdipColorService)); }
        }
        #endregion
    }
         #region ILapdipColorService interface

   public interface ILabdipColorService
   {

       LabdipColor Get(int nLapdipColorID, Int64 nUserID);
       List<LabdipColor> Gets(string sSQL, Int64 nUserID);
       LabdipColor IUD(LabdipColor oOperationStage, int nDBOperation, Int64 nUserID);
   }
   #endregion
}
