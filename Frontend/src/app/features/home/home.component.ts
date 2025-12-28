import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { LanguageService } from '../../Core/services/language.service';
import { AuthService } from '../../Core/auth/auth.service';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [CommonModule, RouterModule, TranslatePipe],
    templateUrl: './home.component.html'
})
export class HomeComponent {
    readonly languageService = inject(LanguageService);
    private authService = inject(AuthService);

    get isLoggedIn(): boolean {
        return this.authService.isLoggedIn();
    }

    get studioLink(): string {
        return this.isLoggedIn ? '/studio' : '/auth/login';
    }

    scrollToSection(sectionId: string): void {
        const element = document.getElementById(sectionId);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }
}

