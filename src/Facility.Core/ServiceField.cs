using System.Diagnostics.CodeAnalysis;

namespace Facility.Core;

/// <summary>
/// Wraps a service field to distinguish default (i.e. unspecified/undefined/missing) from null.
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(ServiceFieldNewtonsoftJsonConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(ServiceFieldSystemTextJsonConverter))]
public readonly struct ServiceField<T> : IEquatable<ServiceField<T>>, IServiceField
{
	/// <summary>
	/// Creates a non-default instance.
	/// </summary>
	public ServiceField(T? value)
	{
		m_value = value;
		m_hasValue = true;
	}

	/// <summary>
	/// Implicitly creates a non-default instance from a (possibly null) value.
	/// </summary>
	[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Use constructor.")]
	public static implicit operator ServiceField<T>(T? value) => new(value);

	/// <summary>
	/// True if this instance is default (i.e. unspecified/undefined/missing).
	/// </summary>
	public bool IsDefault => !m_hasValue;

	/// <summary>
	/// True if this instance has a null value.
	/// </summary>
	public bool IsNull => m_hasValue && m_value is null;

	/// <summary>
	/// The value of this instance, or null/default if the instance is default (i.e. unspecified/undefined/missing).
	/// </summary>
	/// <remarks>This property does not throw an exception when default or null.
	/// Check <see cref="IsDefault" /> to distinguish default from null.</remarks>
	public T? Value => m_value;

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	public bool Equals(ServiceField<T> other) =>
		(m_hasValue && other.m_hasValue) ? ServiceDataUtility.AreEquivalentFieldValues(m_value, other.m_value) : (m_hasValue == other.m_hasValue);

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	public override bool Equals(object? obj) => obj is ServiceField<T> other && Equals(other);

	/// <summary>
	/// Retrieves the hash code of the object.
	/// </summary>
	public override int GetHashCode() => m_hasValue ? (m_value?.GetHashCode() ?? 0) : -1;

	/// <summary>
	/// Returns the text representation of the object.
	/// </summary>
	public override string ToString() => m_value?.ToString() ?? "";

	/// <summary>
	/// Compares two instances for equality.
	/// </summary>
	public static bool operator ==(ServiceField<T> left, ServiceField<T> right) => left.Equals(right);

	/// <summary>
	/// Compares two instances for inequality.
	/// </summary>
	public static bool operator !=(ServiceField<T> left, ServiceField<T> right) => !left.Equals(right);

	object? IServiceField.Value => Value;

	private readonly T? m_value;
	private readonly bool m_hasValue;
}
