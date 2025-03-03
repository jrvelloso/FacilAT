


import { Component, Input, OnInit } from '@angular/core';
import { IListagemRecibosVerdes } from 'src/shared/interfaces/IListagemRecibosVerdes';
import { RecibosVerdesService } from 'src/shared/services/recibos-verdes.service';

@Component({
  selector: 'app-recibos-verdes',
  templateUrl: './recibos-verdes.component.html',
  styleUrls: ['./recibos-verdes.component.css']
})
export class RecibosVerdesComponent implements OnInit {
  @Input() userId: string | null = null; // ✅ Receive userId from HomeComponent
  recibos: IListagemRecibosVerdes[] = [];

  constructor(private recibosVerdesService: RecibosVerdesService) { }

  ngOnInit(): void {
    if (this.userId) {
      this.fetchRecibos();
    } else {
      console.error('User ID is null in RecibosVerdesComponent.');
    }
  }

  fetchRecibos(): void {
    this.recibosVerdesService.getRecibosVerdesByUserId(this.userId!).subscribe({
      next: (data) => {
        this.recibos = data;
        console.log("Recibos Verdes fetched:", this.recibos);
      },
      error: (error) => {
        console.error('Error fetching recibos verdes:', error);
      }
    });
  }
}
