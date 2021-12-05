using MigraDoc.DocumentObjectModel;
using System.Globalization;

namespace Belcukerkka.PdfGenerator
{
    public static class DocumentDecorator
    {
        /// <summary>
        /// Sets different font styles used in specified document.
        /// </summary>
        /// <param name="document">Document which needs to be styled.</param>
        public static void SetStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = DocumentDefaults.FontFamilyArial;
            style.Font.Color = Colors.Black;

            style = document.Styles["Heading1"];
            style.Font.Size = Unit.FromPoint(DocumentDefaults.FontSizeHeaderLarge);
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = DocumentDefaults.Offset;
            style.ParagraphFormat.SpaceBefore = 0;

            style = document.Styles.AddStyle("Heading1_Underlined", "Heading1");
            style.Font.Underline = Underline.Single;

            style = document.Styles["Heading2"];
            style.Font.Size = Unit.FromPoint(DocumentDefaults.FontSizeHeaderSmall);
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = DocumentDefaults.Offset;
            style.ParagraphFormat.SpaceAfter = DocumentDefaults.Offset;

            style = document.Styles.AddStyle("Heading2_Underlined", "Heading2");
            style.Font.Italic = true;
            style.Font.Underline = Underline.Single;

            style = document.Styles.AddStyle("Offset", "Normal");
            style.ParagraphFormat.SpaceAfter = DocumentDefaults.Offset / 2;
            style.ParagraphFormat.SpaceBefore = DocumentDefaults.Offset / 2;
            style.Font.Size = 0;

            style = document.Styles.AddStyle("HalfOffset", "Offset");
            style.ParagraphFormat.SpaceAfter = DocumentDefaults.Offset / 4;
            style.ParagraphFormat.SpaceBefore = DocumentDefaults.Offset / 4;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop(200);

            style = document.Styles.AddStyle("CommonText", "Normal");
            style.Font.Size = Unit.FromPoint(DocumentDefaults.FontSizeRegular);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;

            style = document.Styles.AddStyle("CommonText_Underlined", "Normal");
            style.Font.Size = Unit.FromPoint(DocumentDefaults.FontSizeRegular);
            style.Font.Underline = Underline.Single;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;

            style = document.Styles.AddStyle("ContractHeading", "Heading1");
            style.Font.Name = DocumentDefaults.FontFamilyTimesNewRoman;

            style = document.Styles.AddStyle("ContractHeading_Underlined", "ContractHeading");
            style.Font.Underline = Underline.Single;

            style = document.Styles.AddStyle("ContractText", "CommonText");
            style.Font.Name = DocumentDefaults.FontFamilyTimesNewRoman;

            style = document.Styles.AddStyle("ContractText_NoJustify", "CommonText");
            style.Font.Name = DocumentDefaults.FontFamilyTimesNewRoman;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;

            style = document.Styles.AddStyle("ContractText_Underlined", "ContractText");
            style.Font.Underline = Underline.Single;
        }

        /// <summary>
        /// Sets a whitespace as a separator for number groups.
        /// </summary>
        public static NumberFormatInfo SetNumberFormat()
        {
            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormat.NumberGroupSeparator = " ";

            return numberFormat;
        }
    }
}
