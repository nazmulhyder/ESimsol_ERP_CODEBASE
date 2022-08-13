using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class KnitDyeingRecipeDetailService : MarshalByRefObject, IKnitDyeingRecipeDetailService
    {
        private KnitDyeingRecipeDetail MapObject(NullHandler oReader)
        {
            KnitDyeingRecipeDetail oKnitDyeingRecipeDetail = new KnitDyeingRecipeDetail();
            oKnitDyeingRecipeDetail.KnitDyeingRecipeDetailID = oReader.GetInt32("KnitDyeingRecipeDetailID");
            oKnitDyeingRecipeDetail.KnitDyeingRecipeID = oReader.GetInt32("KnitDyeingRecipeID");
            oKnitDyeingRecipeDetail.ProductID = oReader.GetInt32("ProductID");
            oKnitDyeingRecipeDetail.MUnitType = oReader.GetInt32("MUnitType");
            oKnitDyeingRecipeDetail.ReqQty = oReader.GetDouble("ReqQty");
            oKnitDyeingRecipeDetail.ProductCode = oReader.GetString("ProductCode");
            oKnitDyeingRecipeDetail.ProductName = oReader.GetString("ProductName");
            oKnitDyeingRecipeDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oKnitDyeingRecipeDetail.MUnitTypeName = oReader.GetString("MUnitTypeName");
            return oKnitDyeingRecipeDetail;
        }
        private KnitDyeingRecipeDetail CreateObject(NullHandler oReader)
        {
            KnitDyeingRecipeDetail oKnitDyeingRecipeDetail = new KnitDyeingRecipeDetail();
            oKnitDyeingRecipeDetail = MapObject(oReader);
            return oKnitDyeingRecipeDetail;
        }

        private List<KnitDyeingRecipeDetail> CreateObjects(IDataReader oReader)
        {
            List<KnitDyeingRecipeDetail> oKnitDyeingRecipeDetail = new List<KnitDyeingRecipeDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnitDyeingRecipeDetail oItem = CreateObject(oHandler);
                oKnitDyeingRecipeDetail.Add(oItem);
            }
            return oKnitDyeingRecipeDetail;
        }
        public List<KnitDyeingRecipeDetail> Gets(int id, Int64 nUserID)
        {
            List<KnitDyeingRecipeDetail> oKnitDyeingRecipeDetails = new List<KnitDyeingRecipeDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingRecipeDetailDA.Gets(tc, id);
                oKnitDyeingRecipeDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnitDyeingRecipeDetail oKnitDyeingRecipeDetail = new KnitDyeingRecipeDetail();
                oKnitDyeingRecipeDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitDyeingRecipeDetails;
        }
    }
}
