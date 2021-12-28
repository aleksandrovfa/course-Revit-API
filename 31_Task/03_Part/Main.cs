using Autodesk.Revit.ApplicationServices;
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

namespace _03_Part
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы");


            using (Transaction ts = new Transaction(doc, "Add parameter"))
            {
                ts.Start();
                foreach (var item in references)
                {
                    Element elem = doc.GetElement(item);
                    Pipe pipe = elem as Pipe;
                    double len1 = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                    Parameter param = pipe.LookupParameter("Длина с запасом");
                    param.Set(len1*1.1);
                }
                ts.Commit();
            }
            TaskDialog.Show("Cообщение", "Запись завершена");
            return Result.Succeeded;
        }

    }
}
