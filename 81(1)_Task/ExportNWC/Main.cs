using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace ExportNWC
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            NavisworksExportOptions nweOptions = new NavisworksExportOptions();
            //nweOptions.ExportScope = NavisworksExportScope.Model;
            //nweOptions.ViewId = uidoc.ActiveView.Id;
            string path = "";


            //Выбор папки
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
            }
            else
            {
                return Result.Succeeded;
            }

            using (var ts = new Transaction(doc, "export"))
            {
                ts.Start();
                doc.Export(path, "ExportNWC", nweOptions);
                ts.Commit();
            }
            TaskDialog.Show("Cообщение", "Экспорт в NWC завершен");
            return Result.Succeeded;
        }
    }
}
