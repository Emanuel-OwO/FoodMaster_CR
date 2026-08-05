using appFoodMaster_CR.Layer.DTO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System;
using System.Globalization;
using QuestPDF.Helpers;
using appFoodMaster_CR.Layer;
using appFoodMaster_CR.Layer.Entities;

public class ClienteReportePdfService
{
    public void GenerarPdf(List<Cliente> lista)
    {
        try
        {
            string ruta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "ReporteClientes_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"
            );

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
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

                            c.Item().Text("Reporte de Clientes")
                                .FontSize(15)
                                .SemiBold()
                                .FontColor("#4C7A52");

                            c.Item().PaddingTop(6).Text("Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
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

                                    c.Item().Text(
                                        (item.Nombre + " " + item.PrimerApellido + " " + item.SegundoApellido).Trim())
                                        .Bold()
                                        .FontSize(13)
                                        .FontColor("#1B5E20");

                                    c.Item().Text("Identificación: " + item.Identificacion);
                                    c.Item().Text("Teléfono: " + item.Telefono);
                                    c.Item().Text("Correo: " + item.Correo);
                                    c.Item().Text("Provincia: " + item.IdProvincia);
                                    c.Item().Text("Dirección: " + item.Direccion);

                                    c.Item().Row(row =>
                                    {
                                        // Texto a la izquierda
                                        row.RelativeItem().AlignMiddle().Text("Fotografía")
                                            .SemiBold()
                                            .FontColor("#4C7A52");

                                        // Imagen a la derecha
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

            MessageBox.Show("Error al generar reporte de clientes: " + ex.Message);
        }
    }
}


