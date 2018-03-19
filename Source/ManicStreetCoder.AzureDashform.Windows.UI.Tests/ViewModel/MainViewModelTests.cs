namespace ManicStreetCoder.AzureDashform.Windows.UI.Tests.ViewModel
{
    using System;
    using System.Collections.Generic;
    using AzureDashform.ViewModel;
    using FakeItEasy;
    using FluentAssertions;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using Model;
    using NUnit.Framework;
    using TestStack.BDDfy;
    using UI.Service;
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
        private ITransformationService transformationService;
        private Exception reportedError;

        [SetUp]
        public void Setup()
        {
            this.reportedError = null;
            this.inputTemplate = new InputDashboardArmTemplate("SomeJson");
            this.outputTemplate = new OutputDashboardArmTemplate("SomeOutputJson", "SomeParametersJson");

            this.fileService = A.Fake<ITransformationFileService>();
            this.transformationService = A.Fake<ITransformationService>();

            Messenger.Default.Register<Exception>(this, e => this.reportedError = e);
            mainViewModel = new MainViewModel(this.fileService, transformationService);
            mainViewModel.SourceFilePath = @"C:\Input.json";
            mainViewModel.OutputFolderPath = @"C:\Output";

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
        public void InValidOutputFolderPath()
        {
            var value = default(string);

            this.Given(_ => _.AnInvalidOutputFileSourcePath(value))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheValidationErrorIsRaised(new ValidationError(InvalidOutputSourceFilePathErrorMessage)))
                .And(_ => _.TheOutputIsNotSaved())
                .WithExamples(InvalidFolderPathExamples())
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

        [Test]
        public void ReadingFileThrowsError()
        {
            var exception = new Exception("InputFileException");

            this.Given(_ => _.AnInputFileSourcePathThatCausesAnError(exception))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheErrorMessageIsReportedToTheUser(exception))
                .And(_ => _.TheOutputIsNotSaved())
                .BDDfy();
        }

        [Test]
        public void TransformingFileThrowsError()
        {
            var exception = new Exception("TransformationException");

            this.Given(_ => _.AnInputFileThatCausesAnTransformationError(exception))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheErrorMessageIsReportedToTheUser(exception))
                .And(_ => _.TheOutputIsNotSaved())
                .BDDfy();
        }

        [Test]
        public void SavingFileThrowsError()
        {
            var exception = new Exception("SavingFileException");

            this.Given(_ => _.AnOutputFileSourcePathThatCausesAnError(exception))
                .When(_ => _.TrasformingTheInputFile())
                .Then(_ => _.TheErrorMessageIsReportedToTheUser(exception))
                .BDDfy();
        }

        private void AValidInputFile()
        {
            A.CallTo(() => this.fileService.GetInputDashboardArmTemplate(this.mainViewModel.SourceFilePath))
                .Returns(this.inputTemplate);
        }

        private void AnInvalidInputFileSourcePath(string value)
        {
            this.mainViewModel.SourceFilePath = value;
        }

        private void AnInvalidOutputFileSourcePath(string value)
        {
            this.mainViewModel.OutputFolderPath = value;
        }

        private void AnInputFileSourcePathThatCausesAnError(Exception exception)
        {
            A.CallTo(() => this.fileService.GetInputDashboardArmTemplate(null)).WithAnyArguments().Throws(exception);
        }

        private void AnInputFileThatCausesAnTransformationError(Exception exception)
        {
            A.CallTo(() => this.transformationService.Transform(null)).WithAnyArguments().Throws(exception);
        }

        private void AnOutputFileSourcePathThatCausesAnError(Exception exception)
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(null, null)).WithAnyArguments().Throws(exception);
        }

        private void TrasformingTheInputFile()
        {
            this.mainViewModel.TransformCommand.Execute(null);
        }

        private void TheOutputIsSaved()
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(this.outputTemplate, this.mainViewModel.OutputFolderPath))
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

        private void TheErrorMessageIsReportedToTheUser(Exception exception)
        {
            this.reportedError.Should().Be(exception);
        }

        private void TheOutputIsNotSaved()
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(null, null))
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

        private static ExampleTable InvalidFolderPathExamples()
        {
            return new ExampleTable("value")
            {
                string.Empty,
                (string)null,
                " "
            };
        }
    }
}
