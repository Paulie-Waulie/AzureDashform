namespace ManicStreetCoder.AzureDashform.Windows.UI.Tests.ViewModel
{
    using AzureDashform.ViewModel;
    using FakeItEasy;
    using Model;
    using NUnit.Framework;
    using Service;
    using TestStack.BDDfy;

    [TestFixture]
    public class MainViewModelTests
    {
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

        private void AValidInputFile()
        {
            A.CallTo(() => this.fileService.GetInputDashboardArmTemplate(this.mainViewModel.Details))
                .Returns(this.inputTemplate);
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
    }
}
