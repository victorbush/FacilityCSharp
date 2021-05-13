using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Facility.ConformanceApi.Http;
using Facility.Core;
using Facility.Core.Assertions;
using Facility.Core.Http;
using FluentAssertions;
using NUnit.Framework;

namespace Facility.ConformanceApi.UnitTests
{
	public sealed class DtoValidationTests
	{
		[Test]
		public void RequiredRequestFieldsSpecified()
		{
			var dto = CreateRequiredRequest();
			dto.Validate(out var errorMessage).Should().BeTrue();
			errorMessage.Should().BeNull();
		}

		[Test]
		public void RequiredResponseFieldsSpecified()
		{
			var dto = CreateRequiredResponse();
			dto.Validate(out var errorMessage).Should().BeTrue();
			errorMessage.Should().BeNull();
		}

		[Test]
		public async Task HttpRequiredFieldsSpecified()
		{
			var api = CreateHttpApi();
			var dto = CreateRequiredRequest();
			var result = await api.RequiredAsync(dto, CancellationToken.None);
			result.Should().BeSuccess();
		}

		[Test]
		public async Task HttpRequiredFieldsMissingNoValidation()
		{
			var api = CreateHttpApi(skipClientValidation: true, skipServerValidation: true, requiredResponse: new RequiredResponseDto());
			var result = await api.RequiredAsync(new RequiredRequestDto(), CancellationToken.None);
			result.Should().BeSuccess();
		}

		[Test]
		public void RequiredNormalRequestFieldMissing()
		{
			var dto = CreateRequiredRequest();
			dto.Normal = null;
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetRequiredFieldErrorMessage("normal"));
		}

		[Test]
		public async Task HttpRequiredNormalRequestFieldMissing([Values] bool skipClientValidation)
		{
			var api = CreateHttpApi(skipClientValidation: skipClientValidation);
			var dto = CreateRequiredRequest();
			dto.Normal = null;
			var result = await api.RequiredAsync(dto, CancellationToken.None);
			result.Should().BeFailure(ServiceErrors.CreateRequestFieldRequired("normal"));
		}

