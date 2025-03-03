using System.Text;
using FaturasHandler.IoC.Dto;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FaturasHandler.Crawler.IVA_Periodico
{
    public class IVAExporter
    {
        public void SaveAsCsv(List<FaturaDto> documents, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Data Emissão;Setor;Emitente;Nº Fatura / ATCUD;Total;IVA;Âmbito;Final");

            foreach (var doc in documents)
            {
                sb.AppendLine($"{doc.DataEmissaoDocumento};Outros;{doc.NomeEmitente};{doc.NumeroDocumento};{doc.ValorTotal};{doc.ValorTotalIva};{doc.AmbitoDeAtividade};{doc.ValorTotalIva} €");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public void SaveAsExcel(List<FaturaDto> documents, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Faturas");

                // Headers
                worksheet.Cells["A1"].Value = "Data Emissão";
                worksheet.Cells["B1"].Value = "Setor";
                worksheet.Cells["C1"].Value = "Emitente";
                worksheet.Cells["D1"].Value = "Nº Fatura / ATCUD";
                worksheet.Cells["E1"].Value = "Total";
                worksheet.Cells["F1"].Value = "IVA";
                worksheet.Cells["G1"].Value = "Âmbito";
                worksheet.Cells["H1"].Value = "Final";

                // Apply header formatting
                using (var headerRange = worksheet.Cells["A1:H1"])
                {
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    headerRange.AutoFilter = true;
                }

                int row = 2;
                decimal total = 0, totalIva = 0, finalValue = 0;

                foreach (var doc in documents)
                {
                    worksheet.Cells[$"A{row}"].Value = doc.DataEmissaoDocumento;
                    worksheet.Cells[$"B{row}"].Value = GetSector(doc.ActividadeEmitenteDesc); // Convert Activity to Sector
                    worksheet.Cells[$"C{row}"].Value = $"{doc.NifEmitente} - {doc.NomeEmitente}";
                    worksheet.Cells[$"D{row}"].Value = doc.NumeroDocumento;
                    worksheet.Cells[$"E{row}"].Value = doc.ValorTotal;
                    worksheet.Cells[$"F{row}"].Value = doc.ValorTotalIva;
                    worksheet.Cells[$"G{row}"].Value = GetAmbito(doc.AmbitoDeAtividade);
                    worksheet.Cells[$"H{row}"].Value = $"{doc.ValorTotalIva} €";

                    total += doc.ValorTotal;
                    totalIva += doc.ValorTotalIva;
                    finalValue += doc.ValorTotalIva;

                    row++;
                }

                // Add TOTAL row
                worksheet.Cells[$"D{row}"].Value = "TOTAL";
                worksheet.Cells[$"E{row}"].Value = total;
                worksheet.Cells[$"F{row}"].Value = totalIva;
                worksheet.Cells[$"H{row}"].Value = $"{finalValue} €";

                // Apply number formatting
                worksheet.Cells[$"E2:E{row}"].Style.Numberformat.Format = "€#,##0.00";
                worksheet.Cells[$"F2:F{row}"].Style.Numberformat.Format = "€#,##0.00";
                worksheet.Cells[$"H2:H{row}"].Style.Numberformat.Format = "€#,##0.00";

                // Autofit columns
                worksheet.Cells.AutoFitColumns();

                // Save the Excel file
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private string GetSector(string activity)
        {
            if (string.IsNullOrEmpty(activity))
                return "";
            return activity.Contains("Educação") ? "Educação" : "Outros";
        }

        private string GetAmbito(string ambito)
        {
            return ambito == "Totalmente" ? "100%" : "25%";
        }
    }
}
