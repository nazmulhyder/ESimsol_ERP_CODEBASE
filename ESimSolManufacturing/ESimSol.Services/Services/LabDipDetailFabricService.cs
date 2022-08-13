using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class LabDipDetailFabricService : MarshalByRefObject, ILabDipDetailFabricService
    {
        #region Private functions and declaration
        private static LabDipDetailFabric MapObject(NullHandler oReader)
        {
            LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();

            oLabDipDetailFabric.LDFID = oReader.GetInt32("LDFID");
            oLabDipDetailFabric.LabDipID = oReader.GetInt32("LabDipID");
            oLabDipDetailFabric.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oLabDipDetailFabric.ReviseNo = oReader.GetInt32("ReviseNo");
            oLabDipDetailFabric.ProductID = oReader.GetInt32("ProductID");
            oLabDipDetailFabric.FabricID = oReader.GetInt32("FabricID");
            oLabDipDetailFabric.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oLabDipDetailFabric.ProductName = oReader.GetString("ProductName");
            oLabDipDetailFabric.FabricNo = oReader.GetString("FabricNo");
            oLabDipDetailFabric.Construction = oReader.GetString("Construction");
            oLabDipDetailFabric.ActualConstruction = oReader.GetString("ActualConstruction");
            oLabDipDetailFabric.BuyerReference = oReader.GetString("BuyerReference");
            oLabDipDetailFabric.SCNoFull = oReader.GetString("SCNoFull");
            oLabDipDetailFabric.ExeNo = oReader.GetString("ExeNo");
            oLabDipDetailFabric.OrderType = oReader.GetInt32("OrderType");
            oLabDipDetailFabric.Remarks = oReader.GetString("Remarks");
            oLabDipDetailFabric.LDNo = oReader.GetString("LDNo");
            oLabDipDetailFabric.LabDipNo = oReader.GetString("LabDipNo");
            oLabDipDetailFabric.ColorName = oReader.GetString("ColorName");
            oLabDipDetailFabric.PantonNo = oReader.GetString("PantonNo");
            oLabDipDetailFabric.ColorNo = oReader.GetString("ColorNo");
            oLabDipDetailFabric.RefNo = oReader.GetString("RefNo");
            oLabDipDetailFabric.TwistedGroup = oReader.GetInt32("TwistedGroup");
            oLabDipDetailFabric.WarpWeftType = (EnumWarpWeft)oReader.GetInt16("WarpWeftType");
         
            return oLabDipDetailFabric;
        }

        public static LabDipDetailFabric CreateObject(NullHandler oReader)
        {
            LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();
            oLabDipDetailFabric = MapObject(oReader);
            return oLabDipDetailFabric;
        }

        private List<LabDipDetailFabric> CreateObjects(IDataReader oReader)
        {
            List<LabDipDetailFabric> oLabDipDetailFabric = new List<LabDipDetailFabric>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabDipDetailFabric oItem = CreateObject(oHandler);
                oLabDipDetailFabric.Add(oItem);
            }
            return oLabDipDetailFabric;
        }

        #endregion

        #region Interface implementation
        public LabDipDetailFabricService() { }

        public LabDipDetailFabric IUD(LabDipDetailFabric oLabDipDetailFabric, int nDBOperation, int nUserId)
        {
            LabDipDetailFabric oLDF= new LabDipDetailFabric();
            TransactionContext tc = null;
            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert) // && oLabDipDetailFabric.LabDipID == 0
                {
                    if (!string.IsNullOrEmpty(oLabDipDetailFabric.Params))
                    {
                        reader = LabDipDetailFabricDA.IUD(tc, oLabDipDetailFabric, nDBOperation, nUserId, oLabDipDetailFabric.Params);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oLabDipDetailFabric = CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    else { throw new Exception("No labdip information found to save."); }
                }

                reader = LabDipDetailFabricDA.IUD(tc, oLabDipDetailFabric, nDBOperation, nUserId,"");
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetailFabric = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLabDipDetailFabric = new LabDipDetailFabric(); oLabDipDetailFabric.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLDF = new LabDipDetailFabric();
                oLabDipDetailFabric.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oLabDipDetailFabric;
        }
  
        public LabDipDetailFabric Get(int nId, int nUserId)
        {
            LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LabDipDetailFabricDA.Get(tc, nId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetailFabric = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDipDetailFabric.ErrorMessage = e.Message;
                #endregion
            }

            return oLabDipDetailFabric;
        }
     
        public List<LabDipDetailFabric> Gets(string sSQL, int nUserId)
        {
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipDetailFabricDA.Gets(tc, sSQL);
                oLabDipDetailFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();
                oLabDipDetailFabric.ErrorMessage = e.Message;
                oLabDipDetailFabrics.Add(oLabDipDetailFabric);
                #endregion
            }

            return oLabDipDetailFabrics;
        }

        public List<LabDipDetailFabric> MakeTwistedGroup(string sLabDipDetailFabricID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID)
        {
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipDetailFabricDA.MakeTwistedGroup(tc, sLabDipDetailFabricID, nLabDipID, nTwistedGroup, nParentID, nDBOperation, nUserID);
                oLabDipDetailFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();
                oLabDipDetailFabric.ErrorMessage = e.Message;
                oLabDipDetailFabrics.Add(oLabDipDetailFabric);
                #endregion
            }

            return oLabDipDetailFabrics;
        }
  
        #endregion
    }
}
