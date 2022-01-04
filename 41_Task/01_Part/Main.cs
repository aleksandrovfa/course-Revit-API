using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Part
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string wallInfo = string.Empty;

            var walls = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .ToList();
            foreach (var wall in walls)
            {
                Wall w = doc.GetElement(wall.Id) as Wall;
                if (w != null)
                {
                    Parameter volume = w.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    double vol = UnitUtils.ConvertFromInternalUnits(volume.AsDouble(), UnitTypeId.CubicMeters);
                    wallInfo += $"{w.Name}\t{Math.Round(vol, 2).ToString()}\t{Environment.NewLine}";
                    
                }

            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string csv = Path.Combine(desktopPath, "walls.csv");

            File.WriteAllText(csv, wallInfo);

            TaskDialog.Show("Cообщение", "Данные сохранены");
            return Result.Succeeded;
        }
    }
}
