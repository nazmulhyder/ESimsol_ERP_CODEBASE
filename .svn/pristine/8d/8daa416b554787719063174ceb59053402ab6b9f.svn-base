using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricDyeingRecipe
    
    public class FabricDyeingRecipe : BusinessObject
    {
        public FabricDyeingRecipe()
        {
            FabricDyeingRecipeID = 0;
            FEOSID = 0;
            DyeingSolutionID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int FabricDyeingRecipeID { get; set; }
        public int FEOSID { get; set; }
        public int DyeingSolutionID { get; set; }
        public string SCNoFull { get; set; }
        public string Name { get; set; }

        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public static List<FabricDyeingRecipe> Gets( long nUserID)
        {
            return FabricDyeingRecipe.Service.Gets(nUserID);
        }
        public static List<FabricDyeingRecipe> Gets(string sSQL, Int64 nUserID)
        {
            return FabricDyeingRecipe.Service.Gets(sSQL, nUserID);
        }
     
        public FabricDyeingRecipe Get(int nId, long nUserID)
        {
            return FabricDyeingRecipe.Service.Get(nId,nUserID);
        }

        public FabricDyeingRecipe Save(FabricDyeingRecipe oFabricDyeingRecipe, long nUserID)
        {
            return FabricDyeingRecipe.Service.Save(oFabricDyeingRecipe, nUserID);
        }

        public string Delete(string sSQL, long nUserID)
        {
            return FabricDyeingRecipe.Service.Delete(sSQL, nUserID); 
        }

        #endregion

        #region ServiceFactory
        internal static IFabricDyeingRecipeService Service
        {
            get { return (IFabricDyeingRecipeService)Services.Factory.CreateService(typeof(IFabricDyeingRecipeService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricDyeingRecipe interface
    
    public interface IFabricDyeingRecipeService
    {
        FabricDyeingRecipe Get(int id, long nUserID);
        List<FabricDyeingRecipe> Gets(long nUserID);
        List<FabricDyeingRecipe> Gets(string sSQL, Int64 nUserID);
        string Delete(string sSQL, long nUserID);
        FabricDyeingRecipe Save(FabricDyeingRecipe oFabricDyeingRecipe, long nUserID);
    }
    #endregion
}
