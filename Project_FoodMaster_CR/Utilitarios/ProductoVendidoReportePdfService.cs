using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using appFoodMaster_CR.Layer.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace appFoodMaster_CR.Utilitarios
{
    public class ProductoVendidoReportePdfService
    {
        public void GenerarPdf(List<ProductoVendidoReporteDTO> lista, string producto, string descripcion, string tipoProducto)
        {
            try
            {
                string ruta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "ReporteProductosVendidos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"
                );

                QuestPDF.Settings.License = LicenseType.Community;

                var pdfBytes = Document.Create(documento =>
                {
                    documento.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(25);
                        page.PageColor("#FFFDFE");
                        page.DefaultTextStyle(x => x.FontSize(10).FontColor("#1B3A1E"));

                        page.Header().Column(col =>
                        {
                            col.Item().Background("#E8F5E9").Padding(16).Column(c =>
                            {
                                c.Item().Text("FoodMaster_CR")
                                    .FontSize(24)
                                    .Bold()
                                    .FontColor("#1B5E20");

                                c.Item().Text("Reporte de Productos Vendidos")
                                    .FontSize(15)
                                    .SemiBold()
                                    .FontColor("#4C7A52");

                                c.Item().PaddingTop(6).Text("Producto: " + (string.IsNullOrWhiteSpace(producto) ? "Todos" : producto));
                                c.Item().Text("Descripción: " + (string.IsNullOrWhiteSpace(descripcion) ? "Todas" : descripcion));
                                c.Item().Text("Tipo de producto: " + (string.IsNullOrWhiteSpace(tipoProducto) ? "Todos" : tipoProducto));
                                c.Item().Text("Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                            });
                        });

                        page.Content().PaddingVertical(15).Column(col =>
                        {
                            col.Spacing(12);

                            foreach (var item in lista)
                            {
                                col.Item()
                                    .Background("#F1F8F1")
                                    .Border(1)
                                    .BorderColor("#A5D6A7")
                                    .Padding(12)
                                    .Column(c =>
                                    {
                                        c.Spacing(4);

                                        c.Item().Text(item.Descripcion)
                                            .Bold()
                                            .FontSize(13)
                                            .FontColor("#1B5E20");

                                        c.Item().Text("Código interno: " + item.CodigoInterno);
                                        c.Item().Text("Tipo de producto: " + item.TipoProducto);
                                        c.Item().Text("Precio: " + item.Precio.ToString("N2", CultureInfo.InvariantCulture));
                                        c.Item().Text("Cantidad vendida: " + item.CantidadVendida);

                                        c.Item().Row(row =>
                                        {
                                            row.RelativeItem().AlignMiddle().Text("Fotografía")
                                                .SemiBold()
                                                .FontColor("#4C7A52");

                                            row.ConstantItem(100).Height(90)
                                                .Border(1)
                                                .BorderColor("#A5D6A7")
                                                .Background(Colors.White)
                                                .AlignMiddle()
                                                .AlignCenter()
                                                .Element(cont =>
                                                {
                                                    if (item.Fotografia != null && item.Fotografia.Length > 0)
                                                    {
                                                        cont.Image(item.Fotografia, ImageScaling.FitArea);
                                                    }
                                                    else
                                                    {
                                                        cont.Text("Sin foto")
                                                            .Italic()
                                                            .FontColor("#6B8268");
                                                    }
                                                });
                                        });
                                    });
                            }
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
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message);
            }
        }
    }
}

