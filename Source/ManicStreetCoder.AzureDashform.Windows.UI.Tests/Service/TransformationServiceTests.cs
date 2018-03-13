namespace ManicStreetCoder.AzureDashform.Windows.UI.Tests.Service
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Model;
    using NUnit.Framework;
    using Properties;
    using TestStack.BDDfy;
    using UI.Service;
    using UI.Service.Exceptions;

    [TestFixture]
    public class TransformationServiceTests
    {
        private Exception thrownException;
        private InputDashboardArmTemplate inputDashboardArmTemplate;
        private OutputDashboardArmTemplate output;

        [Test]
        public void InvalidJsonThrowsException()
        {
            var invalidJson = default(string);

            this.Given(_ => _.InvalidJson(invalidJson))
                .When(_ => _.TransformingWithException(), "Transforming")
                .Then(_ => _.AInvalidTemplateExceptionIsThrown())
                .WithExamples(new ExampleTable("invalidJson")
                {
                    "Invalid",
                    "{{invalid}}",
                    "{ invalid }",
                    @"{ ""property"": ""value""",
                    @"{ ""property"": ""[]""",
                    @"{ ""property"": []",
                })
                .BDDfy();
        }

        [Test]
        public void ValidJsonReturnsExpectedResult()
        {
            var validJson = default(string);
            var expectedJsonOutput = default(string);

            this.Given(_ => _.ValidJson(validJson))
                .When(_ => _.Transforming())
                .Then(_ => _.TheOutputTemplateMatchesExpected(expectedJsonOutput))
                .And(_ => _.TheParametersFileMatchesExpected())
                .WithExamples(new ExampleTable(nameof(validJson), nameof(expectedJsonOutput))
                {
                    { Resources.GoldenMasterInputTemplate1, Resources.GoldenMasterOutputTemplate1 }
                })
                .BDDfy();
        }

        private void ValidJson(string json)
        {
            this.inputDashboardArmTemplate = new InputDashboardArmTemplate(json);
        }

        private void InvalidJson(string json)
        {
            this.inputDashboardArmTemplate = new InputDashboardArmTemplate(json);
        }

        private void TransformingWithException()
        {
            this.thrownException = null;
            try
            {
                this.Transforming();
            }
            catch (Exception e)
            {
                this.thrownException = e;
            }
        }

        private void Transforming()
        {
            this.output = new TransformationService().Transform(this.inputDashboardArmTemplate);
        }

        private void AInvalidTemplateExceptionIsThrown()
        {
            this.thrownException.Should().NotBeNull();
            this.thrownException.Should().BeOfType<InvalidInputTemplateException>();
        }

        private void TheOutputTemplateMatchesExpected(string expected)
        {
            Assert.AreEqual(expected, this.output.TemplateJson);
        }

        private void TheParametersFileMatchesExpected()
        {
            Assert.AreEqual(Resources.GoldenMasterParameters, this.output.ParametersJson);
        }
    }
}
