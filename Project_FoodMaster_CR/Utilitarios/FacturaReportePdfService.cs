using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using appFoodMaster_CR.Layer.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace appFoodMaster_CR.Utilitarios
{
    public  class FacturaReportePdfService
    {
        public void GenerarPdf(List<FacturaReporteDTO> lista, decimal total, DateTime fechaInicial, DateTime fechaFinal)
        {
            string ruta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "ReporteFacturas.pdf"
            );

            var pdfBytes = Document.Create(documento =>
            {
                documento.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(28);
                    page.PageColor("#FFFDFE");
                    page.DefaultTextStyle(x => x.FontSize(10).FontColor("#1B3A1E"));

                    page.Header().Column(col =>
                    {
                        col.Item().Background("#E8F5E9").Padding(18).Column(c =>
                        {
                            c.Item().Text("FoodMaster_CR")
                                .FontSize(24)
                                .Bold()
                                .FontColor("#1B5E20");

                            c.Item().Text("Reporte de Facturas")
                                .FontSize(15)
                                .SemiBold()
                                .FontColor("#4C7A52");

                            c.Item().PaddingTop(8).Text("Fecha inicial: " + fechaInicial.ToString("dd/MM/yyyy"));
                            c.Item().Text("Fecha final: " + fechaFinal.ToString("dd/MM/yyyy"));
                            c.Item().Text("Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });
                    });

                    page.Content().PaddingVertical(16).Column(col =>
                    {
                        col.Spacing(14);

                        col.Item().Text("Facturas encontradas")
                            .Bold()
                            .FontSize(13)
                            .FontColor("#1B5E20");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // factura
                                columns.RelativeColumn(2); // fecha
                                columns.RelativeColumn(3); // cliente
                                columns.RelativeColumn(2); // total
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).Text("Factura").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).Text("Fecha").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).Text("Cliente").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).AlignRight().Text("Total").Bold().FontColor("#1B3A1E");
                            });

                            foreach (var item in lista)
                            {
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).Text(item.NumeroFactura);
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).Text(item.Fecha.ToString("dd/MM/yyyy"));
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).Text(item.Cliente);
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).AlignRight().Text(item.TotalColones.ToString("N2", CultureInfo.InvariantCulture));
                            }
                        });

                        col.Item().AlignRight().Width(250).Background("#F1F8F1").Border(1).BorderColor("#A5D6A7").Padding(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Padding(4).Text("Cantidad de facturas:").Bold().FontColor("#1B5E20");
                            table.Cell().Padding(4).AlignRight().Text(lista.Count.ToString()).Bold().FontColor("#1B5E20");

                            table.Cell().Padding(4).Text("Total facturado:").Bold().FontColor("#1B5E20");
                            table.Cell().Padding(4).AlignRight().Text(total.ToString("N2", CultureInfo.InvariantCulture)).Bold().FontColor("#1B5E20");
                        });
                    });

                    page.Footer().PaddingTop(8).Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor("#A5D6A7");
                        col.Item().PaddingTop(5).AlignCenter().Text("Reporte generado por FoodMaster_CR")
                            .FontSize(9)
                            .FontColor("#4C7A52");
                    });
                });
            }).GeneratePdf();

            File.WriteAllBytes(ruta, pdfBytes);
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}

