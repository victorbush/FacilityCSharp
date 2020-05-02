// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Facility.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facility.ConformanceApi
{
	/// <summary>
	/// Request for GetWidget.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class GetWidgetRequestDto : ServiceDto<GetWidgetRequestDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public GetWidgetRequestDto()
		{
		}

		/// <summary>
		/// The widget ID.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// Don't get the widget if it has this ETag.
		/// </summary>
		public string? IfNotETag { get; set; }

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(GetWidgetRequestDto? other)
		{
			return other != null &&
				Id == other.Id &&
				IfNotETag == other.IfNotETag;
		}
	}
}
