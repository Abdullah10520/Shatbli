import { Component, inject } from '@angular/core';
import { ThemeService } from '../../Core/services/theme.service';

@Component({
    selector: 'app-theme-toggle',
    standalone: true,
    template: `
    <button
      (click)="themeService.toggleTheme()"
      class="relative p-2.5 rounded-xl bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 transition-all duration-300 group"
      [attr.aria-label]="themeService.isDark() ? 'تفعيل الوضع الفاتح' : 'تفعيل الوضع الداكن'"
      type="button"
    >
      <!-- أيقونة الشمس (للوضع الفاتح) -->
      <svg
        class="w-5 h-5 text-amber-500 transition-all duration-300"
        [class.opacity-100]="!themeService.isDark()"
        [class.opacity-0]="themeService.isDark()"
        [class.rotate-0]="!themeService.isDark()"
        [class.rotate-90]="themeService.isDark()"
        [class.scale-100]="!themeService.isDark()"
        [class.scale-0]="themeService.isDark()"
        [class.absolute]="themeService.isDark()"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
        stroke-width="2"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z"
        />
      </svg>
      
      <!-- أيقونة القمر (للوضع الداكن) -->
      <svg
        class="w-5 h-5 text-indigo-400 transition-all duration-300"
        [class.opacity-0]="!themeService.isDark()"
        [class.opacity-100]="themeService.isDark()"
        [class.-rotate-90]="!themeService.isDark()"
        [class.rotate-0]="themeService.isDark()"
        [class.scale-0]="!themeService.isDark()"
        [class.scale-100]="themeService.isDark()"
        [class.absolute]="!themeService.isDark()"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
        stroke-width="2"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z"
        />
      </svg>
    </button>
  `,
    styles: [`
    :host {
      display: inline-block;
    }
    
    button {
      display: flex;
      align-items: center;
      justify-content: center;
    }
  `]
})
export class ThemeToggleComponent {
    readonly themeService = inject(ThemeService);
}
