using System;

namespace RandomAutomata
{

	public class Rule
	{

		public Rule ()
		{
			this.states = new byte[ruleLength];
			long workingRuleNumber = ruleNumber;
			for (int i = 0; i < this.states.Length; i++) {
				long temp;
				workingRuleNumber = Math.DivRem (workingRuleNumber, 2L, out temp);
				this.states [i] = (byte)temp;
			}
		}

		public byte GetNextState (int neighbourhood)
		{
			return this.states [neighbourhood];
		}

		private byte[] states;

		private const long ruleNumber = 1436965290;
		private const int ruleLength = 32;
	
	}

}

