#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   Erlimar Silva Campos
//
// Copyright (c) 2001-2017 empira Software GmbH, Cologne Area (Germany)
// Copyright (c) 2020 Erlimar Silva Campos (Brazil)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.IO;
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering.Resources;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace MigraDoc.Rendering
{
    /// <summary>
    /// Renders html forms.
    /// </summary>
    internal class HtmlFormRenderer : ShapeRenderer
    {
        internal HtmlFormRenderer(XGraphics gfx, HtmlForm form, FieldInfos fieldInfos)
            : base(gfx, form, fieldInfos)
        {
            _form = form;
            HtmlFormRenderInfo renderInfo = new HtmlFormRenderInfo();
            renderInfo.DocumentObject = _shape;
            _renderInfo = renderInfo;
        }

        internal HtmlFormRenderer(XGraphics gfx, RenderInfo renderInfo, FieldInfos fieldInfos)
            : base(gfx, renderInfo, fieldInfos)
        {
            _form = (HtmlForm)renderInfo.DocumentObject;
        }

        internal override void Format(Area area, FormatInfo previousFormatInfo)
        {
            HtmlFormFormatInfo formatInfo = (HtmlFormFormatInfo)_renderInfo.FormatInfo;
            formatInfo.Width = XUnit.FromMillimeter(_form.Width.Millimeter);
            formatInfo.Height = XUnit.FromMillimeter(_form.Height.Millimeter);

            base.Format(area, previousFormatInfo);
        }

        protected override XUnit ShapeHeight
        {
            get
            {
                HtmlFormFormatInfo formatInfo = (HtmlFormFormatInfo)_renderInfo.FormatInfo;
                return formatInfo.Height + _lineFormatRenderer.GetWidth();
            }
        }

        protected override XUnit ShapeWidth
        {
            get
            {
                HtmlFormFormatInfo formatInfo = (HtmlFormFormatInfo)_renderInfo.FormatInfo;
                return formatInfo.Width + _lineFormatRenderer.GetWidth();
            }
        }

        internal override void Render()
        {
            RenderFilling();

            HtmlFormFormatInfo formatInfo = (HtmlFormFormatInfo)_renderInfo.FormatInfo;
            Area contentArea = _renderInfo.LayoutInfo.ContentArea;
            XRect destRect = new XRect(contentArea.X, contentArea.Y, formatInfo.Width, formatInfo.Height);

            if (formatInfo.Failure == HtmlFormFailure.None)
            {
                XImage xImage = null;
                try
                {
                    // TODO: Falhar com _form.Content vazio
                    // TODO: Add CropX, CropY, etc...

                    XRect srcRect = new XRect(0, 0, formatInfo.Width, formatInfo.Height);
                    var config = new PdfGenerateConfig
                    {
                        ManualPageSize = new XSize(formatInfo.Width, formatInfo.Height),
                        MarginBottom = 0,
                        MarginTop = 0,
                        MarginLeft = 0,
                        MarginRight = 0
                    };

                    using (var htmlPdfDoc = PdfGenerator.GeneratePdf(_form.Content, config))
                    {
                        using (var pdfStream = new MemoryStream())
                        {
                            htmlPdfDoc.Save(pdfStream);
                            xImage = XPdfForm.FromStream(pdfStream);

                            _gfx.DrawImage(xImage, destRect, srcRect, XGraphicsUnit.Point);
                        }
                    }
                }
                catch (Exception)
                {
                    RenderFailureImage(destRect);
                }
                finally
                {
                    if (xImage != null)
                        xImage.Dispose();
                }
            }
            else
                RenderFailureImage(destRect);

            RenderLine();
        }

        void RenderFailureImage(XRect destRect)
        {
            _gfx.DrawRectangle(XBrushes.LightGray, destRect);
            string failureString;
            HtmlFormFormatInfo formatInfo = (HtmlFormFormatInfo)RenderInfo.FormatInfo;

            switch (formatInfo.Failure)
            {
                case HtmlFormFailure.RenderError:
                default:
                    failureString = Messages2.DisplayHtmlFormRenderError;
                    break;
            }

            // Create stub font
            XFont font = new XFont("Courier New", 8);
            _gfx.DrawString(failureString, font, XBrushes.Red, destRect, XStringFormats.Center);
        }

        readonly HtmlForm _form;
    }
}
