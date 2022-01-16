using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPITrainingLibrary
{
    public class DuctsUtils
    {
        public static List<DuctType> GetDuctTypes(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var wallTypes =
                new FilteredElementCollector(doc)
                    .OfClass(typeof(DuctType))
                    .Cast<DuctType>()
                    .ToList();
            return wallTypes;
        }
        public static MEPSystemType GetDuctSystems(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            MEPSystemType mepSystemType = new FilteredElementCollector(doc)
                                        .OfClass(typeof(MEPSystemType))
                                        .Cast<MEPSystemType>()
                                        .FirstOrDefault(sysType => sysType.SystemClassification == MEPSystemClassification.SupplyAir);
            return mepSystemType;
        }
    }
}
