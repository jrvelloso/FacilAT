import { Component, Input, OnInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { IListagemDeclaracaoEntregues } from 'src/shared/interfaces/IListagemDeclaracaoEntregues';
import { IListagemRecibosVerdes } from 'src/shared/interfaces/IListagemRecibosVerdes';
import { DeclaracoesService } from 'src/shared/services/declaracoes.service';
import { RecibosVerdesService } from 'src/shared/services/recibos-verdes.service';

// Register Chart.js components
Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @Input() userId: string | null = null; // ✅ Receive userId from HomeComponent

  // Data from services
  declaracoes: IListagemDeclaracaoEntregues[] = [];
  recibos: IListagemRecibosVerdes[] = [];

  // Chart instances
  monthlyRevenueChart: any;
  quarterlyBalanceChart: any;
  taxComparisonChart: any;

  // Summary metrics
  totalRevenue = 0;
  totalTaxPaid = 0;
  averageInvoiceAmount = 0;
  latestDeclaration: IListagemDeclaracaoEntregues | null = null;

  constructor(
    private declaracoesService: DeclaracoesService,
    private recibosVerdesService: RecibosVerdesService
  ) { }

  ngOnInit(): void {
    if (this.userId) {
      this.loadData();
    } else {
      console.error('User ID is null in DashboardComponent.');
    }
  }

  loadData(): void {
    // Load tax declarations by userId
    this.declaracoesService.getDeclaracoesByUserId(this.userId!).subscribe(data => {
      this.declaracoes = data.sort((a, b) => {
        return new Date(b.dataRececao).getTime() - new Date(a.dataRececao).getTime();
      });

      console.log('this.declaracoes');
      console.log(this.declaracoes);
      if (this.declaracoes.length > 0) {
        this.latestDeclaration = this.declaracoes[0];
      }

      this.calculateMetrics();
      setTimeout(() => this.createQuarterlyBalanceChart(), 500);
      setTimeout(() => this.createTaxComparisonChart(), 500);
    });

    // Load invoices (recibos verdes) by userId
    this.recibosVerdesService.getRecibosVerdesByUserId(this.userId!).subscribe(data => {
      this.recibos = data.sort((a, b) => {
        return new Date(b.dataEmissao).getTime() - new Date(a.dataEmissao).getTime();
      });

      this.calculateMetrics();
      setTimeout(() => this.createMonthlyRevenueChart(), 500);
    });
  }

  calculateMetrics(): void {
    if (this.recibos.length > 0) {
      this.totalRevenue = this.recibos.reduce((sum, recibo) => sum + (recibo.importanciaRecebida || 0), 0);
      this.averageInvoiceAmount = this.totalRevenue / this.recibos.length;
    }

    if (this.declaracoes.length > 0) {
      this.totalTaxPaid = this.declaracoes.reduce((sum, declaracao) => sum + ((declaracao.impostoLiquidadoCentimos || 0) / 100), 0);
    }
  }

  getQuarter(periodo: string): string {
    if (!periodo || periodo.length < 4) return 'Invalid Period';

    const year = '20' + periodo.substring(0, 2);
    const month = parseInt(periodo.substring(2, 4), 10);
    if (isNaN(month) || month < 1 || month > 12) return 'Invalid Period';

    const quarter = Math.ceil(month / 3);
    return `Q${quarter} ${year}`; // ✅ Fixed template literal
  }

  getDeclaracaoValue(declaracao: IListagemDeclaracaoEntregues): number {
    const value = ((declaracao.valor1 || 0) + (declaracao.valor2 || 0)) / 100;
    return declaracao.indicadorPagamentoReembolso === 'P' ? -value : value;
  }

  createMonthlyRevenueChart(): void {
    const canvas = document.getElementById('monthlyRevenueChart') as HTMLCanvasElement;
    if (!canvas || this.recibos.length === 0) return;

    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const monthlyData: { [key: string]: number } = {};
    this.recibos.forEach(recibo => {
      const date = new Date(recibo.dataEmissao);
      const monthYear = date.toLocaleString('default', { month: 'short', year: 'numeric' });

      if (!monthlyData[monthYear]) {
        monthlyData[monthYear] = 0;
      }
      monthlyData[monthYear] += recibo.importanciaRecebida || 0;
    });

    const labels = Object.keys(monthlyData).sort();
    const values = labels.map(month => monthlyData[month]);

    this.monthlyRevenueChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: 'Receita Mensal (€)',
          data: values,
          backgroundColor: 'rgba(0, 158, 247, 0.7)',
          borderColor: 'rgba(0, 158, 247, 1)',
          borderWidth: 1
        }]
      },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }


  createQuarterlyBalanceChart(): void {
    const canvas = document.getElementById('quarterlyBalanceChart') as HTMLCanvasElement;
    if (!canvas || this.declaracoes.length === 0) return;

    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const labels = this.declaracoes.map(d => this.getQuarter(d.periodo));
    const values = this.declaracoes.map(d => this.getDeclaracaoValue(d));

    this.quarterlyBalanceChart = new Chart(ctx, {
      type: 'line',
      data: { labels: labels, datasets: [{ label: 'Saldo Trimestral (€)', data: values }] },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }

  createTaxComparisonChart(): void {
    const canvas = document.getElementById('taxComparisonChart') as HTMLCanvasElement;
    if (!canvas || this.declaracoes.length === 0) return;

    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const labels = this.declaracoes.map(d => this.getQuarter(d.periodo));
    const liquidTax = this.declaracoes.map(d => (d.impostoLiquidadoCentimos || 0) / 100);
    const deductibleTax = this.declaracoes.map(d => (d.impostoDedutivelCentimos || 0) / 100);

    this.taxComparisonChart = new Chart(ctx, {
      type: 'bar',
      data: { labels: labels, datasets: [{ label: 'Imposto Liquidado (€)', data: liquidTax }, { label: 'Imposto Dedutível (€)', data: deductibleTax }] },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }
}
