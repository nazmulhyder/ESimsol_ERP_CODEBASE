using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class KnittingCompositionService : MarshalByRefObject, IKnittingCompositionService
    {
        #region Private functions and declaration

        private KnittingComposition MapObject(NullHandler oReader)
        {
            KnittingComposition oKnittingComposition = new KnittingComposition();
            oKnittingComposition.KnittingCompositionID = oReader.GetInt32("KnittingCompositionID");
            oKnittingComposition.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oKnittingComposition.FabricID = oReader.GetInt32("FabricID");
            oKnittingComposition.YarnID = oReader.GetInt32("YarnID");
            oKnittingComposition.RatioInPercent = oReader.GetDouble("RatioInPercent");
            oKnittingComposition.YarnName = oReader.GetString("YarnName");
            oKnittingComposition.YarnCode = oReader.GetString("YarnCode");
            oKnittingComposition.FabricName = oReader.GetString("FabricName");
            oKnittingComposition.Qty = oReader.GetDouble("Qty");
            oKnittingComposition.FabricCode = oReader.GetString("FabricCode");
            oKnittingComposition.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingComposition.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oKnittingComposition.LotNo = oReader.GetString("LotNo");
            oKnittingComposition.ColorName = oReader.GetString("ColorName");
            oKnittingComposition.BrandName = oReader.GetString("BrandName");
            return oKnittingComposition;
        }

        private KnittingComposition CreateObject(NullHandler oReader)
        {
            KnittingComposition oKnittingComposition = new KnittingComposition();
            oKnittingComposition = MapObject(oReader);
            return oKnittingComposition;
        }

        private List<KnittingComposition> CreateObjects(IDataReader oReader)
        {
            List<KnittingComposition> oKnittingComposition = new List<KnittingComposition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingComposition oItem = CreateObject(oHandler);
                oKnittingComposition.Add(oItem);
            }
            return oKnittingComposition;
        }

        #endregion

        #region Interface implementation
        public KnittingComposition Save(KnittingComposition oKnittingComposition, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            KnittingComposition oTempKnittingComposition = new KnittingComposition();
            List<KnittingComposition> oKnittingCompositions = new List<KnittingComposition>();

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader = null;
                if (oKnittingComposition.KnittingCompositions.Any())
                {

                    foreach (KnittingComposition oItem in oKnittingComposition.KnittingCompositions)
                    {

                        if (oItem.KnittingCompositionID <= 0)
                        {

                            reader = KnittingCompositionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {

                            reader = KnittingCompositionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }


                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTempKnittingComposition = new KnittingComposition();
                            oTempKnittingComposition = CreateObject(oReader);
                            oKnittingCompositions.Add(oTempKnittingComposition);
                        }
                        reader.Close();
                    }
                    oKnittingComposition.KnittingCompositions = oKnittingCompositions;
                    KnittingCompositionDA.DeleteByIDs(tc, oKnittingCompositions.First().KnittingOrderDetailID, string.Join(",", oKnittingCompositions.Select(x => x.KnittingCompositionID)), nUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingComposition = new KnittingComposition();
                    oKnittingComposition.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingComposition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingComposition oKnittingComposition = new KnittingComposition();
                oKnittingComposition.KnittingCompositionID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingComposition", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingComposition", id);
                KnittingCompositionDA.Delete(tc, oKnittingComposition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public KnittingComposition Get(int id, Int64 nUserId)
        {
            KnittingComposition oKnittingComposition = new KnittingComposition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingCompositionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingComposition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingComposition", e);
                #endregion
            }
            return oKnittingComposition;
        }

        public List<KnittingComposition> Gets(Int64 nUserID)
        {
            List<KnittingComposition> oKnittingCompositions = new List<KnittingComposition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingCompositionDA.Gets(tc);
                oKnittingCompositions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingComposition oKnittingComposition = new KnittingComposition();
                oKnittingComposition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingCompositions;
        }

        public List<KnittingComposition> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingComposition> oKnittingCompositions = new List<KnittingComposition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingCompositionDA.Gets(tc, sSQL);
                oKnittingCompositions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingComposition", e);
                #endregion
            }
            return oKnittingCompositions;
        }

        #endregion
    }

}
