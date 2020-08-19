#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   Stefan Lange
//   Klaus Potzesny
//   David Stephensen
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
using MigraDoc.DocumentObjectModel.Internals;

namespace MigraDoc.DocumentObjectModel.Shapes
{
    /// <summary>
    /// Represents an HtmlForm in the document or paragraph.
    /// </summary>
    public class HtmlForm : Shape
    {
        /// <summary>
        /// Initializes a new instance of the HtmlForm class.
        /// </summary>
        public HtmlForm()
        { }

        /// <summary>
        /// Initializes a new instance of the HtmlForm class with the specified parent.
        /// </summary>
        internal HtmlForm(DocumentObject parent) : base(parent) { }

        /// <summary>
        /// Initializes a new instance of the HtmlForm class from the specified html content.
        /// </summary>
        public HtmlForm(string content)
            : this()
        {
            Content = content;
        }

        #region Methods
        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        public new HtmlForm Clone()
        {
            return (HtmlForm)DeepCopy();
        }

        /// <summary>
        /// Implements the deep copy of the object.
        /// </summary>
        protected override object DeepCopy()
        {
            HtmlForm htmlForm = (HtmlForm)base.DeepCopy();
            return htmlForm;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the content of the form.
        /// </summary>
        public string Content
        {
            get { return _content.Value; }
            set { _content.Value = value; }
        }
        [DV]
        internal NString _content = NString.NullValue;
        #endregion

        #region Internal
        /// <summary>
        /// Returns the meta object of this instance.
        /// </summary>
        internal override Meta Meta
        {
            get { return _meta ?? (_meta = new Meta(typeof(HtmlForm))); }
        }
        static Meta _meta;
        #endregion
    }
}
