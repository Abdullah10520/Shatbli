import { Component, inject, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { ThemeToggleComponent } from '../theme-toggle.component';
import { LanguageSwitcherComponent } from '../language-switcher.component';
import { LanguageService } from '../../../Core/services/language.service';

@Component({
    selector: 'app-navbar',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        TranslatePipe,
        ThemeToggleComponent,
        LanguageSwitcherComponent
    ],
    templateUrl: './navbar.component.html'
})
export class NavbarComponent {
    readonly languageService = inject(LanguageService);

    // Input for current user
    currentUser = input<any>(null);

    // Output for logout event
    logoutClicked = output<void>();

    // Mobile menu state
    mobileMenuOpen = signal(false);

    toggleMobileMenu(): void {
        this.mobileMenuOpen.update(v => !v);
    }

    closeMobileMenu(): void {
        this.mobileMenuOpen.set(false);
    }

    logout(): void {
        this.closeMobileMenu();
        this.logoutClicked.emit();
    }
}

