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

namespace CreateRibbon
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
            string tabName = "plaginFA";
            application.CreateRibbonTab(tabName);
            string utilsFolderPath = @"C:\Program Files\RevitAPITraining\";

            var panel = application.CreateRibbonPanel(tabName, "5 Задание");
            

            var button1 = new PushButtonData("Система", "Создание кнопки", Path.Combine(utilsFolderPath, "CreatingButtons.dll"), "CreatingButtons.Main");
            var button2 = new PushButtonData("Система1", "Изменение типов стен", Path.Combine(utilsFolderPath, "ChangingWall.dll"), "ChangingWall.Main");

            Uri uriImage1 = new Uri(@"C:\Program Files\RevitAPITraining\Images\1.png", UriKind.Absolute);
            BitmapImage largeImage1= new BitmapImage(uriImage1);
            button1.LargeImage = largeImage1;

            Uri uriImage2 = new Uri(@"C:\Program Files\RevitAPITraining\Images\2.png", UriKind.Absolute);
            BitmapImage largeImage2 = new BitmapImage(uriImage2);
            button2.LargeImage = largeImage2;
            panel.AddItem(button1);
            panel.AddItem(button2);
            return Result.Succeeded;
        }
    }
}
