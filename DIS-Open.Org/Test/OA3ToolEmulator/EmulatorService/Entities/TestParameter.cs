using System;
using System.Collections.Generic;

namespace EmulatorService.Entities
{
	public class TestParameter
	{
		public int TestParameterId { get; set; }
		public int TestId { get; set; }
		public string Name { get; set; }
		public Nullable<int> Index { get; set; }
		public string Value { get; set; }
		public Test Test { get; set; }
	}
}

