import { Component, Input, OnInit } from '@angular/core';
import { IListagemDeclaracaoEntregues } from 'src/shared/interfaces/IListagemDeclaracaoEntregues';
import { DeclaracoesService } from 'src/shared/services/declaracoes.service';

@Component({
  selector: 'app-declaracoes-iva',
  templateUrl: './declaracoes-iva.component.html',
  styleUrls: ['./declaracoes-iva.component.css']
})
export class DeclaracoesIvaComponent implements OnInit {
  @Input() userId: string | null = null; // ✅ Receive userId from HomeComponent
  declaracoes: IListagemDeclaracaoEntregues[] = [];

  constructor(private declaracoesService: DeclaracoesService) { }

  ngOnInit(): void {
    console.log('this.userId');
    console.log(this.userId);
    if (this.userId) {
      this.fetchDeclaracoes();
    } else {
      console.error('User ID is null in DeclaracoesIvaComponent.');
    }
  }

  fetchDeclaracoes(): void {
    this.declaracoesService.getDeclaracoesByUserId(this.userId!).subscribe({
      next: (data) => {
        this.declaracoes = data;
        console.log("Declaracoes IVA fetched:", this.declaracoes);
      },
      error: (error) => {
        console.error('Error fetching declarações IVA:', error);
      }
    });
  }

  getQuarter(periodo: string): string {
    if (!periodo || periodo.length < 4) return 'Invalid Period';

    const year = '20' + periodo.substring(0, 2);
    const month = parseInt(periodo.substring(2, 4), 10);
    if (isNaN(month) || month < 1 || month > 12) return 'Invalid Period';

    const quarter = Math.ceil(month / 3);
    return `Q${quarter} ${year}`; // ✅ Fixed template literal
  }

}
