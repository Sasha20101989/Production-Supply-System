using System.Collections.Generic;
using System.Reflection;
using BLL.Contracts;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using NavigationManager.Frame.Extension.WPF;

using UI_Interface.ViewModels;
using UI_Interface.ViewModels.ViewModelsForPages;
using Xunit;

namespace production_supply_system.TEST.UI.ViewModels
{
    public class MasterViewModelTests
    {
        [Fact]
        public void MasterViewModel_PrivateMethod_HasErrors_ReturnsCorrectResult()
        {
            // Arrange

            Mock<INavigationManager> navigationManagerMock = new();

            Mock<IDocumentService> documentServiceMock = new();

            Mock<IExcelService> excelServiceMock = new();

            Mock<IDialogCoordinator> dialogCoordinatorMock = new();

            MasterViewModel masterViewModel = new(navigationManagerMock.Object, documentServiceMock.Object, excelServiceMock.Object, dialogCoordinatorMock.Object);

            // Act

            bool result = InvokePrivateMethod<bool>(masterViewModel, "HasErrors");

            // Assert

            Assert.False(result);
        }

        private static T InvokePrivateMethod<T>(object instance, string methodName, params object[] parameters)
        {
            MethodInfo methodInfo = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            return (T)methodInfo.Invoke(instance, parameters);
        }
    }
}
