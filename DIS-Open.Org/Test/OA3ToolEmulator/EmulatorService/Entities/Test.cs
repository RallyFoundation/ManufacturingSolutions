using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmulatorService.Entities
{
    public class Test
    {
        public Test()
        {
            this.TestParameters = new List<TestParameter>();
            this.TestResults = new List<TestResult>();
        }

        public int TestId { get; set; }
        public string TestName { get; set; }
        public bool IsPositive { get; set; }
        public byte Status { get { return (byte)TestStatus; } set { TestStatus = (TestStatus)value; } }
        public Nullable<System.DateTime> ReadyDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public ICollection<TestParameter> TestParameters { get; set; }
        public ICollection<TestResult> TestResults { get; set; }
        [NotMapped]
        public TestStatus TestStatus { get; set; }
    }
}

