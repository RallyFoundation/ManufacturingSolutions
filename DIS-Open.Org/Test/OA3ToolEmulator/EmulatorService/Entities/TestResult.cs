using System;
using System.Collections.Generic;

namespace EmulatorService.Entities
{
	public class TestResult
	{
		public int TestResultId { get; set; }
		public int TestId { get; set; }
		public bool ActualResult { get; set; }
		public string Name { get; set; }
		public Nullable<int> Index { get; set; }
		public string Value { get; set; }
		public Nullable<System.DateTime> UpdatedDate { get; set; }
		public string Comments { get; set; }
		public Test Test { get; set; }
	}
}

