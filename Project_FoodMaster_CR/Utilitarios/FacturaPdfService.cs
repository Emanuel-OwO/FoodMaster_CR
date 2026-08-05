using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using appFoodMaster_CR.Layer.Entities;

namespace appFoodMaster_CR.Layer.Utilitarios
{
    public static class FacturaPdfService
    {
        public static string GenerarPdfFactura(
              Factura factura,
              List<FacturaDetalle> detalles,
              string nombreCliente,
              string cedulaCliente,
              string nombreUsuario,
              string tipoPago,
              byte[] firmaBytes,
              System.Drawing.Image qrImage)
        {
            if (factura == null)
                throw new Exception("La factura no puede ser nula.");

            if (detalles == null || detalles.Count == 0)
                throw new Exception("La factura no tiene detalle.");

            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FacturasPDF");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string rutaPdf = Path.Combine(carpeta, factura.NumeroFactura + ".pdf");

            byte[] qrBytes = ConvertirImagenAPng(qrImage);

            Document.Create(documento =>
            {
                documento.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(28);
                    page.PageColor("#FFFEFB");
                    page.DefaultTextStyle(x => x.FontSize(10).FontColor("#33352F"));

                    page.Header().Column(col =>
                    {
                        col.Item().Background("#E8F5E9").Padding(18).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("FoodMaster CR")
                                    .FontSize(26)
                                    .Bold()
                                    .FontColor("#1B5E20");

                                c.Item().Text("Factura electrónica")
                                    .FontSize(14)
                                    .SemiBold()
                                    .FontColor("#4C7A52");

                                c.Item().PaddingTop(8).Text("Número de factura: " + factura.NumeroFactura)
                                    .FontSize(11);

                                c.Item().Text("Fecha: " + factura.Fecha.ToString("dd/MM/yyyy HH:mm"))
                                    .FontSize(11);

                                c.Item().Text("Usuario: " + nombreUsuario)
                                    .FontSize(11);

                                c.Item().Text("Estado: " + (factura.Estado ? "Activa" : "Pendiente"))
                                    .FontSize(11);
                            });

                            if (qrBytes != null)
                            {
                                row.ConstantItem(110)
                                   .AlignMiddle()
                                   .AlignRight()
                                   .Background(Colors.White)
                                   .Border(1)
                                   .BorderColor("#A5D6A7")
                                   .Padding(8)
                                   .Image(qrBytes);
                            }
                        });
                    });

                    page.Content().PaddingVertical(16).Column(col =>
                    {
                        col.Spacing(14);

                        col.Item().Background("#F1F8F1").Border(1).BorderColor("#A5D6A7").Padding(12).Column(c =>
                        {
                            c.Item().Text("Datos del cliente")
                                .Bold()
                                .FontSize(13)
                                .FontColor("#1B5E20");

                            c.Item().PaddingTop(4).Text("Cliente: " + nombreCliente).FontSize(11);
                            c.Item().Text("Cédula: " + cedulaCliente).FontSize(11);
                            c.Item().Text("Tipo de pago: " + tipoPago).FontSize(11);
                        });

                        col.Item().Text("Detalle de factura")
                            .Bold()
                            .FontSize(13)
                            .FontColor("#1B5E20");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).Text("Producto").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).Text("Descripcion").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).AlignCenter().Text("Cantidad").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).AlignRight().Text("Precio").Bold().FontColor("#1B3A1E");
                                header.Cell().Background("#A5D6A7").BorderBottom(1).BorderColor("#2E7D32").Padding(7).AlignRight().Text("Subtotal").Bold().FontColor("#1B3A1E");
                            });

                            foreach (var item in detalles)
                            {
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).Text(item.IdProducto);
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).Text(item.NombreProducto);
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).AlignCenter().Text(item.Cantidad.ToString());
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).AlignRight().Text(item.Precio.ToString("N2"));
                                table.Cell().BorderBottom(1).BorderColor("#D7ECD7").Padding(7).AlignRight().Text(item.Subtotal.ToString("N2"));
                            }
                        });

                        col.Item().AlignRight().Width(250).Background("#F1F8F1").Border(1).BorderColor("#A5D6A7").Padding(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            AgregarFilaTotal(table, "SubTotal:", factura.Subtotal.ToString("N2"), false);
                            AgregarFilaTotal(table, "Impuesto:", factura.MontoImpuesto.ToString("N2"), false);
                            AgregarFilaTotal(table, "Total Colones:", factura.TotalColones.ToString("N2"), true);
                            AgregarFilaTotal(table, "Total Dólares:", factura.TotalDolares.ToString("N2"), true);
                        });

                        col.Item().PaddingTop(8).Background("#F1F8F1").Border(1).BorderColor("#A5D6A7").Padding(12).Column(c =>
                        {
                            c.Item().Text("Firma del cliente")
                                .Bold()
                                .FontSize(13)
                                .FontColor("#1B5E20");

                            c.Item().PaddingTop(6);

                            if (firmaBytes != null && firmaBytes.Length > 0)
                            {
                                c.Item().Height(85).Image(firmaBytes);
                            }
                            else
                            {
                                c.Item().Height(50).AlignMiddle().AlignCenter().Text("Sin firma registrada")
                                    .Italic()
                                    .FontColor("#6B8268");
                            }
                        });
                    });

                    page.Footer().PaddingTop(8).Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor("#A5D6A7");
                        col.Item().PaddingTop(5).AlignCenter().Text("Gracias por su compra en FoodMaster CR")
                            .FontSize(9)
                            .FontColor("#4C7A52");
                    });
                });
            }).GeneratePdf(rutaPdf);

            return rutaPdf;
        }

        private static void AgregarFilaTotal(TableDescriptor table, string titulo, string valor, bool negrita)
        {
            if (negrita)
            {
                table.Cell().Padding(4).Text(titulo).Bold().FontColor("#1B5E20");
                table.Cell().Padding(4).AlignRight().Text(valor).Bold().FontColor("#1B5E20");
            }
            else
            {
                table.Cell().Padding(4).Text(titulo);
                table.Cell().Padding(4).AlignRight().Text(valor);
            }
        }

        private static byte[] ConvertirImagenAPng(System.Drawing.Image imagen)
        {
            if (imagen == null)
                return null;

            using (var ms = new MemoryStream())
            {
                imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        // Paleta de colores para la factura con base a los colores del logo por si se me olvida los nombres
        private const string ColorFuerte = "#1B5E20"; // títulos y subtítulos en negrita
        private const string ColorSubtitulo = "#4C7A52"; // subtítulo bajo el título
        private const string ColorFondoHdr = "#E8F5E9"; // fondo del encabezado
        private const string ColorFondoBox = "#F1F8F1"; // fondo de cajas internas
        private const string ColorBorde = "#A5D6A7"; // bordes de cajas
        private const string ColorBordeFuerte = "#2E7D32"; // línea bajo el header de tabla
        private const string ColorTextoTabla = "#1B3A1E"; // texto del header de tabla
        private const string ColorBordeFila = "#D7ECD7"; // separador entre filas
        private const string ColorMuted = "#6B8268"; // "sin firma registrada"
        private const string ColorFooter = "#4C7A52"; // texto del pie de página
    }
}