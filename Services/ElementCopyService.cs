using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;
using BplusLinkCopier.Models;

namespace BplusLinkCopier.Services
{
    public class ElementCopyService
    {
        public static List<LinkedModelItem> GetLinkedModels(Document hostDoc)
        {
            var list = new List<LinkedModelItem>();

            var linkCollector = new FilteredElementCollector(hostDoc)
                .OfClass(typeof(RevitLinkInstance))
                .Cast<RevitLinkInstance>();

            foreach (var linkInst in linkCollector)
            {
                Document linkedDoc = linkInst.GetLinkDocument();
                if (linkedDoc == null) continue; // Unloaded link

                int count = new FilteredElementCollector(linkedDoc)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .GetElementCount();

                list.Add(new LinkedModelItem
                {
                    Name = linkInst.Name,
                    DocumentPath = linkedDoc.PathName,
                    LinkInstance = linkInst,
                    LinkedDocument = linkedDoc,
                    TotalTransform = linkInst.GetTotalTransform(),
                    TotalElementCount = count
                });
            }

            return list;
        }

        public static ObservableCollection<ElementTreeItem> BuildCategoryTree(Document linkedDoc)
        {
            var rootCategories = new ObservableCollection<ElementTreeItem>();

            var elements = new FilteredElementCollector(linkedDoc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .ToElements();

            var categoryGroups = elements
                .Where(e => e.Category != null && e.Category.CategoryType == CategoryType.Model)
                .GroupBy(e => e.Category.Name)
                .OrderBy(g => g.Key);

            foreach (var catGroup in categoryGroups)
            {
                var catItem = new ElementTreeItem
                {
                    Name = catGroup.Key,
                    Type = ItemType.Category,
                    CategoryName = catGroup.Key
                };

                var familyGroups = catGroup
                    .GroupBy(e => GetFamilyName(e))
                    .OrderBy(g => g.Key);

                foreach (var famGroup in familyGroups)
                {
                    var famItem = new ElementTreeItem
                    {
                        Name = famGroup.Key,
                        Type = ItemType.Family,
                        CategoryName = catGroup.Key,
                        FamilyName = famGroup.Key,
                        Parent = catItem
                    };

                    var symbolGroups = famGroup
                        .GroupBy(e => e.Name)
                        .OrderBy(g => g.Key);

                    foreach (var symGroup in symbolGroups)
                    {
                        var symItem = new ElementTreeItem
                        {
                            Name = symGroup.Key,
                            Type = ItemType.FamilySymbol,
                            CategoryName = catGroup.Key,
                            FamilyName = famGroup.Key,
                            Parent = famItem
                        };

                        foreach (var elem in symGroup)
                        {
                            string levelName = GetLevelName(elem);
                            var elemItem = new ElementTreeItem
                            {
                                Name = $"[ID: {elem.Id}] {symGroup.Key} {(string.IsNullOrEmpty(levelName) ? "" : " - " + levelName)}",
                                Type = ItemType.IndividualElement,
                                ElementId = elem.Id,
                                RevitElement = elem,
                                CategoryName = catGroup.Key,
                                FamilyName = famGroup.Key,
                                LevelName = levelName,
                                Parent = symItem
                            };

                            symItem.Children.Add(elemItem);
                        }

                        famItem.Children.Add(symItem);
                    }

                    catItem.Children.Add(famItem);
                }

                rootCategories.Add(catItem);
            }

            return rootCategories;
        }

        private static string GetFamilyName(Element e)
        {
            if (e is ElementType et) return et.FamilyName;
            
            ElementId typeId = e.GetTypeId();
            if (typeId != ElementId.InvalidElementId)
            {
                var typeElem = e.Document.GetElement(typeId) as ElementType;
                if (typeElem != null && !string.IsNullOrEmpty(typeElem.FamilyName))
                {
                    return typeElem.FamilyName;
                }
            }

            return e.Category?.Name ?? "General";
        }

        private static string GetLevelName(Element e)
        {
            if (e.LevelId != ElementId.InvalidElementId)
            {
                var lvl = e.Document.GetElement(e.LevelId) as Level;
                if (lvl != null) return lvl.Name;
            }

            Parameter p = e.get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                       ?? e.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM);
            if (p != null && p.HasValue) return p.AsString();

            return string.Empty;
        }

        public static int CopyElementsToHost(
            Document hostDoc, 
            Document linkedDoc, 
            Transform transform, 
            ICollection<ElementId> elementIdsToCopy)
        {
            if (elementIdsToCopy == null || elementIdsToCopy.Count == 0) return 0;

            using (Transaction trans = new Transaction(hostDoc, "Bplus Link Copy Elements"))
            {
                trans.Start();

                var options = new CopyPasteOptions();
                var copiedIds = ElementTransformUtils.CopyElements(
                    linkedDoc,
                    elementIdsToCopy,
                    hostDoc,
                    transform,
                    options);

                trans.Commit();
                return copiedIds.Count;
            }
        }
    }
}
