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

namespace ExportIFC
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            IFCExportOptions IFCExportOptions = new IFCExportOptions();
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

            using (var ts = new Transaction(doc, "export dwg"))
            {
                ts.Start();
                doc.Export(path, "ExportIFC", IFCExportOptions);
                ts.Commit();
            }
            TaskDialog.Show("Cообщение", "Экспорт в IFC завершен");
            return Result.Succeeded;
        }
    }
}
