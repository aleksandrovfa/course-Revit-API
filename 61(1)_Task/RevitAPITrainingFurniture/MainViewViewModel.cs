﻿using Autodesk.Revit.DB;
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

namespace RevitAPITrainingCreateFurniture
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<FamilySymbol> FurnitureTypes { get; } = new List<FamilySymbol>();
        public FamilySymbol SelectedFurnitureType { get; set; }
        
        public List<Level> Levels { get; }
        public Level SelectedLevel { get; set; }

        public List<XYZ> Points { get; set; } = new List<XYZ>();

        public DelegateCommand SaveCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            FurnitureTypes = FamilySymbolUtils.GetFamilySymbols(_commandData).Where(x=> x.Category.Name == "Мебель").ToList();
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
        }
        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            RaiseCloseRequest();
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точку вставки", ObjectSnapTypes.Endpoints, 1);
            Points[0] = new XYZ(Points[0].X, Points[0].Y, SelectedLevel.ProjectElevation);
            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurnitureType, Points[0], SelectedLevel);
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
