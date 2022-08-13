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

    public class SampleRequirementService : MarshalByRefObject, ISampleRequirementService
    {
        #region Private functions and declaration
        private SampleRequirement MapObject(NullHandler oReader)
        {
            SampleRequirement oSampleRequirement = new SampleRequirement();
            oSampleRequirement.SampleRequirementID = oReader.GetInt32("SampleRequirementID");
            oSampleRequirement.Code = oReader.GetString("Code");
            oSampleRequirement.SampleName = oReader.GetString("SampleName");
            oSampleRequirement.Remark = oReader.GetString("Remark");
            oSampleRequirement.SampleTypeID = oReader.GetInt32("SampleTypeID");
            oSampleRequirement.OrderRecapID = oReader.GetInt32("OrderRecapID");

            return oSampleRequirement;
        }

        private SampleRequirement CreateObject(NullHandler oReader)
        {
            SampleRequirement oSampleRequirement = new SampleRequirement();
            oSampleRequirement = MapObject(oReader);
            return oSampleRequirement;
        }

        private List<SampleRequirement> CreateObjects(IDataReader oReader)
        {
            List<SampleRequirement> oSampleRequirement = new List<SampleRequirement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleRequirement oItem = CreateObject(oHandler);
                oSampleRequirement.Add(oItem);
            }
            return oSampleRequirement;
        }

        #endregion

        #region Interface implementation
        public SampleRequirementService() { }

        public SampleRequirement Save(SampleRequirement oSampleRequirement, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSampleRequirement.SampleRequirementID <= 0)
                {

                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "SampleRequirement", EnumRoleOperationType.Add);
                    reader = SampleRequirementDA.InsertUpdate(tc, oSampleRequirement, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "SampleRequirement", EnumRoleOperationType.Edit);
                    reader = SampleRequirementDA.InsertUpdate(tc, oSampleRequirement, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleRequirement = new SampleRequirement();
                    oSampleRequirement = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleRequirement = new SampleRequirement();
                oSampleRequirement.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSampleRequirement;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SampleRequirement oSampleRequirement = new SampleRequirement();
                oSampleRequirement.SampleRequirementID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "SampleRequirement", EnumRoleOperationType.Delete);
                SampleRequirementDA.Delete(tc, oSampleRequirement, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SampleRequirement. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public SampleRequirement Get(int id, Int64 nUserId)
        {
            SampleRequirement oAccountHead = new SampleRequirement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleRequirementDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SampleRequirement", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<SampleRequirement> Gets(Int64 nUserID)
        {
            List<SampleRequirement> oSampleRequirement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleRequirementDA.Gets(tc);
                oSampleRequirement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleRequirement", e);
                #endregion
            }

            return oSampleRequirement;
        }


        public List<SampleRequirement> Gets(int nSaleOrderID, Int64 nUserID)
        {
            List<SampleRequirement> oSampleRequirement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleRequirementDA.Gets(nSaleOrderID,tc);
                oSampleRequirement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleRequirement", e);
                #endregion
            }

            return oSampleRequirement;
        }

        public List<SampleRequirement> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleRequirement> oSampleRequirement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleRequirementDA.Gets(tc, sSQL);
                oSampleRequirement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleRequirement", e);
                #endregion
            }

            return oSampleRequirement;
        }

        #endregion
    }   

}
