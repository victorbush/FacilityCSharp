// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Facility.Core;

namespace EdgeCases
{
	/// <summary>
	/// Request for SnakeMethod.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class SnakeMethodRequestDto : ServiceDto<SnakeMethodRequestDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public SnakeMethodRequestDto()
		{
		}

		[Newtonsoft.Json.JsonProperty("snake_field")]
		[System.Text.Json.Serialization.JsonPropertyName("snake_field")]
		public string? SnakeField { get; set; }

		/// <summary>
		/// Returns the DTO as JSON.
		/// </summary>
		public override string ToString() => SystemTextJsonServiceSerializer.Instance.ToJson(this);

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(SnakeMethodRequestDto? other)
		{
			return other != null &&
				SnakeField == other.SnakeField;
		}
	}
}
