using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace BplusLinkCopier.Services
{
    public class LinkSelectionFilter : ISelectionFilter
    {
        private readonly Document _linkedDoc;

        public LinkSelectionFilter(Document linkedDoc)
        {
            _linkedDoc = linkedDoc;
        }

        public bool AllowElement(Element elem)
        {
            return elem is RevitLinkInstance;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            if (reference == null || _linkedDoc == null) return false;
            ElementId linkedId = reference.LinkedElementId;
            if (linkedId == ElementId.InvalidElementId) return false;

            Element linkedElem = _linkedDoc.GetElement(linkedId);
            return linkedElem != null && linkedElem.Category != null && linkedElem.Category.CategoryType == CategoryType.Model;
        }
    }
}
