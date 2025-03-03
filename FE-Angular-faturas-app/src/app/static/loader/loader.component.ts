import { ChangeDetectionStrategy, Component } from '@angular/core';
import { LoaderService } from 'src/shared/services/loader.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoaderComponent {
  isLoading = this.loaderService.isLoading$;

  constructor(private loaderService: LoaderService) { }
}
