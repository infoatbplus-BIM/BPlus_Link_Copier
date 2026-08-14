using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

namespace BplusLinkCopier.Models
{
    public enum ItemType
    {
        Category,
        Family,
        FamilySymbol,
        IndividualElement
    }

    public class ElementTreeItem : INotifyPropertyChanged
    {
        private bool? _isChecked = false;
        private bool _isExpanded = false;

        public string Name { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public ElementId ElementId { get; set; } = ElementId.InvalidElementId;
        public Element RevitElement { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;

        public ElementTreeItem Parent { get; set; }
        public ObservableCollection<ElementTreeItem> Children { get; set; } = new ObservableCollection<ElementTreeItem>();

        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                SetProperty(ref _isChecked, value);
                OnCheckedChanged();
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private void OnCheckedChanged()
        {
            if (Children != null && _isChecked.HasValue)
            {
                foreach (var child in Children)
                {
                    child.SetIsCheckedDirect(_isChecked.Value);
                }
            }

            Parent?.VerifyCheckState();
        }

        public void SetIsCheckedDirect(bool value)
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child.SetIsCheckedDirect(value);
                }
            }
        }

        public void VerifyCheckState()
        {
            if (Children == null || Children.Count == 0) return;

            bool hasChecked = false;
            bool hasUnchecked = false;

            foreach (var child in Children)
            {
                if (child.IsChecked == true) hasChecked = true;
                else if (child.IsChecked == false) hasUnchecked = true;
                else if (child.IsChecked == null) { hasChecked = true; hasUnchecked = true; }
            }

            if (hasChecked && hasUnchecked)
                _isChecked = null;
            else if (hasChecked)
                _isChecked = true;
            else
                _isChecked = false;

            OnPropertyChanged(nameof(IsChecked));
            Parent?.VerifyCheckState();
        }

        public void CollectSelectedElements(List<ElementId> ids)
        {
            if (Type == ItemType.IndividualElement && IsChecked == true && ElementId != ElementId.InvalidElementId)
            {
                if (!ids.Contains(ElementId))
                {
                    ids.Add(ElementId);
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.CollectSelectedElements(ids);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
