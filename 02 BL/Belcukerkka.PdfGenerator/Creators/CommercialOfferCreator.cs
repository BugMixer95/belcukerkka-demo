using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Belcukerkka.PdfGenerator.Creators
{
    public class CommercialOfferCreator : BaseDocumentCreator
    {
        public CommercialOfferCreator(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly IWebHostEnvironment _webHostEnvironment;

        private static readonly Dictionary<int, string> PrimarySectionTableColumnNames = new Dictionary<int, string>
        {
            {0, "Номер п/п" },
            {1, "Наименование" },
            {2, "Вес, г" },
            {3, "Тип состава" },
            {4, "Цена за ед. товара" },
            {5, "Количество, шт." },
            {6, "Цена с НДС" }
        };

        private static readonly Dictionary<int, string> SecondarySectionsTableColumnNames = new Dictionary<int, string>
        {
            {0, "Наименование" },
            {1, "Изображение" },
            {2, "Количество в наборе, шт." }
        };

        public async override Task<byte[]> CreateAsync(DocumentRequestModel model)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Document commercialOffer = new Document();

            commercialOffer.Info.Title = "Коммерческое предложение";
            commercialOffer.Info.Author = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "Общество с ограниченной ответственностью \"БЕЛЦУКЕРКА\"",
                LegalEntity.OriolTrade => "Общество с ограниченной ответственностью \"ОРИОЛ ТРЕЙД\"",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            DocumentDecorator.SetStyles(commercialOffer);

            DrawHeaderAndFooter(commercialOffer, model);
            DrawPrimarySection(commercialOffer, model);

            if (model.IsAppendixNeeded)
            {
                DrawSecondarySections(commercialOffer, model);
            }

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = commercialOffer;
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

        private void DrawHeaderAndFooter(Document commercialOffer, DocumentRequestModel model)
        {
            Section section = commercialOffer.AddSection();
            section.PageSetup.StartingNumber = 1;
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.PageWidth = Unit.FromPoint(DocumentDefaults.A4Width);
            section.PageSetup.PageHeight = Unit.FromPoint(DocumentDefaults.A4Height);
            section.PageSetup.LeftMargin = DocumentDefaults.Offset * 1.5;
            section.PageSetup.RightMargin = DocumentDefaults.Offset * 1.5;
            section.PageSetup.TopMargin = DocumentDefaults.Offset * 6.5;
            section.PageSetup.BottomMargin = DocumentDefaults.Offset * 2;

            // drawing header
            var header = section.Headers.Primary;
            Table headerTable = header.AddTable();
            headerTable.Borders.Visible = false;
            headerTable.Borders.Width = 0.25;
            headerTable.Format.Font.Size = DocumentDefaults.FontSizeRegular;
            headerTable.AddColumn(Unit.FromPoint(275));
            headerTable.AddColumn(Unit.FromPoint(540));
            headerTable.AddColumn(Unit.FromPoint(275));

            Row row = headerTable.AddRow();
            row.VerticalAlignment = VerticalAlignment.Top;

            Cell cell = row.Cells[0];
            cell.AddParagraph("\n" + model.DocumentDate.ToString("dd-MM-yyyy"));
            cell.Format.Alignment = ParagraphAlignment.Left;
            
            cell = row.Cells[1];
            
            var legalEntityLogo = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "logo_Belcukerka.png",
                LegalEntity.OriolTrade => "logo_OriolTrade.jpg",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            var headerLogo = cell.AddParagraph().AddImage(Path.Combine(_webHostEnvironment.WebRootPath, DocumentDefaults.ImagePath, legalEntityLogo));
            if (model.LegalEntity == LegalEntity.Belcukerka)
            {
                headerLogo.Width = 200;
                headerLogo.Height = 200;
            }
            else
            {
                headerLogo.Width = 350;
                headerLogo.Height = 100;
            }
            cell.Format.Alignment = ParagraphAlignment.Center;

            row = headerTable.AddRow();
            row.Borders.Bottom.Visible = true;
            row.Borders.Bottom.Width = 5;

            // drawing footer
            var footer = section.Footers.Primary;

            Table footerTable = footer.AddTable();
            footerTable.Borders.Visible = false;
            footerTable.Borders.Width = 0.25;
            footerTable.Format.Font.Size = DocumentDefaults.FontSizeRegular;
            footerTable.AddColumn(Unit.FromPoint(375));
            footerTable.AddColumn(Unit.FromPoint(340));
            footerTable.AddColumn(Unit.FromPoint(375));

            row = footerTable.AddRow();
            row.Borders.Top.Visible = true;
            row.Borders.Top.Width = 3;

            row = footerTable.AddRow();
            row.VerticalAlignment = VerticalAlignment.Bottom;

            cell = row.Cells[0];
            var content = cell.AddParagraph();
            content.AddText("Страница ");
            content.AddPageField();
            content.AddText(" из ");
            content.AddNumPagesField();
            cell.Format.Alignment = ParagraphAlignment.Left;

            cell = row.Cells[1];
            var websiteLink = cell.AddParagraph().AddHyperlink("https://belcukerkka.by", HyperlinkType.Web);
            websiteLink.AddFormattedText("belcukerkka.by");
            cell.Format.Alignment = ParagraphAlignment.Center;

            cell = row.Cells[2];
            string legalEntityName = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "ООО \"БЕЛЦУКЕРКА\"",
                LegalEntity.OriolTrade => "ООО \"ОРИОЛ ТРЕЙД\"",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };
            cell.AddParagraph($"{legalEntityName}, " + model.DocumentDate.ToString("yyyy"));
            cell.Format.Alignment = ParagraphAlignment.Right;
        }

        private void DrawPrimarySection(Document commercialOffer, DocumentRequestModel model)
        {
            var numberFormat = DocumentDecorator.SetNumberFormat();

            // drawing heading
            string legalEntityName = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => "ООО \"БЕЛЦУКЕРКА\"",
                LegalEntity.OriolTrade => "ООО \"ОРИОЛ ТРЕЙД\"",
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };
            var heading = commercialOffer.LastSection.AddParagraph($"Коммерческое предложение от {legalEntityName}", "Heading1");
            heading.Format.Alignment = ParagraphAlignment.Center;

            // drawing table base
            Table table = commercialOffer.LastSection.AddTable();
            table.Borders.Width = 0.25;
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
            table.Format.Font.Name = DocumentDefaults.FontFamilyArial;
            table.Format.Font.Color = Colors.Black;
            table.Borders.Visible = false;
            table.BottomPadding = 10d;
            table.TopPadding = 10d;

            table.AddColumn(Unit.FromPoint(100)); // номер п/п
            table.AddColumn(Unit.FromPoint(175)); // наименование
            table.AddColumn(Unit.FromPoint(150)); // вес
            table.AddColumn(Unit.FromPoint(175)); // тип состава
            table.AddColumn(Unit.FromPoint(150)); // цена
            table.AddColumn(Unit.FromPoint(150)); // количество
            table.AddColumn(Unit.FromPoint(190)); // цена с ндс

            // drawing table header
            Row row = table.AddRow();
            row.Shading.Color = Colors.DarkGray;
            row.Format.Font.Bold = true;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Borders.Bottom.Visible = true;
            row.Borders.Bottom.Width = 2;
            row.HeadingFormat = true;

            Cell cell;

            foreach (var item in PrimarySectionTableColumnNames)
            {
                cell = row.Cells[item.Key];
                cell.AddParagraph(item.Value);
            }

            // drawing primary table content
            var orderItems = model.Order.OrderItems;

            double VAT = model.LegalEntity switch
            {
                LegalEntity.Belcukerka => 0.2,
                LegalEntity.OriolTrade => 0,
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            for (int i = 0; i < orderItems.Count; i++)
            {
                row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Borders.Bottom.Visible = true;

                cell = row.Cells[0];
                cell.AddParagraph((i + 1).ToString());

                cell = row.Cells[1];
                cell.AddParagraph(orderItems[i].Box.BoxParent.Name);
                cell.Format.Alignment = ParagraphAlignment.Left;
                cell.Format.Font.Italic = true;

                cell = row.Cells[2];
                cell.AddParagraph(orderItems[i].Box.Composition.Weight.ToString("#,0", numberFormat));

                cell = row.Cells[3];
                cell.AddParagraph(orderItems[i].Box.Composition.WeightType.Name);

                cell = row.Cells[4];
                cell.AddParagraph((orderItems[i].Box.Price * (1 + VAT)).ToString("0.00"));

                cell = row.Cells[5];
                cell.AddParagraph(orderItems[i].Amount.ToString());

                cell = row.Cells[6];
                cell.AddParagraph((orderItems[i].Box.Price * orderItems[i].Amount * (1 + VAT)).ToString("0.00"));
            }

            // drawing primary table footer
            row = table.AddRow();
            row.Shading.Color = Colors.DarkGray;
            row.Format.Font.Bold = true;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Borders.Top.Visible = true;
            row.Borders.Top.Width = 3;

            cell = row.Cells[1];
            cell.AddParagraph("ИТОГО:");
            cell.Format.Alignment = ParagraphAlignment.Left;

            int totalAmount = 0;
            foreach (var item in orderItems)
                totalAmount += item.Amount;

            cell = row.Cells[5];
            cell.AddParagraph(totalAmount.ToString());

            double totalPrice = 0;
            foreach (var item in orderItems)
                totalPrice += (item.Box.Price * (1 + VAT) * item.Amount);

            cell = row.Cells[6];
            cell.AddParagraph(totalPrice.ToString("0.00"));
        }

        private void DrawSecondarySections(Document commercialOffer, DocumentRequestModel model)
        {
            var orderItems = model.Order.OrderItems;

            for (int i = 0; i < orderItems.Count; i++)
            {
                // drawing heading
                var heading = commercialOffer.LastSection.AddParagraph(
                    $"Приложение {i + 1} - Состав новогоднего подарка \"{orderItems[i].Box.BoxParent.Name}\", " +
                    $"вариант {orderItems[i].Box.Composition.WeightType.Name}-{orderItems[i].Box.Composition.Weight}",
                    "Heading1");
                heading.Format.Alignment = ParagraphAlignment.Center;
                
                // drawing order item image
                var boxPictureParagraph = commercialOffer.LastSection.AddParagraph();
                var boxPicture = boxPictureParagraph.AddImage(
                    Path.Combine(_webHostEnvironment.WebRootPath, "images\\boxes", orderItems[i].Box.BoxParent.ImagePath)
                    );
                boxPicture.Width = 150;
                boxPicture.Height = 150;
                boxPictureParagraph.Format.Alignment = ParagraphAlignment.Center;
                boxPictureParagraph.Format.SpaceAfter = DocumentDefaults.Offset;

                // create table
                Table table = commercialOffer.LastSection.AddTable();
                table.Borders.Width = 0.25;
                table.Format.Alignment = ParagraphAlignment.Center;
                table.Format.Font.Size = DocumentDefaults.FontSizeTableContent;
                table.Format.Font.Name = DocumentDefaults.FontFamilyArial;
                table.Format.Font.Color = Colors.Black;
                table.Borders.Visible = false;
                table.BottomPadding = 10d;
                table.TopPadding = 10d;

                table.AddColumn(Unit.FromPoint(400)); // наименование
                table.AddColumn(Unit.FromPoint(340)); // изображение
                table.AddColumn(Unit.FromPoint(400)); // количество в наборе, шт.

                // drawing secondary table header
                Row row = table.AddRow();
                row.Shading.Color = Colors.DarkGray;
                row.Format.Font.Bold = true;
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Borders.Bottom.Visible = true;
                row.Borders.Bottom.Width = 2;
                row.HeadingFormat = true;

                Cell cell;

                // drawing secondary table content
                foreach (var item in SecondarySectionsTableColumnNames)
                {
                    cell = row.Cells[item.Key];
                    cell.AddParagraph(item.Value);
                }

                var candies = orderItems[i].Box.Composition.Candies;

                for (int j = 0; j < candies.Count; j++)
                {
                    row = table.AddRow();
                    row.VerticalAlignment = VerticalAlignment.Center;
                    row.Borders.Bottom.Visible = true;

                    cell = row.Cells[0];
                    cell.AddParagraph(candies[j].Name);

                    cell = row.Cells[1];
                    var candyPicture = cell.AddParagraph().AddImage(Path.Combine(_webHostEnvironment.WebRootPath, "images\\candies", candies[j].ImagePath));
                    candyPicture.Width = 100;
                    candyPicture.Height = 100;
                    cell.Format.Alignment = ParagraphAlignment.Center;

                    cell = row.Cells[2];
                    cell.AddParagraph(orderItems[i].Box.Composition.CandiesInComposition[j].Amount.ToString());
                }
            }
        }
    }
}
