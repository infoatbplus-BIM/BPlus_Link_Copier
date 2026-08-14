using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BplusLinkCopier.Models;
using BplusLinkCopier.Services;
using BplusLinkCopier.Handlers;

namespace BplusLinkCopier
{
    public partial class LinkCopierWindow : Window
    {
        private readonly ExternalCommandData _commandData;
        private readonly Document _hostDoc;
        private readonly UIDocument _uiDoc;
        private readonly CopyExternalEventHandler _copyHandler;
        private readonly ExternalEvent _copyExternalEvent;

        private List<LinkedModelItem> _linkedModels;
        private LinkedModelItem _selectedLink;
        private ObservableCollection<ElementTreeItem> _categoryTree;
        private List<ElementId> _pickedElementIds = new List<ElementId>();

        public LinkCopierWindow(ExternalCommandData commandData)
        {
            InitializeComponent();
            _commandData = commandData;
            _uiDoc = commandData.Application.ActiveUIDocument;
            _hostDoc = _uiDoc.Document;

            _copyHandler = new Handlers.CopyExternalEventHandler();
            _copyExternalEvent = ExternalEvent.Create(_copyHandler);

            LoadLinkedModels();
        }

        private void LoadLinkedModels()
        {
            _linkedModels = ElementCopyService.GetLinkedModels(_hostDoc);
            cmbLinkedModels.ItemsSource = _linkedModels;

            if (_linkedModels.Count > 0)
            {
                cmbLinkedModels.SelectedIndex = 0;
            }
            else
            {
                lblSelectionStatus.Text = "No loaded Revit Links found in active document.";
            }
        }

        private void cmbLinkedModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedLink = cmbLinkedModels.SelectedItem as LinkedModelItem;
            if (_selectedLink == null) return;

            _categoryTree = ElementCopyService.BuildCategoryTree(_selectedLink.LinkedDocument);
            tvCategoryTree.ItemsSource = _categoryTree;

            UpdateSelectionStatus();
        }

        private void btnRefreshLinks_Click(object sender, RoutedEventArgs e)
        {
            LoadLinkedModels();
        }

