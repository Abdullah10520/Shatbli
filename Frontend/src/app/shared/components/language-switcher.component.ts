import { Component, inject } from '@angular/core';
import { LanguageService } from '../../Core/services/language.service';

@Component({
    selector: 'app-language-switcher',
    standalone: true,
    template: `
    <button
      (click)="languageService.toggleLanguage()"
      class="relative px-3 py-2 rounded-xl bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 transition-all duration-300 text-sm font-semibold"
      [attr.aria-label]="languageService.isArabic() ? 'Switch to English' : 'التحويل للعربية'"
      type="button"
    >
      <span 
        class="transition-all duration-300"
        [class.text-purple-600]="true"
        [class.dark:text-purple-400]="true"
      >
        {{ languageService.isArabic() ? 'EN' : 'عربي' }}
      </span>
    </button>
  `,
    styles: [`
    :host {
      display: inline-block;
    }
  `]
})
export class LanguageSwitcherComponent {
    readonly languageService = inject(LanguageService);
}
