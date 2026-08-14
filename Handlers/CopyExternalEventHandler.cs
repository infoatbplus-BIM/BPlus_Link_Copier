using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BplusLinkCopier.Services;

namespace BplusLinkCopier.Handlers
{
    public class CopyExternalEventHandler : IExternalEventHandler
    {
        public Document HostDoc { get; set; }
        public Document LinkedDoc { get; set; }
        public Transform Transform { get; set; }
        public List<ElementId> ElementIdsToCopy { get; set; }
        public Action<int, Exception> OnCompleted { get; set; }

        public void Execute(UIApplication app)
        {
            try
            {
                if (HostDoc == null || LinkedDoc == null || ElementIdsToCopy == null || ElementIdsToCopy.Count == 0)
                {
                    OnCompleted?.Invoke(0, new InvalidOperationException("Invalid document or element selection."));
                    return;
                }

                int copiedCount = ElementCopyService.CopyElementsToHost(HostDoc, LinkedDoc, Transform, ElementIdsToCopy);
                OnCompleted?.Invoke(copiedCount, null);
            }
            catch (Exception ex)
            {
                OnCompleted?.Invoke(0, ex);
            }
        }

        public string GetName()
        {
            return "Bplus Link Copier External Event Handler";
        }
    }
}
