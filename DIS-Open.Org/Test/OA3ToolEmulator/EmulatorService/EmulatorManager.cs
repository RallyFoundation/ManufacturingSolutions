//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using EmulatorService.Entities;

namespace EmulatorService
{
    public class EmulatorManager {
        #region Private members
        private EmulatorRepository repository = new EmulatorRepository();
        private IKeyStoreProviderProxy keyStoreProviderProxy = new KeyStoreProviderProxy();
        private static NameValueCollection runtimeSection = ConfigurationManager.GetSection("TestRuntime") as NameValueCollection;
        private static NameValueCollection testPramertersSection = ConfigurationManager.GetSection("TestParameters") as NameValueCollection;
        private static NameValueCollection optionalInfesSection = ConfigurationManager.GetSection("OemOptionalInfoes") as NameValueCollection;
        private const string AssembleKeyName = "AssembleKey";
        private const string UpdateKeyName = "UpdateKey";
        private const string ProductKeyIDName = "ProductKeyID";
        private const string KeyName = "Key";
        private const string ParametersName = "Parameters";
        private const string ParameterName = "Parameter";
        private const string nameName = "name";
        private const string valueName = "value";
        private const string ProductKeyStateName = "ProductKeyState";
        private const string HardwareHashName = "HardwareHash";
        private const string OEMOptionalInfoName = "OEMOptionalInfo";
        private const string TrackingInfoName = "TrackingInfo";
        #endregion

        #region Public Methods
        public void ExecuteTest() {
            var tests = repository.GetTests(TestStatus.Ready);
            if (tests.Count > 0) {
                tests.ForEach(t => ExecuteTest(t));
            }
        }

        public void GenerateAssembleKeys() {
            IConfigProxy configProxy = new ConfigProxy(null);
            IKeyProxy keyProxy = new KeyProxy(null, null);
            var count = keyProxy.SearchKeys(new KeySearchCriteria() { KeyState = KeyState.Fulfilled }).Count;
            if (count > 0) {
                var newTest = new Test() {
                    TestName = AssembleKeyName,
                    IsPositive = true,
                    TestStatus = TestStatus.Ready,
                };
                repository.InsertTest(newTest);

                if (testPramertersSection.Count > 0) {
                    var newTestParameters = new List<TestParameter>();
                    for (int i = 0; i < testPramertersSection.Count; i++) {
                        var newTestParameter = new TestParameter() {
                            TestId = newTest.TestId,
                            Index = i,
                            Name = testPramertersSection.GetKey(i),
                            Value = testPramertersSection.Get(i),
                        };
                        newTestParameters.Add(newTestParameter);
                    }
                    repository.InsertTestParameters(newTestParameters);
                }
            }
        }
        #endregion

        #region Private Methods
        private void ExecuteTest(Test test) {
            try {
                repository.UpdateTest(test.TestId, TestStatus.InProgress);
                ReturnValue result = ReturnValue.MSG_KEYPROVIDER_SUCCESS;
                var parameters = repository.GetTestParameters(test.TestId).ToList();
                switch (test.TestName) {
                    case AssembleKeyName:
                        var productKeyInfo = string.Empty;
                        result = (ReturnValue)keyStoreProviderProxy.GetKey(ParaseParameters(parameters), ref productKeyInfo);
                        if (result == ReturnValue.MSG_KEYPROVIDER_SUCCESS)
                            GeneraterUpdateKeyTest(parameters, productKeyInfo);
                        break;
                    case UpdateKeyName:
                        var productKeyId = ParaseProductKeyInfo(parameters);
                        result = (ReturnValue)keyStoreProviderProxy.UpdateKey(ParaseParameters(parameters), productKeyId);
                        break;
                    default:
                        throw new ArgumentException("Test Name should be AssembleKey or UpdateKey.");
                }

                if (parameters.Count > 0) {
                    repository.InsertTestResults(parameters.Select(p => new TestResult() {
                        TestId = test.TestId,
                        ActualResult = result == ReturnValue.MSG_KEYPROVIDER_SUCCESS ? true : false,
                        Name = p.Name,
                        Index = p.Index,
                        Value = p.Value,
                        Comments = result == ReturnValue.MSG_KEYPROVIDER_SUCCESS ? null : result.ToString(),
                    }).ToList());
                }
                else {
                    repository.InsertTestResult(new TestResult() {
                        TestId = test.TestId,
                        ActualResult = result == ReturnValue.MSG_KEYPROVIDER_SUCCESS ? true : false,
                        Comments = result == ReturnValue.MSG_KEYPROVIDER_SUCCESS ? null : result.ToString(),
                    });
                }
                repository.UpdateTest(test.TestId, TestStatus.Complete);
            }
            catch {
                repository.UpdateTest(test.TestId, TestStatus.Aborted);
                throw;
            }
        }

