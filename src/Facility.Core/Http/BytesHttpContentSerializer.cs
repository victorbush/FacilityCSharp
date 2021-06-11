using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Facility.Core.Http
{
	/// <summary>
	/// Serializes and deserializes bytes for HTTP requests and responses.
	/// </summary>
	public class BytesHttpContentSerializer : HttpContentSerializer
	{
		/// <summary>
		/// An instance of BytesHttpContentSerializer.
		/// </summary>
		public static readonly BytesHttpContentSerializer Instance = new();

		/// <summary>
		/// The default media type for the serializer.
		/// </summary>
		protected override string DefaultMediaTypeCore => "application/octet-stream";

		/// <summary>
		/// Determines if the specified media type is supported.
		/// </summary>
		protected override bool IsSupportedMediaTypeCore(string mediaType) => true;

		/// <summary>
		/// Determines if the specified media type is accepted when investigating the Accept header.
		/// </summary>
		protected override bool IsAcceptedMediaTypeCore(string mediaType) => false;

		/// <summary>
		/// Creates HTTP content for the specified DTO.
		/// </summary>
		protected override HttpContent CreateHttpContentCore(object content, string? mediaType)
		{
			mediaType ??= DefaultMediaTypeCore;

			if (content is byte[] byteArray)
			{
				var httpContent = new StreamContent(new MemoryStream(byteArray, writable: false));
				httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mediaType);
				return httpContent;
			}

			throw new ArgumentException($"Unexpected content type: {content.GetType().Name}", nameof(content));
		}

		/// <summary>
		/// Reads an object from the specified HTTP content.
		/// </summary>
		protected override async Task<ServiceResult<object>> ReadHttpContentAsyncCore(Type objectType, HttpContent content, CancellationToken cancellationToken)
		{
			if (objectType == typeof(byte[]))
				return ServiceResult.Success((object) await content.ReadAsByteArrayAsync().ConfigureAwait(false));

			throw new ArgumentException($"Unexpected content type: {objectType.Name}", nameof(content));
		}
	}
}
