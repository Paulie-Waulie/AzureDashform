namespace ManicStreetCoder.AzureDashform.Windows.UI.Tests.ViewModel
{
    using System;
    using System.Collections.Generic;
    using AzureDashform.ViewModel;
    using FakeItEasy;
    using FluentAssertions;
    using GalaSoft.MvvmLight.Messaging;
    using Model;
    using NUnit.Framework;
    using TestStack.BDDfy;
    using UI.Service;
    using UI.ViewModel.Validation;

    [TestFixture]
    public class MainViewModelTests
    {
        private const string OutputPath = @"C:\Output";
        private const string InvalidInputSourceFilePathErrorMessage = "Please provide a valid input source file path.";
        private const string InvalidOutputSourceFilePathErrorMessage = "Please provide a valid output file path.";
        private ITransformationFileService fileService;
        private MainViewModel mainViewModel;
        private InputDashboardArmTemplate inputTemplate;
        private OutputDashboardArmTemplate completeOutputTemplate;
        private OutputDashboardArmTemplate partOfExistingTemplateOutput;
        private ITransformationService transformationService;
        private Exception reportedError;
        private string userMessage;

        [SetUp]
        public void Setup()
        {
            this.reportedError = null;
            this.userMessage = null;
            this.inputTemplate = new InputDashboardArmTemplate("SomeJson");
            this.completeOutputTemplate = new OutputDashboardArmTemplate("SomeOutputJson", "SomeParametersJson");
            this.partOfExistingTemplateOutput = new OutputDashboardArmTemplate("PartOutputJson", "SomeParametersJson");

            this.fileService = A.Fake<ITransformationFileService>();
            this.transformationService = A.Fake<ITransformationService>();

            Messenger.Default.Register<Exception>(this, e => this.reportedError = e);
            Messenger.Default.Register<string>(this, e => this.userMessage = e);
            this.mainViewModel = new MainViewModel(this.fileService, this.transformationService);
            this.mainViewModel.SourceFilePath = @"C:\Input.json";
            this.mainViewModel.OutputFolderPath = OutputPath;

            A.CallTo(() => this.transformationService.Transform(this.inputTemplate, A<TransformationDetails>.That.Matches(x => x.DashboardIsCompleteTemplate.Equals(true)))).Returns(this.completeOutputTemplate);
            A.CallTo(() => this.transformationService.Transform(this.inputTemplate, A<TransformationDetails>.That.Matches(x => x.DashboardIsCompleteTemplate.Equals(false)))).Returns(this.partOfExistingTemplateOutput);
        }

        [Test]
        public void ValidInputFileToBeOutputtedAsCompleteTemplate()
        {
            this.Given(_ => _.AValidInputFile())
                .And(_ => _.TheOutputIsToBeCompleteTemplate())
                .When(_ => _.TransformingTheInputFile())
                .Then(_ => _.TheCompleteTemplateIsSaved())
                .And(_ => _.TheUserIsInformedTheTransformSucceeded())
                .BDDfy();
        }

        [Test]
        public void ValidInputFileToBeOutputtedAsPartOfExistingTemplate()
        {
            this.Given(_ => _.AValidInputFile())
                .And(_ => _.TheOutputIsToBePartOfAnExistingTemplate())
                .When(_ => _.TransformingTheInputFile())
                .Then(_ => _.ThePartialTemplateIsSaved())
                .And(_ => _.TheUserIsInformedTheTransformSucceeded())
                .BDDfy();
        }

        [Test]
        public void InValidSourceFilePath()
        {
            var value = default(string);

            this.Given(_ => _.AnInvalidInputFileSourcePath(value))
                .When(_ => _.TransformingTheInputFile())
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
                .When(_ => _.TransformingTheInputFile())
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
                .When(_ => _.TransformingTheInputFile())
                .Then(_ => _.TheValidationErrorsAreRaised(errors))
                .And(_ => _.TheOutputIsNotSaved())
                .BDDfy();
        }

        [Test]
        public void ReadingFileThrowsError()
        {
            var exception = new Exception("InputFileException");

            this.Given(_ => _.AnInputFileSourcePathThatCausesAnError(exception))
                .When(_ => _.TransformingTheInputFile())
                .Then(_ => _.TheErrorMessageIsReportedToTheUser(exception))
                .And(_ => _.TheOutputIsNotSaved())
                .BDDfy();
        }

        [Test]
        public void TransformingFileThrowsError()
        {
            var exception = new Exception("TransformationException");

            this.Given(_ => _.AnInputFileThatCausesAnTransformationError(exception))
                .When(_ => _.TransformingTheInputFile())
                .Then(_ => _.TheErrorMessageIsReportedToTheUser(exception))
                .And(_ => _.TheOutputIsNotSaved())
                .BDDfy();
        }

        [Test]
        public void SavingFileThrowsError()
        {
            var exception = new Exception("SavingFileException");

            this.Given(_ => _.AnOutputFileSourcePathThatCausesAnError(exception))
                .When(_ => _.TransformingTheInputFile())
                .Then(_ => _.TheErrorMessageIsReportedToTheUser(exception))
                .BDDfy();
        }

        private void AValidInputFile()
        {
            A.CallTo(() => this.fileService.GetInputDashboardArmTemplate(A<TransformationDetails>.Ignored))
                .Returns(this.inputTemplate);
        }

        private void TheOutputIsToBeCompleteTemplate()
        {
            this.mainViewModel.CreateOutputAsCompleteTemplate = true;
        }

        private void TheOutputIsToBePartOfAnExistingTemplate()
        {
            this.mainViewModel.CreateOutputAsCompleteTemplate = false;
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
            A.CallTo(() => this.transformationService.Transform(null, null)).WithAnyArguments().Throws(exception);
        }

        private void AnOutputFileSourcePathThatCausesAnError(Exception exception)
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(null, null)).WithAnyArguments().Throws(exception);
        }

        private void TransformingTheInputFile()
        {
            this.mainViewModel.TransformCommand.Execute(null);
        }

        private void TheCompleteTemplateIsSaved()
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(this.completeOutputTemplate, A<TransformationDetails>.That.Matches(x => x.OutputFilePath.Equals(OutputPath))))
                     .MustHaveHappenedOnceExactly();
        }

        private void ThePartialTemplateIsSaved()
        {
            A.CallTo(() => this.fileService.SaveOutputDashboardArmTemplate(this.partOfExistingTemplateOutput, A<TransformationDetails>.That.Matches(x => x.OutputFilePath.Equals(OutputPath))))
                .MustHaveHappenedOnceExactly();
        }

        private void TheUserIsInformedTheTransformSucceeded()
        {
            this.userMessage.Should().Be("The transform succeeded.");
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