        private void GeneraterUpdateKeyTest(List<TestParameter> testParameters, string productKeyInfo) {
            if (bool.Parse(runtimeSection["IsAutoGenerateReportKey"])) {
                var newTest = new Test() {
                    TestName = UpdateKeyName,
                    IsPositive = true,
                    TestStatus = TestStatus.Ready,
                };
                repository.InsertTest(newTest);

                var newTestParameters = testParameters.Select(t => new TestParameter() {
                    TestId = newTest.TestId,
                    Name = t.Name,
                    Index = t.Index,
                    Value = t.Value,
                }).ToList();

                newTestParameters.Add(new TestParameter() {
                    TestId = newTest.TestId,
                    Name = ProductKeyIDName,
                    Index = testParameters.Count,
                    Value = ParaseParameters(productKeyInfo),
                });
                repository.InsertTestParameters(newTestParameters);
            }
        }

        private string ParaseParameters(string productKeyInfo) {
            XDocument doc = XDocument.Parse(productKeyInfo);
            return (from dm in doc.Elements(KeyName)
                    select dm.Element(ProductKeyIDName).Value).Single();
        }

        private string ParaseParameters(List<TestParameter> parameters) {
            XElement doc = new XElement(ParametersName,
                from p in parameters
                where p.Name != ProductKeyIDName
                select new XElement(ParameterName,
                        new XAttribute(nameName, p.Name),
                        new XAttribute(valueName, p.Value)),
                    new XElement(XElement.Parse(GeneratorOEMOptionalInfo())),
                    new XElement(TrackingInfoName, GeneratorTrackingInfo()));
            return doc.ToString();
        }

        private string ParaseProductKeyInfo(List<TestParameter> parameters) {
            long keyId = long.Parse(parameters.Single(p => p.Name == ProductKeyIDName).Value);

            XElement doc = new XElement(KeyName,
                    new XElement(ProductKeyIDName, keyId),
                    new XElement(ProductKeyStateName, (byte)KeyState.Bound),
                    new XElement(HardwareHashName, GeneratorHardwareHash(keyId.ToString())));
            return doc.ToString();
        }

        private string GeneratorTrackingInfo() {
            return optionalInfesSection[TrackingInfoName];
        }

        private string GeneratorOEMOptionalInfo() {
            return new OemOptionalInfo(
                optionalInfesSection["ZPcModelSku"],
                optionalInfesSection["ZOemExtId"],
                optionalInfesSection["ZManufGeoLoc"],
                optionalInfesSection["ZPgmEligValues"],
                optionalInfesSection["ZChannelRelId"],
                optionalInfesSection["zFrmFactorCl1"],
                optionalInfesSection["zFrmFactorCl2"],
                optionalInfesSection["zScreenSize"],
                optionalInfesSection["zTouchScreen"]).ToString();
        }

        private string GeneratorHardwareHash(string keyId) {
            return HashHelper.CreateHash<SHA1CryptoServiceProvider>(Constants.DefaultEncoding.GetBytes(keyId));
        }
        #endregion
    }
}