		[Test]
		public void RequiredNormalResponseFieldMissing()
		{
			var dto = CreateRequiredResponse();
			dto.Normal = null;
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetRequiredFieldErrorMessage("normal"));
		}

		[Test]
		public async Task HttpRequiredNormalResponseFieldMissing([Values] bool skipClientValidation)
		{
			var dto = CreateRequiredResponse();
			dto.Normal = null;
			var api = CreateHttpApi(requiredResponse: dto, skipClientValidation: skipClientValidation);
			var result = await api.RequiredAsync(CreateRequiredRequest(), CancellationToken.None);
			result.Should().BeFailure(ServiceErrors.CreateResponseFieldRequired("normal"));
		}

		[Test]
		public void RequiredQueryFieldMissing()
		{
			var dto = CreateRequiredRequest();
			dto.Query = null;
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetRequiredFieldErrorMessage("query"));
		}

		[Test]
		public async Task HttpRequiredQueryFieldMissing([Values] bool skipClientValidation)
		{
			var api = CreateHttpApi(skipClientValidation: skipClientValidation);
			var dto = CreateRequiredRequest();
			dto.Query = null;
			var result = await api.RequiredAsync(dto, CancellationToken.None);
			result.Should().BeFailure(ServiceErrors.CreateRequestFieldRequired("query"));
		}

		[Test]
		public void RequiredPathFieldMissing()
		{
			var dto = new GetWidgetRequestDto();
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetRequiredFieldErrorMessage("id"));
		}

		[Test]
		public void RequiredPathFieldSpecified()
		{
			var dto = new GetWidgetRequestDto { Id = 3 };
			dto.Validate(out var errorMessage).Should().BeTrue();
			errorMessage.Should().BeNull();
		}

		[Test]
		public async Task HttpRequiredPathFieldMissing([Values] bool skipClientValidation)
		{
			var api = CreateHttpApi(skipClientValidation: skipClientValidation);
			var result = await api.GetWidgetAsync(new GetWidgetRequestDto(), CancellationToken.None);
			result.Should().BeFailure(ServiceErrors.CreateRequestFieldRequired("id"));
		}

		[Test]
		public async Task HttpRequiredPathFieldSpecified()
		{
			var api = CreateHttpApi();
			var result = await api.GetWidgetAsync(new GetWidgetRequestDto { Id = 3 }, CancellationToken.None);
			result.Should().BeSuccess();
		}

		[Test]
		public void RequiredBodyFieldMissing()
		{
			var dto = new GetWidgetBatchRequestDto();
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetRequiredFieldErrorMessage("ids"));
		}

		[Test]
		public void RequiredBodyFieldSpecified()
		{
			var dto = new GetWidgetBatchRequestDto { Ids = new[] { 3 } };
			dto.Validate(out var errorMessage).Should().BeTrue();
			errorMessage.Should().BeNull();
		}

		[Test]
		public async Task HttpRequiredBodyFieldMissing([Values] bool skipClientValidation)
		{
			var api = CreateHttpApi(skipClientValidation: skipClientValidation);
			var result = await api.GetWidgetBatchAsync(new GetWidgetBatchRequestDto(), CancellationToken.None);
			result.Should().BeFailure(ServiceErrors.CreateRequestFieldRequired("ids"));
		}

		[Test]
		public async Task HttpRequiredBodyFieldSpecified()
		{
			var api = CreateHttpApi();
			var result = await api.GetWidgetBatchAsync(new GetWidgetBatchRequestDto { Ids = new[] { 3 } }, CancellationToken.None);
			result.Should().BeSuccess();
		}

		[Test]
		public void RequiredWidgetNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.Widget = new WidgetDto();
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("widget", ServiceDataUtility.GetRequiredFieldErrorMessage("name")));
		}

		[Test]
		public void RequiredWidgetsNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.Widgets = new[] { CreateWidget(), new WidgetDto() };
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("widgets[1]", ServiceDataUtility.GetRequiredFieldErrorMessage("name")));
		}

		[Test]
		public void RequiredWidgetMatrixNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.WidgetMatrix = new[] { new[] { new WidgetDto() } };
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("widgetMatrix[0][0]", ServiceDataUtility.GetRequiredFieldErrorMessage("name")));
		}

		[Test]
		public void RequiredWidgetResultNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.WidgetResult = ServiceResult.Success(new WidgetDto());
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("widgetResult", ServiceDataUtility.GetRequiredFieldErrorMessage("name")));
		}

		[Test]
		public void RequiredWidgetResultsNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.WidgetResults = new[] { ServiceResult.Success(new WidgetDto()) };
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("widgetResults[0]", ServiceDataUtility.GetRequiredFieldErrorMessage("name")));
		}

		[Test]
		public void RequiredWidgetMapNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.WidgetMap = new Dictionary<string, WidgetDto> { { "key", new WidgetDto() } };
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("widgetMap.key", ServiceDataUtility.GetRequiredFieldErrorMessage("name")));
		}

		[Test]
		public void RequiredHasWidgetNameMissing()
		{
			var dto = CreateRequiredRequest();
			dto.HasWidget = new HasWidgetDto { Widget = new WidgetDto() };
			dto.Validate(out var errorMessage).Should().BeFalse();
			errorMessage.Should().Be(ServiceDataUtility.GetInvalidFieldErrorMessage("hasWidget", ServiceDataUtility.GetInvalidFieldErrorMessage("widget", ServiceDataUtility.GetRequiredFieldErrorMessage("name"))));
		}

		private static RequiredRequestDto CreateRequiredRequest() => new RequiredRequestDto { Query = "query", Normal = "normal" };

		private static RequiredResponseDto CreateRequiredResponse() => new RequiredResponseDto { Normal = "normal" };

		private static WidgetDto CreateWidget() => new WidgetDto { Name = "name" };

		private static HttpClientConformanceApi CreateHttpApi(bool skipClientValidation = false, bool skipServerValidation = false, RequiredResponseDto? requiredResponse = null)
		{
			var service = new FakeConformanceApiService(requiredResponse: requiredResponse);
			var settings = new ServiceHttpHandlerSettings
			{
				SkipRequestValidation = skipServerValidation,
				SkipResponseValidation = skipServerValidation,
			};
			var handler = new ConformanceApiHttpHandler(service, settings) { InnerHandler = new NotFoundHttpHandler() };
			var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://example.com/") };
			return new HttpClientConformanceApi(new HttpClientServiceSettings
			{
				HttpClient = httpClient,
				SkipRequestValidation = skipClientValidation,
				SkipResponseValidation = skipClientValidation,
			});
		}

		private sealed class FakeConformanceApiService : IConformanceApi
		{
			public FakeConformanceApiService(RequiredResponseDto? requiredResponse = null, WidgetDto? widgetResponse = null)
			{
				m_requiredResponse = requiredResponse ?? CreateRequiredResponse();
				m_widgetResponse = widgetResponse ?? CreateWidget();
			}

			public async Task<ServiceResult<GetApiInfoResponseDto>> GetApiInfoAsync(GetApiInfoRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<GetWidgetsResponseDto>> GetWidgetsAsync(GetWidgetsRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<CreateWidgetResponseDto>> CreateWidgetAsync(CreateWidgetRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<GetWidgetResponseDto>> GetWidgetAsync(GetWidgetRequestDto request, CancellationToken cancellationToken = default) => ServiceResult.Success(new GetWidgetResponseDto { Widget = ServiceDataUtility.Clone(m_widgetResponse) });

			public async Task<ServiceResult<DeleteWidgetResponseDto>> DeleteWidgetAsync(DeleteWidgetRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<GetWidgetBatchResponseDto>> GetWidgetBatchAsync(GetWidgetBatchRequestDto request, CancellationToken cancellationToken = default) => ServiceResult.Success(new GetWidgetBatchResponseDto { Results = Array.Empty<ServiceResult<WidgetDto>>() });

			public async Task<ServiceResult<MirrorFieldsResponseDto>> MirrorFieldsAsync(MirrorFieldsRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<CheckQueryResponseDto>> CheckQueryAsync(CheckQueryRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<CheckPathResponseDto>> CheckPathAsync(CheckPathRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<MirrorHeadersResponseDto>> MirrorHeadersAsync(MirrorHeadersRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<MixedResponseDto>> MixedAsync(MixedRequestDto request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

			public async Task<ServiceResult<RequiredResponseDto>> RequiredAsync(RequiredRequestDto request, CancellationToken cancellationToken = default) => ServiceResult.Success(ServiceDataUtility.Clone(m_requiredResponse));

			private readonly RequiredResponseDto m_requiredResponse;
			private readonly WidgetDto m_widgetResponse;
		}

		private sealed class NotFoundHttpHandler : HttpMessageHandler
		{
			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
				Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
		}
	}
}
