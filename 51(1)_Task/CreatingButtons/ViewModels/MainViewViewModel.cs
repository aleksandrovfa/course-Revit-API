using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingButtons
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SelectCommandPipes { get; }
        public DelegateCommand SelectCommandDoors { get; }
        public DelegateCommand SelectCommandVolumeWall { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SelectCommandPipes = new DelegateCommand(OnSelectCommandPipes);
            SelectCommandDoors = new DelegateCommand(OnSelectCommandDoors);
            SelectCommandVolumeWall = new DelegateCommand(OnSelectCommandVolumeWall);
        }

        public event EventHandler HideRequest;

        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;

        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        private void OnSelectCommandPipes()
        {
            RaiseHideRequest();
            List<Element> el  = ElementCollector.GetElement(_commandData, BuiltInCategory.OST_PipeCurves);
            TaskDialog.Show("Количество труб", $"Всего труб: {el.Count}");
            RaiseShowRequest();
        }
        private void OnSelectCommandDoors()
        {
            RaiseHideRequest();
            List<Element> el = ElementCollector.GetElement(_commandData, BuiltInCategory.OST_Doors);
            TaskDialog.Show("Количество дверей", $"Всего дверей: {el.Count}");
            RaiseShowRequest();
        }
        private void OnSelectCommandVolumeWall()
        {
            RaiseHideRequest();
            double volume = VolumeWall.Get(_commandData);
            TaskDialog.Show("Объем стен", $"Общий объем стен: {volume} куб.м");
            RaiseShowRequest();
        }
    }
}
