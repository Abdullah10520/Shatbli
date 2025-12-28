import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../../Core/auth/auth.service';
import { LanguageService } from '../../../Core/services/language.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { FooterComponent } from '../footer/footer.component';

@Component({
    selector: 'app-main-layout',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        NavbarComponent,
        FooterComponent
    ],
    templateUrl: './main-layout.component.html'
})
export class MainLayoutComponent {
    private authService = inject(AuthService);
    private router = inject(Router);
    readonly languageService = inject(LanguageService);

    currentUser = this.authService.currentUserSig;

    logout(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }
}
