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
    public class BodyPartService : MarshalByRefObject, IBodyPartService
    {
        #region Private functions and declaration
        private BodyPart MapObject(NullHandler oReader)
        {
            BodyPart oBodyPart = new BodyPart();
            oBodyPart.BodyPartID = oReader.GetInt32("BodyPartID");
            oBodyPart.BodyPartCode = oReader.GetString("BodyPartCode");
            oBodyPart.BodyPartName = oReader.GetString("BodyPartName");
            oBodyPart.Remarks = oReader.GetString("Remarks");
            return oBodyPart;
        }

        private BodyPart CreateObject(NullHandler oReader)
        {
            BodyPart oBodyPart = new BodyPart();
            oBodyPart = MapObject(oReader);
            return oBodyPart;
        }

        private List<BodyPart> CreateObjects(IDataReader oReader)
        {
            List<BodyPart> oBodyPart = new List<BodyPart>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BodyPart oItem = CreateObject(oHandler);
                oBodyPart.Add(oItem);
            }
            return oBodyPart;
        }

        #endregion

        #region Interface implementation
        public BodyPartService() { }

        public BodyPart Save(BodyPart oBodyPart, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBodyPart.BodyPartID <= 0)
                {
                    reader = BodyPartDA.InsertUpdate(tc, oBodyPart, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BodyPartDA.InsertUpdate(tc, oBodyPart, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBodyPart = new BodyPart();
                    oBodyPart = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oBodyPart .ErrorMessage =   e.Message.Split('!')[0];
                #endregion
            }
            return oBodyPart;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BodyPart oBodyPart = new BodyPart();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BodyPart, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BodyPart", id);
                oBodyPart.BodyPartID = id;
                BodyPartDA.Delete(tc, oBodyPart, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public BodyPart Get(int id, Int64 nUserId)
        {
            BodyPart oAccountHead = new BodyPart();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BodyPartDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BodyPart", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BodyPart> Gets(Int64 nUserID)
        {
            List<BodyPart> oBodyPart = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BodyPartDA.Gets(tc);
                oBodyPart = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BodyPart", e);
                #endregion
            }

            return oBodyPart;
        }
        #endregion
    }
}
