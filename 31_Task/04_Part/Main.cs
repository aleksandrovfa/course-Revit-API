using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Part
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            List<Pipe> pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();


            using (Transaction ts = new Transaction(doc, "Add parameter"))
            {
                ts.Start();
                foreach (var pipe in pipes)
                {
                    double lenOut1 = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER).AsDouble();
                    double lenOut2 = UnitUtils.ConvertFromInternalUnits(lenOut1, UnitTypeId.Millimeters);

                    double lenIn1 = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM).AsDouble();
                    double lenIn2 = UnitUtils.ConvertFromInternalUnits(lenIn1, UnitTypeId.Millimeters);
                    Parameter param = pipe.LookupParameter("Наменование");
                    param.Set(String.Format("Труба {0} / {1}",lenIn2,lenOut2));
                }
                ts.Commit();
            }
            TaskDialog.Show("Cообщение", "Тест");
            return Result.Succeeded;
        }
    }
}