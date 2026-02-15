// DESAFIO: Gerador de Relatórios Complexos
// PROBLEMA: Sistema precisa gerar diferentes tipos de relatórios (PDF, Excel, HTML)
// com múltiplas configurações opcionais (cabeçalho, rodapé, gráficos, tabelas, filtros)
// O código atual usa construtores enormes ou muitos setters, tornando difícil criar relatórios

// ========================================
// PADRÃO BUILDER - SOLUÇÃO (GoF)
// ========================================

using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de BI que gera relatórios customizados para diferentes departamentos
    // Cada relatório pode ter dezenas de configurações opcionais

    public class SalesReport
    {
        public string Title { get; set; }
        public string Format { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IncludeHeader { get; set; }
        public string HeaderText { get; set; }
        public bool IncludeFooter { get; set; }
        public string FooterText { get; set; }
        public bool IncludeCharts { get; set; }
        public string ChartType { get; set; }
        public List<string> Columns { get; set; } = new List<string>();
        public List<string> Filters { get; set; } = new List<string>();
        public string GroupBy { get; set; }
        public bool IncludeTotals { get; set; }
        public string Orientation { get; set; }
        public string PageSize { get; set; }

        public void Generate()
        {
            Console.WriteLine($"\n=== Gerando Relatório: {Title} ===");
            Console.WriteLine($"Formato: {Format}");
            Console.WriteLine($"Período: {StartDate:dd/MM/yyyy} a {EndDate:dd/MM/yyyy}");

            if (IncludeHeader)
                Console.WriteLine($"Cabeçalho: {HeaderText}");

            if (IncludeCharts)
                Console.WriteLine($"Gráfico: {ChartType}");

            Console.WriteLine($"Colunas: {string.Join(", ", Columns)}");

            if (Filters.Count > 0)
                Console.WriteLine($"Filtros: {string.Join(", ", Filters)}");

            if (!string.IsNullOrEmpty(GroupBy))
                Console.WriteLine($"Agrupado por: {GroupBy}");

            if (IncludeFooter)
                Console.WriteLine($"Rodapé: {FooterText}");

            Console.WriteLine("Relatório gerado com sucesso!");
        }
    }

    /// <summary>
    /// Builder fluente para criar relatórios de forma legível e flexível
    /// Resolve Problemas 1 e 2: Construção passo a passo sem construtores gigantes
    /// </summary>
    public class FluentReportBuilder
    {
        private SalesReport _report = new();

        public FluentReportBuilder SetTitle(string title)
        {
            _report.Title = title;
            return this;
        }
        public FluentReportBuilder SetFormat(string format)
        {
            _report.Format = format;
            return this;
        }

        public FluentReportBuilder SetStartDate(DateTime startDate)
        {
            _report.StartDate = startDate;
            return this;
        }

        public FluentReportBuilder SetEndDate(DateTime endDate)
        {
            _report.EndDate = endDate;
            return this;
        }
        public FluentReportBuilder IncludeHeader(string headerText)
        {
            _report.IncludeHeader = true;
            _report.HeaderText = headerText;
            return this;
        }
        public FluentReportBuilder IncludeFooter(string footerText)
        {
            _report.IncludeFooter = true;
            _report.FooterText = footerText;
            return this;
        }
        public FluentReportBuilder IncludeCharts(string chartType)
        {
            _report.IncludeCharts = true;
            _report.ChartType = chartType;
            return this;
        }
        public FluentReportBuilder AddColumn(string column)
        {
            _report.Columns.Add(column);
            return this;
        }
        public FluentReportBuilder AddFilter(string filter)
        {
            _report.Filters.Add(filter);
            return this;
        }
        public FluentReportBuilder SetGroupBy(string groupBy)
        {
            _report.GroupBy = groupBy;
            return this;
        }
        public FluentReportBuilder IncludeTotals()
        {
            _report.IncludeTotals = true;
            return this;
        }
        public FluentReportBuilder SetOrientation(string orientation)
        {
            _report.Orientation = orientation;
            return this;
        }
        public FluentReportBuilder SetPageSize(string pageSize)
        {
            _report.PageSize = pageSize;
            return this;
        }

        public SalesReport Build()
        {
            if (string.IsNullOrEmpty(_report.Title))
                throw new InvalidOperationException("O título do relatório é obrigatório.");
            if (string.IsNullOrEmpty(_report.Format))
                throw new InvalidOperationException("O formato do relatório é obrigatório.");
            if (_report.StartDate == default || _report.EndDate == default)
                throw new InvalidOperationException("As datas de início e fim são obrigatórias.");
            return _report;
        }
    }

    /// <summary>
    /// DIRECTOR: Orquestra a construção de relatórios complexos
    /// Encapsula a lógica de criação e a ordem dos passos de construção
    /// SOLUÇÃO PROBLEMA 3: Evita repetição de código em relatórios similares
    /// </summary>
    public class ReportDirector
    {
        /// <summary>
        /// Constrói um relatório de vendas mensal padrão
        /// </summary>
        public SalesReport ConstructMonthlySalesReport(FluentReportBuilder builder, string month, DateTime startDate, DateTime endDate)
        {
            return builder
                .SetTitle($"Vendas Mensais - {month}")
                .SetFormat("PDF")
                .SetStartDate(startDate)
                .SetEndDate(endDate)
                .IncludeHeader("Relatório de Vendas Mensais")
                .IncludeFooter("Confidencial - Uso Interno")
                .AddColumn("Produto")
                .AddColumn("Quantidade")
                .AddColumn("Valor Total")
                .IncludeCharts("Bar")
                .SetGroupBy("Categoria")
                .IncludeTotals()
                .SetOrientation("Portrait")
                .SetPageSize("A4")
                .Build();
        }

        /// <summary>
        /// Constrói um relatório executivo trimestral
        /// </summary>
        public SalesReport ConstructQuarterlyExecutiveReport(FluentReportBuilder builder, string quarter, DateTime startDate, DateTime endDate)
        {
            return builder
                .SetTitle($"Relatório Executivo - {quarter}")
                .SetFormat("Excel")
                .SetStartDate(startDate)
                .SetEndDate(endDate)
                .IncludeHeader($"Desempenho Trimestral - {quarter}")
                .AddColumn("Vendedor")
                .AddColumn("Região")
                .AddColumn("Meta")
                .AddColumn("Realizado")
                .AddColumn("% Atingimento")
                .IncludeCharts("Line")
                .SetGroupBy("Região")
                .IncludeTotals()
                .AddFilter("Status=Fechado")
                .SetOrientation("Landscape")
                .SetPageSize("A4")
                .Build();
        }

        /// <summary>
        /// Constrói um relatório analítico anual completo
        /// </summary>
        public SalesReport ConstructAnnualAnalyticsReport(FluentReportBuilder builder, int year)
        {
            return builder
                .SetTitle($"Análise Anual de Vendas - {year}")
                .SetFormat("PDF")
                .SetStartDate(new DateTime(year, 1, 1))
                .SetEndDate(new DateTime(year, 12, 31))
                .IncludeHeader($"Relatório Completo de Vendas {year}")
                .IncludeFooter("Gerado automaticamente pelo sistema de BI")
                .AddColumn("Mês")
                .AddColumn("Produto")
                .AddColumn("Categoria")
                .AddColumn("Vendas")
                .AddColumn("Crescimento %")
                .IncludeCharts("Pie")
                .SetGroupBy("Mês")
                .IncludeTotals()
                .AddFilter("Status=Aprovado")
                .SetOrientation("Landscape")
                .SetPageSize("A3")
                .Build();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Relatórios ===");
            Console.WriteLine("Demonstração do Padrão Builder com Director\n");

            // ===================================================================
            // PROBLEMA 1: Construtor com muitos parâmetros - difícil de ler
            // SOLUÇÃO: Fluent Builder
            // ===================================================================
            Console.WriteLine("--- PROBLEMA 1: Relatório com muitas configurações ---");

            var report1 = new FluentReportBuilder()
                .SetTitle("Vendas Mensais")
                .SetFormat("PDF")
                .SetStartDate(new DateTime(2024, 1, 1))
                .SetEndDate(new DateTime(2024, 1, 31))
                .IncludeHeader("Relatório de Vendas")
                .IncludeFooter("Confidencial")
                .IncludeCharts("Bar")
                .AddColumn("Produto")
                .AddColumn("Quantidade")
                .AddColumn("Valor")
                .AddFilter("Status=Ativo")
                .SetGroupBy("Categoria")
                .IncludeTotals()
                .SetOrientation("Portrait")
                .SetPageSize("A4")
                .Build();

            report1.Generate();

            // ===================================================================
            // PROBLEMA 2: Validação de configurações obrigatórias
            // SOLUÇÃO: Validação no método Build()
            // ===================================================================
            Console.WriteLine("\n--- PROBLEMA 2: Garantindo configurações obrigatórias ---");

            var report2 = new FluentReportBuilder()
                .SetTitle("Relatório Trimestral")
                .SetFormat("Excel")
                .SetStartDate(new DateTime(2024, 1, 1))
                .SetEndDate(new DateTime(2024, 3, 31))
                .AddColumn("Vendedor")
                .AddColumn("Região")
                .AddColumn("Total")
                .IncludeCharts("Line")
                .IncludeHeader("Relatório Trimestral")
                .SetGroupBy("Região")
                .IncludeTotals()
                .Build();

            report2.Generate();

            // ===================================================================
            // PROBLEMA 3: Código repetitivo para relatórios similares
            // SOLUÇÃO: Director - Encapsula lógica de construção complexa
            // ===================================================================
            Console.WriteLine("\n--- PROBLEMA 3: Reutilização com Director ---");

            var director = new ReportDirector();

            // Exemplo 1: Relatório mensal padronizado
            var januaryReport = director.ConstructMonthlySalesReport(
                new FluentReportBuilder(),
                "Janeiro/2024",
                new DateTime(2024, 1, 1),
                new DateTime(2024, 1, 31)
            );
            januaryReport.Generate();

            // Exemplo 2: Relatório executivo trimestral
            var q1Report = director.ConstructQuarterlyExecutiveReport(
                new FluentReportBuilder(),
                "Q1 2024",
                new DateTime(2024, 1, 1),
                new DateTime(2024, 3, 31)
            );
            q1Report.Generate();

            // Exemplo 3: Relatório analítico anual
            var annualReport = director.ConstructAnnualAnalyticsReport(
                new FluentReportBuilder(),
                2024
            );
            annualReport.Generate();

            // ===================================================================
            // RESUMO DAS SOLUÇÕES
            // ===================================================================
            Console.WriteLine("\n=== Resumo das Soluções ===");
            Console.WriteLine("\n✅ PROBLEMA 1 - Construtores gigantes:");
            Console.WriteLine("   SOLUÇÃO: Fluent Builder com métodos encadeados");

            Console.WriteLine("\n✅ PROBLEMA 2 - Configurações obrigatórias:");
            Console.WriteLine("   SOLUÇÃO: Validação no método Build()");

            Console.WriteLine("\n✅ PROBLEMA 3 - Código repetitivo:");
            Console.WriteLine("   SOLUÇÃO: Director encapsula receitas de construção");

            Console.WriteLine("\n📊 Redução de Código com Director:");
            Console.WriteLine("   • Sem padrão: ~15 linhas por relatório");
            Console.WriteLine("   • Com Director: ~4 linhas por relatório (-73%)");

            Console.WriteLine("\n🎯 QUANDO USAR:");
            Console.WriteLine("   • Fluent Builder: Relatórios únicos com muitas opções");
            Console.WriteLine("   • Director: Relatórios com estrutura fixa e repetitiva");
        }
    }
}
