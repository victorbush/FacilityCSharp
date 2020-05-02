// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Facility.Core;
using Newtonsoft.Json;

#pragma warning disable 612, 618 // member is obsolete

namespace EdgeCases
{
	/// <summary>
	/// Some old values.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	[JsonConverter(typeof(OldValuesJsonConverter))]
	public partial struct OldValues : IEquatable<OldValues>
	{
		/// <summary>
		/// An old value.
		/// </summary>
		[Obsolete]
		public static readonly OldValues Old = new OldValues("old");

		[Obsolete]
		public static readonly OldValues Older = new OldValues("older");

		/// <summary>
		/// Creates an instance.
		/// </summary>
		public OldValues(string value) => m_value = value;

		/// <summary>
		/// Converts the instance to a string.
		/// </summary>
		public override string ToString() => m_value ?? "";

		/// <summary>
		/// Checks for equality.
		/// </summary>
		public bool Equals(OldValues other) => StringComparer.OrdinalIgnoreCase.Equals(ToString(), other.ToString());

		/// <summary>
		/// Checks for equality.
		/// </summary>
		public override bool Equals(object obj) => obj is OldValues && Equals((OldValues) obj);

		/// <summary>
		/// Gets the hash code.
		/// </summary>
		public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(ToString());

		/// <summary>
		/// Checks for equality.
		/// </summary>
		public static bool operator ==(OldValues left, OldValues right) => left.Equals(right);

		/// <summary>
		/// Checks for inequality.
		/// </summary>
		public static bool operator !=(OldValues left, OldValues right) => !left.Equals(right);

		/// <summary>
		/// Returns true if the instance is equal to one of the defined values.
		/// </summary>
		public bool IsDefined() => s_values.Contains(this);

		/// <summary>
		/// Returns all of the defined values.
		/// </summary>
		public static IReadOnlyList<OldValues> GetValues() => s_values;

		/// <summary>
		/// Used for JSON serialization.
		/// </summary>
		public sealed class OldValuesJsonConverter : ServiceEnumJsonConverter<OldValues>
		{
			/// <summary>
			/// Creates the value from a string.
			/// </summary>
			protected override OldValues CreateCore(string value) => new OldValues(value);
		}

		private static readonly ReadOnlyCollection<OldValues> s_values = new ReadOnlyCollection<OldValues>(
			new[]
			{
				Old,
				Older,
			});

		readonly string m_value;
	}
}
