using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
   public class DyeingForeCastService : MarshalByRefObject, IDyeingForeCastService
    {
        #region Private functions and declaration
       private DyeingForeCast MapObject(NullHandler oReader)
       {
           DyeingForeCast oDyeingForeCast = new DyeingForeCast();
           oDyeingForeCast.DyeingForeCastID = oReader.GetInt32("ID");
           oDyeingForeCast.DyeingType = (EumDyeingType)oReader.GetInt32("DyeingType");
           oDyeingForeCast.DyeingTypeInt = oReader.GetInt32("DyeingType");
           oDyeingForeCast.YetToProdQty = oReader.GetDouble("YetToProdQty");
           oDyeingForeCast.VirtualYetToProdQty = oReader.GetDouble("VirtualYetToProdQty");
           oDyeingForeCast.ProductionHour = oReader.GetDouble("ProductionHour");
           oDyeingForeCast.ProductionCapacity = oReader.GetDouble("ProductionCapacity");
           oDyeingForeCast.CapacityPerHour = oReader.GetDouble("CapacityPerHour");
           oDyeingForeCast.ReqDays = oReader.GetInt32("ReqDays");
           oDyeingForeCast.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
           oDyeingForeCast.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
           oDyeingForeCast.OrderDate = oReader.GetDateTime("OrderDate");
           oDyeingForeCast.ContractorID = oReader.GetInt32("ContractorID");
           oDyeingForeCast.ContractorName = oReader.GetString("ContractorName");
           oDyeingForeCast.DyeingOrderType = (EnumOrderType)oReader.GetInt32("DyeingOrderType");
           oDyeingForeCast.ProductID = oReader.GetInt32("ProductID");
           oDyeingForeCast.ProductCode = oReader.GetString("ProductCode");
           oDyeingForeCast.ProductName = oReader.GetString("ProductName");
           oDyeingForeCast.ReqDyeingPeriod = oReader.GetDouble("ReqDyeingPeriod");
           oDyeingForeCast.ExportPIID = oReader.GetInt32("ExportPIID");
           oDyeingForeCast.ExportPINo = oReader.GetString("ExportPINo");
           oDyeingForeCast.ExportLCID = oReader.GetInt32("ExportLCID");
           oDyeingForeCast.ExportLCNo = oReader.GetString("ExportLCNo");
           return oDyeingForeCast;
       }
       private DyeingForeCast CreateObject(NullHandler oReader)
       {
           DyeingForeCast oDyeingForeCast = new DyeingForeCast();
           oDyeingForeCast = MapObject(oReader);
           return oDyeingForeCast;
       }
       private List<DyeingForeCast> CreateObjects(IDataReader oReader){
           List<DyeingForeCast> oDyeingForeCasts=new List<DyeingForeCast>();
           NullHandler oHandler = new NullHandler(oReader);
           while (oReader.Read())
           {
               DyeingForeCast oItem = CreateObject(oHandler);
               oDyeingForeCasts.Add(oItem);
           }
           return oDyeingForeCasts;
       }
        #endregion
        #region Interface Implement
       public List<DyeingForeCast> Gets(int nBuid, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
       {
           List<DyeingForeCast> oDyeingForeCasts = new List<DyeingForeCast>();
           TransactionContext tc = null;
           try
           {
               tc = TransactionContext.Begin();
               IDataReader reader = null;
               reader = DyeingForeCastDA.Gets(tc, nBuid, eForecastLayout, dStartDate, dEndDate);
               oDyeingForeCasts = CreateObjects(reader);
               reader.Close();
               tc.End();
           }
           catch (Exception e)
           {
               #region Handle Exception
               if (tc != null)
                   tc.HandleError();

               ExceptionLog.Write(e);
               throw new ServiceException("Failed to Get DyeingForeCast", e);
               #endregion
           }
           return oDyeingForeCasts;
       }
       public List<DyeingForeCast> GetsDetails(int nBuid, EumDyeingType eDyeingType, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
       {
           List<DyeingForeCast> oDyeingForeCasts = new List<DyeingForeCast>();
           TransactionContext tc = null;
           try
           {
               tc = TransactionContext.Begin();
               IDataReader reader = null;
               reader = DyeingForeCastDA.GetsDetails(tc, nBuid, eDyeingType, eForecastLayout, dStartDate, dEndDate);
               oDyeingForeCasts = CreateObjects(reader);
               reader.Close();
               tc.End();
           }
           catch (Exception e)
           {
               #region Handle Exception
               if (tc != null)
                   tc.HandleError();

               ExceptionLog.Write(e);
               throw new ServiceException("Failed to Get DyeingForeCast", e);
               #endregion
           }

           return oDyeingForeCasts;
       }
        #endregion
    }
}
