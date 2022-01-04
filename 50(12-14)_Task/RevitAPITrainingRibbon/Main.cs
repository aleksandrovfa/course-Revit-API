using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitAPITrainingRibbon
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Revit API training";
            application.CreateRibbonTab(tabName);
            string utilsFolderPath = @"C:\Program Files\RevitAPITraining\";

            var panel = application.CreateRibbonPanel(tabName, "Трубы");

            var button = new PushButtonData("Система", "Смена системы труб", 
                Path.Combine(utilsFolderPath, "RevitAPITrainingUI.dll"), "RevitAPITrainingUI.Main");

            Uri uriImage = new Uri(@"C:\Program Files\RevitAPITraining\Images\RevitAPITrainingUI_32.png", UriKind.Absolute);
            BitmapImage largeImage = new BitmapImage(uriImage);
            button.LargeImage = largeImage;

            panel.AddItem(button);
            return Result.Succeeded;
        }
    }
}
