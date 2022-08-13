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
    public class KnitDyeingRecipeDetail : BusinessObject
    {
        public KnitDyeingRecipeDetail()
        {
            KnitDyeingRecipeDetailID = 0;
            KnitDyeingRecipeID = 0;
            ProductID = 0;
            MUnitType =0;
            ReqQty = 0;
            ProductCode = "";
            ProductName = "";
            MeasurementUnitID = 0;
            MUnitTypeName = "";
            ErrorMessage = "";
        }
        #region Properties
        public int KnitDyeingRecipeDetailID { get; set; }
        public int KnitDyeingRecipeID { get; set; }
        public int ProductID { get; set; }
        public int MUnitType { get; set; }
        public double ReqQty { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int MeasurementUnitID { get; set; }
        public string MUnitTypeName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        public static List<KnitDyeingRecipeDetail> Gets(int id, long nUserID)
        {
            return KnitDyeingRecipeDetail.Service.Gets(id, nUserID);
        }
        #region ServiceFactory
        internal static IKnitDyeingRecipeDetailService Service
        {
            get { return (IKnitDyeingRecipeDetailService)Services.Factory.CreateService(typeof(IKnitDyeingRecipeDetailService)); }
        }
        #endregion

    }
    #region IKnitDyeingRecipeDetailService interface

    public interface IKnitDyeingRecipeDetailService
    {
        List<KnitDyeingRecipeDetail> Gets(int id, long nUserID);

    }
    #endregion
}
