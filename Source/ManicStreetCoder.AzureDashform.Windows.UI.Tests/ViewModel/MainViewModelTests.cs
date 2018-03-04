namespace ManicStreetCoder.AzureDashform.Windows.UI.Tests.ViewModel
{
    using System.Collections.Generic;
    using AzureDashform.ViewModel;
    using FakeItEasy;
    using FluentAssertions;
    using Model;
    using NUnit.Framework;
    using Service;
    using TestStack.BDDfy;
    using UI.ViewModel.Validation;

    [TestFixture]
    public class MainViewModelTests
    {
        private const string InvalidInputSourceFilePathErrorMessage = "Please provide a valid input source file path.";
        private const string InvalidOutputSourceFilePathErrorMessage = "Please provide a valid output file path.";
        private ITransformationFileService fileService;
        private MainViewModel mainViewModel;
        private InputDashboardArmTemplate inputTemplate;
        private OutputDashboardArmTemplate outputTemplate;

        [SetUp]
        public void Setup()
        {
            this.inputTemplate = new InputDashboardArmTemplate("SomeJson");
            this.outputTemplate = new OutputDashboardArmTemplate("SomeOutputJson");

            this.fileService = A.Fake<ITransformationFileService>();
            var transformationService = A.Fake<ITransformationService>();
            mainViewModel = new MainViewModel(this.fileService, transformationService);
            mainViewModel.Details.SourceFilePath = @"C:\Input.json";
            mainViewModel.Details.OutputFilePath = @"C:\Output.json";

            A.CallTo(() => transformationService.Transform(this.inputTemplate)).Returns(outputTemplate);
        }

        [Test]
        public void ValidInputFile()
        {
            this.Given(_ => _.AValidInputFile())
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheOutputIsSaved())
                .BDDfy();
        }

        [Test]
        public void InValidSourceFilePath()
        {
            var value = default(string);

            this.Given(_ => _.AnInvalidInputFileSourcePath(value))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheValidationErrorIsRaised(new ValidationError(InvalidInputSourceFilePathErrorMessage)))
                .And(_ => _.TheOutputIsNotSaved())
                .WithExamples(InvalidFilePathExamples())
                .BDDfy();
        }

        [Test]
        public void InValidOutputFilePath()
        {
            var value = default(string);

            this.Given(_ => _.AnInvalidOutputFileSourcePath(value))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheValidationErrorIsRaised(new ValidationError(InvalidOutputSourceFilePathErrorMessage)))
                .And(_ => _.TheOutputIsNotSaved())
                .WithExamples(InvalidFilePathExamples())
                .BDDfy();
        }

        [Test]
        public void InValidInputAndOutputFilePaths()
        {
            var errors = new[]
            {
                new ValidationError(InvalidInputSourceFilePathErrorMessage),
                new ValidationError(InvalidOutputSourceFilePathErrorMessage) 
            };

            this.Given(_ => _.AnInvalidInputFileSourcePath(string.Empty))
                .And(_ => _.AnInvalidOutputFileSourcePath(string.Empty))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheValidationErrorsAreRaised(errors))
                .And(_ => _.TheOutputIsNotSaved())
                .BDDfy();
        }

        private void AValidInputFile()
        {
            A.CallTo(() => this.fileService.GetInputDashboardArmTemplate(this.mainViewModel.Details))
                .Returns(this.inputTemplate);
        }

        private void AnInvalidInputFileSourcePath(string value)
        {
            this.mainViewModel.Details.SourceFilePath = value;
        }

        private void AnInvalidOutputFileSourcePath(string value)
        {
            this.mainViewModel.Details.OutputFilePath = value;
        }

        private void TrasformingTheInputFile()
        {
            this.mainViewModel.TransformCommand.Execute(null);
        }

        private void TheOutputIsSaved()
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(this.outputTemplate))
                     .MustHaveHappenedOnceExactly();
        }

        private void TheValidationErrorIsRaised(ValidationError expectedError)
        {
            this.mainViewModel.ValidationErrors.Should().Contain(expectedError);
        }

        private void TheValidationErrorsAreRaised(IEnumerable<ValidationError> expectedErrors)
        {
            this.mainViewModel.ValidationErrors.Should().Contain(expectedErrors);
        }

        private void TheOutputIsNotSaved()
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(null))
                .WithAnyArguments()
                .MustNotHaveHappened();
        }

        private static ExampleTable InvalidFilePathExamples()
        {
            return new ExampleTable("value")
            {
                string.Empty,
                (string)null,
                " ",
                @"C:\File",
                "File.json",
                @"C:>File.json"
            };
        }
    }
}
