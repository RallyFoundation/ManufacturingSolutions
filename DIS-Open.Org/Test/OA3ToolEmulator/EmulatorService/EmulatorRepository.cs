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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmulatorService.Entities;

namespace EmulatorService
{
    public class EmulatorRepository
    {
        private EmulatorContext GetContext()
        {
            return new EmulatorContext();
        }

        public List<Test> GetTests(TestStatus testStatus = TestStatus.Ready)
        {
            using (var context = GetContext())
            {
                return context.Tests.Where(t => t.Status == (byte)testStatus).ToList();
            }
        }

        public List<TestParameter> GetTestParameters(int TestId)
        {
            using (var context = GetContext())
            {
                return context.TestParameters.Where(t => t.TestId == TestId).ToList();
            }
        }

        public void UpdateTest(int testId, TestStatus testStatus)
        {
            using (var context = GetContext())
            {
                var testToUpdate = context.Tests.Single(t => t.TestId == testId);
                testToUpdate.TestStatus = testStatus;
                testToUpdate.UpdatedDate = DateTime.Now;
                context.SaveChanges();
            }
        }

        public void InsertTest(Test test)
        {
            using (var context = GetContext())
            {
                test.ReadyDate = test.UpdatedDate = DateTime.Now;
                context.Tests.Add(test);
                context.SaveChanges();
            }
        }

        public void InsertTestParameters(List<TestParameter> testParameters)
        {
            using (var context = GetContext())
            {
                testParameters.ForEach(t =>
                {
                    context.TestParameters.Add(t);
                });
                context.SaveChanges();
            }
        }

        public void InsertTestResults(List<TestResult> testResults)
        {
            using (var context = GetContext())
            {
                testResults.ForEach(t =>
                {
                    t.UpdatedDate = DateTime.Now;
                    context.TestResults.Add(t);
                });
                context.SaveChanges();
            }
        }

        public void InsertTestResult(TestResult testResult)
        {
            using (var context = GetContext())
            {
                testResult.UpdatedDate = DateTime.Now;
                context.TestResults.Add(testResult);
                context.SaveChanges();
            }
        }
    }
}
