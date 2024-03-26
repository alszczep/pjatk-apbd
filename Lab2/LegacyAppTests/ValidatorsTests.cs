using LegacyApp;

namespace LegacyAppTests;

public class ValidatorsTests
{
    public class IsFullNameValid
    {
        [Fact]
        public void Should_Return_False_When_FirstName_Is_Null()
        {
            string? firstName = null;
            string lastName = "Doe";

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_False_When_LastName_Is_Null()
        {
            string firstName = "John";
            string? lastName = null;

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_False_When_FirstName_And_LastName_Are_Null()
        {
            string? firstName = null;
            string? lastName = null;

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_False_When_FirstName_Is_Empty()
        {
            string firstName = "";
            string lastName = "Doe";

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_False_When_LastName_Is_Empty()
        {
            string firstName = "John";
            string lastName = "";

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_False_When_FirstName_And_LastName_Are_Empty()
        {
            string firstName = "";
            string lastName = "";

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_True_When_FirstName_And_LastName_Are_NonEmpty_Strings()
        {
            string firstName = "John";
            string lastName = "Doe";

            bool result = Validators.IsFullNameValid(firstName, lastName);

            Assert.True(result);
        }
    }

    public class IsEmailValid
    {
        [Fact]
        public void Should_Return_False_When_Email_Without_At_And_Dot()
        {
            string email = "test";

            bool result = Validators.IsEmailValid(email);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_True_When_Email_With_Dot_And_Without_At()
        {
            string email = "test.com";

            bool result = Validators.IsEmailValid(email);

            Assert.True(result);
        }

        [Fact]
        public void Should_Return_True_When_Email_With_At_And_Without_Dot()
        {
            string email = "test@test";

            bool result = Validators.IsEmailValid(email);

            Assert.True(result);
        }

        [Fact]
        public void Should_Return_True_When_Email_With_At_And_Dot()
        {
            string email = "test@test.com";

            bool result = Validators.IsEmailValid(email);

            Assert.True(result);
        }
    }

    public class IsAgeOlderOrEqualTo21
    {
        [Fact]
        public void Should_Return_False_When_Age_Is_Less_Than_21()
        {
            DateTime now = new DateTime(2021, 1, 1);
            DateTime dateOfBirth = new DateTime(2000, 1, 2);

            bool result = Validators.IsAgeOlderOrEqualTo21(now, dateOfBirth);

            Assert.False(result);
        }

        [Fact]
        public void Should_Return_True_When_Age_Is_Equal_To_21()
        {
            DateTime now = new DateTime(2021, 1, 1);
            DateTime dateOfBirth = new DateTime(2000, 1, 1);

            bool result = Validators.IsAgeOlderOrEqualTo21(now, dateOfBirth);

            Assert.True(result);
        }

        [Fact]
        public void Should_Return_True_When_Age_Is_Greater_Than_21()
        {
            DateTime now = new DateTime(2021, 1, 1);
            DateTime dateOfBirth = new DateTime(1999, 1, 1);

            bool result = Validators.IsAgeOlderOrEqualTo21(now, dateOfBirth);

            Assert.True(result);
        }
    }
}
