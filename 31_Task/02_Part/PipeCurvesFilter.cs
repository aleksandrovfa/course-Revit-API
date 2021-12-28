using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI.Selection;

namespace _02_Part
{
    internal class PipeCurvesFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is Pipe;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}