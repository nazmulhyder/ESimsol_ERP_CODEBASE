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
    public class FabricYarnDeliveryChallanService : MarshalByRefObject, IFabricYarnDeliveryChallanService
    {
        #region Private functions and declaration
        private static FabricYarnDeliveryChallan MapObject(NullHandler oReader)
        {
            FabricYarnDeliveryChallan oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
            oFabricYarnDeliveryChallan.FYDChallanID = oReader.GetInt32("FYDChallanID");
            oFabricYarnDeliveryChallan.FYDOID = oReader.GetInt32("FYDOID");
            oFabricYarnDeliveryChallan.FYDChallanNo =oReader.GetString("FYDChallanNo");
            oFabricYarnDeliveryChallan.WUID = oReader.GetInt32("WUID");
            oFabricYarnDeliveryChallan.DisburseBy = oReader.GetInt32("DisburseBy");
            oFabricYarnDeliveryChallan.DisburseDate = oReader.GetDateTime("DisburseDate");
            oFabricYarnDeliveryChallan.FYDNo =oReader.GetString("FYDNo");
            oFabricYarnDeliveryChallan.WUName =oReader.GetString("WUName");
            oFabricYarnDeliveryChallan.DisburseByName =oReader.GetString("DisburseByName");
            oFabricYarnDeliveryChallan.FEONo = oReader.GetString("FEONo");
            oFabricYarnDeliveryChallan.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oFabricYarnDeliveryChallan.FEOID = oReader.GetInt32("FEOID");
  
            return oFabricYarnDeliveryChallan;
        }

         public static FabricYarnDeliveryChallan CreateObject(NullHandler oReader)
        {
            FabricYarnDeliveryChallan oFabricYarnDeliveryChallan = MapObject(oReader);
            return oFabricYarnDeliveryChallan;
        }

        private List<FabricYarnDeliveryChallan> CreateObjects(IDataReader oReader)
        {
            List<FabricYarnDeliveryChallan> oFabricYarnDeliveryChallans = new List<FabricYarnDeliveryChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricYarnDeliveryChallan oItem = CreateObject(oHandler);
                oFabricYarnDeliveryChallans.Add(oItem);
            }
            return oFabricYarnDeliveryChallans;
        }

        #endregion

       #region Interface implementation
        public FabricYarnDeliveryChallanService() { }

        public FabricYarnDeliveryChallan IUD(FabricYarnDeliveryChallan oFabricYarnDeliveryChallan, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = FabricYarnDeliveryChallanDA.IUD(tc, oFabricYarnDeliveryChallan, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
                        oFabricYarnDeliveryChallan = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FabricYarnDeliveryChallanDA.IUD(tc, oFabricYarnDeliveryChallan, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFabricYarnDeliveryChallan.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
                oFabricYarnDeliveryChallan.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFabricYarnDeliveryChallan;
        }

        public FabricYarnDeliveryChallan Get(int nFYDChallanID, Int64 nUserId)
        {
            FabricYarnDeliveryChallan oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricYarnDeliveryChallanDA.Get(tc, nFYDChallanID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
                oFabricYarnDeliveryChallan.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFabricYarnDeliveryChallan;
        }

        public List<FabricYarnDeliveryChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricYarnDeliveryChallan> oFabricYarnDeliveryChallans = new List<FabricYarnDeliveryChallan>();
            FabricYarnDeliveryChallan oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricYarnDeliveryChallanDA.Gets(tc, sSQL);
                oFabricYarnDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryChallan.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFabricYarnDeliveryChallans.Add(oFabricYarnDeliveryChallan);
                #endregion
            }

            return oFabricYarnDeliveryChallans;
        }


        public FabricYarnDeliveryChallan Disburse(FabricYarnDeliveryChallan oFabricYarnDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = FabricYarnDeliveryChallanDA.Disburse(tc, oFabricYarnDeliveryChallan, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
                    oFabricYarnDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryChallan = new FabricYarnDeliveryChallan();
                oFabricYarnDeliveryChallan.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFabricYarnDeliveryChallan;
        }

        #endregion
    }
}
