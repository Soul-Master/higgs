using System;

namespace Higgs.Web.Controls.JqGrid
{
	#region Class StringEnum

    #endregion

	#region Class StringValueAttribute

	/// <summary>
	/// Simple attribute class for storing String Values
	/// </summary>
	public class StringValueAttribute : Attribute
	{
		/// <summary>
		/// Creates a new <see cref="StringValueAttribute"/> instance.
		/// </summary>
		/// <param name="value">Value.</param>
		public StringValueAttribute(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value></value>
		public string Value { get; private set; }
	}

	#endregion
}