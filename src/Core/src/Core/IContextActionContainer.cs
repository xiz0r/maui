using System.Collections.Generic;

namespace Microsoft.Maui
{
	public interface IContextActionContainer
	{
		IList<IMenuElement> ContextActions { get; }
	}
}
