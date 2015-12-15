using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OA3.Automation.KMT
{
    [TestClass]
    public class WorkFlow
    {
        /// <summary>
        /// Standard Keys for centralized mode
        /// </summary>
        [TestMethod]
        public void Centralized_Standard()
        {
            OEM_KeyManagement OEM_M = new OEM_KeyManagement();
            TPI_KeyMangement TPI_M = new TPI_KeyMangement();
            FF_KeyMangement FF_M = new FF_KeyMangement();
            //Launch ALL DIS
            OEM_M.LaunchDIS_OEM();
            TPI_M.LaunchDIS_TPI();
            FF_M.LaunchDIS_FactoryFloor();

            //Assign
            OEM_M.OEM_Assign_TPI();
            TPI_M.TPI_Recall_OEM();

            //Recall
            OEM_M.OEM_Assign_TPI();
            TPI_M.TPI_getKeys_OEM();
            TPI_M.TPI_Assign_DLS();
            FF_M.FF_getKeys_TPI();
            FF_M.FF_Recall_TPI();

            //Revert
            TPI_M.TPI_Assign_DLS();
            CommTestCase.SimulationRegister(CommTestCase.productKey,"Bound");
            FF_M.FF_RevertKeys();

            //Revert
            CommTestCase.SimulationRegister(CommTestCase.productKey, "Consumed");
            FF_M.FF_RevertKeys();

            //Report
            CommTestCase.SimulationRegister(CommTestCase.productKey,"Bound");
            FF_M.FF_Report_TPI();
            TPI_M.TPI_Report_OEM();
            OEM_M.OEM_Report_MS();

        }

        /// <summary>
        /// Standard keys for decentralized mode
        /// </summary>
        [TestMethod]
        public void Decenralized_Standard()
        {
            OEM_KeyManagement OEM_M = new OEM_KeyManagement();
            TPI_KeyMangement TPI_M = new TPI_KeyMangement();
            FF_KeyMangement FF_M = new FF_KeyMangement();
            //Launch ALL DIS
            OEM_M.LaunchDIS_OEM();
            TPI_M.LaunchDIS_TPI();
            FF_M.LaunchDIS_FactoryFloor();

            //Recall
            TPI_M.TPI_Assign_DLS();
            FF_M.FF_Recall_TPI();

            //Revert
            TPI_M.TPI_Assign_DLS();
            FF_M.FF_getKeys_TPI();
            CommTestCase.SimulationRegister(CommTestCase.productKey, "Bound");
            FF_M.FF_RevertKeys();

            CommTestCase.SimulationRegister(CommTestCase.productKey, "Consumed");
            FF_M.FF_RevertKeys();

            //Report
            CommTestCase.SimulationRegister(CommTestCase.productKey, "Bound");
            FF_M.FF_Report_TPI();
            TPI_M.TPI_Report_MS();

        }
    }
}
