using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belcukerkka.PdfGenerator.Creators
{
    public class ContractCreator : BaseDocumentCreator
    {
        public ContractCreator(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly IWebHostEnvironment _webHostEnvironment;

        private static string TemplatesPath { get; set; }
        private static string LineBreakDivider { get; } = "{linebreak}";

        public async override Task<byte[]> CreateAsync(DocumentRequestModel model)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            TemplatesPath = Path.Combine(_webHostEnvironment.WebRootPath, DocumentDefaults.TemplatePath);

            Document contract = new Document();

            contract.Info.Title = "Договор поставки";

            DocumentDecorator.SetStyles(contract);

            Draw(contract, model);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = contract;
            renderer.RenderDocument();

            using (MemoryStream stream = new MemoryStream())
            {
                renderer.Save(stream, false);

                byte[] buffer = new byte[stream.Length];

                stream.Seek(0, SeekOrigin.Begin);
                await stream.FlushAsync();
                await stream.ReadAsync(buffer, 0, (int)stream.Length);

                return buffer;
            }
        }

        private void Draw(Document contract, DocumentRequestModel model)
        {
            Section section = contract.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.PageWidth = Unit.FromPoint(DocumentDefaults.A4Width);
            section.PageSetup.PageHeight = Unit.FromPoint(DocumentDefaults.A4Height);
            section.PageSetup.LeftMargin = DocumentDefaults.Offset * 3;
            section.PageSetup.RightMargin = DocumentDefaults.Offset * 3;
            section.PageSetup.TopMargin = DocumentDefaults.Offset * 1.5;
            section.PageSetup.BottomMargin = DocumentDefaults.Offset * 1.5;

            var paragraph = section.AddParagraph("", "ContractHeading");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddText("ДОГОВОР ПОСТАВКИ № ");
            paragraph.AddFormattedText(model.Order.InvoiceNumber.ToString(), "ContractHeading_Underlined");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("г. Минск");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText(model.DocumentDate.ToString("«dd» MMMM yyyyг."));

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            string legalEntityName = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "ООО «Белцукерка»",
                LegalEntity.OriolTrade => "ООО «Ориол Трейд»",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };
            paragraph.AddText(legalEntityName);
            paragraph.AddText(", именуемое в дальнейшем Поставщик, в лице директора ");
            string boss = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "Подолянчика А.И.",
                LegalEntity.OriolTrade => "Пускина А.И.",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };
            paragraph.AddText(boss);
            paragraph.AddText(", действующей на основании Устава, с одной стороны, и ");

            paragraph.AddFormattedText(model.ContractTerms.ContractCustomerCompany, "ContractText_Underlined");
            paragraph.AddText(", именуемое в дальнейшем Покупатель, в лице ");
            paragraph.AddFormattedText(model.ContractTerms.ContractCustomerName, "ContractText_Underlined");
            paragraph.AddText(", действуюшего(ей) на основании ");
            paragraph.AddFormattedText(model.ContractTerms.ContractBasedOn, "ContractText_Underlined");
            paragraph.AddText(", с другой стороны, а вместе именуемые Стороны, заключили настоящий договор о нижеследующем.");

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("1. ПРЕДМЕТ ДОГОВОРА");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            paragraph = section.AddParagraph("", "ContractText");

            string template = Path.Combine(TemplatesPath, "1.0. Предмет_договора.txt");
            string textToPaste = string.Empty;

            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                stream.Close();
            }

            paragraph.AddText(textToPaste);

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("1.2. Общая сумма договора составляет: ");

            var orderItems = model.Order.OrderItems;

            double VAT = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => 0.2,
                LegalEntity.OriolTrade => 0,
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            double total = 0;

            for (int i = 0; i < orderItems.Count; i++)
            {
                total += orderItems[i].Box.Price * orderItems[i].Amount * (1 + VAT);
            }

            paragraph.AddFormattedText(total.ToString("0.00"), "ContractText_Underlined");
            paragraph.AddText(" белорусских рублей.");

            paragraph = section.AddParagraph("", "ContractText");

            template = Path.Combine(TemplatesPath, "1.3. Цены.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                stream.Close();
            }

            paragraph.AddText(textToPaste);

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("2. КАЧЕСТВО, УПАКОВКА, ПРИЕМКА");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            template = Path.Combine(TemplatesPath, "2. Качество_упаковка_приемка.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                var arrayToPaste = textToPaste.Split(LineBreakDivider);

                for (int i = 0; i < arrayToPaste.Length; i++)
                {
                    arrayToPaste[i] = arrayToPaste[i].Replace("\r\n", "");

                    paragraph = section.AddParagraph("", "ContractText");
                    paragraph.AddText(arrayToPaste[i]);
                }

                stream.Close();
            }

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("3. СРОКИ И УСЛОВИЯ ПОСТАВКИ");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            template = Path.Combine(TemplatesPath, "3.0. Сроки_и_условия_поставки.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                var arrayToPaste = textToPaste.Split(LineBreakDivider);

                for (int i = 0; i < arrayToPaste.Length; i++)
                {
                    arrayToPaste[i] = arrayToPaste[i].Replace("\r\n", "");

                    paragraph = section.AddParagraph("", "ContractText");
                    paragraph.AddText(arrayToPaste[i]);
                }

                stream.Close();
            }

            paragraph.AddFormattedText(model.ContractTerms.ContractSupplyDate, "ContractText_Underlined");

            paragraph = section.AddParagraph("", "ContractText");

            template = Path.Combine(TemplatesPath, "3.6. Право_собственности.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                stream.Close();
            }

            paragraph.AddText(textToPaste);

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("4. ПОРЯДОК РАСЧЕТОВ");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            template = Path.Combine(TemplatesPath, "4. Порядок_расчетов.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                var arrayToPaste = textToPaste.Split(LineBreakDivider);

                for (int i = 0; i < arrayToPaste.Length; i++)
                {
                    arrayToPaste[i] = arrayToPaste[i].Replace("\r\n", "");

                    paragraph = section.AddParagraph("", "ContractText");
                    paragraph.AddText(arrayToPaste[i]);
                }

                stream.Close();
            }

            paragraph = section.AddParagraph("", "ContractText_NoJustify");
            paragraph.AddText(model.ContractTerms.ContractPaymentTerms);

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("5. ОТВЕТСТВЕННОСТЬ СТОРОН");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            template = Path.Combine(TemplatesPath, "5. Ответственность_сторон.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                var arrayToPaste = textToPaste.Split(LineBreakDivider);

                for (int i = 0; i < arrayToPaste.Length; i++)
                {
                    arrayToPaste[i] = arrayToPaste[i].Replace("\r\n", "");

                    paragraph = section.AddParagraph("", "ContractText");
                    paragraph.AddText(arrayToPaste[i]);
                }

                stream.Close();
            }

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("6. СРОК ДЕЙСТВИЯ, ПОРЯДОК ИЗМЕНЕНИЯ И РАСТОРЖЕНИЯ ДОГОВОРА");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            template = Path.Combine(TemplatesPath, "6. Срок_действия.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                var arrayToPaste = textToPaste.Split(LineBreakDivider);

                for (int i = 0; i < arrayToPaste.Length; i++)
                {
                    arrayToPaste[i] = arrayToPaste[i].Replace("\r\n", "");

                    paragraph = section.AddParagraph("", "ContractText");
                    paragraph.AddText(arrayToPaste[i]);
                }

                stream.Close();
            }

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("7. АНТИКОРРУПЦИОННАЯ ОГОВОРКА");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            template = Path.Combine(TemplatesPath, "7. Антикоррупционная_оговорка.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                var arrayToPaste = textToPaste.Split(LineBreakDivider);

                for (int i = 0; i < arrayToPaste.Length; i++)
                {
                    arrayToPaste[i] = arrayToPaste[i].Replace("\r\n", "");

                    paragraph = section.AddParagraph("", "ContractText");
                    paragraph.AddText(arrayToPaste[i]);
                }

                stream.Close();
            }

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("", "ContractText");
            paragraph.AddText("8. УРЕГУЛИРОВАНИЕ СПОРОВ И РАЗНОГЛАСИЙ");
            paragraph.Format.KeepWithNext = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            paragraph = section.AddParagraph("", "ContractText");

            template = Path.Combine(TemplatesPath, "8. Урегулирование_споров_и_разногласий.txt");
            using (StreamReader stream = new StreamReader(template, Encoding.UTF8))
            {
                textToPaste = stream.ReadToEnd();
                stream.Close();
            }

            paragraph.AddText(textToPaste);

            section.AddParagraph("", "HalfOffset");

            paragraph = section.AddParagraph("ЮРИДИЧЕСКИЕ АДРЕСА И БАНКОВСКИЕ РЕКВИЗИТЫ СТОРОН", "ContractText");
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            section.AddParagraph("", "HalfOffset");

            Table table = section.AddTable();
            table.Borders.Visible = true;
            table.Format.Font.Size = DocumentDefaults.FontSizeRegular;
            table.Format.Font.Name = DocumentDefaults.FontFamilyTimesNewRoman;

            table.AddColumn(Unit.FromPoint(470));
            table.AddColumn(Unit.FromPoint(470));

            Row row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;
            row.TopPadding = Unit.FromPoint(5);
            row.BottomPadding = Unit.FromPoint(5);

            Cell cell = row.Cells[0];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph().AddFormattedText("ПОСТАВЩИК", "ContractText");

            cell = row.Cells[1];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph().AddFormattedText("ПОКУПАТЕЛЬ", "ContractText");

            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Top;
            row.Borders.Bottom.Visible = false;

            cell = row.Cells[0];
            cell.Format.Alignment = ParagraphAlignment.Left;
            cell.Format.SpaceBefore = Unit.FromPoint(5);
            cell.Format.LeftIndent = Unit.FromPoint(5);

            string companyInfo = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => DocumentDefaults.LegalEntityBelcukerkka,
                LegalEntity.OriolTrade => DocumentDefaults.LegalEntityOriolTrade,
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            cell.AddParagraph(companyInfo);

            row = table.AddRow();
            row.Borders.Top.Visible = false;
            row.Borders.Bottom.Visible = false;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = Unit.FromPoint(150);

            if (model.IsSignatureNeeded)
            {
                cell = row.Cells[0];
                cell.Format.LeftIndent = Unit.FromPoint(10);

                var signature = model.LegalEntity switch
                {
                    LegalEntity.Belcukerka => "signature_Belcukerka.png",
                    LegalEntity.OriolTrade => "signature_OriolTrade.png",
                    _ => throw new ArgumentException("Unrecognized Enum value!")
                };

                var signatureImage = cell.AddImage(Path.Combine(_webHostEnvironment.WebRootPath, DocumentDefaults.ImagePath, signature));
                signatureImage.Height = Unit.FromPoint(280);
                signatureImage.Width = signatureImage.Height * 1.1;
            }

            row = table.AddRow();
            row.Height = Unit.FromPoint(100);
            row.HeightRule = RowHeightRule.Exactly;
            row.Format.Alignment = ParagraphAlignment.Right;
            row.Format.RightIndent = Unit.FromPoint(20);
            row.VerticalAlignment = VerticalAlignment.Top;
            row.TopPadding = Unit.FromPoint(5);

            cell = row.Cells[0];
            cell.AddParagraph().AddFormattedText("____________________ от Поставщика", "ContractText");
            cell.Borders.Top.Visible = false;
            cell.Borders.Top.Width = 2;

            cell = row.Cells[1];
            cell.AddParagraph().AddFormattedText("____________________ от Покупателя", "ContractText");
            cell.Borders.Top.Visible = false;
            cell.Borders.Top.Width = 2;
        }
    }
}
