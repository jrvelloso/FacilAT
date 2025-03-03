import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RouterService } from 'src/shared/services/router.service';

@NgModule({
  imports: [RouterModule.forRoot(RouterService.getRoutes(), {
    // useHash: true,
    scrollPositionRestoration: 'enabled',
    // scrollOffset: [0, 115],
    // anchorScrolling: 'enabled',x
    onSameUrlNavigation: 'reload'

  })],
  exports: [RouterModule],
})

export class AppRoutingModule { }
