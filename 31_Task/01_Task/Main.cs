using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Task
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> reference = uidoc.Selection.PickObjects(ObjectType.Face,"Выберите элементы по грани");

            // Список для уникальных Id стен.
            // Нужен для обхода дублирование стен
            List<ElementId> walls = new List<ElementId>();

            double volume = 0;

            foreach (var item in reference)
            {
                Element element = doc.GetElement(item);
                if (element is Wall)
                {
                    Wall w = element as Wall;

                    //Если объем стены подсчитался то повторно этот объем уже не считается
                    if (!walls.Contains(w.Id))
                    {
                        walls.Add(w.Id);
                        double vol1 = w.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                        double vol2 = UnitUtils.ConvertFromInternalUnits(vol1, UnitTypeId.CubicMeters);

                        volume += vol2;

                    }
                }
            }
            TaskDialog.Show("Объем стен", "Выбрано стен:" + walls.Count.ToString() + "\n" + "Общим объемом:" + volume.ToString());
            return Result.Succeeded;

        }
    }
}
