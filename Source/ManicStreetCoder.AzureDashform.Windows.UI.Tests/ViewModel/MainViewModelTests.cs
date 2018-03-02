namespace ManicStreetCoder.AzureDashform.Windows.UI.Tests.ViewModel
{
    using AzureDashform.ViewModel;
    using FakeItEasy;
    using Model;
    using NUnit.Framework;
    using Service;

    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void Given_A_Valid_Input_File_Then_The_Output_Is_Saved()
        {
            var input = new InputDashboardArmTemplate("SomeJson");
            var output = new OutputDashboardArmTemplate("SomeOutputJson");
            var fileService = A.Fake<ITransformationFileService>();
            var transformationService = A.Fake<ITransformationService>();
            var viewModel = new MainViewModel(fileService, transformationService);

            A.CallTo(() => fileService.GetInputDashboardArmTemplate(viewModel.Details)).Returns(input);
            A.CallTo(() => transformationService.Transform(input)).Returns(output);

            viewModel.TransformCommand.Execute(null);

            A.CallTo(() => fileService.SaveOutputDashboardArmTemplate(output)).MustHaveHappenedOnceExactly();
        }
    }
}
