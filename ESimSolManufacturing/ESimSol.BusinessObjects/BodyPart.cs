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
    #region BodyPart
    
    public class BodyPart : BusinessObject
    {
        public BodyPart()
        {
            BodyPartID = 0;
            BodyPartCode = "";
            BodyPartName = "";
            Remarks = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int BodyPartID { get; set; }
        public string BodyPartCode { get; set; }
        public string BodyPartName { get; set; }         
        public string Remarks { get; set; }         
        public string ErrorMessage { get; set; }
        public string BodyPartNameCode
        {
            get 
            {
                return this.BodyPartName + "[" + this.BodyPartCode + "]";
            }
        }
        #endregion

        
        #region Functions

        public static List<BodyPart> Gets(long nUserID)
        {
            return BodyPart.Service.Gets( nUserID);
        }

        public BodyPart Get(int id, long nUserID)
        {
            return BodyPart.Service.Get(id, nUserID);
        }

        public BodyPart Save(long nUserID)
        {
            return BodyPart.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return BodyPart.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBodyPartService Service
        {
            get { return (IBodyPartService)Services.Factory.CreateService(typeof(IBodyPartService)); }
        }

        #endregion
    }
    #endregion

    #region IBodyPart interface
     
    public interface IBodyPartService
    {         
        BodyPart Get(int id, Int64 nUserID);         
        List<BodyPart> Gets(Int64 nUserID);         
        string Delete(int id, Int64 nUserID);         
        BodyPart Save(BodyPart oBodyPart, Int64 nUserID);
    }
    #endregion
}
