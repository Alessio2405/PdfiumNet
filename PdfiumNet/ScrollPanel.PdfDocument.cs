using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using PdfiumViewer.Core;
using PdfiumViewer.Drawing;
using PdfiumViewer.Enums;

namespace PdfiumViewer
{
	// ScrollPanel.PdfDocument
	public partial class ScrollPanel
	{
		public void Render(int page, Graphics graphics, float dpiX, float dpiY, Rectangle bounds, PdfRenderFlags flags)
		{
			Document.Render(page, graphics, dpiX, dpiY, bounds, flags);
		}

		public System.Drawing.Image Render(int page, float dpiX, float dpiY, PdfRenderFlags flags)
		{
			return Document.Render(page, dpiX, dpiY, flags);
		}

		public System.Drawing.Image Render(int page, int width, int height, float dpiX, float dpiY, PdfRenderFlags flags)
		{
			return Document.Render(page, width, height, dpiX, dpiY, flags);
		}

		public System.Drawing.Image Render(int page, int width, int height, float dpiX, float dpiY, PdfRotation rotate, PdfRenderFlags flags)
		{
			return Document.Render(page, width, height, dpiX, dpiY, rotate, flags);
		}

		public void Save(string path)
		{
			Document.Save(path);
		}

		public void Save(Stream stream)
		{
			Document.Save(stream);
		}

		public PdfMatches Search(string text, bool matchCase, bool wholeWord)
		{
			return Document?.Search(text, matchCase, wholeWord);
		}

		public PdfMatches Search(string text, bool matchCase, bool wholeWord, int page)
		{
			return Document?.Search(text, matchCase, wholeWord, page);
		}

		public PdfMatches Search(string text, bool matchCase, bool wholeWord, int startPage, int endPage)
		{
			return Document?.Search(text, matchCase, wholeWord, startPage, endPage);
		}

		public PrintDocument CreatePrintDocument()
		{
			return Document.CreatePrintDocument();
		}

		public PrintDocument CreatePrintDocument(PdfPrintMode printMode)
		{
			return Document.CreatePrintDocument(printMode);
		}

		public PrintDocument CreatePrintDocument(PdfPrintSettings settings)
		{
			return Document.CreatePrintDocument(settings);
		}

		public PdfPageLinks GetPageLinks(int page, Size size)
		{
			return Document.GetPageLinks(page, size);
		}

		public void DeletePage(int page)
		{
			Document.DeletePage(page);
		}

		public void RotatePage(int page, PdfRotation rotate)
		{
			Rotate = rotate;
			OnPagesDisplayModeChanged();
		}

		public PdfInformation GetInformation()
		{
			return Document?.GetInformation();
		}

		public string GetPdfText(int page)
		{
			return Document?.GetPdfText(page);
		}

		public string GetPdfText(PdfTextSpan textSpan)
		{
			return Document?.GetPdfText(textSpan);
		}

		public IList<PdfRectangle> GetTextBounds(PdfTextSpan textSpan)
		{
			return Document?.GetTextBounds(textSpan);
		}

		public PointF PointToPdf(int page, Point point)
		{
			return Document.PointToPdf(page, point);
		}

		public Point PointFromPdf(int page, PointF point)
		{
			return Document.PointFromPdf(page, point);
		}

		public RectangleF RectangleToPdf(int page, Rectangle rect)
		{
			return Document.RectangleToPdf(page, rect);
		}

		public Rectangle RectangleFromPdf(int page, RectangleF rect)
		{
			return Document.RectangleFromPdf(page, rect);
		}

		bool isFirstLoad = true;
		Grid gridAncestor = null;

		public void GotoPage(int page, bool forceRender = false, bool isCurrentlyVisible = true)
		{
			if (IsDocumentLoaded)
			{
				PageNo = page;
				PageNoLast = page;

				// ContinuousMode will be rendered in OnScrollChanged
				if (PagesDisplayMode != PdfViewerPagesDisplayMode.ContinuousMode || forceRender)
				{
					CurrentPageSize = CalculatePageSize(page);
					RenderPage(Frame1, page, CurrentPageSize.Width, CurrentPageSize.Height);
					Frame1.AddAdorner();

					if (PagesDisplayMode == PdfViewerPagesDisplayMode.BookMode && page + 1 < Document.PageCount)
					{
						var nextPageSize = CalculatePageSize(page + 1);
						RenderPage(Frame2, page + 1, nextPageSize.Width, nextPageSize.Height);
						Frame2.AddAdorner();
						PageNoLast = page + 1;
					}

					AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
					layer?.Update();
				}

				//this.UpdateScrollViewerContent(48, this.ActualHeight);
				if (!isCurrentlyVisible)
				{
					ScrollToPage(PageNo, forceRender);
					//this.UpdateScrollViewerContent(40, this.ActualHeight);
					this.UpdateScrollViewerContent(0, this.ActualHeight);
				}
				this.UpdateScrollViewerContent(0, this.ActualHeight);
			}
		}
		public void ScrollToSpecificPage(int page, bool forceRender = false)
		{
			if (IsDocumentLoaded)
			{
				PageNo = page;
				PageNoLast = page;

				// ContinuousMode will be rendered in OnScrollChanged
				if (PagesDisplayMode != PdfViewerPagesDisplayMode.ContinuousMode || forceRender)
				{
					CurrentPageSize = CalculatePageSize(page);
					RenderPage(Frame1, page, CurrentPageSize.Width, CurrentPageSize.Height);
					Frame1.AddAdorner();

					if (PagesDisplayMode == PdfViewerPagesDisplayMode.BookMode && page < Document.PageCount)
					{
						var nextPageSize = CalculatePageSize(page);
						RenderPage(Frame2, page, nextPageSize.Width, nextPageSize.Height);
						Frame2.AddAdorner();
						PageNoLast = page;
					}

					AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
					layer?.Update();
				}

				ScrollToPage(PageNo, forceRender);
			}
		}

		
		public static T FindAncestorByName<T>(System.Windows.DependencyObject child, string name)
			where T : System.Windows.FrameworkElement
		{
			System.Windows.DependencyObject current = child;
			while (current != null)
			{
				if (current is T fe && fe.Name == name)
					return fe;

				current = System.Windows.Media.VisualTreeHelper.GetParent(current);
			}
			return null;
		}
		public void NextPage()
		{
			if (IsDocumentLoaded)
			{
				this.isPageNoSet = true;
				var extentVal = PagesDisplayMode == PdfViewerPagesDisplayMode.BookMode ? 2 : 1;
				var newPage = PageNo + extentVal;
				var maxPage = PagesDisplayMode == PdfViewerPagesDisplayMode.BookMode ?
					PageCount - 1 : PageCount - 1;

				ScrollToSpecificPage(Math.Min(newPage, maxPage));
				this.isPageNoSet = false;
			}
		}

		public void PreviousPage()
		{
			if (IsDocumentLoaded)
			{
				this.isPageNoSet = true;
				var extentVal = PagesDisplayMode == PdfViewerPagesDisplayMode.BookMode ? 2 : 1;
				int newPage = 0;

				newPage = PageNo - extentVal;

				newPage = Math.Max(newPage, 0);

				ScrollToSpecificPage(newPage, true);

				this.isPageNoSet = false;
			}
		}
	}
}
