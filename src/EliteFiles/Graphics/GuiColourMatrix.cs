﻿using System.Xml.Linq;

namespace EliteFiles.Graphics
{
    /// <summary>
    /// Represents a GUI colour transformation matrix.
    /// </summary>
    public sealed class GuiColourMatrix : IRgbTransformMatrix
    {
        private readonly GuiColourMatrixEntry[] _v = new GuiColourMatrixEntry[3];

        private GuiColourMatrix(GuiColourMatrixEntry r, GuiColourMatrixEntry g, GuiColourMatrixEntry b)
        {
            _v[0] = r;
            _v[1] = g;
            _v[2] = b;
        }

        /// <summary>
        /// Gets the default GUI colour transformation matrix.
        /// </summary>
        public static GuiColourMatrix Default { get; } = new GuiColourMatrix(
            new GuiColourMatrixEntry(1, 0, 0),
            new GuiColourMatrixEntry(0, 1, 0),
            new GuiColourMatrixEntry(0, 0, 1));

        /// <summary>
        /// Gets the localisation name.
        /// </summary>
        public string? LocalisationName { get; private set; }

        /// <summary>
        /// Gets the red matrix component.
        /// </summary>
        public GuiColourMatrixEntry MatrixRed => _v[0];

        /// <summary>
        /// Gets the green matrix component.
        /// </summary>
        public GuiColourMatrixEntry MatrixGreen => _v[1];

        /// <summary>
        /// Gets the blue matrix component.
        /// </summary>
        public GuiColourMatrixEntry MatrixBlue => _v[2];

        /// <summary>
        /// Gets the matrix value at the given row and column.
        /// </summary>
        /// <param name="row">The matrix row.</param>
        /// <param name="col">The matrix column.</param>
        /// <returns>The matrix value.</returns>
        public double this[int row, int col] => _v[col][row];

        internal static GuiColourMatrix FromXml(XElement xml)
        {
            var r = GuiColourMatrixEntry.FromXml(xml.Element("MatrixRed"));
            var g = GuiColourMatrixEntry.FromXml(xml.Element("MatrixGreen"));
            var b = GuiColourMatrixEntry.FromXml(xml.Element("MatrixBlue"));

            return new GuiColourMatrix(r, g, b)
            {
                LocalisationName = xml.Element("LocalisationName")?.Value,
            };
        }
    }
}
