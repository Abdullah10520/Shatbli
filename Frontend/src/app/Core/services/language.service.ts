import { Injectable, signal, effect, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';

export type Language = 'ar' | 'en';

@Injectable({
    providedIn: 'root'
})
export class LanguageService {
    private readonly platformId = inject(PLATFORM_ID);
    private readonly translateService = inject(TranslateService);
    private readonly LANG_KEY = 'shatbli-lang';

    // اللغة الحالية
    readonly currentLang = signal<Language>(this.getInitialLanguage());

    // هل اللغة عربية؟
    readonly isArabic = signal<boolean>(this.currentLang() === 'ar');

    // هل الاتجاه RTL؟
    readonly isRTL = signal<boolean>(this.currentLang() === 'ar');

    constructor() {
        // تهيئة ngx-translate
        this.translateService.addLangs(['ar', 'en']);
        this.translateService.setDefaultLang('ar');

        // تطبيق اللغة الأولية
        this.applyLanguage(this.currentLang());

        // تحديث عند تغيير اللغة
        effect(() => {
            const lang = this.currentLang();
            this.isArabic.set(lang === 'ar');
            this.isRTL.set(lang === 'ar');

            if (isPlatformBrowser(this.platformId)) {
                this.applyLanguage(lang);
                localStorage.setItem(this.LANG_KEY, lang);
            }
        });
    }

    /**
     * الحصول على اللغة الافتراضية
     */
    private getInitialLanguage(): Language {
        if (isPlatformBrowser(this.platformId)) {
            const savedLang = localStorage.getItem(this.LANG_KEY) as Language;
            if (savedLang === 'ar' || savedLang === 'en') {
                return savedLang;
            }
        }
        // العربية افتراضياً
        return 'ar';
    }

    /**
     * تطبيق اللغة على الصفحة
     */
    private applyLanguage(lang: Language): void {
        this.translateService.use(lang);

        if (isPlatformBrowser(this.platformId)) {
            const html = document.documentElement;

            // تغيير الاتجاه
            if (lang === 'ar') {
                html.setAttribute('dir', 'rtl');
                html.setAttribute('lang', 'ar');
            } else {
                html.setAttribute('dir', 'ltr');
                html.setAttribute('lang', 'en');
            }
        }
    }

    /**
     * التبديل بين اللغتين
     */
    toggleLanguage(): void {
        this.currentLang.set(this.currentLang() === 'ar' ? 'en' : 'ar');
    }

    /**
     * تعيين لغة محددة
     */
    setLanguage(lang: Language): void {
        this.currentLang.set(lang);
    }
}
