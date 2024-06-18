// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;
using Facility.Core.Http;

namespace Facility.Benchmarks.Http
{
	/// <summary>
	/// Common service elements.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class HttpClientBenchmarkService : HttpClientService, IBenchmarkService
	{
		/// <summary>
		/// Creates the service.
		/// </summary>
		public HttpClientBenchmarkService(HttpClientServiceSettings? settings = null)
			: base(settings, s_defaults)
		{
		}

		public Task<ServiceResult<GetUsersResponseDto>> GetUsersAsync(GetUsersRequestDto request, CancellationToken cancellationToken = default) =>
			TrySendRequestAsync(BenchmarkServiceHttpMapping.GetUsersMapping, request, cancellationToken);

		private static readonly HttpClientServiceDefaults s_defaults = new HttpClientServiceDefaults
		{
#if NET8_0_OR_GREATER
			ContentSerializer = HttpContentSerializer.Create(BenchmarkServiceJsonServiceSerializer.Instance),
#else
			ContentSerializer = HttpContentSerializer.Create(SystemTextJsonServiceSerializer.Instance),
#endif
		};
	}
}
