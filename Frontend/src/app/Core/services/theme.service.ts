import { Injectable, signal, effect, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export type Theme = 'light' | 'dark';

@Injectable({
    providedIn: 'root'
})
export class ThemeService {
    private readonly platformId = inject(PLATFORM_ID);
    private readonly THEME_KEY = 'shatbli-theme';

    // الوضع الحالي
    readonly theme = signal<Theme>(this.getInitialTheme());

    // هل الوضع الداكن مفعل؟
    readonly isDark = signal<boolean>(this.theme() === 'dark');

    constructor() {
        // تحديث الـ DOM عند تغيير الوضع
        effect(() => {
            const currentTheme = this.theme();
            this.isDark.set(currentTheme === 'dark');

            if (isPlatformBrowser(this.platformId)) {
                this.applyTheme(currentTheme);
                localStorage.setItem(this.THEME_KEY, currentTheme);
            }
        });
    }

    /**
     * الحصول على الوضع الافتراضي
     */
    private getInitialTheme(): Theme {
        if (isPlatformBrowser(this.platformId)) {
            // أولاً: التحقق من التفضيل المحفوظ
            const savedTheme = localStorage.getItem(this.THEME_KEY) as Theme;
            if (savedTheme === 'light' || savedTheme === 'dark') {
                return savedTheme;
            }

            // ثانياً: التحقق من تفضيل النظام
            if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
                return 'dark';
            }
        }

        return 'light';
    }

    /**
     * تطبيق الوضع على الـ DOM
     */
    private applyTheme(theme: Theme): void {
        const root = document.documentElement;

        if (theme === 'dark') {
            root.classList.add('dark');
        } else {
            root.classList.remove('dark');
        }
    }

    /**
     * التبديل بين الوضعين
     */
    toggleTheme(): void {
        this.theme.set(this.theme() === 'dark' ? 'light' : 'dark');
    }

    /**
     * تعيين وضع محدد
     */
    setTheme(theme: Theme): void {
        this.theme.set(theme);
    }
}
