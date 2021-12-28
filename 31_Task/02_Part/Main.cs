using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Part
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element, new PipeCurvesFilter(), "Выберите трубы");
            double length = 0;

            foreach (var item in references)
            {
                Element element = doc.GetElement(item);
                Pipe pipe = element as Pipe;
                double len1 = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                double len2 = UnitUtils.ConvertFromInternalUnits(len1, UnitTypeId.Millimeters);
                length += len2;
            }

            TaskDialog.Show("Длина труб", "Выбрано труб:" + references.Count.ToString() + "\n" + "Общая длина в мм: " + length.ToString("0.00"));
            return Result.Succeeded;
        }
    }
}
