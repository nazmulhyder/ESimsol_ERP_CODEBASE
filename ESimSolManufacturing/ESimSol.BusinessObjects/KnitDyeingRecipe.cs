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
    public class KnitDyeingRecipe : BusinessObject
    {
        public KnitDyeingRecipe()
        {
            KnitDyeingRecipeID = 0;
            RecipeCode = "";
            RecipeName = "";
            Note = "";
            BUID = 0;
            UserName = "";
            IsActive = false;
            ErrorMessage = "";
            KnitDyeingRecipeDetails = new List<KnitDyeingRecipeDetail>();

        }
        #region Properties
        public int KnitDyeingRecipeID { get; set; }
        public string RecipeCode { get; set; }
        public string RecipeName { get; set; }
        public string Note { get; set; }
        public int BUID { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }
        public List<KnitDyeingRecipeDetail> KnitDyeingRecipeDetails { get; set; }
        #endregion
        #region Derived Properties
        public string ActivityStatus
        {
            get
            {
                if (IsActive == true)
                {
                    return "Active";
                }
                else
                {
                    return "InActive";
                }
            }
        }
        #endregion
        public KnitDyeingRecipe Save(long nUserID)
        {
            return KnitDyeingRecipe.Service.Save(this, nUserID);
        }
        public static List<KnitDyeingRecipe> Gets(string sSQL, long nUserID)
        {
            return KnitDyeingRecipe.Service.Gets(sSQL, nUserID);
        }
        public KnitDyeingRecipe Get(int id, long nUserID)
        {
            return KnitDyeingRecipe.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return KnitDyeingRecipe.Service.Delete(id, nUserID);
        }
        public KnitDyeingRecipe Activity(long nUserID)
        {
            return KnitDyeingRecipe.Service.Activity(this, nUserID);
        }
        #region ServiceFactory
        internal static IKnitDyeingRecipeService Service
        {
            get { return (IKnitDyeingRecipeService)Services.Factory.CreateService(typeof(IKnitDyeingRecipeService)); }
        }
        #endregion

    }
    #region IKnitDyeingRecipeService interface

    public interface IKnitDyeingRecipeService
    {
        KnitDyeingRecipe Save(KnitDyeingRecipe oKnitDyeingRecipe, Int64 nUserID);
        KnitDyeingRecipe Activity(KnitDyeingRecipe oKnitDyeingRecipe, Int64 nUserID);
        List<KnitDyeingRecipe> Gets(string sSQL, long nUserID);
        KnitDyeingRecipe Get(int id, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
