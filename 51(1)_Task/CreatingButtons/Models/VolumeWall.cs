using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingButtons
{
    public class VolumeWall
    {
        public static double Get(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            List<Element> elements = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .Cast<Element>()
                .ToList();
            double volume = 0;

            foreach (var element in elements)
            {
                if (element is Wall)
                {
                    Wall w = element as Wall;
                    double vol1 = w.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                    double vol2 = UnitUtils.ConvertFromInternalUnits(vol1, UnitTypeId.CubicMeters);
                    volume += vol2;
                }
            }
            return volume;
        }

    }
}
