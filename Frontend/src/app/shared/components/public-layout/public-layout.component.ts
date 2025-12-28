import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthService } from '../../../Core/auth/auth.service';
import { LanguageService } from '../../../Core/services/language.service';
import { ThemeToggleComponent } from '../theme-toggle.component';
import { LanguageSwitcherComponent } from '../language-switcher.component';
import { FooterComponent } from '../footer/footer.component';

@Component({
    selector: 'app-public-layout',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        TranslatePipe,
        ThemeToggleComponent,
        LanguageSwitcherComponent,
        FooterComponent
    ],
    templateUrl: './public-layout.component.html'
})
export class PublicLayoutComponent {
    private authService = inject(AuthService);
    private router = inject(Router);
    readonly languageService = inject(LanguageService);

    // Mobile menu state
    mobileMenuOpen = signal(false);

    get isLoggedIn(): boolean {
        return this.authService.isLoggedIn();
    }

    get currentUser() {
        return this.authService.currentUserSig();
    }

    toggleMobileMenu(): void {
        this.mobileMenuOpen.update(v => !v);
    }

    closeMobileMenu(): void {
        this.mobileMenuOpen.set(false);
    }

    logout(): void {
        this.authService.logout();
        this.closeMobileMenu();
        this.router.navigate(['/']);
    }
}