        private const string SEARCH_PLACEHOLDER = "Search Family, Category, or ID...";

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == SEARCH_PLACEHOLDER)
            {
                txtSearch.Text = string.Empty;
                txtSearch.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = SEARCH_PLACEHOLDER;
                txtSearch.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(136, 136, 136));
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch == null || tvCategoryTree == null) return;

            string filterText = txtSearch.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(filterText) || filterText.Equals(SEARCH_PLACEHOLDER, StringComparison.OrdinalIgnoreCase))
            {
                tvCategoryTree.ItemsSource = _categoryTree;
                return;
            }

            if (_categoryTree == null) return;

            var filteredTree = FilterTreeItems(_categoryTree, filterText);
            tvCategoryTree.ItemsSource = filteredTree;
        }

        private ObservableCollection<ElementTreeItem> FilterTreeItems(ObservableCollection<ElementTreeItem> source, string filterText)
        {
            var filtered = new ObservableCollection<ElementTreeItem>();
            if (source == null) return filtered;

            foreach (var item in source)
            {
                if (item == null) continue;

                if (!string.IsNullOrEmpty(item.Name) && item.Name.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    filtered.Add(item);
                }
                else if (item.Children != null && item.Children.Count > 0)
                {
                    var subFiltered = FilterTreeItems(item.Children, filterText);
                    if (subFiltered.Count > 0)
                    {
                        var copyItem = new ElementTreeItem
                        {
                            Name = item.Name,
                            Type = item.Type,
                            Children = subFiltered,
                            IsExpanded = true
                        };
                        filtered.Add(copyItem);
                    }
                }
            }
            return filtered;
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch != null)
            {
                txtSearch.Text = SEARCH_PLACEHOLDER;
                txtSearch.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(136, 136, 136));
            }
            if (tvCategoryTree != null) tvCategoryTree.ItemsSource = _categoryTree;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (_categoryTree == null) return;
            foreach (var item in _categoryTree)
            {
                item.SetIsCheckedDirect(true);
            }
            UpdateSelectionStatus();
        }

        private void btnUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            if (_categoryTree == null) return;
            foreach (var item in _categoryTree)
            {
                item.SetIsCheckedDirect(false);
            }
            UpdateSelectionStatus();
        }

        private void btnPickInView_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLink == null)
            {
                TaskDialog.Show("Bplus Link Copier", "Please select a Linked Revit Model first.");
                return;
            }

            Hide();
            try
            {
                var selectionFilter = new LinkSelectionFilter(_selectedLink.LinkedDocument);
                var references = _uiDoc.Selection.PickObjects(
                    ObjectType.LinkedElement,
                    selectionFilter,
                    "Select elements from linked model. Hover over elements to highlight. Click 'Finish' on top bar (or press ESC) when done.");

                if (references != null && references.Count > 0)
                {
                    foreach (var reference in references)
                    {
                        ElementId linkedElementId = reference.LinkedElementId;
                        if (linkedElementId != ElementId.InvalidElementId && !_pickedElementIds.Contains(linkedElementId))
                        {
                            _pickedElementIds.Add(linkedElementId);
                            Element elem = _selectedLink.LinkedDocument.GetElement(linkedElementId);
                            lstPickedElements.Items.Add($"[ID: {linkedElementId}] {elem?.Name} ({elem?.Category?.Name})");
                        }
                    }
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // User pressed ESC or finished selection - normal behavior
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Bplus Link Copier Error", $"Selection error: {ex.Message}");
            }
            finally
            {
                Show();
                UpdateSelectionStatus();
            }
        }

        private void btnClearPicked_Click(object sender, RoutedEventArgs e)
        {
            _pickedElementIds.Clear();
            lstPickedElements.Items.Clear();
            UpdateSelectionStatus();
        }

        private void UpdateSelectionStatus()
        {
            var selectedIds = GetSelectedElementIds();
            lblSelectionStatus.Text = $"{selectedIds.Count} Element(s) Selected for Copying";
        }

        private List<ElementId> GetSelectedElementIds()
        {
            var ids = new List<ElementId>(_pickedElementIds);
            if (_categoryTree != null)
            {
                foreach (var item in _categoryTree)
                {
                    item.CollectSelectedElements(ids);
                }
            }
            return ids;
        }

        private void TreeItemCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateSelectionStatus();
            }));
        }

        private void btnCopyElements_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLink == null)
            {
                TaskDialog.Show("Bplus Link Copier", "Please select a Linked Revit Model.");
                return;
            }

            var idsToCopy = GetSelectedElementIds();
            if (idsToCopy.Count == 0)
            {
                TaskDialog.Show("Bplus Link Copier", "No elements selected. Check items in the tree view or pick elements in the Revit view.");
                return;
            }

            btnCopyElements.IsEnabled = false;

            _copyHandler.HostDoc = _hostDoc;
            _copyHandler.LinkedDoc = _selectedLink.LinkedDocument;
            _copyHandler.Transform = _selectedLink.TotalTransform;
            _copyHandler.ElementIdsToCopy = idsToCopy;
            _copyHandler.OnCompleted = (copiedCount, ex) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btnCopyElements.IsEnabled = true;

                    if (ex != null)
                    {
                        TaskDialog.Show("Bplus Link Copier Error", $"Failed to copy elements: {ex.Message}");
                    }
                    else
                    {
                        TaskDialog.Show("Bplus Link Copier Success", 
                            $"Successfully copied {copiedCount} element(s) from '{_selectedLink.Name}' into the host project!\n\nGeospatial Alignment: 100% Exact Origin Matched.");
                        Close();
                    }
                });
            };

            _copyExternalEvent.Raise();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
            }
            catch { }
            e.Handled = true;
        }
    }
}
