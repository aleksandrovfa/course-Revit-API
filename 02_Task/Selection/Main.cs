using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selection
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference selectedElementRef = uidoc.Selection.PickObject(ObjectType.Face, "Выберите элемент по грани");
            Element element = doc.GetElement(selectedElementRef);



            TaskDialog.Show("Selection", $"Имя: {element.Name}{Environment.NewLine} Id: {element.Id}");
            return Result.Succeeded;
        }
    }
}
