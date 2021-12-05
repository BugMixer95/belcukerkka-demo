using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using NickBuhro.NumToWords.Russian;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Belcukerkka.PdfGenerator.Creators
{
    public class InvoiceCreator : BaseDocumentCreator
    {
        public InvoiceCreator(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly IWebHostEnvironment _webHostEnvironment;

        private static readonly Dictionary<int, string> MainTableColumnsNamesBelcukerkka = new Dictionary<int, string>
        {
            {0, "№ п/п" },
            {1, "Наименование" },
            {2, "Ед. изм." },
            {3, "Кол-во" },
            {4, "Цена за единицу без НДС, руб." },
            {5, "Стоимость без НДС, руб." },
            {6, "Ставка НДС, %" },
            {7, "Сумма НДС, руб." },
            {8, "Стоимость продукции с НДС, руб." }
        };

        private static readonly Dictionary<int, string> MainTableColumnsNamesOriolTrade = new Dictionary<int, string>
        {
            {0, "№ п/п" },
            {1, "Наименование" },
            {2, "Ед. изм." },
            {3, "Кол-во" },
            {4, "Цена за единицу, руб." },
            {5, "Стоимость, руб." },
            {6, "НДС, %" },
            {7, "Всего с НДС, руб." }
        };

        public async override Task<byte[]> CreateAsync(DocumentRequestModel model)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Document invoice = new Document();

            invoice.Info.Title = "Счёт";

            DocumentDecorator.SetStyles(invoice);

            Draw(invoice, model);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = invoice;
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

        private void Draw(Document invoice, DocumentRequestModel model)
        {
            var numberFormat = DocumentDecorator.SetNumberFormat();

            Section section = invoice.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.PageWidth = Unit.FromPoint(DocumentDefaults.A4Width);
            section.PageSetup.PageHeight = Unit.FromPoint(DocumentDefaults.A4Height);
            section.PageSetup.LeftMargin = DocumentDefaults.Offset * 1.5;
            section.PageSetup.RightMargin = DocumentDefaults.Offset * 1.5;
            section.PageSetup.TopMargin = DocumentDefaults.Offset * 1.5;
            section.PageSetup.BottomMargin = DocumentDefaults.Offset * 1.5;

            // create table header
            Table table = section.AddTable();
            table.Borders.Visible = false;
            table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
            table.Format.Font.Name = DocumentDefaults.FontFamilyArial;

            table.AddColumn(Unit.FromPoint(820)); // информация об ООО
            table.AddColumn(Unit.FromPoint(270)); // лого

            Row row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Top;

            Cell cell = row.Cells[0];
            cell.Format.Alignment = ParagraphAlignment.Left;

            string companyInfo = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => DocumentDefaults.LegalEntityBelcukerkka,
                LegalEntity.OriolTrade => DocumentDefaults.LegalEntityOriolTrade,
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };
            cell.AddParagraph(companyInfo);

            cell = row.Cells[1];
            var logoParagraph = cell.AddParagraph();
            logoParagraph.Format.Alignment = ParagraphAlignment.Right;

            var legalEntityLogo = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "logo_Belcukerka.png",
                LegalEntity.OriolTrade => "logo_OriolTrade.jpg",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            var logoImage = logoParagraph.AddImage(Path.Combine(_webHostEnvironment.WebRootPath, DocumentDefaults.ImagePath, legalEntityLogo));

            if (model.LegalEntity == LegalEntity.Belcukerka)
            {
                logoImage.Width = Unit.FromPoint(200);
                logoImage.Height = Unit.FromPoint(200);
            }
            else
            {
                logoImage.Width = Unit.FromPoint(350);
                logoImage.Height = Unit.FromPoint(100);
            }

            row = table.AddRow();
            row.Borders.Bottom.Visible = true;
            row.Borders.Bottom.Width = 2;

            // draw header text
            CultureInfo ci = new CultureInfo("ru-RU");

            Paragraph heading = invoice.LastSection.AddParagraph();
            heading.Format.Alignment = ParagraphAlignment.Center;
            heading.Format.Font.Bold = true;
            heading.Style = "Heading2_Underlined";
            heading.AddFormattedText("Счёт № ", "Heading2");
            heading.AddSpace(7);
            heading.AddFormattedText(model.Order.InvoiceNumber.ToString());
            heading.AddSpace(7);
            heading.AddFormattedText(" от ", "Heading2");
            heading.AddSpace(5);
            heading.AddFormattedText(model.DocumentDate.ToString("d MMMM yyyy", ci) + "г.");
            heading.AddSpace(5);

            // draw customer info
            table = invoice.LastSection.AddTable();
            table.Borders.Visible = false;
            table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
            table.Format.Font.Name = DocumentDefaults.FontFamilyArial;
            table.TopPadding = Unit.FromPoint(10);
            table.BottomPadding = Unit.FromPoint(10);

            table.AddColumn(Unit.FromPoint(200)); // name
            table.AddColumn(Unit.FromPoint(890)); // info

            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;

            cell = row.Cells[0];
            cell.Format.Alignment = ParagraphAlignment.Right;
            cell.Format.Font.Bold = true;
            cell.AddParagraph("ПОКУПАТЕЛЬ:");

            cell = row.Cells[1];
            cell.AddParagraph(model.Order.Customer.Name);
            cell.Borders.Bottom.Visible = true;

            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Top;

            cell = row.Cells[0];
            cell.Format.Alignment = ParagraphAlignment.Right;
            cell.Format.Font.Bold = true;
            cell.AddParagraph("РЕКВИЗИТЫ:");

            cell = row.Cells[1];
            cell.AddParagraph(model.CustomerInfo);
            cell.Borders.Bottom.Visible = true;

            // draw whitespace
            invoice.LastSection.AddParagraph("", "Offset");

            // draw main table
            table = invoice.LastSection.AddTable();
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Borders.Visible = true;
            table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
            table.Format.Font.Name = DocumentDefaults.FontFamilyArial;
            table.TopPadding = Unit.FromPoint(10);
            table.BottomPadding = Unit.FromPoint(10);

            if (model.LegalEntity == LegalEntity.Belcukerka)
            {
                table.AddColumn(40); // number of position
                table.AddColumn(280); // name
                table.AddColumn(55); // unit
                table.AddColumn(55); // count
                table.AddColumn(140); // price without VAT for a unit
                table.AddColumn(130); // total price without VAT
                table.AddColumn(120); // VAT rate
                table.AddColumn(120); // VAT amount
                table.AddColumn(150); // total price with VAT
            }
            else
            {
                table.AddColumn(40); // number of position
                table.AddColumn(280); // name
                table.AddColumn(55); // unit
                table.AddColumn(55); // count
                table.AddColumn(200); // price without VAT for a unit
                table.AddColumn(190); // total price without VAT
                table.AddColumn(120); // VAT rate
                table.AddColumn(150); // total price with VAT
            }

            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Format.Font.Bold = true;
            row.TopPadding = Unit.FromPoint(5);
            row.BottomPadding = Unit.FromPoint(5);

            if (model.LegalEntity == LegalEntity.Belcukerka)
            {
                foreach (var item in MainTableColumnsNamesBelcukerkka)
                {
                    cell = row.Cells[item.Key];
                    cell.AddParagraph(item.Value.ToString());
                }
            }
            else
            {
                foreach (var item in MainTableColumnsNamesOriolTrade)
                {
                    cell = row.Cells[item.Key];
                    cell.AddParagraph(item.Value.ToString());
                }
            }

            int totalQuantity = 0;
            double totalAmountNoVAT = 0;
            double totalVAT = 0;

            double totalAmount = 0;

            var orderItems = model.Order.OrderItems;

            double VAT = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => 0.2,
                LegalEntity.OriolTrade => 0,
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };
            int VATRate = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => 20,
                LegalEntity.OriolTrade => 0,
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            if (model.LegalEntity == LegalEntity.Belcukerka)
            {
                for (int i = 0; i < orderItems.Count; i++)
                {
                    totalQuantity += orderItems[i].Amount;
                    totalAmountNoVAT += (orderItems[i].Box.Price * orderItems[i].Amount);
                    totalVAT += (orderItems[i].Box.Price * orderItems[i].Amount) * VAT;

                    row = table.AddRow();
                    row.VerticalAlignment = VerticalAlignment.Center;

                    cell = row.Cells[0];
                    cell.AddParagraph((i + 1).ToString());

                    cell = row.Cells[1];
                    cell.AddParagraph($"{orderItems[i].Box.BoxParent.Name}, " +
                        $"вариант \"{orderItems[i].Box.Composition.WeightType.Name}-{orderItems[i].Box.Composition.Weight}\"");
                    cell.Format.Alignment = ParagraphAlignment.Left;

                    cell = row.Cells[2];
                    cell.AddParagraph("шт.");

                    cell = row.Cells[3];
                    cell.AddParagraph(orderItems[i].Amount.ToString());

                    cell = row.Cells[4];
                    cell.AddParagraph(orderItems[i].Box.Price.ToString("#,0.00", numberFormat));

                    cell = row.Cells[5];
                    cell.AddParagraph((orderItems[i].Box.Price * orderItems[i].Amount).ToString("#,0.00", numberFormat));

                    cell = row.Cells[6];
                    cell.AddParagraph(VATRate.ToString("#,0.00", numberFormat));

                    cell = row.Cells[7];
                    cell.AddParagraph((orderItems[i].Box.Price * orderItems[i].Amount * VAT).ToString("#,0.00", numberFormat));

                    cell = row.Cells[8];
                    cell.AddParagraph((orderItems[i].Box.Price * orderItems[i].Amount * (1 + VAT)).ToString("#,0.00", numberFormat));
                }

                var totalAmountWithVAT = totalAmountNoVAT + totalVAT;

                row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Format.Font.Bold = true;

                cell = row.Cells[0];
                cell.MergeRight = 1;
                cell.AddParagraph("ИТОГО:");

                cell = row.Cells[2];
                cell.AddParagraph("x");

                cell = row.Cells[3];
                cell.AddParagraph(totalQuantity.ToString());

                cell = row.Cells[4];
                cell.AddParagraph("x");

                cell = row.Cells[5];
                cell.AddParagraph(totalAmountNoVAT.ToString("#,0.00", numberFormat));

                cell = row.Cells[7];
                cell.AddParagraph(totalVAT.ToString("#,0.00", numberFormat));

                cell = row.Cells[8];
                cell.AddParagraph(totalAmountWithVAT.ToString("#,0.00", numberFormat));

                totalAmount = totalAmountWithVAT;
            }
            else
            {
                for (int i = 0; i < orderItems.Count; i++)
                {
                    totalQuantity += orderItems[i].Amount;
                    totalAmountNoVAT += (orderItems[i].Box.Price * orderItems[i].Amount);
                    totalVAT += (orderItems[i].Box.Price * orderItems[i].Amount) * VAT;

                    row = table.AddRow();
                    row.VerticalAlignment = VerticalAlignment.Center;

                    cell = row.Cells[0];
                    cell.AddParagraph((i + 1).ToString());

                    cell = row.Cells[1];
                    cell.AddParagraph($"{orderItems[i].Box.BoxParent.Name}, " +
                        $"вариант \"{orderItems[i].Box.Composition.WeightType.Name}-{orderItems[i].Box.Composition.Weight}\"");
                    cell.Format.Alignment = ParagraphAlignment.Left;

                    cell = row.Cells[2];
                    cell.AddParagraph("шт.");

                    cell = row.Cells[3];
                    cell.AddParagraph(orderItems[i].Amount.ToString());

                    cell = row.Cells[4];
                    cell.AddParagraph(orderItems[i].Box.Price.ToString("#,0.00", numberFormat));

                    cell = row.Cells[5];
                    cell.AddParagraph((orderItems[i].Box.Price * orderItems[i].Amount).ToString("#,0.00", numberFormat));

                    cell = row.Cells[6];
                    cell.AddParagraph("БЕЗ НДС");

                    cell = row.Cells[7];
                    cell.AddParagraph((orderItems[i].Box.Price * orderItems[i].Amount * (1 + VAT)).ToString("#,0.00", numberFormat));
                }

                var totalAmountWithVAT = totalAmountNoVAT + totalVAT;

                row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Format.Font.Bold = true;

                cell = row.Cells[0];
                cell.MergeRight = 1;
                cell.AddParagraph("ИТОГО:");

                cell = row.Cells[5];
                cell.AddParagraph(totalAmountNoVAT.ToString("#,0.00", numberFormat));

                cell = row.Cells[7];
                cell.AddParagraph(totalAmountWithVAT.ToString("#,0.00", numberFormat));

                totalAmount = totalAmountWithVAT;
            }

            // draw whitespace
            invoice.LastSection.AddParagraph("", "HalfOffset");

            // draw numbers to words
            table = invoice.LastSection.AddTable();
            table.Borders.Visible = false;
            table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
            table.Format.Font.Name = DocumentDefaults.FontFamilyArial;
            table.TopPadding = Unit.FromPoint(10);
            table.BottomPadding = Unit.FromPoint(10);

            table.AddColumn(320); // name
            table.AddColumn(770); // number to words

            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;

            cell = row.Cells[0];
            cell.AddParagraph("Всего к оплате с НДС: ");
            cell.Format.Font.Bold = true;
            cell.Format.Alignment = ParagraphAlignment.Right;

            cell = row.Cells[1];
            var numToWords = RussianConverter.FormatCurrency((decimal)totalAmount, UnitOfMeasure.Ruble);
            cell.AddParagraph(numToWords);
            cell.Format.Alignment = ParagraphAlignment.Left;

            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;

            if (model.LegalEntity == LegalEntity.Belcukerka)
            {
                cell = row.Cells[0];
                cell.AddParagraph("в том числе НДС: ");
                cell.Format.Font.Bold = true;
                cell.Format.Alignment = ParagraphAlignment.Right;

                cell = row.Cells[1];
                numToWords = RussianConverter.FormatCurrency((decimal)totalVAT, UnitOfMeasure.Ruble);
                cell.AddParagraph(numToWords);
                cell.Format.Alignment = ParagraphAlignment.Left;
            }
            else
            {
                cell = row.Cells[0];
                cell.MergeRight = 1;
                cell.AddParagraph("Без НДС. На основании п.3.12 ст.286 Налогового кодекса Республики Беларусь (особенная часть)");
                cell.Format.Font.Bold = false;
                cell.Format.Alignment = ParagraphAlignment.Center;
            }

            // draw whitespace
            invoice.LastSection.AddParagraph("", "Offset");

            // draw signature block
            table = invoice.LastSection.AddTable();
            table.Borders.Visible = false;
            table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
            table.Format.Font.Name = DocumentDefaults.FontFamilyArial;
            table.KeepTogether = true;

            table.AddColumn(Unit.FromPoint(440)); // поставщик
            table.AddColumn(Unit.FromPoint(100)); // пусто
            table.AddColumn(Unit.FromPoint(600)); // покупатель

            row = table.AddRow();
            row.Format.Font.Bold = true;
            row.Format.Alignment = ParagraphAlignment.Left;
            row.KeepWith = 2;

            cell = row.Cells[0];
            cell.AddParagraph("Поставщик");

            cell = row.Cells[2];
            cell.AddParagraph("Покупатель");

            row = table.AddRow();
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = Unit.FromPoint(200);

            if (model.IsSignatureNeeded)
            {
                cell = row.Cells[0];

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
            row.Format.Alignment = ParagraphAlignment.Center;
            row.TopPadding = Unit.FromPoint(5);

            cell = row.Cells[0];
            cell.AddParagraph("М.П.");
            cell.Borders.Top.Visible = true;
            cell.Borders.Top.Width = 2;

            cell = row.Cells[2];
            cell.AddParagraph("М.П.");
            cell.Borders.Top.Visible = true;
            cell.Borders.Top.Width = 2;
        }
    }
}
