using CommunityToolkit.WinUI.Notifications;

using File.Manager;

using UI_Interface.Contracts;

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace UI_Interface.Services {
    public partial class ToastNotificationsService
    {
        private readonly ISystemService _systemService;

        public ToastNotificationsService(ISystemService systemService)
        {
            _systemService = systemService;
        }

        public void ShowToastNotificationMessage(string header, string message, string inputFilePath = null, string ngFilePath = null, string outputPath = null)
        {
            string ngButtonContent = "NG File";
            string inputButtonContent = "Input File";
            string outputButtonContent = "Output File";
            string openInputFilePath = inputFilePath;
            string openOutputFilePath = outputPath;
            string openNgFilePath = ngFilePath;

            ToastContent content = new()
            {
                Launch = "ToastContentActivationParams",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = header
                            },
                            new AdaptiveText()
                            {
                                Text = message
                            }
                        }
                    }
                },
            };

            if (inputFilePath != null)
            {
                content.Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton(inputButtonContent, $"OpenInputFile;{openInputFilePath}")
                        {
                            ActivationType = ToastActivationType.Foreground,
                        }
                    }
                };
            }
            else if (ngFilePath != null)
            {
                content.Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton(ngButtonContent, $"OpenNGFile;{openNgFilePath}")
                        {
                            ActivationType = ToastActivationType.Foreground,
                        }
                    }
                };
            }

            if (inputFilePath != null && ngFilePath != null)
            {
                content.Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                       new ToastButton(ngButtonContent, $"OpenNGFile;{openNgFilePath}")
                       {
                           ActivationType = ToastActivationType.Foreground,
                       },
                       new ToastButton(inputButtonContent, $"OpenInputFile;{openInputFilePath}")
                       {
                           ActivationType = ToastActivationType.Foreground,
                       }
                    }
                };
            }

            if (outputPath != null)
            {
                content.Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                       new ToastButton(outputButtonContent, $"OpenOutputFile;{openOutputFilePath}")
                       {
                           ActivationType = ToastActivationType.Foreground,
                       }
                    }
                };
            }

            XmlDocument doc = new();
            doc.LoadXml(content.GetContent());

            ToastNotification toast = new(doc)
            {
                ExpirationTime = null
            };

            toast.Activated += Toast_Activated;

            ShowToastNotification(toast);
        }

        private async void Toast_Activated(ToastNotification sender, object args)
        {
            if (args is ToastActivatedEventArgs toastArgs)
            {
                string activationArguments = toastArgs.Arguments;
                string[] arguments = activationArguments.Split(';');

                if (arguments.Length == 2)
                {
                    string action = arguments[0];
                    string filePath = arguments[1];

                    if (action.Equals("OpenInputFile"))
                    {
                        await _systemService.OpenExcelFile(filePath);
                    }
                    else if (action.Equals("OpenNGFile"))
                    {
                        await _systemService.OpenExcelFile(filePath);
                    }
                    else if (action.Equals("OpenOutputFile"))
                    {
                        await _systemService.OpenExcelFile(filePath);
                    }
                }
            }
        }
    }
}
