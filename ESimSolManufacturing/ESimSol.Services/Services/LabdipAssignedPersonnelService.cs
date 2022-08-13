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
    public class LabdipAssignedPersonnelService : MarshalByRefObject, ILabdipAssignedPersonnelService
    {
        #region Private functions and declaration
        private LabdipAssignedPersonnel MapObject(NullHandler oReader)
        {
            LabdipAssignedPersonnel oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();
            oLabdipAssignedPersonnel.LabdipAssignedPersonnelID = oReader.GetInt32("LabdipAssignedPersonnelID");
            oLabdipAssignedPersonnel.LabdipDetailID = oReader.GetInt32("LabdipDetailID");
            oLabdipAssignedPersonnel.EmployeeID = oReader.GetInt32("EmployeeID");
            oLabdipAssignedPersonnel.EmployeeName = oReader.GetString("EmployeeName");
            return oLabdipAssignedPersonnel;
        }

        public static LabdipAssignedPersonnel CreateObject(NullHandler oReader)
        {
            LabdipAssignedPersonnel oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();
            LabdipAssignedPersonnelService oService = new LabdipAssignedPersonnelService();
            oLabdipAssignedPersonnel = oService.MapObject(oReader);
            return oLabdipAssignedPersonnel;
        }
        private List<LabdipAssignedPersonnel> CreateObjects(IDataReader oReader)
        {
            List<LabdipAssignedPersonnel> oLabdipAssignedPersonnels = new List<LabdipAssignedPersonnel>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabdipAssignedPersonnel oItem = CreateObject(oHandler);
                oLabdipAssignedPersonnels.Add(oItem);
            }
            return oLabdipAssignedPersonnels;
        }

        #endregion

        #region Interface implementation
        public LabdipAssignedPersonnelService() { }

        public LabdipAssignedPersonnel IUD(LabdipAssignedPersonnel oLabdipAssignedPersonnel, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LabdipAssignedPersonnelDA.IUD(tc, oLabdipAssignedPersonnel, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();
                    oLabdipAssignedPersonnel = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLabdipAssignedPersonnel = new LabdipAssignedPersonnel(); oLabdipAssignedPersonnel.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabdipAssignedPersonnel.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oLabdipAssignedPersonnel;
        }

        public LabdipAssignedPersonnel Get(int nLabdipAssignedPersonnelID, long nUserID)
        {
            LabdipAssignedPersonnel oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabdipAssignedPersonnelDA.Get(tc, nLabdipAssignedPersonnelID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipAssignedPersonnel = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabdipAssignedPersonnel.ErrorMessage = e.Message;
                #endregion
            }

            return oLabdipAssignedPersonnel;
        }

        public List<LabdipAssignedPersonnel> Gets(string sSQL, long nUserID)
        {
            List<LabdipAssignedPersonnel> oLabdipAssignedPersonnels = new List<LabdipAssignedPersonnel>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabdipAssignedPersonnelDA.Gets(tc, sSQL);
                oLabdipAssignedPersonnels = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LabdipAssignedPersonnel oLabdipAssignedPersonnel = new LabdipAssignedPersonnel();
                oLabdipAssignedPersonnel.ErrorMessage = e.Message;
                oLabdipAssignedPersonnels.Add(oLabdipAssignedPersonnel);
                #endregion
            }

            return oLabdipAssignedPersonnels;
        }


        #endregion
    }
}