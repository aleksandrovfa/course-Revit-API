using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingCreateDuct
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<DuctType> DuctTypes { get; } = new List<DuctType>();
        public List<Level> Levels { get; }
        public DelegateCommand SaveCommand { get; }
        public double WallHeight { get; set; }
        public List<XYZ> Points { get; set; } = new List<XYZ>();

        public MEPSystemType DuctSystems;

        public DuctType SelectedDuctType { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            DuctTypes = DuctsUtils.GetDuctTypes(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            WallHeight = 1000;
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точки", ObjectSnapTypes.Endpoints, 2);
            DuctSystems = DuctsUtils.GetDuctSystems(commandData);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points.Count <2 || SelectedDuctType == null ||
                SelectedLevel == null)
                return;
            for (int i = 0; i < Points.Count; i++)
            {
                double z = SelectedLevel.Elevation + UnitUtils.ConvertToInternalUnits(WallHeight, UnitTypeId.Millimeters);
                Points[i] = new XYZ(Points[i].X, Points[i].Y, z);
            }

            using (var ts = new Transaction(doc, "Create wall"))
            {
                ts.Start();
                Duct duct = Duct.Create(doc, DuctSystems.Id, SelectedDuctType.Id, SelectedLevel.Id, Points[0], Points[1]);
                ts.Commit();
            }
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
    //public class DuctsUtils
    //{
    //    public static List<DuctType> GetDuctTypes(ExternalCommandData commandData)
    //    {
    //        UIApplication uiapp = commandData.Application;
    //        UIDocument uidoc = uiapp.ActiveUIDocument;
    //        Document doc = uidoc.Document;

    //        var wallTypes =
    //            new FilteredElementCollector(doc)
    //                .OfClass(typeof(DuctType))
    //                .Cast<DuctType>()
    //                .ToList();
    //        return wallTypes;
    //    }
    //    public static MEPSystemType GetDuctSystems(ExternalCommandData commandData)
    //    {
    //        UIApplication uiapp = commandData.Application;
    //        UIDocument uidoc = uiapp.ActiveUIDocument;
    //        Document doc = uidoc.Document;

    //        MEPSystemType mepSystemType = new FilteredElementCollector(doc)
    //                                    .OfClass(typeof(MEPSystemType))
    //                                    .Cast<MEPSystemType>()
    //                                    .FirstOrDefault(sysType => sysType.SystemClassification == MEPSystemClassification.SupplyAir);
    //        return mepSystemType;
    //    }
    //    public static List<XYZ> GetPoints(ExternalCommandData commandData,
    //                        string promptMessage, ObjectSnapTypes objectSnapTypes, int pointCount = 0)
    //    {
    //        UIApplication uiapp = commandData.Application;
    //        UIDocument uidoc = uiapp.ActiveUIDocument;

    //        List<XYZ> points = new List<XYZ>();

    //        int pointNumber = 0;
    //        while (true)
    //        {
    //            XYZ pickedPoint = null;
    //            try
    //            {
    //                pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promptMessage);
    //                points.Add(pickedPoint);
    //                pointNumber++;
    //                if (pointNumber == pointCount)
    //                {
    //                    break;
    //                }
    //            }
    //            catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
    //            {
    //                break;
    //            }

    //        }

    //        return points;
    //    }
    //}
}
