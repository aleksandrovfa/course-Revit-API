using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
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

            List<Duct> familyInstances = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_DuctCurves)
                .WhereElementIsNotElementType()
                .Cast<Duct>()
                .ToList();

            string Text = null;
            foreach (var item in familyInstances.GroupBy(x => x.ReferenceLevel.Name).ToList())
            {
                Text += item.Key.ToString() + ": " + item.Count().ToString() + " воздуховодов" + "\n";
            }
            TaskDialog.Show("Количество воздуховодов", Text);

            //Dictionary <string, List<Duct>> dictLayers = new Dictionary<string, List<Duct>>();

            //foreach (var Element in familyInstances)
            //{
            //    Duct duct = Element as Duct;
            //    Level level = duct.ReferenceLevel;
            //    if (!dictLayers.ContainsKey(level.Name))
            //    {
            //        dictLayers.Add(level.Name, new List<Duct>());
            //    }
            //    dictLayers[level.Name].Add(duct);

            //}


            //string Text = null;
            //Text += "Всего уровней: " + dictLayers.Count.ToString()+ "\n";
            //foreach (var item in dictLayers)
            //{
            //    Text +=   item.Key.ToString() +": " + item.Value.Count().ToString() + " воздуховодов" + "\n";  
            //}

            //TaskDialog.Show("Количество воздуховодов", Text);

            return Result.Succeeded;
        }
    }
}
