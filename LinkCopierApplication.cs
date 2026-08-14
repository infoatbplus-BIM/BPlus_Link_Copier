using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace BplusLinkCopier
{
    public class LinkCopierApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                CreateRibbonUI(application);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Bplus Link Copier Startup Error", ex.ToString());
                return Result.Failed;
            }
        }

        private void CreateRibbonUI(UIControlledApplication application)
        {
            string tabName = "Bplus";
            try { application.CreateRibbonTab(tabName); } catch { }

            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Model Transfer");
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData copierButtonData = new PushButtonData(
                "cmdLinkCopier",
                "Link Copier",
                assemblyPath,
                "BplusLinkCopier.LinkCopierCommand")
            {
                ToolTip = "Inspect, select, and copy elements from linked Revit models with 100% origin accuracy."
            };

            if (panel.AddItem(copierButtonData) is PushButton copierButton)
            {
                copierButton.LargeImage = GetEmbeddedImage("BplusLinkCopier.Icon_Copier_32.png");
                copierButton.Image = GetEmbeddedImage("BplusLinkCopier.Icon_Copier_16.png");
            }
        }

        private System.Windows.Media.ImageSource GetEmbeddedImage(string resourceName)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null) return null;
                    
                    PngBitmapDecoder decoder = new PngBitmapDecoder(
                        stream, 
                        BitmapCreateOptions.PreservePixelFormat, 
                        BitmapCacheOption.OnLoad);
                    
                    return decoder.Frames[0];
                }
            }
            catch { return null; }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
