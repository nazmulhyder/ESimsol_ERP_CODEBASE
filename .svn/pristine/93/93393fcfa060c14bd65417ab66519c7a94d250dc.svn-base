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
    public class FabricYarnOrderAllocateService : MarshalByRefObject, IFabricYarnOrderAllocateService
    {
        #region Private functions and declaration
        private static FabricYarnOrderAllocate MapObject(NullHandler oReader)
        {
            FabricYarnOrderAllocate oFabricYarnOrderAllocate = new FabricYarnOrderAllocate();
            oFabricYarnOrderAllocate.FYOAID = oReader.GetInt32("FYOAID");
            oFabricYarnOrderAllocate.FYOID = oReader.GetInt32("FYOID");
            oFabricYarnOrderAllocate.WUID = oReader.GetInt32("WUID");
            oFabricYarnOrderAllocate.LotID = oReader.GetInt32("LotID");
            oFabricYarnOrderAllocate.Qty = oReader.GetDouble("Qty");
            oFabricYarnOrderAllocate.ApproveBy = oReader.GetInt32("ApproveBy");
            oFabricYarnOrderAllocate.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFabricYarnOrderAllocate.FEOID = oReader.GetInt32("FEOID");
            oFabricYarnOrderAllocate.LotNo = oReader.GetString("LotNo");
            oFabricYarnOrderAllocate.Balance = oReader.GetDouble("Balance");
            oFabricYarnOrderAllocate.ProductID = oReader.GetInt32("ProductID");
            oFabricYarnOrderAllocate.ProductName = oReader.GetString("ProductName");
            oFabricYarnOrderAllocate.ProductCode = oReader.GetString("ProductCode");
            oFabricYarnOrderAllocate.LocationName = oReader.GetString("LocationName");
            oFabricYarnOrderAllocate.OperationUnitName = oReader.GetString("OperationUnitName");
            oFabricYarnOrderAllocate.ApproveByName = oReader.GetString("ApproveByName");
            oFabricYarnOrderAllocate.MUnit = oReader.GetString("MUnit");
            
            return oFabricYarnOrderAllocate;
        }

        public static FabricYarnOrderAllocate CreateObject(NullHandler oReader)
        {
            FabricYarnOrderAllocate oFabricYarnOrderAllocate = MapObject(oReader);
            return oFabricYarnOrderAllocate;
        }

        private List<FabricYarnOrderAllocate> CreateObjects(IDataReader oReader)
        {
            List<FabricYarnOrderAllocate> oFabricYarnOrderAllocate = new List<FabricYarnOrderAllocate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricYarnOrderAllocate oItem = CreateObject(oHandler);
                oFabricYarnOrderAllocate.Add(oItem);
            }
            return oFabricYarnOrderAllocate;
        }

        #endregion

        #region Interface implementation
        public FabricYarnOrderAllocateService() { }

        public FabricYarnOrderAllocate IUD(FabricYarnOrderAllocate oFabricYarnOrderAllocate, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = FabricYarnOrderAllocateDA.IUD(tc, oFabricYarnOrderAllocate, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricYarnOrderAllocate = new FabricYarnOrderAllocate();
                        oFabricYarnOrderAllocate = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FabricYarnOrderAllocateDA.IUD(tc, oFabricYarnOrderAllocate, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFabricYarnOrderAllocate.ErrorMessage = Global.DeleteMessage;
                }
                else
                {
                    throw new Exception("Invalid Operation In Service");
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrderAllocate = new FabricYarnOrderAllocate();
                oFabricYarnOrderAllocate.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFabricYarnOrderAllocate;
        }

        public FabricYarnOrderAllocate Get(int nFYOAID, Int64 nUserId)
        {
            FabricYarnOrderAllocate oFabricYarnOrderAllocate = new FabricYarnOrderAllocate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricYarnOrderAllocateDA.Get(tc, nFYOAID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnOrderAllocate = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrderAllocate = new FabricYarnOrderAllocate();
                oFabricYarnOrderAllocate.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFabricYarnOrderAllocate;
        }

        public List<FabricYarnOrderAllocate> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricYarnOrderAllocate> oFabricYarnOrderAllocates = new List<FabricYarnOrderAllocate>();
            FabricYarnOrderAllocate oFabricYarnOrderAllocate = new FabricYarnOrderAllocate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricYarnOrderAllocateDA.Gets(tc, sSQL);
                oFabricYarnOrderAllocates = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrderAllocate.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFabricYarnOrderAllocates.Add(oFabricYarnOrderAllocate);
                #endregion
            }

            return oFabricYarnOrderAllocates;
        }

        public List<FabricYarnOrderAllocate> Approve(List<FabricYarnOrderAllocate> oFYOAs, Int64 nUserID)
        {
            List<FabricYarnOrderAllocate> results = new List<FabricYarnOrderAllocate>();
            var  oFYOA = new FabricYarnOrderAllocate();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricYarnOrderAllocate oItem in oFYOAs)
                {
                    reader = FabricYarnOrderAllocateDA.IUD(tc, oItem, (int)EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFYOA=new FabricYarnOrderAllocate();
                        oFYOA = CreateObject(oReader);
                        if (oFYOA.FYOAID > 0)
                        {
                            results.Add(oFYOA);
                        }
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                results = new List<FabricYarnOrderAllocate>();
                oFYOA = new FabricYarnOrderAllocate();
                oFYOA.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                results.Add(oFYOA);
                #endregion
            }
            return results;
        }


        #endregion
    }
}
