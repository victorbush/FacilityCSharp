using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace Facility.Core.UnitTests;

[TestFixtureSource(nameof(JsonServiceSerializers))]
public class ServiceDtoTests : JsonServiceSerializerTestsBase
{
	public ServiceDtoTests(JsonServiceSerializer jsonSerializer)
		: base(jsonSerializer)
	{
	}

	[Test]
	public void ToStringUsesJson()
	{
		var dto = new TestDto { Id = 3, Name = "Three", Children = new[] { new TestDto { Name = "child" } } };
		var json = @"{""id"":3,""name"":""Three"",""children"":[{""name"":""child""}]}";
		dto.ToString().Should().Be(json);
		JsonSerializer.ToJson(dto).Should().Be(json);
	}

	[Test]
	public void BasicEquivalence()
	{
		var empty = new TestDto();
		var full = new TestDto { Id = 3, Name = "Three", Children = new[] { new TestDto { Name = "child" } } };

		empty.IsEquivalentTo(null).Should().BeFalse();
		empty.IsEquivalentTo(empty).Should().BeTrue();
		empty.IsEquivalentTo(new TestDto()).Should().BeTrue();
		empty.IsEquivalentTo(full).Should().BeFalse();
		full.IsEquivalentTo(new TestDto { Id = 3 }).Should().BeFalse();

		full.IsEquivalentTo(null).Should().BeFalse();
		full.IsEquivalentTo(empty).Should().BeFalse();
		full.IsEquivalentTo(new TestDto { Id = 3, Name = "Three", Children = new[] { new TestDto { Name = "child" } } }).Should().BeTrue();
		full.IsEquivalentTo(full).Should().BeTrue();
		full.IsEquivalentTo(new TestDto { Id = 3 }).Should().BeFalse();
	}

	[Test]
	public void BasicCloning()
	{
		var first = new TestDto { Id = 3, Name = "Three", Children = new[] { new TestDto { Name = "child" } } };
		var second = JsonSerializer.Clone(first);
		first.IsEquivalentTo(second).Should().Be(true);
		second.Id += 1;
		first.IsEquivalentTo(second).Should().Be(false);
	}

	[SuppressMessage("ReSharper", "All", Justification = "unit test")]
	private class TestDto : ServiceDto<TestDto>
	{
		public int? Id { get; set; }

		public string? Name { get; set; }

		public IReadOnlyList<TestDto>? Children { get; set; }

		public override bool IsEquivalentTo(TestDto? other)
		{
			return other != null &&
				other.Id == Id &&
				other.Name == Name &&
				ServiceDataUtility.AreEquivalentArrays(other.Children, Children, ServiceDataUtility.AreEquivalentDtos);
		}
	}
}
