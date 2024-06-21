
using Shouldly;
using Xunit.Abstractions;


namespace Tests;

public class Tests
{
    private readonly ITestOutputHelper testOutputHelper;

    public Tests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    private Task TODO()
    {
        throw new InvalidOperationException("TODO");
    }
    [Fact]
    public async void Should_ThrowException_WhenDateIsMoreThatDateDue()
    {
        await Should.ThrowAsync<InvalidOperationException>(this.TODO());
    }
}
