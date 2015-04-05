using System;
using System.Collections.Generic;

namespace FerroJson
{
    public interface IPropertyValidationError
    {
        IList<string> Errors { get; set; }
		Object AttemptedValue { get; set; }
		ValidationErrorNodeSet items { get; set; }
		string PropertyDescription { get; set; }
    }

    public class PropertyValidationError : IPropertyValidationError
    {
		public IList<string> Errors { get; set; }
		public Object AttemptedValue { get; set; }
		public ValidationErrorNodeSet items { get; set; }
	    public string PropertyDescription { get; set; }
    }

	public class ValidationErrorNodeSet : SortedSet<ValidationNode> {}

	public interface IValidationNode
	{
		int ArrayIndex { get; set; }
	}

	public class ValidationNode : SortedDictionary<string, IPropertyValidationError>, IComparable<ValidationNode>, IValidationNode
	{
		public int ArrayIndex { get; set; }

		public int CompareTo(ValidationNode other)
		{
			return ArrayIndex.CompareTo(other.ArrayIndex);
		}
	}
}
