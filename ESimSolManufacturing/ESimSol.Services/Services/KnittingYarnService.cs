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

namespace ESimSol.Services.Services
{
    public class KnittingYarnService : MarshalByRefObject, IKnittingYarnService
    {
        #region Private functions and declaration

        private KnittingYarn MapObject(NullHandler oReader)
        {
            KnittingYarn oKnittingYarn = new KnittingYarn();
            oKnittingYarn.KnittingYarnID = oReader.GetInt32("KnittingYarnID");
            oKnittingYarn.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingYarn.YarnID = oReader.GetInt32("YarnID");
            oKnittingYarn.Remarks = oReader.GetString("Remarks");
            oKnittingYarn.YarnName = oReader.GetString("YarnName");
            oKnittingYarn.YarnCode = oReader.GetString("YarnCode");
            return oKnittingYarn;
        }

        private KnittingYarn CreateObject(NullHandler oReader)
        {
            KnittingYarn oKnittingYarn = new KnittingYarn();
            oKnittingYarn = MapObject(oReader);
            return oKnittingYarn;
        }

        private List<KnittingYarn> CreateObjects(IDataReader oReader)
        {
            List<KnittingYarn> oKnittingYarn = new List<KnittingYarn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingYarn oItem = CreateObject(oHandler);
                oKnittingYarn.Add(oItem);
            }
            return oKnittingYarn;
        }

        #endregion

        #region Interface implementation
        public KnittingYarn Save(KnittingYarn oKnittingYarn, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnittingYarn.KnittingYarnID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarn", EnumRoleOperationType.Add);
                    reader = KnittingYarnDA.InsertUpdate(tc, oKnittingYarn, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarn", EnumRoleOperationType.Edit);
                    reader = KnittingYarnDA.InsertUpdate(tc, oKnittingYarn, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarn = new KnittingYarn();
                    oKnittingYarn = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingYarn = new KnittingYarn();
                    oKnittingYarn.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingYarn;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingYarn oKnittingYarn = new KnittingYarn();
                oKnittingYarn.KnittingYarnID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingYarn", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingYarn", id);
                KnittingYarnDA.Delete(tc, oKnittingYarn, EnumDBOperation.Delete, nUserId,"");
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

        public KnittingYarn Get(int id, Int64 nUserId)
        {
            KnittingYarn oKnittingYarn = new KnittingYarn();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingYarnDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarn = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingYarn", e);
                #endregion
            }
            return oKnittingYarn;
        }

        public List<KnittingYarn> Gets(Int64 nUserID)
        {
            List<KnittingYarn> oKnittingYarns = new List<KnittingYarn>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnDA.Gets(tc);
                oKnittingYarns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingYarn oKnittingYarn = new KnittingYarn();
                oKnittingYarn.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingYarns;
        }

        public List<KnittingYarn> Gets(int id, Int64 nUserID)
        {
            List<KnittingYarn> oKnittingYarns = new List<KnittingYarn>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnDA.Gets(tc,id);
                oKnittingYarns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingYarn oKnittingYarn = new KnittingYarn();
                oKnittingYarn.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingYarns;
        }

        public List<KnittingYarn> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingYarn> oKnittingYarns = new List<KnittingYarn>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnDA.Gets(tc, sSQL);
                oKnittingYarns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingYarn", e);
                #endregion
            }
            return oKnittingYarns;
        }

        #endregion
    }

}
