using System;
using System.Collections.Generic;
using System.Text;

namespace FirebaseFunctions
{
	/// <summary>
	/// サンプル用のFirebaseに登録するレコード
	/// </summary>
	public class TestRecord
	{
		public string Id { get; set; }
		public int IntValue { get; set; }
		public string StringValue { get; set; }

		public override string ToString()
		{
			return $"Id={Id}, IntValue={IntValue}, StringValue={StringValue}{Environment.NewLine}";
		}
	}
}
