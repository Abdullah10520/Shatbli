import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { LanguageService } from '../../Core/services/language.service';

@Component({
    selector: 'app-not-found',
    standalone: true,
    imports: [CommonModule, RouterModule, TranslatePipe],
    templateUrl: './not-found.component.html'
})
export class NotFoundComponent {
    readonly languageService = inject(LanguageService);
}
