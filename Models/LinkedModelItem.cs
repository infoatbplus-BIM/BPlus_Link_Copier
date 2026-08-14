using System;
using Autodesk.Revit.DB;

namespace BplusLinkCopier.Models
{
    public class LinkedModelItem
    {
        public string Name { get; set; } = string.Empty;
        public string DocumentPath { get; set; } = string.Empty;
        public RevitLinkInstance LinkInstance { get; set; }
        public Document LinkedDocument { get; set; }
        public Transform TotalTransform { get; set; }
        public int TotalElementCount { get; set; }

        public override string ToString()
        {
            return $"{Name} ({TotalElementCount} Elements)";
        }
    }
}
