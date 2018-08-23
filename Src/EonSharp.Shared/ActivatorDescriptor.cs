using System;

namespace EonSharp
{
	public class ActivatorDescriptor
	{
		public Type ClassType { get; set; }
		public object[] Parameters { get; set; }

		public ActivatorDescriptor()
		{

		}
		public ActivatorDescriptor(Type classType, object[] parameters = null)
		{
			this.ClassType = classType;
			this.Parameters = parameters;
		}
	}
}