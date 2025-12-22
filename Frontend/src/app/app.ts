import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LanguageService } from './Core/services/language.service';
import { ThemeService } from './Core/services/theme.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  // حقن الخدمات لتهيئتها عند بدء التطبيق
  private readonly languageService = inject(LanguageService);
  private readonly themeService = inject(ThemeService);

  protected readonly title = signal('shatbli-client');
}

