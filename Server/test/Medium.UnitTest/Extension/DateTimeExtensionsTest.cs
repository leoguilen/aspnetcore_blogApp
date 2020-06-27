using FluentAssertions;
using Medium.Core.Common.Extension;
using System;
using Xunit;

namespace Medium.UnitTest.Extension
{
    public class DateTimeExtensionsTest
    {
        [Fact]
        public void ShouldBe_ReturnedDateTimeNow_WithoutSecondsValue()
        {
            var dateTimeNow = DateTime.Now;

            var dateTimeFormated = dateTimeNow
                .DefaultFormat();

            dateTimeFormated.Should().HaveSecond(0);
        }
    }
}
